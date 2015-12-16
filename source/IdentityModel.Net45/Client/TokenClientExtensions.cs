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

using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Tokens;

namespace IdentityModel.Client
{
    public static partial class TokenClientExtensions
    {
        public static Task<TokenResponse> RequestClientCredentialsAssertionAsync(this TokenClient client, X509Certificate2 certificate, string scope = null, object extra = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var fields = new Dictionary<string, string>
            {
                { OAuth2Constants.GrantType, OAuth2Constants.GrantTypes.ClientCredentials },
                { OAuth2Constants.ClientAssertionType, OAuth2Constants.ClientAssertionTypes.JwtBearer },
                { OAuth2Constants.ClientAssertion, ClientAssertionTokenFactory.CreateToken(client.ClientId, client.Address, certificate) }
            };

            if (!string.IsNullOrWhiteSpace(scope))
            {
                fields.Add(OAuth2Constants.Scope, scope);
            }

            return client.RequestAsync(Merge(client, fields, extra), cancellationToken);
        }
    }
}