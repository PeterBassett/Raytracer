using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using Raytracer.Rendering.Core;

namespace Raytracer.FileTypes.VBRayScene
{
    class VbRaySceneLoader : ISceneLoader
    {
        [ImportMany] private List<IVbRaySceneItemLoader> _loaderList;
        private Dictionary<string, IVbRaySceneItemLoader> _loaders;
        private CompositionContainer _container;

        public VbRaySceneLoader()
        {
            LoadAddins();
        }

        public Scene LoadScene(Stream sceneStream)
        {
	        var scene = new Scene();
	        	
	        var oText = new Tokeniser();

            using (var sr = new StreamReader(sceneStream))
            {               
                while (!sr.EndOfStream)
                {
                    string strObjectTag = oText.GetToken(sr);

                    if (strObjectTag == "")
                        continue;

                    IVbRaySceneItemLoader loader = FindLoaderForTag(strObjectTag);

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

        private IVbRaySceneItemLoader FindLoaderForTag(string strObjectType)
        {
            strObjectType = strObjectType.ToLowerInvariant();

            if (_loaders.ContainsKey(strObjectType))
                return _loaders[strObjectType];

            return null;
        }

        private void LoadAddins()
        {
            _loaderList = null;

            using (var catalog = new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly()))
            {                
                _container = new CompositionContainer(catalog);
                _container.ComposeParts(this);
            }

            if (_loaderList == null)
                throw new CompositionException();
                
            _loaders = _loaderList.ToDictionary(l => l.LoaderType.ToLowerInvariant(), l => l);
        }

        public void Dispose()
        {
            if (_container != null)
            {
                _container.Dispose();
                _container = null;
            }
        }

        public bool CanLoadStream(Stream sceneStream)
        {
            return true;
        }
    }
}
