using System.Collections.Generic;
using Raytracer.Rendering.Primitives;
using System.IO;
using Raytracer.MathTypes;
using Raytracer.Rendering.Materials;

namespace Raytracer.FileTypes.ObjFile
{   
    class ObjFileLoader
    {
        private readonly Dictionary<string, Material> _materials = new Dictionary<string, Material>();

        private const int VertexCacheInitialLength = 3;
        private int[,] _vertexIndexCache = new int[VertexCacheInitialLength, 3];
        private int _vertexIndexCacheLength = VertexCacheInitialLength;

        private const int ParseCacheInitialLength = 3;
        private string[] _parseCache = new string[ParseCacheInitialLength];
        private int _parseCacheLength = ParseCacheInitialLength;
        
        public void LoadFile(string strObjfile, List<Triangle> triangles, List<Material> materials)
        {
            var verticies = new List<Point>();
            var textureCoordinates = new List<Vector2>();
            var vertexNormals = new List<Normal>();
            Material currentMaterial = null;

            using (var sr = new StreamReader(strObjfile))
            {
                while (!sr.EndOfStream)
                {                    
                    var line = sr.ReadLine();

                    int lineItemsFound = ParseLine(line);

                    if (lineItemsFound == 0)
                        continue;

                    switch (_parseCache[0])
                    {
                        case "mtllib":
                            LoadMaterial(_parseCache[1]);
                            break;
                        case "usemtl":
                            currentMaterial = FindMaterial(_parseCache[1]);
                            break;
                        case "v":
                            verticies.Add(new Point(float.Parse(_parseCache[1]), float.Parse(_parseCache[2]), float.Parse(_parseCache[3])));
                            break;
                        case "vt":
                            textureCoordinates.Add(new Vector2(float.Parse(_parseCache[1]), float.Parse(_parseCache[2])));
                            break;
                        case "vn":
                            vertexNormals.Add(new Normal(float.Parse(_parseCache[1]), float.Parse(_parseCache[2]), float.Parse(_parseCache[3])));
                            break;
                        case "f":
                            {
                                CreateTriangles(lineItemsFound, verticies, triangles, currentMaterial, textureCoordinates, vertexNormals);
                            }
                            break;
                    }
                }
            }

            materials.AddRange(_materials.Values);
        }
        
        private int ParseLine(string line)
        {
            int itemsfound = 0;
            int curPos= 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == ' ')
                {
                    ResizeParseCache(itemsfound);

                    _parseCache[itemsfound] = line.Substring(curPos, i - curPos);
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

                    _parseCache[itemsfound] = line.Substring(curPos);
                    itemsfound++;
                }
            }

            for (int i = itemsfound; i < _parseCache.Length; i++)
            {
                _parseCache[i] = null;
            }

            return itemsfound;
        }

        private void ResizeParseCache(int requiredSize)
        {
            if (requiredSize < _parseCacheLength)
                return;

            var temp = new string[_parseCacheLength * 2];

            for (int i = 0; i < _parseCacheLength; i++)
            {
                temp[i] = _parseCache[i];
            }

            _parseCache = temp;
            _parseCacheLength = temp.Length;
        }

        private void LoadMaterial(string strMaterialFile)
        {
            var materialLoader = new MtlFileLoader();
            var materials = new List<Material>();
            materialLoader.LoadFile(strMaterialFile, materials);

            foreach (var mat in materials)
            {
                _materials.Add(mat.Name, mat);
            }
        }

        private Material FindMaterial(string strName)
        {
            if (_materials.ContainsKey(strName))
                return _materials[strName];
            return null;
        }

        private void CreateTriangles(int itemsInParseCache, List<Point> verticies, List<Triangle> triangles, Material currentMaterial, List<Vector2> textureCoordinates, List<Normal> vertexNormals)
        {
            if (_vertexIndexCacheLength <= itemsInParseCache)
            {
                _vertexIndexCache = new int[_vertexIndexCacheLength * 2, 3];
                _vertexIndexCacheLength = _vertexIndexCacheLength * 2;
            }

            int index = 0;
            for (int i = 1; i < itemsInParseCache; i++)
            {
                int vertex;
                int? textureCoordinateParsed, normalParsed;

                ParseVertex(_parseCache[i], out vertex, out textureCoordinateParsed, out normalParsed);

                vertex = vertex < 0 ? verticies.Count + vertex : vertex - 1; 
                var texture = textureCoordinateParsed.HasValue? (textureCoordinateParsed.Value < 0 ? textureCoordinates.Count + textureCoordinateParsed.Value : textureCoordinateParsed.Value - 1) : -1;
                var normal = normalParsed.HasValue? (normalParsed.Value < 0 ? vertexNormals.Count + normalParsed.Value : normalParsed.Value - 1) : -1;

                _vertexIndexCache[index, 0] = vertex;
                _vertexIndexCache[index, 1] = texture;
                _vertexIndexCache[index, 2] = normal; 

                index++;
            }

            Vector2 t1 = Vector2.Zero;
            Normal n1 = Normal.Invalid;

            const int vertices = 0;
            const int textures = 1;
            const int normals = 2;

            Point v1 = verticies[_vertexIndexCache[0, vertices]];

            if (_vertexIndexCache[0, textures] != -1)
                t1 = textureCoordinates[_vertexIndexCache[0, textures]];

            if (_vertexIndexCache[0, normals] != -1)
                n1 = vertexNormals[_vertexIndexCache[0, normals]];

            for (int i = 0; i < index - 2; ++i)
            {
                var v2 = verticies[_vertexIndexCache[i + 1, vertices]];
                var v3 = verticies[_vertexIndexCache[i + 2, vertices]];

                if (v1 == v2 || v1 == v3 || v2 == v3)
                    continue;

                var tri = new Triangle();
                tri.Vertices[0] = v1;
                tri.Vertices[1] = v2;
                tri.Vertices[2] = v3;

                if (_vertexIndexCache[i + 1, textures] != -1 && _vertexIndexCache[i + 2, textures] != -1)
                {                    
                    tri.TextureUVs[0] = t1;
                    tri.TextureUVs[1] = textureCoordinates[_vertexIndexCache[i + 1, textures]];
                    tri.TextureUVs[2] = textureCoordinates[_vertexIndexCache[i + 2, textures]];
                }
                else
                    tri.TextureUVs = null;

                if (_vertexIndexCache[i + 1, normals] != -1 && _vertexIndexCache[i + 2, normals] != -1)
                {                    
                    tri.Normals[0] = n1;
                    tri.Normals[1] = vertexNormals[_vertexIndexCache[i + 1, normals]];
                    tri.Normals[2] = vertexNormals[_vertexIndexCache[i + 2, normals]];
                }
                else
                    tri.Normals = null;

                tri.Pos = (v1 + v2 + v3) / 3.0;

                tri.Material = currentMaterial;
                triangles.Add(tri);
            }
        }

        private void ParseVertex(string v, out int vertex, out int? texture, out int? normal)
        {
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
