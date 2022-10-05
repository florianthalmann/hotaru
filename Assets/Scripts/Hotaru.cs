using System.Linq;
using static System.Math;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotaru : MonoBehaviour
{
    public Swarm swarm;
	public Transform orientation;
	AudioSource source;
    int iterations = 0;
    float speed;
    int frequency;
    float growth;

    // Start is called before the first frame update
    void Start()
    {
		speed = Random.Range(swarm.minSpeed, swarm.maxSpeed);
        frequency = Random.Range(50, 400);
        growth = 0.01f*Random.value;
    }

	void InitHum()
    {
		source = gameObject.AddComponent<AudioSource>();
		//source.clip = Resources.Load<AudioClip>("Sounds/"
		//    + Path.GetFileNameWithoutExtension(audioFiles[i%audioFiles.Length].Name));
		//int clip = Random.Range(0, 4);
		//source.clip = Resources.Load<AudioClip>("Sounds/buzz"+clip);
		source.clip = Resources.Load<AudioClip>("Sounds/hum");
		int pitch = Random.Range(-24, 24);
		source.pitch = (float)Pow(2f, pitch != 0 ? pitch / 12f : 0);
		print(source.pitch);
		source.volume = 0.0f;
		source.loop = true;
		source.spatialBlend = 1;
		source.dopplerLevel = 0.5f;
		source.maxDistance = swarm.manager.maxSoundDistance;
		source.rolloffMode = AudioRolloffMode.Custom;
		source.reverbZoneMix = 1;
		gameObject.AddComponent<AudioEchoFilter>();
	}

	public void StartHum() {
		if (!source) { InitHum(); }
		source.Play();
	}

	public void StopHum() {
		source.Stop();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	private void FixedUpdate()
	{
		//randomize and clamp speed
		speed += swarm.speedFluctuation;
		speed = Mathf.Clamp(speed, swarm.minSpeed, swarm.maxSpeed);
		//add wavering
		transform.rotation = Quaternion.Slerp(transform.rotation,
			Random.rotation, swarm.waveringAmount);

		////randomize size
		//iterations++;
		//Vector3 change = new Vector3(growth, growth, growth);
		//if ((iterations / frequency) % 2 == 1) change *= -1;
		//gameObject.transform.localScale += change;

		//keep within bounds
		if (!swarm.manager.bounds.Contains(transform.position))
        {
			Vector3 direction = swarm.manager.bounds.center - transform.position;
			transform.rotation = Quaternion.Slerp(transform.rotation,
				Quaternion.LookRotation(direction), swarm.rotationSpeed * Time.deltaTime);

		}
		else if (Random.value < swarm.flockingAmount)
			ApplyRules();

		//move forward
		if (orientation) orientation.rotation = transform.rotation;
		transform.Translate(0, 0, Time.deltaTime * speed);
	}

    private void ApplyRules()
    {
		Vector3 vcentre = Vector3.zero;
		Vector3 vavoid = Vector3.zero;
		float gSpeed = 0.01f;
		float nDistance;
		float flockSize = 0;

		foreach (GameObject s in swarm.hotaru.Where(x => x != this.gameObject))
		{
			nDistance = Vector3.Distance(s.transform.position, this.transform.position);
			if (nDistance <= swarm.neighbourDistance)
			{
				vcentre += s.transform.position;
				flockSize++;

				if (nDistance < 1.0f)
				{
					vavoid = vavoid + (this.transform.position - s.transform.position);
				}

				gSpeed = gSpeed + s.GetComponent<Hotaru>().speed;
			}
		}

		if (flockSize > 0)
		{
			vcentre = vcentre / flockSize;
			speed = gSpeed / flockSize;

			Vector3 direction = (vcentre + vavoid) - transform.position;
			if (direction != Vector3.zero)
				transform.rotation = Quaternion.Slerp(transform.rotation,
													  Quaternion.LookRotation(direction),
													  swarm.rotationSpeed * Time.deltaTime);
		}

		if (source) source.volume = 0.03f; // (1+(flockSize/10));
	}
}
