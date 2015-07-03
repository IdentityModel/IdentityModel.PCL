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
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityModel.Client
{
    public class OAuth2Client
    {
        protected HttpClient _client;
        
        public enum ClientAuthenticationStyle
        {
            BasicAuthentication,
            PostValues,
            None
        };

        public ClientAuthenticationStyle AuthenticationStyle { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public OAuth2Client(Uri address)
            : this(address, new HttpClientHandler())
        { }

        public OAuth2Client(Uri address, HttpMessageHandler innerHttpClientHandler)
        {
            if (address == null) throw new ArgumentNullException("address");
            if (innerHttpClientHandler == null) throw new ArgumentNullException("innerHttpClientHandler");
            
            _client = new HttpClient(innerHttpClientHandler)
            {
                BaseAddress = address
            };

            AuthenticationStyle = ClientAuthenticationStyle.None;
        }

        public OAuth2Client(Uri address, string clientId, string clientSecret, ClientAuthenticationStyle style = ClientAuthenticationStyle.BasicAuthentication)
            : this(address, clientId, clientSecret, new HttpClientHandler(), style)
        { }

        public OAuth2Client(Uri address, string clientId, ClientAuthenticationStyle style = ClientAuthenticationStyle.BasicAuthentication)
            : this(address, clientId, string.Empty, new HttpClientHandler(), style)
        { }

        public OAuth2Client(Uri address, string clientId, HttpMessageHandler innerHttpClientHandler)
            : this(address, clientId, string.Empty, innerHttpClientHandler, ClientAuthenticationStyle.PostValues)
        { }

        public OAuth2Client(Uri address, string clientId, string clientSecret, HttpMessageHandler innerHttpClientHandler, ClientAuthenticationStyle style = ClientAuthenticationStyle.BasicAuthentication)
            : this(address, innerHttpClientHandler)
        {
            if (string.IsNullOrEmpty(clientId)) throw new ArgumentNullException("ClientId");

            if (style == ClientAuthenticationStyle.BasicAuthentication)
            {
                _client.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(clientId, clientSecret);
            }
            else if (style == ClientAuthenticationStyle.PostValues)
            {
                AuthenticationStyle = style;
                ClientId = clientId;
                ClientSecret = clientSecret;
            }
        }

        public TimeSpan Timeout 
        { 
            set
            {
                _client.Timeout = value;
            }
        }

        public virtual async Task<TokenResponse> RequestAsync(IDictionary<string, string> form, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _client.PostAsync(string.Empty, new FormUrlEncodedContent(form), cancellationToken).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return new TokenResponse(content);
            }
            else
            {
                return new TokenResponse(response.StatusCode, response.ReasonPhrase);
            }
        }
    }
}