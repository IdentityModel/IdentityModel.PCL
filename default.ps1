properties {
	$base_directory = Resolve-Path .
	$src_directory = "$base_directory\source"
	$output_directory = "$base_directory\build"
	$dist_directory = "$base_directory\distribution"
	$sln_file = "$src_directory\IdentityModel.sln"
	$target_config = "Release"
	$nuget_path = "$base_directory\nuget.exe"

	$buildNumber = 0;
	$version = "1.11.0.0"
	$preRelease = $null
}

task default -depends Clean, CreateNuGetPackage
task appVeyor -depends Clean, CreateNuGetPackage

task Clean {
	rmdir $output_directory -ea SilentlyContinue -recurse
	rmdir $dist_directory -ea SilentlyContinue -recurse
	exec { msbuild /nologo /verbosity:quiet $sln_file /p:Configuration=$target_config /t:Clean }
}

task Compile -depends UpdateVersion {
	exec { msbuild /nologo /verbosity:q $sln_file /p:Configuration=$target_config /p:TargetFrameworkVersion=v4.5 }
}

task UpdateVersion {
	$vSplit = $version.Split('.')
	if($vSplit.Length -ne 4)
	{
		throw "Version number is invalid. Must be in the form of 0.0.0.0"
	}
	$major = $vSplit[0]
	$minor = $vSplit[1]
	$patch = $vSplit[2]
	$assemblyFileVersion =  "$major.$minor.$patch.$buildNumber"
	$assemblyVersion = "$major.$minor.0.0"
	$versionAssemblyInfoFile = "$src_directory/VersionAssemblyInfo.cs"
	"using System.Reflection;" > $versionAssemblyInfoFile
	"" >> $versionAssemblyInfoFile
	"[assembly: AssemblyVersion(""$assemblyVersion"")]" >> $versionAssemblyInfoFile
	"[assembly: AssemblyFileVersion(""$assemblyFileVersion"")]" >> $versionAssemblyInfoFile
}

task CreateNuGetPackage -depends Compile {
	$vSplit = $version.Split('.')
	if($vSplit.Length -ne 4)
	{
		throw "Version number is invalid. Must be in the form of 0.0.0.0"
	}
	$major = $vSplit[0]
	$minor = $vSplit[1]
	$patch = $vSplit[2]
	$packageVersion =  "$major.$minor.$patch"
	if($preRelease){
		$packageVersion = "$packageVersion-$preRelease"
	}

	if ($buildNumber -ne 0){
		$packageVersion = $packageVersion + "-build" + $buildNumber.ToString().PadLeft(5,'0')
	}

	new-item $dist_directory -type directory

	copy-item $src_directory\IdentityModel.nuspec $dist_directory

	new-item $dist_directory\lib\net45 -type directory
	copy-item $output_directory\net45\IdentityModel.dll $dist_directory\lib\net45\
	copy-item $output_directory\net45\IdentityModel.pdb $dist_directory\lib\net45\

	new-item $dist_directory\lib\portable-net45+wp80+win8+wpa81 -type directory
	copy-item $output_directory\portable\IdentityModel.dll $dist_directory\lib\portable-net45+wp80+win8+wpa81\
	copy-item $output_directory\portable\IdentityModel.pdb $dist_directory\lib\portable-net45+wp80+win8+wpa81\

	exec { . $nuget_path pack $dist_directory\IdentityModel.nuspec -BasePath $dist_directory -o $dist_directory -version $packageVersion }
}
