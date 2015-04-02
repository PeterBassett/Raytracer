using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;
using Raytracer.Rendering.FileTypes;

namespace Raytracer.Rendering.BackgroundMaterials
{
    class CubemapBackground2 : IBackgroundMaterial
    {
	    int _tileSize;
	    Bmp _image;     
   
        int _width;
        int _left; 
        int _front;
        int _right;
        int _back;
        int _up; 
        int _down; 
            
        public CubemapBackground2(string filename)
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

            _tileSize = _image.Width/3;

            var s = _tileSize;
            _width = _image.Width;
            _left = s*_image.Width;
            _front = (s+s*_image.Width);
            _right = (s*2+s*_image.Width);
            _back = (s+s*3*_image.Width);
            _up = s;
            _down = (s+s*2*_image.Width);
        }

        public Colour Shade(Ray ray)
        {
            var d = ray.Dir;
            var ax = Math.Abs(d.X);
            var ay = Math.Abs(d.Y);
            var az = Math.Abs(d.Z);
            var s = _tileSize;
            var u = 0.0;
            var v = 0.0;
            int o;
            if(ax >= ay && ax >= az){
                // right
                if(d.X > 0.0){
                    u = 1.0-(d.Z/d.X+1.0)*0.5;
                    v = (d.Y/d.X+1.0)*0.5;
                    o = this._right;
                }
                // left
                else {
                    u = 1.0-(d.Z/d.X+1.0)*0.5;
                    v = 1.0-(d.Y/d.X+1.0)*0.5;
                    o = this._left;
                }
            }
            else if(ay >= ax && ay >= az){
                // up
                if(d.Y <= 0.0) {
                    u = (d.X/d.Y+1.0)*0.5;
                    v = 1.0-(d.Z/d.Y+1.0)*0.5;
                    o = this._up;
                }
                // down
                else{
                    u = (d.X/d.Y+1.0)*0.5;
                    v = 1.0-(d.Z/d.Y+1.0)*0.5;
                    o = this._down;
                }
            }
            else{
                // front
                if(d.Z>0.0) {
                    u = (d.X/d.Z+1.0)*0.5;
                    v = (d.Y/d.Z+1.0)*0.5;
                    o = this._front;
                }
                // back
                else{
                    u = (d.X/d.Z+1.0)*0.5;
                    v = (d.Y/d.Z+1.0)*0.5;
                    o = this._back;
                }
            }
            o += (int)(Math.Floor(u*s) + Math.Floor(v*s)*this._width);
            return this._image.GetPixel(o % _image.Width, o / _image.Width);
        }
    }
}
