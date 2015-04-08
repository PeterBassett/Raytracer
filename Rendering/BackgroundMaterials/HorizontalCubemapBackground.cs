using System;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.BackgroundMaterials
{
    class HorizontalCubemapBackground : CubemapBackground
    {
        public HorizontalCubemapBackground(string filename) : base(filename)
        {	        		        
        }

        protected override void VerifyCubemapDimensions()
        {
            _tileSize = _image.Size.Height / 3;

            if (_image.Size.Width != _tileSize * 4)
                throw new ArgumentOutOfRangeException("Cubemap file must be horizontally oriented.");
        }

        protected override void GetFaceCoordinates(ref Vector3 r, Axis imax, ref Face face, ref double s, ref double t)
        {
            switch (imax)
            {
                case Axis.LeftRight:
                    if (r.X > 0)
                    {
                        face = Face.Right;
                        s = -r.Z;
                        t = -r.Y;
                    }
                    else
                    {
                        face = Face.Left;
                        s = r.Z;
                        t = -r.Y;
                    }
                    break;
                case Axis.TopBottom:
                    if (r.Y > 0)
                    {
                        face = Face.Top;
                        s = r.X;
                        t = r.Z;
                    }
                    else
                    {
                        face = Face.Bottom;
                        s = r.X;
                        t = -r.Z;
                    }
                    break;
                case Axis.FrontBack:
                    if (r.Z > 0)
                    {
                        face = Face.Front;
                        s = r.X;
                        t = -r.Y;
                    }
                    else
                    {
                        face = Face.Back;
                        s = -r.X;
                        t = -r.Y;
                    }
                    break;
            }
        }

        protected override void GetFaceOffsets(Face face, ref int x_offset, ref int y_offset)
        {
            switch (face)
            {
                case Face.Back:
                    x_offset = _tileSize * 3;
                    y_offset = _tileSize;
                    break;
                case Face.Left:
                    x_offset = 0;
                    y_offset = _tileSize;
                    break;
                case Face.Bottom:
                    x_offset = _tileSize;
                    y_offset = _tileSize * 2;
                    break;
                case Face.Right:
                    x_offset = _tileSize * 2;
                    y_offset = _tileSize;
                    break;
                case Face.Front:
                    x_offset = y_offset = _tileSize;
                    break;
                case Face.Top:
                    x_offset = _tileSize;
                    y_offset = 0;
                    break;
            }
        }
    }
}
