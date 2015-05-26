using System;

namespace Raytracer.Rendering.Core
{
    class Colour
    {
        float _red;
        float _green;
        float _blue;
        
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

        public Colour(double red, double green, double blue)
            : this((float)red, (float)green, (float)blue)
        {
        }

        public Colour(float red, float green, float blue)
        {
            _red = red;
            _green = green;
            _blue = blue;
        }        

	    public float Sum() 
        { 
            return Red + Green + Blue; 
        }       

        public float Red 
        {
            get
            {
                return _red;
            }
            set
            {
                _red = value;
            }
        }
        public float Green
        {
            get
            {
                return _green;
            }
            set
            {
                _green = value;
            }
        }
        public float Blue
        {
            get
            {
                return _blue;
            }
            set
            {
                _blue = value;
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
            var colourObj = obj as Colour;
            if (colourObj == null)
                return false;
            
            return this == colourObj;
        }

        public override int GetHashCode()
        {
            return Red.GetHashCode() ^ Green.GetHashCode() ^ Blue.GetHashCode();
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

            const float epsilon = 0.00001f;

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
            Red = Clamp(Red);
            Green = Clamp(Green);
            Blue = Clamp(Blue);
        }

        private float Clamp(float component)
        {
            return Math.Min(Math.Max(component, 0.0f), 1.0f);
        }

        public System.Drawing.Color ToColor()
        {
            return System.Drawing.Color.FromArgb((int)(255.0 * Red),
                                                 (int)(255.0 * Green),
                                                 (int)(255.0 * Blue));
        }
        
        public float this[int index]
	    {
		    get 
            {
                switch (index)
                {
                    case 0:
                        return _red;
                    case 1:
                        return _green;
                    case 2:
                        return _blue;
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
