using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.Rendering.Primitives;
using Raytracer.Rendering;
using System.IO;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.FileTypes.ObjFile
{
    
    using Raytracer.Rendering.Materials;

    class ObjFileLoader
    {
        Dictionary<string, Material> Materials = new Dictionary<string, Material>();

        const int VertexCacheInitialLength = 3;
        int[,] VertexIndexCache = new int[VertexCacheInitialLength, 3];
        int VertexIndexCacheLength = VertexCacheInitialLength;

        const int ParseCacheInitialLength = 3;    
        string[] ParseCache = new string[ParseCacheInitialLength];
        int ParseCacheLength = ParseCacheInitialLength;
        
        public void LoadFile(string strObjfile, List<Triangle> triangles, List<Material> materials)
        {
            var verticies = new List<Vector3>();
            var textureCoordinates = new List<Vector2>();
            var vertexNormals = new List<Vector3>();
            Material currentMaterial = null;
            //var sr = new BufferedStreamReader(strObjfile);

            using (StreamReader sr = new StreamReader(strObjfile))
            {
                while (!sr.EndOfStream)
                {                    
                    var line = sr.ReadLine();

                    int lineItemsFound = 0;
                    ParseLine(line, ref lineItemsFound);

                    if (lineItemsFound == 0)
                        continue;

                    switch (ParseCache[0])
                    {
                        case "mtllib":
                            LoadMaterial(ParseCache[1]);
                            break;
                        case "usemtl":
                            currentMaterial = FindMaterial(ParseCache[1]);
                            break;
                        case "v":
                            verticies.Add(new Vector3(float.Parse(ParseCache[1]), float.Parse(ParseCache[2]), float.Parse(ParseCache[3])));
                            break;
                        case "vt":
                            textureCoordinates.Add(new Vector2(float.Parse(ParseCache[1]), float.Parse(ParseCache[2])));
                            break;
                        case "vn":
                            vertexNormals.Add(new Vector3(float.Parse(ParseCache[1]), float.Parse(ParseCache[2]), float.Parse(ParseCache[3])));
                            break;
                        case "f":
                            {
                                CreateTriangles(lineItemsFound, verticies, triangles, currentMaterial, textureCoordinates, vertexNormals);
                            }
                            break;
                    }
                }
            }

            materials.AddRange(Materials.Values);
        }
        
        private void ParseLine(string line, ref int ValidItemsInParseCache)
        {
            int itemsfound = 0;
            int curPos= 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == ' ')
                {
                    ResizeParseCache(itemsfound);

                    ParseCache[itemsfound] = line.Substring(curPos, i - curPos);
                    itemsfound++;

                    curPos = i + 1;

                    while (i+1 < line.Length - 1 && line[i+1] == ' ')
                    {
                        i++;
                        curPos = i + 1;
                    }                                        
                }
                else if(i == line.Length - 1)
                {
                    ResizeParseCache(itemsfound);

                    ParseCache[itemsfound] = line.Substring(curPos);
                    itemsfound++;
                }
            }

            for (int i = itemsfound; i < ParseCache.Length; i++)
            {
                ParseCache[i] = null;
            }

            ValidItemsInParseCache = itemsfound;
        }

        private void ResizeParseCache(int requiredSize)
        {
            if (requiredSize < ParseCacheLength)
                return;

            var temp = new string[ParseCacheLength * 2];

            for (int i = 0; i < ParseCacheLength; i++)
            {
                temp[i] = ParseCache[i];
            }

            ParseCache = temp;
            ParseCacheLength = temp.Length;
        }

        private void LoadMaterial(string strMaterialFile)
        {
            MtlFileLoader materialLoader = new MtlFileLoader();
            List<Material> materials = new List<Material>();
            materialLoader.LoadFile(strMaterialFile, materials);

            foreach (var mat in materials)
            {
                Materials.Add(mat.Name, mat);
            }
        }

        private Material FindMaterial(string strName)
        {
            if (Materials.ContainsKey(strName))
                return Materials[strName];
            else
                return null;
        }
                
        private void CreateTriangles(int itemsInParseCache, List<Vector3> verticies, List<Triangle> triangles, Material currentMaterial, List<Vector2> textureCoordinates, List<Vector3> vertexNormals)
        {
            if (VertexIndexCacheLength <= itemsInParseCache)
            {
                VertexIndexCache = new int[VertexIndexCacheLength * 2, 3];
                VertexIndexCacheLength = VertexIndexCacheLength * 2;
            }

            int index = 0;
            for (int i = 1; i < itemsInParseCache; i++)
            {
                int vertex;
                int? textureCoordinateParsed, normalParsed;

                ParseVertex(ParseCache[i], out vertex, out textureCoordinateParsed, out normalParsed);

                vertex = vertex < 0 ? verticies.Count + vertex : vertex - 1; 
                var texture = textureCoordinateParsed.HasValue? (textureCoordinateParsed.Value < 0 ? textureCoordinates.Count + textureCoordinateParsed.Value : textureCoordinateParsed.Value - 1) : -1;
                var normal = normalParsed.HasValue? (normalParsed.Value < 0 ? vertexNormals.Count + normalParsed.Value : normalParsed.Value - 1) : -1;

                VertexIndexCache[index, 0] = vertex;
                VertexIndexCache[index, 1] = texture;
                VertexIndexCache[index, 2] = normal; 

                index++;
            }

            Vector3 v1, v2, v3;
            Vector2 t1 = Vector2.Zero, t2 = Vector2.Zero, t3 = Vector2.Zero;
            Vector3 n1 = Vector3.Zero, n2 = Vector3.Zero, n3 = Vector3.Zero;

            const int Vertex = 0;
            const int Texture = 1;
            const int Normal = 2;

            v1 = verticies[VertexIndexCache[0, Vertex]];

            if (VertexIndexCache[0, Texture] != -1)
                t1 = textureCoordinates[VertexIndexCache[0, Texture]];

            if (VertexIndexCache[0, Normal] != -1)
                n1 = vertexNormals[VertexIndexCache[0, Normal]];

            for (int i = 0; i < index - 2; ++i)
            {
                v2 = verticies[VertexIndexCache[i + 1, Vertex]];
                v3 = verticies[VertexIndexCache[i + 2, Vertex]];

                if (v1 == v2 || v1 == v3 || v2 == v3)
                    continue;

                Triangle tri = new Triangle();
                tri.Vertex[0] = v1;
                tri.Vertex[1] = v2;
                tri.Vertex[2] = v3;

                if (VertexIndexCache[i + 1, Texture] != -1 && VertexIndexCache[i + 2, Texture] != -1)
                {
                    t2 = textureCoordinates[VertexIndexCache[i + 1, Texture]];
                    t3 = textureCoordinates[VertexIndexCache[i + 2, Texture]];

                    tri.Texture[0] = t1;
                    tri.Texture[1] = t2;
                    tri.Texture[2] = t3;
                }
                else
                    tri.Texture = null;

                if (VertexIndexCache[i + 1, Normal] != -1 && VertexIndexCache[i + 2, Normal] != -1)
                {
                    n2 = vertexNormals[VertexIndexCache[i + 1, Normal]];
                    n3 = vertexNormals[VertexIndexCache[i + 2, Normal]];

                    tri.Normal[0] = n1;
                    tri.Normal[1] = n2;
                    tri.Normal[2] = n3;
                }
                else
                    tri.Normal = null;

                tri.Pos = (v1 + v2 + v3) / 3.0;

                tri.Material = currentMaterial;
                triangles.Add(tri);
            }
        }

        private int GetTextureCoordinate(string p)
        {
            throw new NotImplementedException();
        }
        
        private int GetNormal(string v)
        {
            return 0;//throw new NotImplementedException();
        }

        private void ParseVertex(string v, out int vertex, out int? texture, out int? normal)
        {
            vertex = 0;
            texture = null;
            normal = null;

            var items = v.Split('/');

            vertex = int.Parse(items[0]);

            int t;
            if (items.Length >= 2 && int.TryParse(items[1], out t))            
                texture = t;

            int n;
            if (items.Length >= 3 && int.TryParse(items[2], out n))
                normal = n;
        }

        /*
        private int GetVertex(string v)
        {
            var end = 0;
            for (int i = 0; i < v.Length; i++)
            {                
                if (v[i] == '/')
                    break;

                end = i + 1;
            }

            var index = int.Parse(v.Substring(0, end)) - 1;

            if (index < 0)
                return index + 1;
            else
                return index;
        }*/
    }
}
