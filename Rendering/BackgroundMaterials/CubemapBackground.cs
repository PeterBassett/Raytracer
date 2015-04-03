using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;
using Raytracer.Rendering.FileTypes;

namespace Raytracer.Rendering.BackgroundMaterials
{
    abstract class CubemapBackground : IBackgroundMaterial
    {
        protected enum Axis
        {
            FrontBack,
            LeftRight,
            TopBottom
        }

        protected enum Face
        {
            Front,
            Back,
            Left,
            Right,
            Top,
            Bottom
        }

	    protected int _tileSize;
        protected Bmp _image;

        public CubemapBackground(string filename)
        {
            LoadCubeMapImage(filename);

            VerifyCubemapDimensions();
        }

        private void LoadCubeMapImage(string path)
        {
            using(var bmp = new Bitmap(path))
                _image = new Bmp(bmp);
        }

        protected abstract void VerifyCubemapDimensions();

        protected static Axis GetDominantAxis(Vector3 r, ref double rmax)
        {
            double temp;
            Axis axis = Axis.LeftRight;
            rmax = Math.Abs(r.X);

            temp = Math.Abs(r.Y);
            if (temp > rmax)
            {
                rmax = temp;
                axis = Axis.TopBottom;
            }

            temp = Math.Abs(r.Z);
            if (temp > rmax)
            {
                rmax = temp;
                axis = Axis.FrontBack;
            }

            return axis;
        }

        public Colour Shade(Ray ray)
        {
            Vector3 r = ray.Dir;

            r.Normalize();

	        double rmax = 0;
            Axis imax = GetDominantAxis(r, ref rmax);
	
	        Face face = Face.Front;
	        double s = 0.0f, t = 0.0f;

            GetFaceCoordinates(ref r, imax, ref face, ref s, ref t);
	
	        s = 0.5f * (s / rmax + 1.0f);
	        t = 0.5f * (t / rmax + 1.0f);
	
	        return SampleFace(face, s, t);
        }

        protected Colour SampleFace(Face face, double s, double t)
        {
            s *= _tileSize;
            t *= _tileSize;

            int pxl = (int)Math.Floor(s - 0.5f);
            int pxh = (int)Math.Floor(s + 0.5f);
            int pyl = (int)Math.Floor(t - 0.5f);
            int pyh = (int)Math.Floor(t + 0.5f);

            double xs = s - 0.5f - pxl;
            double ys = t - 0.5f - pyl;

            var cll = GetPixelFromFace(face, pxl, pyl);
            var chl = GetPixelFromFace(face, pxh, pyl);
            var clh = GetPixelFromFace(face, pxl, pyh);
            var chh = GetPixelFromFace(face, pxh, pyh);

            return (1.0f - xs) * (1.0f - ys) * cll + xs * (1.0f - ys) * chl + (1.0f - xs) * ys * clh + xs * ys * chh;
        }

        Colour GetPixelFromFace(Face face, int px, int py)
        {
            int x_offset = 0, y_offset = 0;

            GetFaceOffsets(face, ref x_offset, ref y_offset);

            px = x_offset + Math.Max(0, Math.Min(_tileSize - 1, px));
            py = y_offset + Math.Max(0, Math.Min(_tileSize - 1, py));

            return _image.GetPixel(px, py);
        }

        protected abstract void GetFaceCoordinates(ref Vector3 r, Axis imax, ref Face face, ref double s, ref double t);

        protected abstract void GetFaceOffsets(Face face, ref int x_offset, ref int y_offset);
    }
}
