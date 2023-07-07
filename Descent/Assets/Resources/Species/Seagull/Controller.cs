using UnityEngine;

namespace Species
{
    namespace Seabirds
    {
        public class Controller : MonoBehaviour
        {
            private int spawnCount = 50;
            public GameObject prefab = null;

            private Seagull[] seagulls;

            private void Start()
            {
                seagulls = new Seagull[spawnCount];
                for (var i = 0; i < seagulls.Length; i++)
                {
                    var instance = Instantiate(prefab).transform;
                    var center = transform.position;
                    var offset = Random.insideUnitSphere * 15;
                    offset.y *= 0.75f;
                    seagulls[i] = new(instance, center + offset);
                }
            }

            private void Update()
            {
                foreach (var bird in seagulls)
                {
                    bird.Update(Time.deltaTime);
                }
            }
        }

        public class Seagull
        {
            public Transform transform;
            public Vector3 position
            {
                get { return transform.position; }
                set
                {
                    prevPosition = transform.position;
                    transform.position = value;
                }
            }
            private Vector3 prevPosition;

            private float t;
            private float tSpeed;
            private float radius;
            private Vector3 center;

            public Seagull(Transform transform, Vector3 center)
            {
                this.transform = transform;
                this.center = center;

                t = Random.Range(0, Mathf.PI * 2);
                tSpeed = Random.Range(1f, 1.25f);
                radius = Random.Range(8f, 16f);
            }

            public void Update(float dT)
            {
                t += tSpeed * dT;
                var point = center + new Vector3(Mathf.Cos(t), 0, Mathf.Sin(t)) * radius;

                transform.LookAt(point);
                position = point;
            }
        }
    }
}
