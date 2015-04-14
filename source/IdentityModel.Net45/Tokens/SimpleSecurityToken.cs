/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see LICENSE
 */

namespace IdentityModel.Tokens
{
    public class SimpleSecurityToken : WrappedSecurityToken<string>
    {
        public SimpleSecurityToken(string token)
            : base(token)
        { }
    }
}
