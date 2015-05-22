﻿using System;
using System.ComponentModel.Composition;
using Raytracer.Rendering.Lights;
using Raytracer.Rendering.Core;
using Raytracer.MathTypes;

namespace Raytracer.FileTypes.XMLRayScene.Loaders.Lights
{
    [Export(typeof(XmlRayElementParser))]
    class ProjectionLightParser : XmlRayElementParser
    {
        public override string LoaderType { get { return "ProjectionLight"; } }

        public override dynamic LoadObject(XmlRaySceneLoader loader, Scene scene, System.Xml.Linq.XElement element, string elementName, Func<dynamic> createDefault)
        {
            var transform = loader.LoadObject(scene, element, "Transform", () => Transform.CreateLookAtTransform(new Point(0, 10, 0), new Point(0, 0, 0), new Vector(0, 0, 1)));

            var colour = loader.LoadObject(scene, element, "Colour", () => new Colour(1));
            var power = loader.LoadObject(scene, element, "Power", () => 1000f);
            var totalWidthInDegrees = loader.LoadObject(scene, element, "Width", () => 45f);
            var texture = loader.LoadObject<string>(scene, element, "Texture", () => { throw new ArgumentOutOfRangeException("Texture is required"); });

            return new ProjectionLight(colour, ImageReader.Read(texture), power, totalWidthInDegrees, transform);
        }
    }
}
