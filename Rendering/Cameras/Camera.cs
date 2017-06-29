using System;
using Raytracer.MathTypes;
using Raytracer.Rendering.Core;
using Raytracer.Rendering.Distributions;
using Raytracer.Rendering.Samplers;

namespace Raytracer.Rendering.Cameras
{
    class Camera : ICamera
    {
        private Transform _transform;
        private Point _location;
        private Size _dimensions;
        private double _fieldOfView;
        private double _nearWidth;
        private double _nearHeight;
        private double _m;
        private double _focalDistance;
	    private double _apertureRadius;

        public Camera(Transform transform, Size outputDimensions, double focalDistance, double apertureRadius, double fieldOfView)
	    {
            _transform = transform;
            _location = _transform.ToWorldSpace(new Point(0, 0, 0));
            _fieldOfView = fieldOfView;
            _focalDistance = focalDistance;
            _apertureRadius = apertureRadius;
            OutputDimensions = outputDimensions;

            _m = 1 / Math.Tan(_fieldOfView * Math.PI / 360.0);
	    }

        public Ray GenerateRayForPixel(Vector2 coordinate)//, Distribution distribution)
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

            var up = new Vector();
            up.X = 0;
            up.Y = 1;
            up.Z = 0;

            var u = Vector.CrossProduct(up, dir).Normalize();
            var v = Vector.CrossProduct(dir, u).Normalize();
            u = _transform.ToObjectSpace(u).Normalize();
            v = _transform.ToObjectSpace(v).Normalize();

            dir = _transform.ToObjectSpace(dir);
            dir = dir.Normalize();

            var origin = _location;

            if(_apertureRadius > 0) 
            {
                //var distribution = new StratifiedDistribution();
                var distribution = new RandomDistribution();

                var lensOffset = _apertureRadius * Sampler.ConcentricSampleDisk(distribution.TwoD(1, 0, 0, 1, 1)[0]);
                var pointOnLens = _transform.ToObjectSpace(new Vector(lensOffset.X, lensOffset.Y, 0));

                origin = _location + pointOnLens;
                var focalPoint = _location + (dir * _focalDistance);
		                        
                dir = (focalPoint - origin).Normalize();
	        }
	        
            return new Ray(origin, dir);
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
