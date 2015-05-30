﻿/*
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
using IdentityModel.Client;
#if __UNIVERSAL__
using Windows.Web.Http.Headers;
#else
using System.Net.Http.Headers;
#endif

#if __UNIVERSAL__
namespace Windows.Web.Http
#else
namespace System.Net.Http
#endif
{
    public static class HttpClientExtensions
    {
        public static void SetBasicAuthentication(this HttpClient client, string userName, string password)
        {
			client.DefaultRequestHeaders.Authorization = new BasicAuthentication(userName, password).HeaderValue;
        }

        public static void SetToken(this HttpClient client, string scheme, string token)
        {
            client.DefaultRequestHeaders.Authorization = 
#if __UNIVERSAL__
				new HttpCredentialsHeaderValue(scheme, token);
#else
				new AuthenticationHeaderValue(scheme, token);
#endif
        }

        public static void SetBearerToken(this HttpClient client, string token)
        {
            client.SetToken("Bearer", token);
        }
    }
}
