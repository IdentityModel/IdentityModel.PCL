// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Security.Cryptography;

namespace IdentityModel.Jwt
{
    public static class JwkNetExtensions
    {
        public static JsonWebKey ToJsonWebKey(this RSACryptoServiceProvider provider,
            string alg = "RS256", string kid = null)
        {
            var key = provider.ExportParameters(false);

            var n = Base64Url.Encode(key.Modulus);
            var e = Base64Url.Encode(key.Exponent);
            return new JsonWebKey()
            {
                N = n,
                E = e,
                Kid = kid ?? CryptoRandom.CreateUniqueId(),
                Kty = "RSA",
                Alg = alg,
            };
        }

        public static RSACryptoServiceProvider CreateProvider(int keySize = 2048)
        {
            var csp = new CspParameters
            {
                Flags = CspProviderFlags.CreateEphemeralKey,
                KeyNumber = (int)KeyNumber.Signature
            };

            return new RSACryptoServiceProvider(keySize, csp);
        }
    }
}
