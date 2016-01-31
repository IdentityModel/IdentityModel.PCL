// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Security.Claims;

namespace IdentityModel.Client
{
    public static class UserInfoResponseExtensions
    {
        public static ClaimsIdentity GetClaimsIdentity(this UserInfoResponse response)
        {
            if (!response.IsError && !response.IsHttpError)
            {
                var id = new ClaimsIdentity("UserInfo");
                foreach (var c in response.Claims)
                {
                    id.AddClaim(new Claim(c.Item1, c.Item2));
                }

                return id;
            }

            return null;
        }
    }
}