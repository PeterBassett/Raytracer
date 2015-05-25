using System;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Materials;
using Raytracer.MathTypes;

using Raytracer.Rendering.Core;
using System.Xml.Linq;
using System.Linq;
using Raytracer.FileTypes.XMLRayScene.Extensions;
using System.IO;
using System.Collections.Generic;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Materials
{
    [Export(typeof(XmlRayElementParser))]
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

            Func<XElement, bool> childrenOfRoot = (e) =>
            {
                return e.Parent == document.Root;
            };

            Func<XElement, bool> searchByElement = (e) =>
            {   
                return string.Compare(e.Name.LocalName, includeByElement.Value, true) == 0;
            };

            Func<XElement, bool> searchByName = (e) =>
            {
                var nameAttribute = e.AttributeCaseInsensitive("name");

                if(nameAttribute == null)
                    return false;

                return string.Compare(nameAttribute.Value, includeByName.Value, true) == 0;
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

        private static IEnumerable<XElement> SearchElement(XElement element, Func<XElement, bool> [] predicates)
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
