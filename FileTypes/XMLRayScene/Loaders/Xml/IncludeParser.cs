using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Raytracer.FileTypes.XMLRayScene.Extensions;
using Raytracer.Properties.Annotations;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Xml
{
    [Export(typeof(XmlRayElementParser)), UsedImplicitly]
    class IncludeParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "Include"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, SystemComponents components, XElement element, string elementName, Func<dynamic> createDefault)
        {
            var includeFile = element.AttributeCaseInsensitive("href");

            if (includeFile == null)
                throw new ArgumentNullException("href must be specified");

            if (!File.Exists(includeFile.Value))
                throw new FileNotFoundException();

            var document = XDocument.Load(includeFile.Value);

            element.AddAfterSelf(SearchForElements(element, document));

            return null;
        }

        private static IEnumerable<XElement> SearchForElements(XElement element, XDocument document)
        {
            var includeByElement = element.AttributeCaseInsensitive("includeByElement");
            var includeByName = element.AttributeCaseInsensitive("includeByName");

            Func<XElement, bool> childrenOfRoot = e => e.Parent == document.Root;

            Func<XElement, bool> searchByElement = e => string.Compare(e.Name.LocalName, includeByElement.Value, StringComparison.OrdinalIgnoreCase) == 0;

            Func<XElement, bool> searchByName = e =>
            {
                var nameAttribute = e.AttributeCaseInsensitive("name");

                if(nameAttribute == null)
                    return false;

                return string.Compare(nameAttribute.Value, includeByName.Value, StringComparison.OrdinalIgnoreCase) == 0;
            };

            var predicates = new List<Func<XElement, bool>>();

            if (includeByElement != null)
                predicates.Add(searchByElement);

            if (includeByName != null)
                predicates.Add(searchByName);

            if (!predicates.Any())
                predicates.Add(childrenOfRoot);

            return SearchElement(document.Root, predicates.ToArray());
        }

        private static IEnumerable<XElement> SearchElement(XElement element, IEnumerable<Func<XElement, bool>> predicates)
        {
            foreach (var predicate in predicates)
            {
                if (predicate(element))
                    yield return element;
            }

            foreach (var child in element.Elements())
            {
                foreach (var item in SearchElement(child, predicates))
                    yield return item;
            }
        }
    }
}
