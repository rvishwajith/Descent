using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSimulation : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject prefab;
    public int spawnCount = 50;
    public float spawnRadius = 5;

    [Header("Behaviour")]
    public Transform target;
    public float moveSpeed = 1f;
    public float detectionDistance = 1f;

    private List<Fish> school = new();

    void Start()
    {
        for (var i = 0; i < spawnCount; i++)
        {
            var fish = new Fish();
            fish.transform = GameObject.Instantiate(prefab).transform;
            fish.transform.position = target.position + Random.insideUnitSphere * spawnRadius;
            school.Add(fish);
        }
    }

    void FixedUpdate()
    {
        foreach (var fish in school)
        {
            fish.transform.position += fish.transform.forward * Time.fixedDeltaTime * moveSpeed;
        }
    }
}

public struct Fish
{
    public Transform transform;
    public int id;
}