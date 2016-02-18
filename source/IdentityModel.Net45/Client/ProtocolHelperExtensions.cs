// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel.Extensions;

namespace IdentityModel.Client
{
    public static class ProtocolHelperExtensions
    {
        public static string ToCodeChallenge(this string input)
        {
            return input.ToSha256(HashStringEncoding.Base64Url);
        }
    }
}