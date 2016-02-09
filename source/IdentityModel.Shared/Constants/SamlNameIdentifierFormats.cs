// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace IdentityModel.Constants
{
    public static class SamlNameIdentifierFormats
    {
        public const string EmailAddressString               = "urn:oasis:names:tc:SAML:1.1:nameid-format:emailAddress";
        public const string EncryptedString                  = "urn:oasis:names:tc:SAML:2.0:nameid-format:encrypted";
        public const string EntityString                     = "urn:oasis:names:tc:SAML:2.0:nameid-format:entity";
        public const string KerberosString                   = "urn:oasis:names:tc:SAML:2.0:nameid-format:kerberos";
        public const string PersistentString                 = "urn:oasis:names:tc:SAML:2.0:nameid-format:persistent";
        public const string TransientString                  = "urn:oasis:names:tc:SAML:2.0:nameid-format:transient";
        public const string UnspecifiedString                = "urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified";
        public const string WindowsDomainQualifiedNameString = "urn:oasis:names:tc:SAML:1.1:nameid-format:WindowsDomainQualifiedName";
        public const string X509SubjectNameString            = "urn:oasis:names:tc:SAML:1.1:nameid-format:X509SubjectName";
    }
}