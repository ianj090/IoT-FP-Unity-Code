using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject[] cloudPrefabs;
    private float timeBtwSpawn;
    public float minTimeBtwSpawn;
    public float maxTimeBtwSpawn;
    private float setTimeBtwSpawn;

    void Update()
    {
        setTimeBtwSpawn = Random.Range(minTimeBtwSpawn, maxTimeBtwSpawn);
        if (timeBtwSpawn <= 0) {
            SpawnCloud();
            timeBtwSpawn = setTimeBtwSpawn;
            ChangeLocation();
        } else {
            timeBtwSpawn -= Time.deltaTime;
        }
    }

    void SpawnCloud()
    {
        GameObject cloudPrefab = cloudPrefabs[Random.Range(0, cloudPrefabs.Length)];
        Instantiate(cloudPrefab, transform.position, Quaternion.identity);
    }

    void ChangeLocation() {
        transform.position = new Vector2(transform.position.x, Random.Range(1.5f, 3.5f));
    }
}
