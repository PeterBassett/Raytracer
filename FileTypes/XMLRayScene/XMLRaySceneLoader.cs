using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Raytracer.FileTypes.XMLRayScene.Extensions;
using Raytracer.FileTypes.XMLRayScene.Loaders;
using Raytracer.Rendering.Core;

namespace Raytracer.FileTypes.XMLRayScene
{
    class XmlRaySceneLoader : ISceneLoader
    {
        [ImportMany] 
        private List<IXmlRaySceneItemLoader> _loaderList;
        [ImportMany] 
        private List<XmlRayElementParser> _parserList;

        private Dictionary<string, IXmlRaySceneItemLoader> _loaders;
        private Dictionary<string, XmlRayElementParser> _parsers;
        
        private CompositionContainer _container;

        public XmlRaySceneLoader()
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

        private IXmlRaySceneItemLoader FindLoaderForTag(string strObjectType)
        {
            strObjectType = strObjectType.ToLowerInvariant();

            if (_loaders.ContainsKey(strObjectType))
                return _loaders[strObjectType];

            return _loaders[""];
        }

        private XmlRayElementParser FindParserForTag(string strObjectType)
        {
            strObjectType = strObjectType.ToLowerInvariant();

            if (_parsers.ContainsKey(strObjectType))
                return _parsers[strObjectType];

            return null;
        }

        private void LoadAddins()
        {
            _loaderList = null;
            _parserList = null;

            using (var catalog = new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly()))
            {                
                _container = new CompositionContainer(catalog);
                _container.ComposeParts(this);
            }

            if (_loaderList == null)
                throw new CompositionException("No IXmlRaySceneItemLoader implementations available");
            if (_parserList == null)
                throw new CompositionException("No XmlRayElementParser implementations available");

            _loaders = _loaderList.ToDictionary(l => l.LoaderType.ToLowerInvariant(), l => l);
            _parsers = _parserList.ToDictionary(l => l.LoaderType.ToLowerInvariant(), l => l);
        }

        public void Dispose()
        {
            if (_container != null)
            {
                _container.Dispose();
                _container = null;
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
