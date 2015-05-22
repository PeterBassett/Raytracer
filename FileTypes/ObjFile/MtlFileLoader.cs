using System;
using System.Collections.Generic;
using System.IO;
using Raytracer.Rendering.Materials;
using Raytracer.Rendering.Core;

namespace Raytracer.FileTypes.ObjFile
{
    class MtlFileLoader
    {
        internal void LoadFile(string strMaterialFile, List<Material> materials)
        {
            Material currentMaterial = null;
            
            using (var sr = new StreamReader(strMaterialFile))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    
                    if (line == null) 
                        continue;

                    line = line.TrimStart(' ', '\t');

                    var items = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (items.Length == 0)
                        continue;

                    switch (items[0])
                    {
                        case "newmtl":
                            currentMaterial = new Material
                            {
                                Name = items[1]
                            };
                            materials.Add(currentMaterial);
                            break;
                        case "Ka":                            
                            currentMaterial.Ambient = LoadColour(items);
                            break;
                        case "Kd":
                            currentMaterial.Diffuse = LoadColour(items);
                            break;
                        case "Ks":
                            currentMaterial.Specular = LoadColour(items);
                            break;
                        case "Ns":
                            currentMaterial.SpecularExponent = float.Parse(items[1]);
                            break;
                        case "Ni":
                            currentMaterial.Refraction = float.Parse(items[1]);
                            break;
                        case "map_Kd":
                            if (!(currentMaterial is MaterialTexture))
                            {
                                var index = materials.IndexOf(currentMaterial);
                                currentMaterial = new MaterialTexture(currentMaterial);
                                materials[index] = currentMaterial;
                            }

                            ((MaterialTexture)currentMaterial).LoadDiffuseMap(items[1]);
                            break;
                        case "map_Ka":
                            if (!(currentMaterial is MaterialTexture))
                            {
                                var index = materials.IndexOf(currentMaterial);
                                currentMaterial = new MaterialTexture(currentMaterial);
                                materials[index] = currentMaterial;
                            }

                            ((MaterialTexture)currentMaterial).LoadAmbientMap(items[1]);
                            break;
                    }
                }
            }
        }

        private Colour LoadColour(string[] items)
        {
            return new Colour
            {
                Red = float.Parse(items[1]),
                Green = float.Parse(items[2]),
                Blue = float.Parse(items[3]),
            };
        }
    }
}
