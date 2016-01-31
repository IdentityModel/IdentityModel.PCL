// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel.Constants;
using System;
using System.IdentityModel.Tokens;

namespace IdentityModel.Tokens
{
    public class HmacSigningCredentials : SigningCredentials
    {
        public HmacSigningCredentials(string base64EncodedKey)
            : this(Convert.FromBase64String(base64EncodedKey))
        { }

        public HmacSigningCredentials(byte[] key)
            : base(new InMemorySymmetricSecurityKey(key),
                    CreateSignatureAlgorithm(key), 
                    CreateDigestAlgorithm(key))
        { }

        protected static string CreateSignatureAlgorithm(byte[] key)
        {
            switch (key.Length)
            {
                case 32:
                    return Algorithms.HmacSha256Signature;
                case 48:
                    return Algorithms.HmacSha384Signature;
                case 64:
                    return Algorithms.HmacSha512Signature;
                default:
                    throw new InvalidOperationException("Unsupported key length");
            }
        }

        protected static string CreateDigestAlgorithm(byte[] key)
        {
            switch (key.Length)
            {
                case 32:
                    return Algorithms.Sha256Digest;
                case 48:
                    return Algorithms.Sha384Digest;
                case 64:
                    return Algorithms.Sha512Digest;
                default:
                    throw new InvalidOperationException("Unsupported key length");
            }
        }
    }
}

