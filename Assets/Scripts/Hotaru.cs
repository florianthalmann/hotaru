using System.Linq;
using static System.Math;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotaru : MonoBehaviour
{
	public int[] song;
	private AudioSource source;
	private int bps = 5;
	private int singingPosition = -1;
	private float nextUp = 0;
    public Swarm swarm;
	public Transform orientation;
    //int iterations = 0;
    float speed;
    //int frequency;
    //float growth;

    // Start is called before the first frame update
    void Start()
    {
        //this.song = new int[] { 1, 1, 1, 1, 1 };
        this.song = Enumerable.Repeat(0, 16)
            .Select(i => Random.Range(0, 2)).ToArray();
        speed = Random.Range(swarm.minSpeed, swarm.maxSpeed);
        //frequency = Random.Range(50, 400);
        //growth = 0.01f*Random.value;
    }

	void InitHum()
    {
		source = gameObject.AddComponent<AudioSource>();
		//source.clip = Resources.Load<AudioClip>("Sounds/"
		//    + Path.GetFileNameWithoutExtension(audioFiles[i%audioFiles.Length].Name));
		//int clip = Random.Range(0, 4);
		//source.clip = Resources.Load<AudioClip>("Sounds/buzz"+clip);
		source.clip = Resources.Load<AudioClip>("Sounds/hum2");
		int pitch = Random.Range(-12, 30);
		source.pitch = (float)Pow(2f, pitch != 0 ? pitch / 12f : 0);
		source.volume = 0.0f;
		source.loop = true;
		source.spatialBlend = 1;
		source.dopplerLevel = 0.5f;
		source.maxDistance = swarm.manager.maxSoundDistance;
		source.rolloffMode = AudioRolloffMode.Custom;
		source.reverbZoneMix = 1;
		//gameObject.AddComponent<AudioEchoFilter>();
	}

	public void StartSinging() {
		if (!source) { InitHum(); }
		//print(string.Join(", ", this.song));
		this.singingPosition = 0;
		this.nextUp = 0;
	}

	//private IEnumerator Sing()
 //   {
	//	while (true)
 //       {
	//		foreach (int s in this.song)
	//		{
	//			if (source.isPlaying && s == 0) source.Pause();
	//			if (!source.isPlaying && s == 1) source.Play();
	//			yield return new WaitForSeconds(0.3f);

	//		}
	//	}
	//}

	public void StopSinging() {
		//if (this.singing != null) StopCoroutine(this.singing);
		this.singingPosition = -1;
		source.Stop();
	}

    // Update is called once per frame
    void Update()
    {
		//print(this.singingPosition + ", " + this.nextUp);
		if (this.singingPosition >= 0)
        {
			if (this.nextUp < 0)
            {
				if (this.source.isPlaying && this.song[this.singingPosition] == 0) source.Pause();
				if (!this.source.isPlaying && this.song[this.singingPosition] == 1) source.Play();
				this.singingPosition = (this.singingPosition + 1) % this.song.Length;
				this.nextUp += (1f/this.bps);
			}
			this.nextUp -= Time.deltaTime;
        }
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

		GameObject[] others = swarm.hotaru.Where(h => h != this.gameObject).ToArray();
		float[] distances = others.Select(h => Vector3.Distance(h.transform.position, this.transform.position)).ToArray();
		List<(GameObject,float)> flock = others.Zip(distances, (o,d) => (o, d))
			.Where((h, i) => distances[i] < swarm.neighbourDistance).ToList();

		if (flock.Count > 0)
        {
			//update position and rotation based on neighbors
			flock.ForEach(t =>
			{
				var (h, d) = t;
				vcentre += h.transform.position;

				if (d < 1.0f)
				{
					vavoid = vavoid + (this.transform.position - h.transform.position);
				}

				gSpeed = gSpeed + h.GetComponent<Hotaru>().speed;
			});

			vcentre = vcentre / flock.Count;
			speed = gSpeed / flock.Count;

			Vector3 direction = (vcentre + vavoid) - transform.position;
			if (direction != Vector3.zero)
				transform.rotation = Quaternion.Slerp(transform.rotation,
													  Quaternion.LookRotation(direction),
													  swarm.rotationSpeed * Time.deltaTime);

			//imitate a random song from the neighbors from now on
			if (Random.value < 0.1)
            {
				this.song = flock.Select(p => p.Item1.GetComponent<Hotaru>().song)
					.OrderBy(p => Random.value).First();
			}
		}

		if (source) source.volume = 0.03f; // (1+(flockSize/10));
	}
}
