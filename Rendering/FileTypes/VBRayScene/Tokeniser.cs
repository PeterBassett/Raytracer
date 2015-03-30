using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Raytracer.Rendering.FileTypes.VBRayScene
{
    class Tokeniser
    {
        public string GetToken(StreamReader file)
        {            
	        char cChar = 'A';
	        string strToken = "";
	        // ignore the whitespace
	        while(cChar != ' ')
	        {
		        // look at the next char
                cChar = (char)file.Read();
	
		        if(file.EndOfStream)
			        return strToken;

                if (IsWhiteSpace(cChar))
                {
                    cChar = EatWhiteSpace(file);
                }

                if (file.EndOfStream)
                    return strToken;

		        switch(cChar)
		        {
		            case '\'' :
			            {
				            EatComment(file);
				            break;
			            }
		            case '\"' :
			            {
				            strToken = EatStringConstant(file);
				            break;
			            }
		            case '(' :
		            case ')' :
		            case ',' :
			            {
				            if(strToken != "")
					            return strToken;

				            break;
			            }
		            default :
			            {
				            // read a character
				            strToken += cChar;
                            break;
			            }
		        };
	        }

	        return strToken;
        }

        private char EatWhiteSpace(StreamReader file)
        {
            char cChar = ' ';

            while (IsWhiteSpace(cChar) && !file.EndOfStream)
            {
                cChar = (char)file.Read();
            }
            return cChar;
        }

        bool IsWhiteSpace(char cChar)
        {
            return cChar == ' ' ||
                    cChar == '\t' ||
                    cChar == '\r' ||
                    cChar == '\n';
        }

        void EatComment(StreamReader file)
        {
	        char cChar = ' ';

            while (cChar != '\'' && !file.EndOfStream)
	        {
		        cChar = (char)file.Read();
	        }
        }

        string EatStringConstant(StreamReader file)
        {
            char cChar = ' ';
            string strToken = "";

	        while(cChar != '\"')
	        {
		        cChar = (char)file.Read();
		        strToken += cChar;
	        }

            if(strToken.Length > 0)
	            strToken = strToken.Substring(0, strToken.Length - 1);

	        return strToken;
        }
    }
}
