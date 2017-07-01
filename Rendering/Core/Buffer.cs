using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raytracer.Rendering.Core
{
    class Buffer
    {
        public enum Channel 
        {
	        ColorChannel,
	        VarianceChannel,
	        StandardDeviationChannel,
	        SamplesChannel
        }

        struct Pixel
        {
	        private int _samples;
	        private Colour M, V;    

            public void AddSample(Colour sample)
            {
                this.Samples++;

	            if(this.Samples == 1)
                {
                    this.V = Colour.Black;
		            this.M = sample;
		            return;
	            }

	            var m = this.M;

	            this.M = this.M + ((sample - this.M) / (double)this.Samples);
	            this.V = this.V + ((sample - m) * (sample - this.M));
            }

            public int Samples 
            { 
                get
                {
                    return this._samples;
                }
                set
                {
                    this._samples = value;
                }
            }

            public Colour Colour 
            {
                get 
                {
	                return this.M;
                }
            }

            public Colour Variance
            {
                get
                {
	                if(this.Samples < 2)
                    {
		                return Colour.Black;
	                }

	                return this.V / (this.Samples - 1);
                }
            }

            public Colour StandardDeviation
            {
                get
                {
	                return this.Variance.Pow(0.5);
                }
            }
        }


        private int W, H;
        private Pixel [] Pixels;

        public Buffer (int w, int h)
	    {
            Pixels = new Pixel[w*h];
	        W = w;
            H = h;           
        }

        public void AddSample(int x, int y, Colour sample) 
        {
	        this.Pixels[y * this.W + x].AddSample(sample);
        }

        public int Samples(int x, int y)
        {
	        return this.Pixels[y * this.W + x].Samples;
        }

        public Colour Colour(int x, int y)
        {
	        return this.Pixels[y * this.W + x].Colour;
        }

        public Colour Variance(int x, int y)
        {
	        return this.Pixels[y * this.W + x].Variance;
        }

        public Colour StandardDeviation(int x, int y) 
        {
	        return this.Pixels[y * this.W + x].StandardDeviation;
        }

        public void WriteToBmp(Channel channel, IBmp image) 
        {	        
	        float maxSamples = 0;
	        
            if(channel == Channel.SamplesChannel) 
            {
                foreach (var pixel in Pixels)
	            {
                    maxSamples = Math.Max(maxSamples, (float)pixel.Samples);
		        }
	        }

            image.BeginWriting();
            for (int y = 0; y < this.H; y++)
			{
                for (int x = 0; x < this.W; x++)
			    {
			        Colour c;

			        switch(channel) 
                    {
                        case Channel.ColorChannel:
				            c = this.Colour(x, y);
                            if(c != null)
                                c = c.Pow(1.0 / 2.2);
                            break;
			            case Channel.VarianceChannel:
				            c = this.Variance(x, y);
                            break;
                        case Channel.StandardDeviationChannel:
				            c = this.StandardDeviation(x, y);
                            break;
                        case Channel.SamplesChannel:
                            var p = (float)this.Samples(x, y) / maxSamples;
				            c = new Colour(p);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
			        }

                    if (c != null)
                    {
                        c.Clamp();
                        image.SetPixel(x, y, c);
                    }
		        }
	        }
            image.EndWriting();
        }

        public MathTypes.Size Size
        {
            get
            {
                return new MathTypes.Size(this.W, this.H);
            }
        }

        internal void BeginWriting()
        {
        }

        internal void EndWriting()
        {
        }
    }
}
