using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.Materials
{
    using Vector = Vector3D;

    class MaterialCheckerboard : MaterialWithSubMaterials
    {        
        public MaterialCheckerboard ()
	    {
            Size = new Vector(1.0f, 1.0f, 1.0f);
        	SubMaterial1 = null;
	        SubMaterial2 = null;
	    }

        public override void SolidifyMaterial(IntersectionInfo info, Material output)
        {        
	        Material mat;
	        int x = (int)(info.HitPoint.X / Size.X);
	        int y = (int)(info.HitPoint.Y / Size.Y);
	        int z = (int)(info.HitPoint.Z / Size.Z);

	        if (info.HitPoint.X < 0.0f)
		        x -= 1;
	        if (info.HitPoint.Y < 0.0f)
		        y -= 1;
	        if (info.HitPoint.Z < 0.0f)
		        z -= 1;

	        if ( ((x+y+z) & 1) == 1)
	            mat = SubMaterial1;
	        else
	            mat = SubMaterial2;

            mat.SolidifyMaterial(info, output);
            
	        //CloneElements(mat);
        }
    }
}
