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
        /// Converts a XElement to a XmlElement.
        /// </summary>
        /// <param name="element">The XElement.</param>
        /// <returns>A XmlElement</returns>
        public static XmlElement ToXmlElement(this XElement element)
        {
            Contract.Requires(element != null);
            Contract.Ensures(Contract.Result<XmlElement>() != null);


            return new XmlConverter(element).CreateXmlElement();
        }
    }
}