using UnityEngine;

public static class BoidMath
{
    public static Vector3 CenterOfMass(Boid[] boids)
    {
        var centerOfMass = Vector3.zero;
        foreach (var boid in boids)
        {
            centerOfMass += boid.transform.position;
        }
        centerOfMass /= boids.Length;
        return centerOfMass;
    }

    public static float Distance(Boid boid, Boid neighbor)
    {
        return (boid.transform.position - neighbor.transform.position).magnitude;
    }

    public static float Distance(Boid boid, Transform transform)
    {
        return (boid.transform.position - transform.position).magnitude;
    }

    public static Vector3 Attract(Boid boid, Vector3 point)
    {
        return point - boid.transform.position;
    }

    public static Vector3 Attract(Boid boid, Boid neighbor) // Attract(A) = B - A
    {
        return neighbor.transform.position - boid.transform.position;
    }

    public static Vector3 Avoid(Boid boid, Boid neighbor) // Avoid(A) = A - B.
    {
        return boid.transform.position - neighbor.transform.position;
    }

    public static Vector3 Avoid(Boid boid, Vector3 point) // Avoid(A) = A - B.
    {
        return boid.transform.position - point;
    }
}

public static class BoidData
{
    public static class Forces
    {
        public static float AVOID_NEIGHBOR = 0.1f;
        public static float TOWARDS_CENTER = 0.01f;
    }

    public static class Detection
    {
        public static float FOV = 240;
        public static float NEIGHBOR_DIST = 3;
        public static float OBSTACLE_DIST = 4;
        public static float PREDATOR_DIST = 3;
    }

    public static class Obstacles
    {
        public static LayerMask MASK;
        public static int MAX_TURNS = 5;
        public static float TURN_FACTOR = 18;
    }

    public static class Movement
    {
        public static float SPEED;
        public static float ACCELERATION;
    }
}