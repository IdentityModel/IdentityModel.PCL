// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace IdentityModel.Tokens
{
    public class SimpleSecurityToken : WrappedSecurityToken<string>
    {
        public SimpleSecurityToken(string token)
            : base(token)
        { }
    }
}
