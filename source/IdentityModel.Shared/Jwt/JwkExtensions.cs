// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Newtonsoft.Json;
using System.Text;

namespace IdentityModel.Jwt
{
    public static class JsonWebKeyExtensions
    {
        public static string ToJwkString(this IdentityModel.Jwt.JsonWebKey key)
        {
            var json = JsonConvert.SerializeObject(key);
            // using UTF8 because we don't have ASCII in PCLs
            return Base64Url.Encode(Encoding.UTF8.GetBytes(json));
        }
    }
}
