using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;
using Raytracer.Rendering.FileTypes;

namespace Raytracer.Rendering.BackgroundMaterials
{
    class CubemapBackground : IBackgroundMaterial
    {
	    int _tileSize;
	    Bmp _image;        

        public CubemapBackground(string filename)
        {	        		        
            LoadCubeMapImage(filename);

            _tileSize = _image.Width / 3;

            if (_image.Height != _tileSize * 4)
                throw new ArgumentOutOfRangeException("Cubemap file must be vertically oriented.");
        }

        private void LoadCubeMapImage(string path)
        {
            using(var bmp = new Bitmap(path))
                _image = new Bmp(bmp);
        }
	
        public Colour Shade(Ray ray)
        {
	        Vector3 r = ray.Dir;
	
	        int imax = 0;
	        var rmax = Math.Abs(r.X);

            if (Math.Abs(r.Y) > rmax)
            {
                rmax = Math.Abs(r.Y);
                imax = 1;
            }

            if (Math.Abs(r.Z) > rmax)
            {
                rmax = Math.Abs(r.Z);
                imax = 2;
            }
	
	        int face = -1;
	        double s = 0.0f, t = 0.0f;
	
	        switch ( imax ) {
	        case 0:
		        if ( r[0] > 0 ) {
			        face = 3;
			        s = r[2];
			        t = r[1];
		        }
		        else {
			        face = 1;
			        s = -r[2];
			        t = r[1];
		        }
		        break;
	        case 1:
		        if ( r[1] > 0 ) {
			        face = 0;
			        s = r[0];
			        t = r[2];
		        }
		        else {
			        face = 4;
			        s = r[0];
			        t = -r[2];
		        }
		        break;
	        case 2:
		        if ( r[2] > 0 ) {
			        face = 5;
			        s = r[0];
			        t = -r[1];
		        }
		        else {
			        face = 2;
			        s = r[0];
			        t = r[1];
		        }
		        break;
	        }
	
	        s = 0.5f * (s / rmax + 1.0f);
	        t = 0.5f * (t / rmax + 1.0f);
	
	        return SampleFace(face, s, t);
        }

        Colour SampleFace(int face, double s, double t) 
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

        Colour GetPixelFromFace(int face, int px, int py)
        {
            int x_offset = 0, y_offset = 0;

            switch (face)
            {
                case 0:
                    x_offset = _tileSize;
                    y_offset = _tileSize * 3;
                    break;
                case 1:
                    x_offset = 0;
                    y_offset = _tileSize * 2;
                    break;
                case 2:
                    x_offset = _tileSize;
                    y_offset = _tileSize * 2;
                    break;
                case 3:
                    x_offset = y_offset = _tileSize * 2;
                    break;
                case 4:
                    x_offset = y_offset = _tileSize;
                    break;
                case 5:
                    x_offset = _tileSize;
                    y_offset = 0;
                    break;
            }

            px = x_offset + Math.Max(0, Math.Min(_tileSize - 1, px));
            py = y_offset + Math.Max(0, Math.Min(_tileSize - 1, py));

            return _image.GetPixel(px, py);
        }
    }
}
