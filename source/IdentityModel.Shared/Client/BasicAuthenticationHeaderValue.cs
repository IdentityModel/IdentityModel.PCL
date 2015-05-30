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
using System;
#if __UNIVERSAL__
using Windows.Web.Http.Headers;
#else
using System.Net.Http.Headers;
#endif
using System.Text;

namespace IdentityModel.Client
{
	public class BasicAuthentication
	{
		private string _userName;
		private string _password;

		public BasicAuthentication(string userName, string password)
        {
			_userName = userName;
			_password = password;
		}

        private static string EncodeCredential(string userName, string password)
        {
            Encoding encoding = Encoding.UTF8;
            string credential = String.Format("{0}:{1}", userName, password);

            return Convert.ToBase64String(encoding.GetBytes(credential));
        }

#if __UNIVERSAL__
		public HttpCredentialsHeaderValue HeaderValue
#else
		public AuthenticationHeaderValue HeaderValue
#endif
		{
			get
			{
#if __UNIVERSAL__
				return new HttpCredentialsHeaderValue("Basic", EncodeCredential(_userName, _password));
#else
				return new AuthenticationHeaderValue("Basic", EncodeCredential(_userName, _password));
#endif
			}
		}
    }
}