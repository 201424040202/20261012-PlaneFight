using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject enemy0Prefab;
    public float enemy0SpawnInterval = 1;

    public GameObject enemy1Prefab;
    public float enemy1SpawnInterval = 5;

    public GameObject enemy2Prefab;
    public float enemy2SpawnInterval = 10;

    public GameObject award1Prefab;
    public float award1SpawnInterval = 4;

    public GameObject award2Prefab;
    public float award2SpawnInterval = 7;

    public GameObject award3Prefab;
    public float award3SpawnInterval = 11;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy0", 1, enemy0SpawnInterval);
        InvokeRepeating("SpawnEnemy1", 2, enemy1SpawnInterval);
        InvokeRepeating("SpawnEnemy2", 3, enemy2SpawnInterval);
        InvokeRepeating("SpawnAward1", 4, award1SpawnInterval);
        InvokeRepeating("SpawnAward2", 5, award2SpawnInterval);
        InvokeRepeating("SpawnAward3", 6, award3SpawnInterval);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemy0()
    {
        float x = Random.Range(-1.98f, 1.98f);
        Instantiate(enemy0Prefab, new Vector3(x, transform.position.y, transform.position.z), transform.rotation);
    }

    void SpawnEnemy1()
    {
        float x = Random.Range(-1.95f, 1.95f);
        Instantiate(enemy1Prefab, new Vector3(x, transform.position.y, transform.position.z), transform.rotation);
    }

    void SpawnEnemy2()
    {
        float x = Random.Range(-1.46f, 1.46f);
        Instantiate(enemy2Prefab, new Vector3(x, transform.position.y, transform.position.z), transform.rotation);
    }

    void SpawnAward1()
    {
        float x = Random.Range(-1.96f, 1.96f);
        Instantiate(award1Prefab, new Vector3(x, transform.position.y, transform.position.z), transform.rotation);
    }

    void SpawnAward2()
    {
        float x = Random.Range(-1.96f, 1.96f);
        Instantiate(award2Prefab, new Vector3(x, transform.position.y, transform.position.z), transform.rotation);
    }

    void SpawnAward3()
    {
        float x = Random.Range(-1.96f, 1.96f);
        Instantiate(award3Prefab, new Vector3(x, transform.position.y, transform.position.z), transform.rotation);
    }

}
