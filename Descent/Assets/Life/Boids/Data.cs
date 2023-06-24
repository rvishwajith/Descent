namespace Boids
{
    using UnityEngine;

    public class Spawner
    {
        public int count = 100;

        public Entity[] CreateEntities()
        {
            var entities = new Entity[count];
            return entities;
        }
    }

    // Reference: https://github.com/SebLague/Boids/blob/master/Assets/Settings/Settings.asset
    public static class Weights
    {
        public static float ALIGN = 2,
            COHESION = 1,
            SEPERATE = 2.5f,
            TARGET = 2;
    }

    public static class Perception
    {
        public static float
            NEIGHBOR_AVOID = 1f,
            NEIGBOR_CENTER = 2.5f,
            COLLISION_AVOID = 5f,
            PREDATOR_AVOID = 5f;
    }
}