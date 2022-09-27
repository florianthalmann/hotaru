using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    [SerializeField]
    float yLimit = 7;
    [SerializeField]
    float buoyancyForce = 15;
    Rigidbody rbody;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < yLimit)
        {
            rbody.AddForce(transform.up * buoyancyForce);
        }
    }
}