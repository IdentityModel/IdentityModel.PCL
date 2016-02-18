// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Security.Cryptography;
using System.Text;

namespace IdentityModel.Extensions
{
    public static class HashStringExtensions
    {
        public static string ToSha256(this string input, HashStringEncoding encoding = HashStringEncoding.Base64)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.ASCII.GetBytes(input);
                var hash = sha.ComputeHash(bytes);

                return Encode(hash, encoding);
            }
        }

        public static string ToSha512(this string input, HashStringEncoding encoding = HashStringEncoding.Base64)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            using (var sha = SHA512.Create())
            {
                var bytes = Encoding.ASCII.GetBytes(input);
                var hash = sha.ComputeHash(bytes);

                return Encode(hash, encoding);
            }
        }

        private static string Encode(byte[] hash, HashStringEncoding encoding)
        {
            if (encoding == HashStringEncoding.Base64)
            {
                return Convert.ToBase64String(hash);
            }
            else if (encoding == HashStringEncoding.Base64Url)
            {
                return Base64Url.Encode(hash);
            }

            throw new ArgumentException("Invalid encoding");
        }
    }
}