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
#if __UNIVERSAL__
using Windows.Web;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
#else
using System.Net;
using System.Net.Http;
#endif
using System.Threading.Tasks;

namespace IdentityModel.Client
{
    public class UserInfoClient
    {
        protected HttpClient _client;
		protected Uri _endpoint;

#if __UNIVERSAL__
		protected TimeSpan _timeout;
#endif

        public UserInfoClient(Uri endpoint, string token)
#if __UNIVERSAL__
			: this(endpoint, token, new HttpBaseProtocolFilter())
#else
            : this(endpoint, token, new HttpClientHandler())
#endif
		{
			_endpoint = endpoint;
		}

#if __UNIVERSAL__
        public UserInfoClient(Uri endpoint, string token, HttpBaseProtocolFilter innerHttpClientHandler)
#else
		public UserInfoClient(Uri endpoint, string token, HttpClientHandler innerHttpClientHandler)
#endif
        {
            if (endpoint == null)
                throw new ArgumentNullException("endpoint");

            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException("token");

            if (innerHttpClientHandler == null)
                throw new ArgumentNullException("innerHttpClientHandler");

			_endpoint = endpoint;

			_client = new HttpClient(innerHttpClientHandler)
            {
#if !__UNIVERSAL__
                BaseAddress = endpoint
#endif
            };

            _client.SetBearerToken(token);
        }

        public TimeSpan Timeout
        {
            set
            {
#if __UNIVERSAL__
				_timeout = value;
#else
				_client.Timeout = value;
#endif
			}
        }

        public async Task<UserInfoResponse> GetAsync()
        {
#if __UNIVERSAL__
            var response = await _client.GetAsync(_endpoint);
#else
            var response = await _client.GetAsync("");
#endif

#if __UNIVERSAL__
			if (response.StatusCode != HttpStatusCode.Ok)
#else
            if (response.StatusCode != HttpStatusCode.OK)
#endif
				return new UserInfoResponse(response.StatusCode, response.ReasonPhrase);

            var content = await response.Content.ReadAsStringAsync();
            return new UserInfoResponse(content);
        }
    }
}
