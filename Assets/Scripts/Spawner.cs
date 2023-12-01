using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private PlayerController playerController;
    public GameObject pickUp; 
    private float timeBtwSpawnPickUp = 0.5f;
    public float minTimeBtwSpawnPickUp = 0.5f;
    public float maxTimeBtwSpawnPickUp = 2.5f;
    private float setTimeBtwSpawnPickUp;

    public GameObject[] obstacles; // Made it an array in case I want to have many different obstacle sprites (if there is time)
    private float timeBtwSpawnObstacle = 2.0f;
    public float minTimeBtwSpawnObstacle = 1.5f;
    public float maxTimeBtwSpawnObstacle = 6.0f;
    private float setTimeBtwSpawnObstacle;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) {
            playerController = player.GetComponent<PlayerController>();
        }
    }

    void Update()
    {
        if (playerController == null) {
            return;
        }

        // AdjustSpawnTimes();

        // Wind Pick Ups
        if (timeBtwSpawnPickUp <= 0) {
            Instantiate(pickUp, transform.position, Quaternion.identity);
            timeBtwSpawnPickUp = Random.Range(minTimeBtwSpawnPickUp, maxTimeBtwSpawnPickUp);
            ChangeLocation();
        } else {
            timeBtwSpawnPickUp -= Time.deltaTime;
        }

        // Obstacles
        if (timeBtwSpawnObstacle <= 0) {
            SpawnObstacle();
            timeBtwSpawnObstacle = Random.Range(minTimeBtwSpawnObstacle, maxTimeBtwSpawnObstacle);
            ChangeLocation();
        } else {
            timeBtwSpawnObstacle -= Time.deltaTime;
        }
    }

    // void AdjustSpawnTimes()
    // {
    //     float speedFactor = playerController.speedVal / 100.0f; // Basically a ratio compared to max of 100

    //     float adjustedMinTimeBtwSpawnPickUp = Mathf.Max(0.2f, minTimeBtwSpawnPickUp - speedFactor);
    //     float adjustedMaxTimeBtwSpawnPickUp = Mathf.Max(0.5f, maxTimeBtwSpawnPickUp - speedFactor);

    //     float adjustedMinTimeBtwSpawnObstacle = Mathf.Max(0.5f, minTimeBtwSpawnObstacle - speedFactor);
    //     float adjustedMaxTimeBtwSpawnObstacle = Mathf.Max(1.0f, maxTimeBtwSpawnObstacle - speedFactor);
    // }

    void ChangeLocation() {
        transform.position = new Vector2(transform.position.x, Random.Range(-3.0f, 3.0f));
    }

    void SpawnObstacle() {
        GameObject obstaclePrefab = obstacles[Random.Range(0, obstacles.Length)];
        Instantiate(obstaclePrefab, transform.position, Quaternion.identity);
    }
}
