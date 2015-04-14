/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see LICENSE
 */

using System;
using System.IdentityModel.Tokens;

namespace IdentityModel.Tokens
{
    public class WrappedSecurityToken<T> : SecurityToken
    {
        T _token;

        public T Token
        {
            get
            {
                return _token;
            }
        }

        public WrappedSecurityToken(T token)
        {
            _token = token;
        }

        #region Not Implemented
        public override string Id
        {
            get { throw new NotImplementedException(); }
        }

        public override System.Collections.ObjectModel.ReadOnlyCollection<SecurityKey> SecurityKeys
        {
            get { throw new NotImplementedException(); }
        }

        public override DateTime ValidFrom
        {
            get { throw new NotImplementedException(); }
        }

        public override DateTime ValidTo
        {
            get { throw new NotImplementedException(); }
        }
        #endregion
    }
}
