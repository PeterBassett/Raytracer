using System;
using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Materials;

namespace Raytracer.FileTypes.VBRayScene.Loaders
{
    

    [Export(typeof(IVBRaySceneItemLoader))]
    class NoiseMaterialLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "NoiseMaterial"; } }
        public void  LoadObject(System.IO.StreamReader file, Scene scene)
        {
            Tokeniser oText = new Tokeniser();
                    
            Material mat1 = null;            
            Material mat2 = null;

            // get the name
            var Name = oText.GetToken(file);
            
            string strMaterial = oText.GetToken(file);
            mat1 = scene.FindMaterial(strMaterial);

            if (mat1 == null)
                throw new Exception("Cannot find material '" + strMaterial + "' for NoiseMaterial.");

            strMaterial = oText.GetToken(file);
            mat2 = scene.FindMaterial(strMaterial);

            if (mat2 == null)
                throw new Exception("Cannot find material '" + strMaterial + "' for NoiseMaterial.");

            var SubMaterial1 = mat1;
            var SubMaterial2 = mat2;
            
            var Seed = int.Parse(oText.GetToken(file));
            
            var Persistence = float.Parse(oText.GetToken(file));
            var Octaves = int.Parse(oText.GetToken(file));
            var Scale = float.Parse(oText.GetToken(file));
            var Offset = float.Parse(oText.GetToken(file));

            Vector size = new Vector();
            size.X = float.Parse(oText.GetToken(file));
            size.Y = float.Parse(oText.GetToken(file));
            size.Z = float.Parse(oText.GetToken(file));

            MaterialNoise mat = new MaterialNoise(mat1, mat2, Seed, Persistence, Octaves, Scale, Offset, size);
            scene.AddMaterial(mat, mat.Name);
        }

        // Is a number (n) a prime?
        bool IsPrime(long n) 
        {
	        long rootn = (int)Math.Sqrt((double)n);
	        long i;
	        // Check each number between 2 & sqrt(n) to see if it is a factor of n
	        for (i=2; i<=rootn; i++) {
		        // If so, the number is not a prime
		        if ((n % i) == 0)
			        return false;
	        }
	        // If not, the number has no factors & is a prime
	        return true;
        }

        // Find the first number that is higher than n & is also a prime
        // There's probably a quicker way of doing this, but it's only ever done once
        int GetNextPrime(int n) 
        {
	        while (!IsPrime(n))
		        n++;
	        return n;
        }
    }
}
