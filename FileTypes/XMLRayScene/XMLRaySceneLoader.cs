using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using Raytracer.Rendering.Core;
using System.Xml.Linq;
using Raytracer.FileTypes.XMLRayScene;
using Raytracer.FileTypes.XMLRayScene.Loaders;
using Raytracer.FileTypes.XMLRayScene.Extensions;
using System.ComponentModel;

namespace Raytracer.FileTypes.VBRayScene
{
    class XMLRaySceneLoader : ISceneLoader
    {
        [ImportMany]
        protected List<IXMLRaySceneItemLoader> LoaderList;
        [ImportMany]
        protected List<XMLRayElementParser> XmlElementParserList;
        private Dictionary<string, IXMLRaySceneItemLoader> Loaders;
        private Dictionary<string, XMLRayElementParser> Parsers;
        
        private CompositionContainer container = null;

        public XMLRaySceneLoader()
        {
            LoadAddins();
        }

        public Scene LoadScene(Stream sceneStream)
        {
	        var scene = new Scene();

            var document = XDocument.Load(sceneStream);
            
            LoadElement(scene, document.Root);
            
            scene.LoadComplete();

            return scene;           
        }

        public void LoadElement(Scene scene, XElement element)
        {
            var loader = FindLoaderForTag(element.Name.LocalName);
            loader.LoadObject(this, element, scene);
        }

        private IXMLRaySceneItemLoader FindLoaderForTag(string strObjectType)
        {
            strObjectType = strObjectType.ToLower();

            if (Loaders.ContainsKey(strObjectType))
                return Loaders[strObjectType];

            return Loaders[""];
        }

        private XMLRayElementParser FindParserForTag(string strObjectType)
        {
            strObjectType = strObjectType.ToLower();

            if (Parsers.ContainsKey(strObjectType))
                return Parsers[strObjectType];

            return null;
        }

        private void LoadAddins()
        {
            LoaderList = null;

            using (var catalog = new System.ComponentModel.Composition.Hosting.AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly()))
            {                
                container = new CompositionContainer(catalog);
                container.ComposeParts(this);
            }

            Loaders = LoaderList.ToDictionary(l => l.LoaderType.ToLower(), l => l);

            Parsers = XmlElementParserList.ToDictionary(l => l.LoaderType.ToLower(), l => l);
        }

        public void Dispose()
        {
            if (container != null)
            {
                container.Dispose();
                container = null;
            }
        }

        public T LoadObject<T>(Scene scene, XElement element, Func<T> createDefault)
        {
            var loader = FindParserForTag(element.Name.LocalName);
            var value = loader.LoadObject(this, scene, element, element.Name.LocalName, () => createDefault);

            return (T)value;
        }

        public T LoadObject<T>(Scene scene, XElement parent, string elementName, Func<T> createDefault)
        {
            var element = parent.ElementCaseInsensitive(elementName);

            if (element == null)
            {
                var attribute = parent.AttributeCaseInsensitive(elementName);

                if(attribute == null)
                    return createDefault();

                var conv = TypeDescriptor.GetConverter(typeof(T));
                return (T)conv.ConvertFromString(attribute.Value);
            }
            else
            {
                var loader = FindParserForTag(element.Name.LocalName);

                if (loader != null)
                    return (T)loader.LoadObject(this, scene, element, elementName, () => createDefault());
                else
                {
                    try
                    {
                        var conv = TypeDescriptor.GetConverter(typeof(T));
                        return (T)conv.ConvertFromString(element.Value);
                    }
                    catch 
                    {
                        return createDefault();
                    }
                }
            }
        }

        public bool CanLoadStream(Stream sceneStream)
        {
            sceneStream.Seek(0, SeekOrigin.Begin);

            var reader = new StreamReader(sceneStream);
            var firstLine = reader.ReadLine();

            return firstLine.Trim().StartsWith("<?xml");
        }
    }
}
