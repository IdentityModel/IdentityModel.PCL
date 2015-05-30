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
using System.Collections.Generic;
using System.Linq;
#if __UNIVERSAL__
using Windows.Web;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
#else
using System.Net;
using System.Net.Http;
#endif
using System.Threading;
using System.Threading.Tasks;

namespace IdentityModel.Client
{
	public class OAuth2Client
	{
		protected HttpClient _client;
		protected ClientAuthenticationStyle _authenticationStyle;
		protected Uri _address;
		protected string _clientId;
		protected string _clientSecret;

#if __UNIVERSAL__
		protected TimeSpan _timeout;
#endif

		public enum ClientAuthenticationStyle
		{
			BasicAuthentication,
			PostValues,
			None
		};

		public OAuth2Client(Uri address)
#if __UNIVERSAL__
			: this(address, new HttpBaseProtocolFilter())
#else
			: this(address, new HttpClientHandler())
#endif
		{
			_address = address;
		}

#if __UNIVERSAL__
		public OAuth2Client(Uri address, HttpBaseProtocolFilter innerHttpClientHandler)
#else
		public OAuth2Client(Uri address, HttpMessageHandler innerHttpClientHandler)
#endif
		{
			if (address == null)
				throw new ArgumentNullException("address");

			if (innerHttpClientHandler == null)
			{
				throw new ArgumentNullException("innerHttpClientHandler");
			}

			_address = address;

			_client = new HttpClient(innerHttpClientHandler)
			{						
#if !__UNIVERSAL__	
				BaseAddress = address
#endif
			};
		
			_address = address;
			_authenticationStyle = ClientAuthenticationStyle.None;
		}

		public OAuth2Client(Uri address, string clientId, string clientSecret, ClientAuthenticationStyle style = ClientAuthenticationStyle.BasicAuthentication)
#if __UNIVERSAL__
			: this(address, clientId, clientSecret, new HttpBaseProtocolFilter(), style)
#else
			: this(address, clientId, clientSecret, new HttpClientHandler(), style)
#endif
			{ }

#if __UNIVERSAL__
		public OAuth2Client(Uri address, string clientId, string clientSecret, HttpBaseProtocolFilter innerHttpClientHandler, ClientAuthenticationStyle style = ClientAuthenticationStyle.BasicAuthentication)
#else
		public OAuth2Client(Uri address, string clientId, string clientSecret, HttpMessageHandler innerHttpClientHandler, ClientAuthenticationStyle style = ClientAuthenticationStyle.BasicAuthentication)
#endif
		: this(address, innerHttpClientHandler)
		{
			if (style == ClientAuthenticationStyle.BasicAuthentication)
			{
				_client.DefaultRequestHeaders.Authorization = 
					new BasicAuthentication(clientId, clientSecret).HeaderValue;
			}
			else if (style == ClientAuthenticationStyle.PostValues)
			{
				_authenticationStyle = style;
				_clientId = clientId;
				_clientSecret = clientSecret;
			}
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

		public string CreateCodeFlowUrl(
			string clientId, 
			string scope = null, 
			string redirectUri = null, 
			string state = null, 
			string nonce = null, 
			string loginHint = null,
			string acrValues = null,
			Dictionary<string, string> additionalValues = null)
		{
			return CreateAuthorizeUrl(
				clientId: clientId,
				responseType: OAuth2Constants.ResponseTypes.Code,
				scope: scope,
				redirectUri: redirectUri,
				state: state,
				nonce: nonce,
				loginHint: loginHint,
				acrValues: acrValues,
				additionalValues: additionalValues);
		}

		public string CreateImplicitFlowUrl(
			string clientId, 
			string scope = null, 
			string redirectUri = null, 
			string state = null, 
			string nonce = null, 
			string loginHint = null,
			string acrValues = null,
			Dictionary<string, string> additionalValues = null)
		{
			return CreateAuthorizeUrl(
				clientId: clientId,
				responseType: OAuth2Constants.ResponseTypes.Token,
				scope: scope,
				redirectUri: redirectUri,
				state: state,
				nonce: nonce,
				loginHint: loginHint,
				acrValues: acrValues,
				additionalValues: additionalValues);
		}

		public string CreateAuthorizeUrl(
			string clientId, 
			string responseType, 
			string scope = null, 
			string redirectUri = null, 
			string state = null, 
			string nonce = null,
			string loginHint = null,
			string acrValues = null,
			string responseMode = null,
			Dictionary<string, string> additionalValues = null)
		{
			var values = new Dictionary<string, string>
			{
				{ OAuth2Constants.ClientId, clientId },
				{ OAuth2Constants.ResponseType, responseType }
			};

			if (!string.IsNullOrWhiteSpace(scope))
			{
				values.Add(OAuth2Constants.Scope, scope);
			}

			if (!string.IsNullOrWhiteSpace(redirectUri))
			{
				values.Add(OAuth2Constants.RedirectUri, redirectUri);
			}

			if (!string.IsNullOrWhiteSpace(state))
			{
				values.Add(OAuth2Constants.State, state);
			}

			if (!string.IsNullOrWhiteSpace(nonce))
			{
				values.Add(OAuth2Constants.Nonce, nonce);
			}

			if (!string.IsNullOrWhiteSpace(loginHint))
			{
				values.Add(OAuth2Constants.LoginHint, loginHint);
			}

			if (!string.IsNullOrWhiteSpace(acrValues))
			{
				values.Add(OAuth2Constants.AcrValues, acrValues);
			}

			if (!string.IsNullOrWhiteSpace(responseMode))
			{
				values.Add(OAuth2Constants.ResponseMode, responseMode);
			}

			return CreateAuthorizeUrl(_address, Merge(values, additionalValues));
		}

		public static string CreateAuthorizeUrl(Uri endpoint, Dictionary<string, string> values)
		{
			var qs = string.Join("&", values.Select(kvp => String.Format("{0}={1}",
				System.Net.WebUtility.UrlEncode(kvp.Key), System.Net.WebUtility.UrlEncode(kvp.Value))).ToArray());
			return string.Format("{0}?{1}", endpoint.AbsoluteUri, qs);
		}

		public Task<TokenResponse> RequestResourceOwnerPasswordAsync(string userName, string password, string scope = null, Dictionary<string, string> additionalValues = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			var fields = new Dictionary<string, string>
			{
				{ OAuth2Constants.GrantType, OAuth2Constants.GrantTypes.Password },
				{ OAuth2Constants.UserName, userName },
				{ OAuth2Constants.Password, password }
			};

			if (!string.IsNullOrWhiteSpace(scope))
			{
				fields.Add(OAuth2Constants.Scope, scope);
			}

			return RequestAsync(Merge(fields, additionalValues), cancellationToken);
		}

		public Task<TokenResponse> RequestAuthorizationCodeAsync(string code, string redirectUri, Dictionary<string, string> additionalValues = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			var fields = new Dictionary<string, string>
			{
				{ OAuth2Constants.GrantType, OAuth2Constants.GrantTypes.AuthorizationCode },
				{ OAuth2Constants.Code, code },
				{ OAuth2Constants.RedirectUri, redirectUri }
			};

			return RequestAsync(Merge(fields, additionalValues), cancellationToken);
		}

		public Task<TokenResponse> RequestRefreshTokenAsync(string refreshToken, Dictionary<string, string> additionalValues = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			var fields = new Dictionary<string, string>
			{
				{ OAuth2Constants.GrantType, OAuth2Constants.GrantTypes.RefreshToken },
				{ OAuth2Constants.RefreshToken, refreshToken }
			};

			return RequestAsync(Merge(fields, additionalValues), cancellationToken);
		}

		public Task<TokenResponse> RequestClientCredentialsAsync(string scope = null, Dictionary<string, string> additionalValues = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			var fields = new Dictionary<string, string>
			{
				{ OAuth2Constants.GrantType, OAuth2Constants.GrantTypes.ClientCredentials }
			};

			if (!string.IsNullOrWhiteSpace(scope))
			{
				fields.Add(OAuth2Constants.Scope, scope);
			}

			return RequestAsync(Merge(fields, additionalValues), cancellationToken);
		}

		public Task<TokenResponse> RequestCustomGrantAsync(string grantType, string scope = null, Dictionary<string, string> additionalValues = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			var fields = new Dictionary<string, string>
			{
				{ OAuth2Constants.GrantType, grantType }
			};

			if (!string.IsNullOrWhiteSpace(scope))
			{
				fields.Add(OAuth2Constants.Scope, scope);
			}

			return RequestAsync(Merge(fields, additionalValues), cancellationToken);
		}

		public Task<TokenResponse> RequestCustomAsync(Dictionary<string, string> values, CancellationToken cancellationToken = default(CancellationToken))
		{
			return RequestAsync(Merge(values), cancellationToken);
		}

		public Task<TokenResponse> RequestAssertionAsync(string assertionType, string assertion, string scope = null, Dictionary<string, string> additionalValues = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			var fields = new Dictionary<string, string>
			{
				{ OAuth2Constants.GrantType, assertionType },
				{ OAuth2Constants.Assertion, assertion },
			};

			if (!string.IsNullOrWhiteSpace(scope))
			{
				fields.Add(OAuth2Constants.Scope, scope);
			}

			return RequestAsync(Merge(fields, additionalValues), cancellationToken);
		}

		public async Task<TokenResponse> RequestAsync(Dictionary<string, string> form, CancellationToken cancellationToken = default(CancellationToken))
		{
#if __UNIVERSAL__
			var response = await _client.PostAsync(_address,
				new HttpFormUrlEncodedContent(form)).AsTask(cancellationToken).ConfigureAwait(false);
#else
			var response = await _client.PostAsync(string.Empty,
				new FormUrlEncodedContent(form), cancellationToken).ConfigureAwait(false); 
#endif

#if __UNIVERSAL__
			if (response.StatusCode == HttpStatusCode.Ok ||
#else
			if (response.StatusCode == HttpStatusCode.OK || 
#endif
				response.StatusCode == HttpStatusCode.BadRequest)
			{
#if __UNIVERSAL__
				var content = await response.Content.ReadAsStringAsync().AsTask(cancellationToken).ConfigureAwait(false);
#else
				var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
#endif
				return new TokenResponse(content);
			}
			else
			{
				return new TokenResponse(response.StatusCode, response.ReasonPhrase);
			}
		}

		private Dictionary<string, string> Merge(Dictionary<string, string> explicitValues, Dictionary<string, string> additionalValues = null)
		{
			var merged = explicitValues;

			if (_authenticationStyle == ClientAuthenticationStyle.PostValues)
			{
				merged.Add(OAuth2Constants.ClientId, _clientId);
				merged.Add(OAuth2Constants.ClientSecret, _clientSecret);
			}

			if (additionalValues != null)
			{
				merged =
					explicitValues.Concat(additionalValues.Where(add => !explicitValues.ContainsKey(add.Key)))
										 .ToDictionary(final => final.Key, final => final.Value);
			}

			return merged;
		}
	}
}