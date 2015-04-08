using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raytracer.MathTypes;

namespace Raytracer.Rendering.Accellerators
{
    using Vector = Vector3;
    using Raytracer.Rendering.Primitives;

    class Octree : IAccelerator
    {
        public class Node
        {
            public Node [] children = null;
            public AABB Bounds;

            public List<Traceable> objects;
            public double MaxItems
            {
                get;
                private set;
            }

            public double MinDimension
            {
                get;
                private set;
            }

            public Node(AABB bounds, double maxItems, double minDimension)
            {
                this.objects = new List<Traceable>();
                this.Bounds = bounds;
                this.MaxItems = maxItems;
                this.MinDimension = minDimension;
            }

            internal void CreateChildren()
            {
                children = new Node[8];

                children[0] = new Node(GetAABBSubQuadrant(this.Bounds, true, true, true), this.MaxItems, this.MinDimension);
                children[1] = new Node(GetAABBSubQuadrant(this.Bounds, true, false, true), this.MaxItems, this.MinDimension);
                children[2] = new Node(GetAABBSubQuadrant(this.Bounds, false, true, true), this.MaxItems, this.MinDimension);
                children[3] = new Node(GetAABBSubQuadrant(this.Bounds, false, false, true), this.MaxItems, this.MinDimension);

                children[4] = new Node(GetAABBSubQuadrant(this.Bounds, true, true, false), this.MaxItems, this.MinDimension);
                children[5] = new Node(GetAABBSubQuadrant(this.Bounds, true, false, false), this.MaxItems, this.MinDimension);
                children[6] = new Node(GetAABBSubQuadrant(this.Bounds, false, true, false), this.MaxItems, this.MinDimension);
                children[7] = new Node(GetAABBSubQuadrant(this.Bounds, false, false, false), this.MaxItems, this.MinDimension);
            }
            
            internal AABB GetAABBSubQuadrant(AABB parent, bool Up, bool Left, bool Forward)
            {
                var min = new Vector();
                var max = new Vector();

                if (Up)
                    min.X = parent.Min.X;
                else
                    min.X = parent.Min.X + (parent.Width / 2.0f);

                if (Left)
                    min.Y = parent.Min.Y;
                else
                    min.Y = parent.Min.Y + (parent.Height / 2.0f);

                if (Forward)
                    min.Z = parent.Min.Z;
                else
                    min.Z = parent.Min.Z + (parent.Depth / 2.0f);

                max.X = min.X + parent.Width / 2.0f;
                max.Y = min.Y + parent.Height / 2.0f;
                max.Z = min.Z + parent.Depth / 2.0f;

                return new AABB(min, max);
            }

            public void Add(Traceable obj)
            {
                if (this.children == null)
                {
                    this.objects.Add(obj);
                    Subdivide();
                }
                else
                {
                    foreach (var child in children)
                    {
                        if (obj.Intersect(child.Bounds))
                        {
                            child.Add(obj);
                        }
                    }
                }
            }

            private void Subdivide()
            {
                if(objects.Count < this.MaxItems)
                    return;

                if (this.Bounds.Width / 2 < this.MinDimension)
                    return;

                if (this.Bounds.Height / 2 < this.MinDimension)
                    return;

                if (this.Bounds.Depth / 2 < this.MinDimension)
                    return;

                this.CreateChildren();

                while (this.objects.Count > 0)
                {
                    var obj = this.objects[this.objects.Count - 1];

                    foreach (var childNode in children)
                    {
                        if (obj.Intersect(childNode.Bounds))
                        {
                            childNode.Add(obj);
                        }
                    }
                    this.objects.RemoveAt(this.objects.Count - 1);
                }
            }

            public IEnumerable<Traceable> Intersect(Ray ray)
            {
                if (this.Bounds.Intersect(ray))
                {                  
                    if (this.children != null)
                    {
                        List<Traceable> traceableObjects = new List<Traceable>();

                        foreach (var childNode in this.children)
                        {
                            if (childNode != null)
                                traceableObjects.AddRange(childNode.Intersect(ray));
                        }

                        return traceableObjects;
                    }
                    else
                    {
                        return this.objects;
                    }
                }

                return new List<Traceable>();
            }

            public int GetContainedObjects()
            {                
                if (this.children != null)
                {
                    return (from child in this.children
                            where child != null
                            select child.GetContainedObjects()).Sum();
                }
                else
                {
                    if (this.objects != null)
                        return this.objects.Count;
                    else
                        return 0;
                }                
            }

            internal void PruneEmptyNodes()
            {
                if(this.children != null)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (this.children[i] == null)
                            continue;

                        this.children[i].PruneEmptyNodes();

                        if (this.children[i].GetContainedObjects() == 0)
                        {
                            this.children[i] = null;
                        }
                    }
                }
            }
        }

        private Node _root = null;
        private const double _minNodeWidth = 0.05;
        private const int _maxItemsPerNode = 8;
        
        public void Build(IEnumerable<Traceable> primitives)
        {
            AABB bounds = AABB.Empty;

            foreach (var item in primitives)
	        {
		        bounds.InflateToEncapsulate(item.GetAABB());
	        }
            
            _root = new Node(bounds, _maxItemsPerNode, _minNodeWidth);
        }

        public IEnumerable<Traceable> Intersect(Ray ray)
        {
            return this._root.Intersect(ray);
        }

        public void PruneEmptyNodes()
        {
            this._root.PruneEmptyNodes();
        }        
    }
}
