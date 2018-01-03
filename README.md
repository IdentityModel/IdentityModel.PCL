** all new work is happening [here](https://github.com/IdentityModel/IdentityModel2) **

# IdentityModel
A helper library for claims-based identity, OAuth 2.0 and OpenID Connect.

## .NET, Portable .NET, WinRT and Windows Phone 8.1

### TokenClient
Client library for OAuth 2.0 and OpenID Connect token endpoints.

Features:

* Support for client credentials & resource owner password credential flow
* Support for exchanging authorization codes with tokens
* Support for refreshing refresh tokens
* Support for client credentials via Basic Authentication, POST body and X.509 client certificates
* Extensible for custom parameters
* Parsing of token response messages

Example:
```csharp
var client = new TokenClient(
    "https://server/token",
    "client_id",
    "secret");
    
var response = await client.RequestClientCredentialsAsync("scope");
var token = response.AccessToken;
```


### AuthorizeRequest
Helper class for creating authorize request URLs (e.g. for code and implicit flow).

### UserInfoClient
Client library for an OpenID Connect user info endpoint

### Base64 URL encoder/decoder
Helper for working with URL safe base64 encodings

### Epoch Time Extensions
Helper for converting `DateTime` and `DateTimeOffset` to/from Epoch Time

### Time Constant Comparer
Helper for comparing strings without leaking timing information

### JWT/OpenID Connect Claim Types
Constants for standard claim types used in JWT, OAuth 2.0 and OpenID Connect

## Full .NET Framework only

### Extensions

**XML Extensions**  
Helpers for converting between `XmlDocument`, `XDocument`, `XmlElement` and `XElement` and working with `XmlReader`

**Security Token Extensions**  
Helpers for converting between `SecurityToken`, `GenericXmlSecurityToken`, `ClaimsPrincipal` and strings

**Fluent API to access the X.509 Certificate store**  
e.g. do  
`var cert = X509.LocalMachine.My.SubjectDistinguishedName.Find("CN=sts").First();`

**System.IdentityModel Helpers**  
Generic UserName security token helper, simple security token, HMAC signing credentials
