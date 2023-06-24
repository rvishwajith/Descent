namespace Boids
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Manager : MonoBehaviour
    {
        public GameObject prefab;
        public Entity[] boids;

        private void Start()
        {
            Spawner spawner = new();
            boids = spawner.CreateEntities();
        }
    }

    public class Entity
    {
        public Transform transform;
    }

    public class OctreeCell
    {
        List<Entity> boids = new();

        public void Add(Entity boid)
        {
            boids.Add(boid);
        }

        public void Reset()
        {
            boids.Clear();
        }
    }
}