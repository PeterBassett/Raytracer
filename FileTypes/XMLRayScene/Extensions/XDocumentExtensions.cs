using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Raytracer.FileTypes.XMLRayScene.Extensions
{
    static class XDocumentExtensions
    {
        public static XElement ElementCaseInsensitive(this XContainer source, XName name)
        {
            return ElementsCaseInsensitive(source, name).FirstOrDefault();
        }

        public static IEnumerable<XElement> ElementsCaseInsensitive(this XContainer source, XName name)
        {
            return source.Elements()
                .Where(e => e.Name.Namespace == name.Namespace
                    && e.Name.LocalName.Equals(name.LocalName, StringComparison.OrdinalIgnoreCase));
        }

        public static XAttribute AttributeCaseInsensitive(this XElement source, XName name)
        {
            return AttributesCaseInsensitive(source, name).FirstOrDefault();
        }

        private static IEnumerable<XAttribute> AttributesCaseInsensitive(this XElement source, XName name)
        {
            return source.Attributes()
                .Where(e => e.Name.Namespace == name.Namespace
                    && e.Name.LocalName.Equals(name.LocalName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
