// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Security.Claims;

namespace IdentityModel.Tokens
{
    public class SimpleSecurityTokenHandler : SecurityTokenHandler
    {
        private string[] _identifier;
        public delegate ClaimsPrincipal ValidateTokenDelegate(string tokenString);

        public ValidateTokenDelegate Validator { get; set; }

        public SimpleSecurityTokenHandler(string identifier)
            : this(identifier, null)
        { }

        public SimpleSecurityTokenHandler(ValidateTokenDelegate validator) 
            : this(Guid.NewGuid().ToString(), validator)
        { }

        public SimpleSecurityTokenHandler(string identifier, ValidateTokenDelegate validator)
        {
            _identifier = new string[] { identifier };
            Validator = validator;
        }

        public override SecurityToken ReadToken(string tokenString)
        {
            return new SimpleSecurityToken(tokenString);
        }

        public override ReadOnlyCollection<ClaimsIdentity> ValidateToken(SecurityToken token)
        {
            var simpleToken = token as SimpleSecurityToken;
            if (simpleToken == null)
            {
                throw new ArgumentException("SecurityToken is not a SimpleSecurityToken");
            }

            var identity = Validator(simpleToken.Token).Identity as ClaimsIdentity;

            if (identity != null)
            {
                if (Configuration != null && Configuration.SaveBootstrapContext)
                {
                    identity.BootstrapContext = new BootstrapContext(simpleToken, this);
                }

                return new List<ClaimsIdentity> { identity }.AsReadOnly();
            }
            else
            {
                throw new SecurityTokenValidationException("No identity");
            }
        }

        public override string WriteToken(SecurityToken token)
        {
            var simpleToken = token as SimpleSecurityToken;
            if (simpleToken == null)
            {
                throw new ArgumentException("SecurityToken is not a SimpleSecurityToken");
            }

            return simpleToken.Token;
        }

        public override string[] GetTokenTypeIdentifiers()
        {
            return _identifier;
        }

        public override Type TokenType
        {
            get { return typeof(SimpleSecurityToken); }
        }
    }
}
