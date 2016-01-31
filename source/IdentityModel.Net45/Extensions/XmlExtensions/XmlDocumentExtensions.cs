// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Diagnostics.Contracts;
using System.Xml;
using System.Xml.Linq;

namespace IdentityModel.Extensions
{
    public static partial class XmlExtensions
    {
        /// <summary>
        /// Converts a XmlDocument to a XDocument.
        /// </summary>
        /// <param name="document">The XmlDocument.</param>
        /// <returns>A XDocument</returns>
        public static XDocument ToXDocument(this XmlDocument document)
        {
            Contract.Requires(document != null);
            Contract.Ensures(Contract.Result<XDocument>() != null);


            return new XmlConverter(document).CreateXDocument();
        }   
    }
}