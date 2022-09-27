using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Bounds bounds = new Bounds(new Vector3(0, 25, 0), new Vector3(100, 50, 100));
    public int maxSoundDistance = 50;




    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0, -9f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //Physics.gravity = new Vector3(0, -1f, 0);
    }
}
