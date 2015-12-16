/*
 * Copyright 2014, 2015 Dominick Baier, Brock Allen
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace IdentityModel.Tokens
{
    public static class ClientAssertionTokenFactory
    {
        public static string CreateToken(string clientId, string audience, X509Certificate2 certificate, bool embedCertificate = true, DateTime? nowOverride = null)
        {
            var now = (nowOverride ?? DateTime.Now).ToUniversalTime();

            var token = new JwtSecurityToken(
                    clientId,
                    audience,
                    new List<Claim>()
                    {
                        new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString()),
                        new Claim(JwtClaimTypes.Subject, clientId),
                        new Claim(JwtClaimTypes.IssuedAt, EpochTime.GetIntDate(now).ToString(), ClaimValueTypes.Integer64)
                    },
                    now,
                    now.AddMinutes(1),
                    new X509SigningCredentials(certificate,
                                               SecurityAlgorithms.RsaSha256Signature,
                                               SecurityAlgorithms.Sha256Digest)
                );

            if (embedCertificate)
            {
                var rawCertificate = Convert.ToBase64String(certificate.Export(X509ContentType.Cert));
                token.Header.Add(JwtHeaderParameterNames.X5c, new[] { rawCertificate });
            }

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(token);
        }
    }
}