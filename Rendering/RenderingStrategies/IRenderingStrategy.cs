using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering.Renderers;
using Raytracer.Rendering.Core;

namespace Raytracer.Rendering.RenderingStrategies
{
    interface IRenderingStrategy
    {
        void RenderScene(IRenderer renderer, IBmp frameBuffer);
    }
}
