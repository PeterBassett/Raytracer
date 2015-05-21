using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using Raytracer.Rendering.Core;
using System.Globalization;

namespace Raytracer.FileTypes.VBRayScene
{
    class VBRaySceneLoader : ISceneLoader
    {
        [ImportMany]
        protected List<IVBRaySceneItemLoader> LoaderList;
        private Dictionary<string, IVBRaySceneItemLoader> Loaders;
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
            strObjectType = strObjectType.ToLowerInvariant();

            if (Loaders.ContainsKey(strObjectType))
                return Loaders[strObjectType];

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

            Loaders = LoaderList.ToDictionary(l => l.LoaderType.ToLowerInvariant(), l => l);
        }

        public void Dispose()
        {
            if (container != null)
            {
                container.Dispose();
                container = null;
            }
        }

        public bool CanLoadStream(Stream sceneStream)
        {
            return true;
        }
    }
}
