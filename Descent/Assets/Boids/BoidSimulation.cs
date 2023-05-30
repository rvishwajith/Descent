using UnityEngine;

public class Boid
{
    public Transform transform;
    public Vector3 velocity;
    public int id;
    public bool isTarget = false;
}

class BoidSimulation
{
    private Boid[] boids;
    private Transform center;

    public BoidSimulation(Transform[] transforms, Transform center)
    {
        boids = new Boid[transforms.Length];
        int i = 0;
        foreach (var transform in transforms)
        {
            Boid boid = new();
            boids[i] = boid;
            boid.transform = transform;
            boid.velocity = boid.transform.forward * 5;
            boid.id = i;
            i++;
        }
        boids[0].isTarget = true;
        this.center = center;
    }

    public void DrawGizmo()
    {
        foreach (var boid in boids)
        {
            if (!boid.isTarget) continue;

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(boid.transform.position, boid.transform.forward * BoidData.Detection.OBSTACLE_DIST);

            foreach (var neighbor in boids)
            {
                if (boid.id == neighbor.id) continue;

                Gizmos.color = Color.yellow;
                if (BoidMath.Distance(boid, neighbor) < BoidData.Detection.NEIGHBOR_DIST)
                {
                    // Debug.DrawLine(boid.transform.position, neighbor.transform.position);
                }
            }

        }
    }

    public void Simulate(float deltaT)
    {
        var centerOfMass = center.position;

        foreach (var boid in boids)
        {
            var velocity = boid.velocity;
            if (BoidMath.Distance(boid, center) > 8f)
            {
                velocity += (1.5f * deltaT * BoidMath.Attract(boid, centerOfMass));
            }

            foreach (var neighbor in boids)
            {
                if (boid.id == neighbor.id) continue;

                if (BoidMath.Distance(boid, neighbor) < BoidData.Detection.NEIGHBOR_DIST)
                {
                    velocity = Vector3.Lerp(velocity, neighbor.velocity, deltaT * 1.25f); // Cohesion
                    velocity += (8f * deltaT * BoidMath.Avoid(boid, neighbor) / 2); // Avoidance
                }
            }

            if (BoidMath.Distance(boid, center) < 8f)
            {
                AvoidObstacles(boid);
            }
            else
            {
                velocity += (0.5f * deltaT * BoidMath.Attract(boid, centerOfMass));
            }

            boid.velocity = velocity.normalized * 5;
            boid.transform.forward = boid.velocity / 5;
            boid.transform.position += boid.velocity.normalized * BoidData.Movement.SPEED * deltaT;
        }
    }

    private RaycastHit hit;

    void AvoidObstacles(Boid boid)
    {
        int i = 0;
        while (i < BoidData.Obstacles.MAX_TURNS)
        {
            var ray = new Ray(boid.transform.position, boid.transform.forward);
            if (Physics.Raycast(ray, out hit, BoidData.Detection.OBSTACLE_DIST, BoidData.Obstacles.MASK))
            {
                var strength = Mathf.InverseLerp(BoidData.Detection.OBSTACLE_DIST, 0, hit.distance);
                boid.velocity += BoidData.Obstacles.TURN_FACTOR * strength * BoidMath.Avoid(boid, hit.point);
            }
            else return;
            i++;
        }
    }
}