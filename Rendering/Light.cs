using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering.FileTypes;
using Raytracer.MathTypes;

namespace Raytracer.Rendering
{
    using Vector = Vector3D;
    class Light
    {
        public Light()
        {
            Ambient = new Colour();
            Diffuse = new Colour();
            Pos = new Vector(0.0f, 0.0f, 0.0f);
        }

        private float [] m_fFalloff = new float[2];
        private float m_fBrightness;
        public Colour Ambient { get; set; }
        public Colour Diffuse { get; set; }
        public Vector Pos { get; set; }

        float Brightness(float fLen) 
        { 
            return m_fFalloff[0] + 1 - fLen / m_fFalloff[1]; 
        }

	    void SetFalloff(float f1, float f2) 
        { 
            m_fFalloff[0] = f1; 
            m_fFalloff[1] = f2; 
        }        
    }
}
