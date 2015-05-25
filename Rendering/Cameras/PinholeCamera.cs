using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.Cameras
{
    class PinholeCamera : ICamera
    {
        private Point _location;
        private Vector _viewPointRotation;
        private Size _dimensions;
        private double _fieldOfView;
        private double _nearWidth;
        private double _nearHeight;

        public PinholeCamera(Point location, Vector viewPointRotation, Size outputDimensions, double fieldOfView)
	    {
            _location = location;
            _viewPointRotation = viewPointRotation;
            _fieldOfView = fieldOfView;
            OutputDimensions = outputDimensions;                       
	    }

        public Ray GenerateRayForPixel(Vector2 coordinate)
        {
            var x = coordinate.X - _dimensions.Width / 2.0;
            var y = coordinate.Y - _dimensions.Height / 2.0;

            double scaleFactor = _nearWidth / (double)_dimensions.Width;

            x *= scaleFactor;
            y *= scaleFactor;

            var dir = new Vector();
            dir.X = x;
            dir.Y = y;
            dir.Z = 1;

            dir.RotateX(_viewPointRotation.X, ref dir);
            dir.RotateY(_viewPointRotation.Y, ref dir);
            dir.RotateZ(_viewPointRotation.Z, ref dir);
            dir = dir.Normalize();

            return new Ray(_location, dir);
        }


        public Size OutputDimensions
        {
            get
            {
                return _dimensions;
            }
            set
            {
                _dimensions = value;

                _nearWidth = 2.0f * Math.Tan(MathLib.Deg2Rad(_fieldOfView) / 2.0f);
                var aspect = (double)_dimensions.Height / (double)_dimensions.Width;
                _nearHeight = _nearWidth * aspect;
            }
        }
    }
}
