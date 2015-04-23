using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.Primitives
{
    // # class CSG

    // Holds a binary space partition tree representing a 3D solid. Two solids can
    // be combined using the `union()`, `subtract()`, and `intersect()` methods.
    class CSG
    {
        // Constructive Solid Geometry (CSG) is a modeling technique that uses Boolean
        // operations like union and intersection to combine 3D solids. This library
        // implements CSG operations on meshes elegantly and concisely using BSP trees,
        // and is meant to serve as an easily understandable implementation of the
        // algorithm. All edge cases involving overlapping coplanar polygons in both
        // solids are correctly handled.
        // 
        // Example usage:
        // 
        //     var cube = CSG.cube();
        //     var sphere = CSG.sphere({ radius: 1.3 });
        //     var polygons = cube.subtract(sphere).toPolygons();
        // 
        // ## Implementation Details
        // 
        // All CSG operations are implemented in terms of two functions, `clipTo()` and
        // `invert()`, which remove parts of a BSP tree inside another BSP tree and swap
        // solid and empty space, respectively. To find the union of `a` and `b`, we
        // want to remove everything in `a` inside `b` and everything in `b` inside `a`,
        // then combine polygons from `a` and `b` into one solid:
        // 
        //     a.root.clipTo(b.root);
        //     b.root.clipTo(a.root);
        //     a.root.build(b.root.allPolygons());
        // 
        // The only tricky part is handling overlapping coplanar polygons in both trees.
        // The code above keeps both copies, but we need to keep them in one tree and
        // remove them in the other tree. To remove them from `b` we can clip the
        // inverse of `b` against `a`. The code for union now looks like this:
        // 
        //     a.root.clipTo(b.root);
        //     b.root.clipTo(a.root);
        //     b.root.invert();
        //     b.root.clipTo(a.root);
        //     b.root.invert();
        //     a.root.build(b.root.allPolygons());
        // 
        // Subtraction and intersection naturally follow from set operations. If
        // union is `A | B`, subtraction is `A - B = ~(~A | B)` and intersection is
        // `A & B = ~(~A | ~B)` where `~` is the complement operator.
        // 
        // ## License
        // 
        // Copyright (c) 2011 Evan Wallace (http://madebyevan.com/), under the MIT license.


        // # class Polygon

        // Represents a convex polygon. The vertices used to initialize a polygon must
        // be coplanar and form a convex loop. They do not have to be `CSG.Vertex`
        // instances but they must behave similarly (duck typing can be used for
        // customization).
        // 
        // Each convex polygon has a `shared` property, which is shared between all
        // polygons that are clones of each other or were split from the same polygon.
        // This can be used to define per-polygon properties (such as surface color).

        public class Polygon 
        {            
            public Plane plane = null;
            public List<Vertex> vertices = null;
            public Polygon(List<Vertex> vertices) 
            {
                this.vertices = vertices;
                //this.shared = shared;
                this.plane = Plane.fromPoints(vertices[0].pos, vertices[1].pos, vertices[2].pos);
            }
        
            public Polygon clone() 
            {
                var vertices = (from v in this.vertices 
                               select v.clone()).ToList();

                return new Polygon(vertices);
            }

            public void flip() 
            {
                this.vertices.Reverse();
                this.vertices.ForEach( (v) => v.flip() );
                this.plane.flip();
            }
        }

        // # class Vertex
        // Represents a vertex of a polygon. Use your own vertex class instead of this
        // one to provide additional features like texture coordinates and vertex
        // colors. Custom vertex classes need to provide a `pos` property and `clone()`,
        // `flip()`, and `interpolate()` methods that behave analogous to the ones
        // defined by `CSG.Vertex`. This class provides `normal` so convenience
        // functions like `CSG.sphere()` can return a smooth vertex normal, but `normal`
        // is not used anywhere else.
        public struct Vertex
        {
            public Vector pos;
            public Vector normal;

            public Vertex(Vector pos, Vector normal) 
            {
                this.pos = pos;
                this.normal = normal;
            }
        
            public Vertex clone() {
                return new Vertex(this.pos, this.normal);
            }

            // Invert all orientation-specific data (e.g. vertex normal). Called when the
            // orientation of a polygon is flipped.
            public void flip() {
                this.normal = this.normal.Negated();
            }

            // Create a new vertex between this vertex and `other` by linearly
            // interpolating all properties using a parameter of `t`. Subclasses should
            // override this to interpolate additional properties.
            public Vertex interpolate(Vertex other, double t)
            {
                return new CSG.Vertex(
                    this.pos.Lerp(other.pos, t),
                    this.normal.Lerp(other.normal, t)
                );
            }
        }

        public class Plane
        {
            Vector normal;
            double w;

            // Represents a plane in 3D space.
            public Plane(Vector normal, double w)
            {
              this.normal = normal;
              this.w = w;
            }

            // `CSG.Plane.EPSILON` is the tolerance used by `splitPolygon()` to decide if a
            // point is on the plane.
            public static double EPSILON = (double)1e-5;

            public static Plane fromPoints(Vector a, Vector b, Vector c) 
            {
              var n = Vector.CrossProduct(b - a, c - a);
              n.Normalize();
              return new Plane(n, Vector.DotProduct(n, a));
            }

        
            public Plane clone() 
            {
                return new Plane(this.normal, this.w);
            }

            public void flip() {
                this.normal = this.normal.Negated();
                this.w = -this.w;
            }

          // Split `polygon` by this plane if needed, then put the polygon or polygon
          // fragments in the appropriate lists. Coplanar polygons go into either
          // `coplanarFront` or `coplanarBack` depending on their orientation with
          // respect to this plane. Polygons in front or in back of this plane go into
          // either `front` or `back`.
          public void splitPolygon(Polygon polygon, List<Polygon> coplanarFront, List<Polygon> coplanarBack, List<Polygon> front, List<Polygon> back) 
          {
            const int COPLANAR = 0;
            const int FRONT = 1;
            const int BACK = 2;
            const int SPANNING = 3;

            // Classify each point as well as the entire polygon into one of the above
            // four classes.
            var polygonType = 0;
            var types = new int[polygon.vertices.Count];

            for (var i = 0; i < polygon.vertices.Count; i++) 
            {
                var t = Vector.DotProduct(this.normal, polygon.vertices[i].pos) - this.w;
                var type = (t < -Plane.EPSILON) ? BACK : (t > Plane.EPSILON) ? FRONT : COPLANAR;
                polygonType |= type;
                types[i] = type;                
            }

            // Put the polygon in the correct list, splitting it when necessary.
            switch (polygonType) 
            {
              case COPLANAR:
                (Vector.DotProduct(this.normal, polygon.plane.normal) > 0 ? coplanarFront : coplanarBack).Add(polygon);
                break;
              case FRONT:
                front.Add(polygon);
                break;
              case BACK:
                back.Add(polygon);
                break;
              case SPANNING:
                var f = new List<Vertex>();
                var b = new List<Vertex>();

                for (var i = 0; i < polygon.vertices.Count; i++) 
                {
                    var j = (i + 1) % polygon.vertices.Count;
                    var ti = types[i];
                    var tj = types[j];
                    var vi = polygon.vertices[i];
                    var vj = polygon.vertices[j];

                    if (ti != BACK) 
                        f.Add(vi);

                    if (ti != FRONT) 
                        b.Add(ti != BACK ? vi.clone() : vi);

                    if ((ti | tj) == SPANNING) 
                    {                        
                        var t = (this.w - Vector.DotProduct(this.normal, vi.pos)) / Vector.DotProduct(this.normal, vj.pos - vi.pos);
                        var v = vi.interpolate(vj, t);
                        f.Add(v);
                        b.Add(v.clone());
                    }
                }
                if (f.Count >= 3) 
                    front.Add(new CSG.Polygon(f)); //, polygon.shared));
                if (b.Count >= 3)
                    back.Add(new CSG.Polygon(b)); //, polygon.shared));
                break;
            }
          }
        }
        
        
        // # class Node

        // Holds a node in a BSP tree. A BSP tree is built from a collection of polygons
        // by picking a polygon to split along. That polygon (and all other coplanar
        // polygons) are added directly to that node and the other polygons are added to
        // the front and/or back subtrees. This is not a leafy BSP tree since there is
        // no distinction between internal and leaf nodes.
        private class Node
        {
            Plane plane = null;
            Node front = null;
            Node back = null;
            List<Polygon> polygons = new List<Polygon>();

            public Node ()
	        {

	        }            

            public Node clone() 
            {
                var node = new CSG.Node();
                node.plane = this.plane != null ? this.plane.clone() : null;
                node.front = this.front != null ? this.front.clone() : null;
                node.back = this.back != null ? this.back.clone() : null;
                node.polygons = this.polygons.Select((p) => p.clone()).ToList();

                return node;
            }

          // Convert solid space to empty space and empty space to solid space.
            public void invert() 
            {
                for (var i = 0; i < this.polygons.Count; i++) 
                {
                    this.polygons[i].flip();
                }
                this.plane.flip();

                if (this.front != null) 
                    this.front.invert();

                if (this.back != null) 
                    this.back.invert();

                var temp = this.front;
                this.front = this.back;
                this.back = temp;
            }

            // Recursively remove all polygons in `polygons` that are inside this BSP
            // tree.
            public List<Polygon> clipPolygons(List<Polygon> polygons) 
            {
                var front = new List<Polygon>();
                var back = new List<Polygon>();

                for (var i = 0; i < polygons.Count; i++) 
                {
                    this.plane.splitPolygon(polygons[i], front, back, front, back);
                }

                if (this.front != null)
                    if (front.Count > 0)
                        front = this.front.clipPolygons(front);

                if (this.back != null)
                {
                    if (back.Count > 0)
                        back = this.back.clipPolygons(back);
                }
                else
                    back = new List<Polygon>();

                return front.Union(back).ToList();
            }

              // Remove all polygons in this BSP tree that are inside the other BSP tree
              // `bsp`.
            public void clipTo(Node bsp)
            {
                this.polygons = bsp.clipPolygons(this.polygons);

                if (this.front != null) 
                    this.front.clipTo(bsp);

                if (this.back != null) 
                    this.back.clipTo(bsp);
            }

            // Return a list of all polygons in this BSP tree.
            public List<Polygon> allPolygons() 
            {
                IEnumerable<Polygon> polygons = new List<Polygon>(this.polygons);

                if (this.front != null) 
                    polygons = polygons.Union(this.front.allPolygons());
                if (this.back != null) 
                    polygons = polygons.Union(this.back.allPolygons());

                return polygons.ToList();
            }

            // Build a BSP tree out of `polygons`. When called on an existing tree, the
            // new polygons are filtered down to the bottom of the tree and become new
            // nodes there. Each set of polygons is partitioned using the first polygon
            // (no heuristic is used to pick a good split).
            public void build(List<Polygon> polygons) 
            {
                if (polygons.Count == 0) 
                    return;

                if (this.plane == null) 
                    this.plane = polygons[0].plane.clone();

                List<Polygon> front = new List<Polygon>();
                List<Polygon> back = new List<Polygon>();
                
                for (var i = 0; i < polygons.Count; i++) 
                {
                    this.plane.splitPolygon(polygons[i], this.polygons, this.polygons, front, back);
                }

                if (front.Count > 0) 
                {
                    if (this.front == null) 
                        this.front = new Node();
                    
                    this.front.build(front);
                }

                if (back.Count > 0) 
                {
                    if (this.back == null) 
                        this.back = new CSG.Node();

                    this.back.build(back);
                }
            }
        }

        private Node root = null;

        public CSG() 
        {
            this.root = new Node();
        }

        // Construct a CSG solid from a list of `CSG.Polygon` instances.
        public static CSG fromPolygons(List<Polygon> polygons) 
        {
            var bsp = new CSG();
            bsp.root.build(polygons);
            return bsp;
        }

        public CSG clone() 
        {
            var bsp = new CSG();
            bsp.root = this.root.clone();
            return bsp;
        }

        public List<Polygon> toPolygons() 
        {
            return this.root.allPolygons();
        }

        // Return a new CSG solid representing space in either this solid or in the
        // solid `bsp`. Neither this solid nor the solid `bsp` are modified.
        // 
        //     A.union(B)
        // 
        //     +-------+            +-------+
        //     |       |            |       |
        //     |   A   |            |       |
        //     |    +--+----+   =   |       +----+
        //     +----+--+    |       +----+       |
        //          |   B   |            |       |
        //          |       |            |       |
        //          +-------+            +-------+
        // 
        public CSG union(CSG bsp) 
        {
            var a = this.clone();
            var b = bsp.clone();

            a.root.clipTo(b.root);
            b.root.clipTo(a.root);
            b.root.invert();
            b.root.clipTo(a.root);
            b.root.invert();
            a.root.build(b.root.allPolygons());

            return a;
        }

        // Return a new CSG solid representing space in this solid but not in the
        // solid `bsp`. Neither this solid nor the solid `bsp` are modified.
        // 
        //     A.subtract(B)
        // 
        //     +-------+            +-------+
        //     |       |            |       |
        //     |   A   |            |       |
        //     |    +--+----+   =   |    +--+
        //     +----+--+    |       +----+
        //          |   B   |
        //          |       |
        //          +-------+
        // 
        public CSG subtract(CSG bsp) 
        {
            var a = this.clone();
            var b = bsp.clone();

            a.root.invert();
            a.root.clipTo(b.root);
            b.root.clipTo(a.root);
            b.root.invert();
            b.root.clipTo(a.root);
            b.root.invert();
            a.root.build(b.root.allPolygons());
            a.root.invert();

            return a;
        }

        // Return a new CSG solid representing space both this solid and in the
        // solid `bsp`. Neither this solid nor the solid `bsp` are modified.
        // 
        //     A.intersect(B)
        // 
        //     +-------+
        //     |       |
        //     |   A   |
        //     |    +--+----+   =   +--+
        //     +----+--+    |       +--+
        //          |   B   |
        //          |       |
        //          +-------+
        // 
        public CSG intersect(CSG bsp) 
        {
            var a = this.clone();
            var b = bsp.clone();

            a.root.invert();
            b.root.clipTo(a.root);
            b.root.invert();
            a.root.clipTo(b.root);
            b.root.clipTo(a.root);
            a.root.build(b.root.allPolygons());
            a.root.invert();

            return a;
        }

        // Return a new CSG solid with solid and empty space switched. This solid is
        // not modified.
        public CSG inverse() 
        {
            var bsp = this.clone();
            bsp.root.invert();
            return bsp;
        }        

        // Construct an axis-aligned solid cube. Optional parameters are `center` and
        // `radius`, which default to `[0, 0, 0]` and `1`.
        // 
        // Example code:
        // 
        //     var cube = CSG.cube({
        //       center: [0, 0, 0],
        //       radius: 1
        //     });
        public static CSG cube()
        {
            return cube(new Vector(), 1);
        }
        public static CSG cube(Vector center, double radius) 
        {          
          var c = new Vector(center);
          var r = radius;  

          return fromPolygons(
                new int [][][] {          
                    new int [][] { new int [] {0, 4, 6, 2}, new int [] {-1, 0, 0}},
                    new int [][] { new int [] {1, 3, 7, 5}, new int [] {+1, 0, 0}},
                    new int [][] { new int [] {0, 1, 5, 4}, new int [] {0, -1, 0}},
                    new int [][] { new int [] {2, 6, 7, 3}, new int [] {0, +1, 0}},
                    new int [][] { new int [] {0, 2, 3, 1}, new int [] {0, 0, -1}},
                    new int [][] { new int [] {4, 5, 7, 6}, new int [] {0, 0, +1}}
                }.Select((info) => 
                {
                    return new Polygon(info[0].Select((i) => {
                      var pos = new Vector(
                        c.X + r * (2 * ((i & 1) > 0 ? 1 : 0) - 1),
                        c.Y + r * (2 * ((i & 2) > 0 ? 1 : 0) - 1),
                        c.Z + r * (2 * ((i & 4) > 0 ? 1 : 0) - 1)
                      );

                      var verts = info[1].Select(ind => (double)ind).ToArray();

                      return new CSG.Vertex(pos, new Vector(verts[0], verts[1], verts[2]));
                    }).ToList());
                }).ToList()
            );

        }

        // Construct a solid sphere. Optional parameters are `center`, `radius`,
        // `slices`, and `stacks`, which default to `[0, 0, 0]`, `1`, `16`, and `8`.
        // The `slices` and `stacks` parameters control the tessellation along the
        // longitude and latitude directions.
        // 
        // Example usage:
        // 
        //     var sphere = CSG.sphere({
        //       center: [0, 0, 0],
        //       radius: 1,
        //       slices: 16,
        //       stacks: 8
        //     });
        public static CSG sphere() 
        {
            return sphere(new Vector(), 1);
        }

        public static CSG sphere(Vector center, double radius)
        {
            return sphere(center, radius, 16, 8);
        }

        public static CSG sphere(Vector center, double radius, int slices, int stacks) 
        {          
            var c = new Vector(center);
            var r = radius;

            double dSlices = slices;
            double dStacks = stacks;

            var polygons = new List<Polygon>();
            
            var vertex = new Func<double, double, Vertex>( (theta, phi) =>
            {
                theta *= Math.PI * 2;
                phi *= Math.PI;

                var dir = new Vector(
                    Math.Cos(theta) * Math.Sin(phi),
                    Math.Cos(phi),
                    Math.Sin(theta) * Math.Sin(phi)
                );

                return new Vertex(c + (dir * r), dir);
            });

            for (var i = 0; i < dSlices; i++) 
            {
                for (var j = 0; j < dStacks; j++) 
                {
                    var vertices = new List<Vertex>();
                    vertices.Add(vertex(i / dSlices, j / dStacks));

                    if (j > 0) 
                        vertices.Add(vertex((i + 1) / dSlices, j / dStacks));

                    if (j < dStacks - 1) 
                        vertices.Add(vertex((i + 1) / dSlices, (j + 1) / dStacks));
                    
                    vertices.Add(vertex(i / dSlices, (j + 1) / dStacks));

                    polygons.Add(new Polygon(vertices));
                }
            }
            
            return fromPolygons(polygons);
        }

        // Construct a solid cylinder. Optional parameters are `start`, `end`,
        // `radius`, and `slices`, which default to `[0, -1, 0]`, `[0, 1, 0]`, `1`, and
        // `16`. The `slices` parameter controls the tessellation.
        // 
        // Example usage:
        // 
        //     var cylinder = CSG.cylinder({
        //       start: [0, -1, 0],
        //       end: [0, 1, 0],
        //       radius: 1,
        //       slices: 16
        //     });
        public static CSG cylinder()
        {
            return cylinder(new Vector(0, -1, 0), new Vector(0, 1, 0), 1);
        }

        public static CSG cylinder(Vector s, Vector e, double radius)
        {
            return cylinder(s, e, radius, 16);
        }

        public static CSG cylinder(Vector s, Vector e, double radius, int slices)
        {    
            var ray = e - s;
            var r = radius;
            double dSlices = slices;

            var axisZ = ray;
            axisZ.Normalize();
            
            var isY = (Math.Abs(axisZ.Y) > 0.5);

            var axisX = Vector.CrossProduct(new Vector(isY ? 1 : 0, !isY ? 1 : 0, 0), axisZ);
            axisX.Normalize();

            var axisY = Vector.CrossProduct(axisX, axisZ);
            axisY.Normalize();
            
            var start = new Vertex(s, axisZ.Negated());
            var end = new Vertex(e, axisZ);

            var polygons = new List<Polygon>();
            var point = new Func<double, double, double, Vertex>((stack, slice, normalBlend) =>
            {
                var angle = slice * Math.PI * 2;
                //var outVec = axisX.times(Math.Cos(angle)).plus(axisY.times(Math.Sin(angle)));
                var outVec = (axisX * Math.Cos(angle)) + (axisY * Math.Sin(angle));

                //var pos = s.plus(ray.times(stack)).plus(outVec.times(r));
                var pos = s + ((ray * stack) + (outVec * r));

                //var normal = outVec.times(1 - Math.Abs(normalBlend)).plus(axisZ.times(normalBlend));
                var normal = (outVec * (1 - Math.Abs(normalBlend))) + (axisZ * normalBlend);
                return new Vertex(pos, normal);
            });

            for (var i = 0; i < slices; i++)
            {
                double t0 = i / (double)slices;
                double t1 = (i + 1) / (double)slices;

                polygons.Add(new Polygon(new List<Vertex> { start, point(0, t0, -1), point(0, t1, -1) } ));
                
                /*polygons.Add(new Polygon(new List<Vertex> { point(0, t1, 0), point(0, t0, 0), point(2, t0, 0), point(2, t1, 0) }));                
                polygons.Add(new Polygon(new List<Vertex> { end, point(2, t1, -2), point(2, t0, -2) }));*/

                polygons.Add(new Polygon(new List<Vertex> { point(0, t1, 0), point(0, t0, 0), point(1, t0, 0), point(1, t1, 0) }));
                polygons.Add(new Polygon(new List<Vertex> { end, point(1, t1, -1), point(1, t0, -1) }));
            }

            return fromPolygons(polygons);
        }
    }
}