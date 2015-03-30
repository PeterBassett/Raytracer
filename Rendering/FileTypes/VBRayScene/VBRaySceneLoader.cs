using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering;
using System.IO;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Raytracer.Rendering.FileTypes.VBRayScene
{
    class VBRaySceneLoader : ISceneLoader
    {
        [ImportMany]
        protected List<IVBRaySceneItemLoader> LoaderList;
        [ImportMany]
        protected List<IVBRaySceneItemSaver> SaverList;
        private Dictionary<string, IVBRaySceneItemLoader> Loaders;
        private Dictionary<Type, List<IVBRaySceneItemSaver>> Savers;
        private CompositionContainer container = null;

        public VBRaySceneLoader()
        {
            LoadAddins();
        }

        public Scene LoadScene(Stream sceneStream)
        {
	        Scene scene = new Scene();
	        	
	        Tokeniser oText = new Tokeniser();

            using (StreamReader sr = new StreamReader(sceneStream))
            {               
                while (!sr.EndOfStream)
                {
                    string strObjectTag = oText.GetToken(sr);

                    if (strObjectTag == "")
                        continue;

                    IVBRaySceneItemLoader loader = FindLoaderForTag(strObjectTag);

                    if (loader != null)
                        loader.LoadObject(sr, scene);
                    else
                    {
                        throw new Exception("Couldn't create object loader for object '" + strObjectTag + "'");
                    }
                }

                scene.LoadComplete();

                return scene;
            }
        }

        private IVBRaySceneItemLoader FindLoaderForTag(string strObjectType)
        {
            strObjectType = strObjectType.ToLower();

            if (Loaders.ContainsKey(strObjectType))
                return Loaders[strObjectType];

            return null;
        }

        private IEnumerable<IVBRaySceneItemSaver> FindSaverForObjectType(Type type)
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
    }
}
