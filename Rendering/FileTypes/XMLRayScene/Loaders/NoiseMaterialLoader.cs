using System;
using System.ComponentModel.Composition;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Materials;

namespace Raytracer.Rendering.FileTypes.XMLRayScene.Loaders
{
    

    [Export(typeof(IVBRaySceneItemLoader))]
    class NoiseMaterialLoader : IVBRaySceneItemLoader
    {
        public string LoaderType { get { return "NoiseMaterial"; } }
        public void  LoadObject(System.IO.StreamReader file, Scene scene)
        {
            Tokeniser oText = new Tokeniser();
            
            MaterialNoise mat = new MaterialNoise();

            Material mat1 = null;            
            Material mat2 = null;

            // get the name
            mat.Name = oText.GetToken(file);

            scene.AddMaterial(mat, mat.Name);

            string strMaterial = oText.GetToken(file);
            mat1 = scene.FindMaterial(strMaterial);

            if (mat1 == null)
                throw new Exception("Cannot find material '" + strMaterial + "' for NoiseMaterial.");

            strMaterial = oText.GetToken(file);
            mat2 = scene.FindMaterial(strMaterial);

            if (mat2 == null)
                throw new Exception("Cannot find material '" + strMaterial + "' for NoiseMaterial.");

            mat.SubMaterial1 = mat1;
            mat.SubMaterial2 = mat2;

            mat.Seed = int.Parse(oText.GetToken(file));

            mat.Persistence = float.Parse(oText.GetToken(file));
            mat.Octaves = int.Parse(oText.GetToken(file));
            mat.Scale = float.Parse(oText.GetToken(file));
            mat.Offset = float.Parse(oText.GetToken(file));

            Random rnd = new Random(mat.Seed);

            // resize the primes array
            mat.Primes = new int[mat.Octaves * 3];

            long i = 0;
            for (; i < mat.Octaves; i++)
            {
                mat.Primes[i * 3] = GetNextPrime(rnd.Next(10000) + 10000);
                mat.Primes[i * 3 + 1] = GetNextPrime((rnd.Next(10000) + 10000) * 5);
                mat.Primes[i * 3 + 2] = GetNextPrime((rnd.Next(10000) + 10000) * 10000);
            }

            mat.Max = 0.0f;
            for (i = 0; i < mat.Octaves; i++)
                mat.Max += (float)Math.Pow(mat.Persistence, i);

            Vector size = new Vector();
            size.X = float.Parse(oText.GetToken(file));
            size.Y = float.Parse(oText.GetToken(file));
            size.Z = float.Parse(oText.GetToken(file));
            mat.Size = size;
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
