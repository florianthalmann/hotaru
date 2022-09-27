using System.IO;
using static System.Math;
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
            hotaru[i+1] = Instantiate(hotaruPrefab, position, Quaternion.identity);
            hotaru[i+1].GetComponent<Hotaru>().swarm = this;
            //spheres[i].GetComponent<Rigidbody>().velocity = Random.onUnitSphere*speed;
            AudioSource source = hotaru[i+1].AddComponent<AudioSource>();
            //source.clip = Resources.Load<AudioClip>("Sounds/"
            //    + Path.GetFileNameWithoutExtension(audioFiles[i%audioFiles.Length].Name));
            int clip = Random.Range(0, 4);
            source.clip = Resources.Load<AudioClip>("Sounds/buzz"+clip);
            int pitch = Random.Range(-12, 12)*2;
            source.pitch = (float)Pow(2f, pitch != 0 ? 1f / pitch : 0);
            source.volume = 0.0f;
            source.loop = true;
            source.spatialBlend = 1;
            source.dopplerLevel = 0.5f;
            source.maxDistance = manager.maxSoundDistance;
            source.rolloffMode = AudioRolloffMode.Custom;
            source.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
