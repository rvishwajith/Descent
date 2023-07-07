using UnityEngine;
using System.Collections.Generic;

namespace Kelp
{
    namespace Advanced
    {
        public class Entity
        {
            private Transform transform = null;
            private Stalk stalk = null;

            /*
            private Leaf[] leaves = null;
            */
            private Leaf2[] leaves = null;

            private Bounds bounds;
            private float height;

            public Entity(Transform transform, float height)
            {
                this.transform = transform;
                this.height = height;
                stalk = new(transform.position, height);
                CreateLeaves();

                UpdateBounds();

                /*
                var leafList = new List<Leaf>();
                while (y <= height)
                {
                    var origin = transform.position + transform.up * y;
                    var direction = transform.right + transform.forward * Random.Range(-zDirRange, zDirRange);
                    leafList.Add(new(origin, direction, leafLength));
                    y += Random.Range(yOffsetMin, yOffsetMax);
                }
                leaves = leafList.ToArray();
                */
            }

            public void CreateLeaves()
            {
                var leafList = new List<Leaf2>();
                int numLeaves = 30;

                for (int i = 0; i < numLeaves; i++)
                {
                    var index = Random.Range(0, stalk.points.Length - 1);
                    var t = Random.Range(0f, 1f);
                    leafList.Add(new(stalk, index, t));
                }
                leaves = leafList.ToArray();
            }

            public void Update(float dT)
            {
                UpdateBounds();

                var colliders = Physics.OverlapBox(bounds.center, bounds.extents);
                stalk.Update(dT, colliders);
                foreach (var leaf in leaves)
                {
                    leaf.Update(dT, colliders);
                }
            }

            public void UpdateBounds()
            {
                var center = transform.position + (transform.up * (height / 2));
                bounds.center = center;

                var extents = new Vector3(height / 4, height / 2, height / 4);
                bounds.extents = extents;
            }

            public bool InFrustum(Plane[] planes)
            {
                return GeometryUtility.TestPlanesAABB(planes, bounds);
            }

            public void DrawGizmos()
            {
                if (stalk != null)
                    stalk.DrawGizmos();
                else
                    Gizmos.DrawRay(transform.position, Vector3.up * height);

                if (leaves != null)
                {
                    foreach (var leaf in leaves)
                    {
                        if (leaf != null)
                            leaf.DrawGizmos();
                    }
                }
            }
        }
    }
}