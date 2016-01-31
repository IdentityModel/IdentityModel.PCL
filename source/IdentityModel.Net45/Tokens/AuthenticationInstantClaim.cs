// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Security.Claims;
using System.Xml;

namespace IdentityModel.Tokens
{
    /// <summary>
    /// Helper class to create an authentication instant claim
    /// </summary>
    public static class AuthenticationInstantClaim
    {
        /// <summary>
        /// Returns an authentication instant claim for the current date/time
        /// </summary>
        /// <value>authentication instant claim.</value>
        public static Claim Now
        {
            get
            {
                return new Claim(ClaimTypes.AuthenticationInstant, XmlConvert.ToString(DateTime.UtcNow, DateTimeFormats.Generated), ClaimValueTypes.DateTime);
            }
        }
    }
}
