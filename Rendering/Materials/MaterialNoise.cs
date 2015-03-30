using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering.FileTypes;

namespace Raytracer.Rendering.Materials
{
    class MaterialNoise : MaterialWithSubMaterials
    {
        public int Seed { get; set; }
        public float Persistence { get; set; }
        public int Octaves { get; set; }
        public float Scale { get; set; }
        public float Offset { get; set; }
        public int[] Primes { get; set; }
        public float Max { get; set; }

        public MaterialNoise()
        {
            Seed = 0;
            Octaves = 1;
            Persistence = 0.5f;
            Scale = 1.0f;
            Offset = 0.0f; 
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
    }
}
