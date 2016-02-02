// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Security.Cryptography;
using System.Text;

namespace IdentityModel.Client
{
    public static class ProtocolHelperExtensions
    {
        public static string ToCodeChallenge(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.ASCII.GetBytes(input);
                var hash = sha.ComputeHash(bytes);

                return Base64Url.Encode(hash);
            }
        }
    }
}