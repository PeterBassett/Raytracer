using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raytracer.Rendering.Core
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

        public Colour(Colour col) : this(col.Red, col.Green, col.Blue) 
        {
        }

        public Colour(double fRed, double fGreen, double fBlue)
            : this((float)fRed, (float)fGreen, (float)fBlue)
        {
        }

        public Colour(float fRed, float fGreen, float fBlue)
        {
            m_Red = fRed;
            m_Green = fGreen;
            m_Blue = fBlue;
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

	    /*public static Colour operator * (Colour a, double f) 
	    { 
		    return new Colour(a.Red * f, a.Green * f, a.Blue * f); 
	    }*/

        public static Colour operator /(Colour a, double f)
        {
            return new Colour(a.Red / f, a.Green / f, a.Blue / f);
        }

        public override bool Equals(Object obj)
        {
            Colour colourObj = obj as Colour;
            if (colourObj == null)
                return false;
            else
                return this == colourObj;
        }

        public override int GetHashCode()
        {
            return this.Red.GetHashCode() ^ this.Green.GetHashCode() ^ this.Blue.GetHashCode();
        }

        /// <summary>
        /// Tests whether two specified colours are equal.
        /// </summary>
        public static bool operator == (Colour a, Colour b)
        {
            if (Object.Equals(a, null))
            {
                return Object.Equals(b, null);
            }

            if (Object.Equals(b, null))
            {
                return Object.Equals(a, null);
            }

            float epsilon = 0.00001f;

            return Math.Abs(a.Red - b.Red) < epsilon &&
                    Math.Abs(a.Green - b.Green) < epsilon &&
                    Math.Abs(a.Blue - b.Blue) < epsilon;
        }

        /// <summary>
        /// Tests whether two specified colours are not equal.
        /// </summary>
        public static bool operator !=(Colour a, Colour b)
        {
            return !(a == b);
        }

        internal void Clamp()
        {
            this.Red = Clamp(this.Red);
            this.Green = Clamp(this.Green);
            this.Blue = Clamp(this.Blue);
        }

        internal float Clamp(float component)
        {
            return Math.Min(Math.Max(component, 0.0f), 1.0f);
        }

        public System.Drawing.Color ToColor()
        {
            return System.Drawing.Color.FromArgb((int)(255.0 * this.Red),
                                                 (int)(255.0 * this.Green),
                                                 (int)(255.0 * this.Blue));
        }
        
        public float this[int index]
	    {
		    get 
            {
                switch (index)
                {
                    case 0:
                        return m_Red;
                    case 1:
                        return m_Green;
                    case 2:
                        return m_Blue;
                    default:
                        throw new ArgumentOutOfRangeException("index");
                }
            }
	    }

        public override string ToString()
        {
            return string.Format("R:{0},G:{1},B:{2}", Red, Green, Blue);
        }
    }
}
