namespace Kelp
{
    using System.Collections.Generic;
    using UnityEngine;

    namespace Advanced
    {
        public class Entity
        {
            private Transform transform;
            private float height;
            private Leaf[] leaves;

            public Entity(Transform transform, float height)
            {
                this.transform = transform;
                this.height = height;

                var numLeaves = (int)(height * 2);
                var leafLength = 2f;
                leaves = new Leaf[numLeaves];

                var zRange = 0f;
                for (var i = 0; i < leaves.Length; i++)
                {
                    var origin = transform.position + transform.up * (i * 0.5f + 0.5f);
                    Vector3 direction = new(1, 0, Random.Range(-zRange, zRange));
                    if (i % 2 == 0)
                        direction.x = -1;
                    leaves[i] = new(origin, direction, leafLength);
                }
            }

            public void Update(float dT, Collider[] colliders)
            {
                foreach (var leaf in leaves)
                    leaf.Update(dT, colliders);
            }

            public void DrawGizmos()
            {
                Gizmos.DrawRay(transform.position, Vector3.up * height);
                foreach (var leaf in leaves)
                    leaf.DrawGizmos();
            }
        }
    }
}