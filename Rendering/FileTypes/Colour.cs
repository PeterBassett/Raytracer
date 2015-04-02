using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raytracer.Rendering.FileTypes
{
    class Colour
    {
        float m_Red;
        float m_Green;
        float m_Blue;
        
	    public Colour() : this(0.0f, 0.0f, 0.0f)
        { 
        } 

        public Colour(float fCol)
            : this(fCol, fCol, fCol)
        { 
        }

        public Colour(float fRed, float fGreen, float fBlue) 
        { 
            m_Red = fRed;
            m_Green = fGreen;
            m_Blue = fBlue; 
        }

        public Colour(double fRed, double fGreen, double fBlue)
        {
            m_Red = (float)fRed;
            m_Green = (float)fGreen;
            m_Blue = (float)fBlue;
        }

        public Colour(Colour col)
	    {
            m_Red = col.Red;
            m_Green = col.Green;
            m_Blue = col.Blue;
	    }

	    public float Sum() 
        { 
            return Red + Green + Blue; 
        }
        
        public void Set(float fCol) 
        { 
            Red = fCol; 
            Green = fCol; 
            Blue = fCol; 
        }

        public float Red 
        {
            get
            {
                return m_Red;
            }
            set
            {
                m_Red = value;
            }
        }
        public float Green
        {
            get
            {
                return m_Green;
            }
            set
            {
                m_Green = value;
            }
        }
        public float Blue
        {
            get
            {
                return m_Blue;
            }
            set
            {
                m_Blue = value;
            }
        }

        public float Brightness 
        {
            get 
            {
                return Red + Green + Blue;
            }    
        }
               
        public static Colour operator +(Colour a, Colour b)
        {
            return new Colour(a.Red + b.Red,
                              a.Green + b.Green,
                              a.Blue + b.Blue);
        }

        public static Colour operator -(Colour a, Colour b)
        {
            return new Colour(a.Red - b.Red,
                              a.Green - b.Green,
                              a.Blue - b.Blue);
        }
        
	    public static Colour operator * (Colour a, Colour b) 
	    { 
            return new Colour(a.Red * b.Red, a.Green * b.Green, a.Blue * b.Blue); 		
	    }

        public static Colour operator *(Colour a, double f)
        {
            return new Colour(a.Red * f, a.Green * f, a.Blue * f);
        }

        public static Colour operator *(double f, Colour a)
        {
            return new Colour(a.Red * f, a.Green * f, a.Blue * f);
        }

	    public static Colour operator * (Colour a, float f) 
	    { 
		    return new Colour(a.Red * f, a.Green * f, a.Blue * f); 
	    }

        public static Colour operator /(Colour a, float f)
        {
            return new Colour(a.Red / f, a.Green / f, a.Blue / f);
        }

        internal void Clamp()
        {
            this.Red = Math.Min(this.Red, 1.0f);
            this.Green = Math.Min(this.Green, 1.0f);
            this.Blue = Math.Min(this.Blue, 1.0f);
        }

        public System.Drawing.Color ToColor()
        {
            return System.Drawing.Color.FromArgb((int)(255.0 * this.Red),
                                                 (int)(255.0 * this.Green),
                                                 (int)(255.0 * this.Blue));
        }
    }
}
