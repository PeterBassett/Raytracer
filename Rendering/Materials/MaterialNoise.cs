using System;
using Raytracer.Rendering.Core;
using Raytracer.FileTypes;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.Materials
{
    class MaterialNoise : MaterialWithSubMaterials
    {
        private int Seed { get; set; }
        private float Persistence { get; set; }
        private int Octaves { get; set; }
        private float Scale { get; set; }
        private float Offset { get; set; }
        private int[] Primes { get; set; }
        private float Max { get; set; }

        public MaterialNoise(Material material1, Material material2, int seed, float persistence, int octaves, float scale, float offset, Vector size)
        {            
            this.SubMaterial1 = material1;
            this.SubMaterial2 = material2;
            this.Seed = seed;
            this.Octaves = octaves;
            this.Persistence = persistence;
            this.Scale = scale;
            this.Offset = offset;
            this.Size = size;

            var rnd = new Random(this.Seed);

            // resize the primes array
            this.Primes = new int[this.Octaves * 3];

            long i = 0;
            for (; i < Octaves; i++)
            {
                this.Primes[i * 3]       = GetNextPrime(rnd.Next(10000) + 10000);
                this.Primes[i * 3 + 1]   = GetNextPrime((rnd.Next(10000) + 10000) * 5);
                this.Primes[i * 3 + 2]   = GetNextPrime((rnd.Next(10000) + 10000) * 10000);
            }

            Max = 0.0f;
            for (i = 0; i < Octaves; i++)
                Max += (float)Math.Pow(Persistence, i);
        }

        public override void SolidifyMaterial(IntersectionInfo info, Material output)
        {
            float noise, total = 0;
            int i;
            float freq, amp;
            
            for (i = 0; i < Octaves; i++)
            {
                freq = (float)(1 << i);
                amp = (float)Math.Pow(Persistence, i);
                noise = InterpolatedNoise(info.HitPoint.X * freq * Size.X, info.HitPoint.Y * freq * Size.Y, info.HitPoint.Z * freq * Size.Z, i);

                total += noise * amp;
            }

            noise = ((total / Max) + Offset) * Scale;
            noise = noise * 0.5f + 0.5f;

            if (noise > 1.0f) noise = 1.0f;
            else if (noise < 0.0f) noise = 0.0f;

            Material mat1 = new Material();
            SubMaterial1.SolidifyMaterial(info, mat1);
            Material mat2 = new Material();
            SubMaterial2.SolidifyMaterial(info, mat2);

            Colour col = new Colour();
            col.Red = Lerp(mat1.Ambient.Red, mat2.Ambient.Red, noise);
            col.Green = Lerp(mat1.Ambient.Green, mat2.Ambient.Green, noise);
            col.Blue = Lerp(mat1.Ambient.Blue, mat2.Ambient.Blue, noise);
            output.Ambient = col;

            col = new Colour();
            col.Red = Lerp(mat1.Diffuse.Red, mat2.Diffuse.Red, noise);
            col.Green = Lerp(mat1.Diffuse.Green, mat2.Diffuse.Green, noise);
            col.Blue = Lerp(mat1.Diffuse.Blue, mat2.Diffuse.Blue, noise);
            output.Diffuse = col;

            col = new Colour();
            col.Red = Lerp(mat1.Emissive.Red, mat2.Emissive.Red, noise);
            col.Green = Lerp(mat1.Emissive.Green, mat2.Emissive.Green, noise);
            col.Blue = Lerp(mat1.Emissive.Blue, mat2.Emissive.Blue, noise);
            output.Emissive = col;

            col = new Colour();
            col.Red = Lerp(mat1.Reflective.Red, mat2.Reflective.Red, noise);
            col.Green = Lerp(mat1.Reflective.Green, mat2.Reflective.Green, noise);
            col.Blue = Lerp(mat1.Reflective.Blue, mat2.Reflective.Blue, noise);
            output.Reflective = col;

            col = new Colour();
            col.Red = Lerp(mat1.Transmitted.Red, mat2.Transmitted.Red, noise);
            col.Green = Lerp(mat1.Transmitted.Green, mat2.Transmitted.Green, noise);
            col.Blue = Lerp(mat1.Transmitted.Blue, mat2.Transmitted.Blue, noise);
            output.Transmitted = col;

            col = new Colour();
            col.Red = Lerp(mat1.Specular.Red, mat2.Specular.Red, noise);
            col.Green = Lerp(mat1.Specular.Green, mat2.Specular.Green, noise);
            col.Blue = Lerp(mat1.Specular.Blue, mat2.Specular.Blue, noise);
            output.Specular = col;

            output.Refraction = Lerp(mat1.Refraction, mat2.Refraction, noise);
            output.Specularity = Lerp(mat1.Specularity, mat2.Specularity, noise);
            output.SpecularExponent = Lerp(mat1.SpecularExponent, mat2.SpecularExponent, noise);
        }

        private float Lerp(float _a, float _b, float _t)
        {
            return ((_a) + (_t) * ((_b) - (_a)));
        }

        float Noise3D(int x, int y, int z, int i) {
	        int n;

	        n = x + y*57 + z*251;
	        n = (n<<13) ^ n;

	        return (1.0f - ((n * (n * n * Primes[i] + Primes[i+1]) + Primes[i+2]) & 0x7fffffff) / 1073741824.0f);
        }

        private float InterpolatedNoise(double fx, double fy, double fz, int i)
        {
            return InterpolatedNoise((float)fx, (float)fy, (float)fz, i);
        }

        private float InterpolatedNoise(float fx, float fy, float fz, int i)
        {
	        int ix, iy, iz, dx, dy, dz;
	        float fracx, fracy, fracz;
	        float a, b, c, d;
	        float e, f;

	        ix = (int )fx;
	        if (fx > 0) {
		        dx = 1;
		        fracx = fx - (float )ix;
		        fracx = fracx*fracx * (3-2*fracx);
	        }
	        else {
		        dx = -1;
		        fracx = fx - (float )ix;
		        fracx = fracx*fracx * (3+2*fracx);
	        }

	        iy = (int )fy;
	        if (fy > 0) {
		        dy = 1;
		        fracy = fy - (float )iy;
		        fracy = fracy*fracy * (3-2*fracy);
	        }
	        else {
		        dy = -1;
		        fracy = fy - (float )iy;
		        fracy = fracy*fracy * (3+2*fracy);
	        }

	        iz = (int )fz;
	        if (fz > 0) {
		        dz = 1;
		        fracz = fz - (float )iz;
		        fracz = fracz*fracz * (3-2*fracz);
	        }
	        else {
		        dz = -1;
		        fracz = fz - (float )iz;
		        fracz = fracz*fracz * (3+2*fracz);
	        }

	        i *= 3;
	        a = Noise3D(ix,    iy,    iz, i);
	        b = Noise3D(ix+dx, iy,    iz, i);
	        c = Noise3D(ix,    iy+dy, iz, i);
	        d = Noise3D(ix+dx, iy+dy, iz, i);
	        a = Lerp(a, b, fracx);
	        b = Lerp(c, d, fracx);
	        e = Lerp(a, b, fracy);

	        a = Noise3D(ix,    iy,    iz+dz, i);
	        b = Noise3D(ix+dx, iy,    iz+dz, i);
	        c = Noise3D(ix,    iy+dy, iz+dz, i);
	        d = Noise3D(ix+dx, iy+dy, iz+dz, i);
	        a = Lerp(a, b, fracx);
	        b = Lerp(c, d, fracx);
	        f = Lerp(a, b, fracy);

	        return Lerp(e, f, fracz);
        }

        // Is a number (n) a prime?
        bool IsPrime(long n)
        {
            long rootn = (int)Math.Sqrt((double)n);
            long i;
            // Check each number between 2 & sqrt(n) to see if it is a factor of n
            for (i = 2; i <= rootn; i++)
            {
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
