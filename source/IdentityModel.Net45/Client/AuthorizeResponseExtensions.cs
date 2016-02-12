// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace IdentityModel.Client
{
    public static class AuthorizeResponseExtensions
    {
        public static bool ValidateCodeHash(this AuthorizeResponse response, string c_hash)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }

            if (String.IsNullOrWhiteSpace(c_hash))
            {
                throw new ArgumentNullException("c_hash");
            }

            var code = response.Code;
            if (String.IsNullOrWhiteSpace(code))
            {
                return false;
            }

            using (var sha = SHA256.Create())
            {
                var codeHash = sha.ComputeHash(Encoding.ASCII.GetBytes(code));
                byte[] leftBytes = new byte[16];
                Array.Copy(codeHash, leftBytes, 16);

                var codeHashB64 = Base64Url.Encode(leftBytes);

                return string.Equals(c_hash, codeHashB64, StringComparison.Ordinal);
            }
        }
    }
}