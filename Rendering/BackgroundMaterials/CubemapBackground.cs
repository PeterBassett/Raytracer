using System;
using System.Drawing;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
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
        protected IBmp _image;

        protected CubemapBackground(string filename)
        {
            LoadCubeMapImage(filename);

            VerifyCubemapDimensions();
        }

        private void LoadCubeMapImage(string path)
        {
            using(var bmp = new Bitmap(path))
                _image = ImageReader.Read(bmp);
        }

        protected abstract void VerifyCubemapDimensions();

        protected static Axis GetDominantAxis(Vector r, ref double rmax)
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
            var r = ray.Dir;

            r.Normalize();

	        double rmax = 0;
            Axis imax = GetDominantAxis(r, ref rmax);

            Face face;
	        double s, t;
            GetFaceCoordinates(r, imax, out face, out s, out t);
	
	        s = 0.5f * (s / rmax + 1.0f);
	        t = 0.5f * (t / rmax + 1.0f);
	
	        var c = SampleFace(face, s, t);

            //if(c == new Colour(31f / 255, 34f / 255, 43f / 255))
                //Console.WriteLine();

            return c;
        }

        protected Colour SampleFace(Face face, double s, double t)
        {
            s *= _tileSize;
            t *= _tileSize;

            var pxl = (int)Math.Floor(s - 0.5f);
            var pxh = (int)Math.Floor(s + 0.5f);
            var pyl = (int)Math.Floor(t - 0.5f);
            var pyh = (int)Math.Floor(t + 0.5f);

            var xs = s - 0.5f - pxl;
            var ys = t - 0.5f - pyl;

            var cll = GetPixelFromFace(face, pxl, pyl);
            var chl = GetPixelFromFace(face, pxh, pyl);
            var clh = GetPixelFromFace(face, pxl, pyh);
            var chh = GetPixelFromFace(face, pxh, pyh);

            return (1.0f - xs) * (1.0f - ys) * cll + xs * (1.0f - ys) * chl + (1.0f - xs) * ys * clh + xs * ys * chh;
        }

        Colour GetPixelFromFace(Face face, int px, int py)
        {
            int x_offset, y_offset;

            GetFaceOffsets(face, out x_offset, out y_offset);

            px = x_offset + Math.Max(0, Math.Min(_tileSize - 1, px));
            py = y_offset + Math.Max(0, Math.Min(_tileSize - 1, py));

            return _image.GetPixel(px, py);
        }

        protected abstract void GetFaceCoordinates(Vector r, Axis imax, out Face face, out double s, out double t);

        protected abstract void GetFaceOffsets(Face face, out int x_offset, out int y_offset);
    }
}
