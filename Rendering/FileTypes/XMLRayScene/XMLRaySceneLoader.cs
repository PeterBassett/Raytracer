using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using Raytracer.Rendering.Core;
using System.Xml.Linq;
using Raytracer.Rendering.FileTypes.XMLRayScene;
using Raytracer.Rendering.FileTypes.XMLRayScene.Loaders;
using Raytracer.Rendering.FileTypes.XMLRayScene.Extensions;
using System.ComponentModel;

namespace Raytracer.Rendering.FileTypes.VBRayScene
{
    class XMLRaySceneLoader : ISceneLoader
    {
        [ImportMany]
        protected List<IXMLRaySceneItemLoader> LoaderList;
        [ImportMany]
        protected List<XMLRayElementParser> XmlElementParserList;
        [ImportMany]
        protected List<IXMLRaySceneItemSaver> SaverList;
        private Dictionary<string, IXMLRaySceneItemLoader> Loaders;
        private Dictionary<string, XMLRayElementParser> Parsers;
        private Dictionary<Type, List<IXMLRaySceneItemSaver>> Savers;
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


        private IEnumerable<IXMLRaySceneItemSaver> FindSaverForObjectType(Type type)
        {
            if (Savers.ContainsKey(type))
                return Savers[type];
            
            return null;
        }

        private void LoadAddins()
        {
            LoaderList = null;
            SaverList = null;

            using (var catalog = new System.ComponentModel.Composition.Hosting.AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly()))
            {                
                container = new CompositionContainer(catalog);
                container.ComposeParts(this);
            }

            Loaders = LoaderList.ToDictionary(l => l.LoaderType.ToLower(), l => l);

            Parsers = XmlElementParserList.ToDictionary(l => l.LoaderType.ToLower(), l => l);

            Savers = (from s in SaverList
                      group s by s.SaverForType into g
                      select g).ToDictionary(g => g.Key, g => g.ToList());
        }

        public void Dispose()
        {
            if (container != null)
            {
                container.Dispose();
                container = null;
            }
        }

        public void SaveScene(StreamWriter output, Scene scene)
        {
            foreach (var saver in FindSaverForObjectType(typeof(Scene)))
	        {
                saver.SaveObject(output, scene);
	        }
            
            foreach (var light in scene.Lights)
            {
                foreach (var saver in FindSaverForObjectType(light.GetType()))
                {
                    saver.SaveObject(output, light);
                }
            }

            foreach (var material in scene.Materials)
            {
                foreach (var saver in FindSaverForObjectType(material.GetType()))
                {
                    saver.SaveObject(output, material);
                }
            }

            foreach (var primitive in scene.Primitives)
            {
                foreach (var saver in FindSaverForObjectType(primitive.GetType()))
                {
                    saver.SaveObject(output, primitive);
                }
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
                var value = loader.LoadObject(this, scene, element, elementName, () => createDefault);

                return (T)value;
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
