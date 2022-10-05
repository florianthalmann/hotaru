using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm : MonoBehaviour
{
    public GameManager manager;
    public GameObject hotaruPrefab;
    public GameObject playerHotaru;
    public GameObject[] hotaru;
    [Header("Spheres")]
    public int numHotaru = 20;
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(0.0f, 1.0f)]
    public float flockingAmount;
    [Range(1.0f, 50.0f)]
    public float neighbourDistance;
    [Range(0.0f, 5.0f)]
    public float rotationSpeed;
    [Range(0.0f, 1.0f)]
    public float waveringAmount;
    [Range(0.0f, 1.0f)]
    public float speedFluctuation;

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0, 0f, 0);
        //var audioFiles = new DirectoryInfo("Assets/Resources/Sounds/").GetFiles("*.mp3");
        hotaru = new GameObject[numHotaru + 1];
        hotaru[0] = playerHotaru;
        //print(audioFiles.Length);
        for (int i = 0; i < numHotaru; i++)
        {
            Vector3 position = new Vector3(
                Random.Range(manager.bounds.min.x, manager.bounds.max.x),
                Random.Range(manager.bounds.min.y, manager.bounds.max.y),
                Random.Range(manager.bounds.min.z, manager.bounds.max.z));
            //25*(Random.insideUnitSphere + new Vector3(0, 1, 0));
            hotaru[i + 1] = Instantiate(hotaruPrefab, position, Quaternion.identity);
            hotaru[i + 1].GetComponent<Hotaru>().swarm = this;
            //spheres[i].GetComponent<Rigidbody>().velocity = Random.onUnitSphere*speed;
            hotaru[i + 1].GetComponent<Hotaru>().StartHum();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
