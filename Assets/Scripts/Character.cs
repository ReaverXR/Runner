using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

	private enum State {
		Alive = 0,
		Dead
	}

	private const int MAX_JUMPS = 2;
	private const float JUMP_FORCE = 500f;

	public delegate void OnDeathEvent();
	public event OnDeathEvent OnDeath;

	Vector3 jumpVector;

	int numJumps = 0;
	bool downLastFrame = false;

	//private Rigidbody rigidbody;

	private State state = State.Alive;

	// Use this for initialization
	void Start () {

	}

	void Awake() {
		//rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
	
		if(state == State.Alive)
		{
			if(Input.GetKeyDown(KeyCode.Mouse0) && !downLastFrame && numJumps < MAX_JUMPS)
			{
				Jump ();
			}
			else
				downLastFrame = false;
		}
	}

	void Jump()
	{
		this.GetComponent<Rigidbody>().AddForce(Vector3.up*JUMP_FORCE, ForceMode.Acceleration );
		numJumps++;
		downLastFrame = true;
	}

	void KillFrom(Vector3 point)
	{
		rigidbody.constraints = RigidbodyConstraints.None;

		if(point.y <= (gameObject.transform.localPosition.y - gameObject.transform.localScale.y/2))
		{
			rigidbody.AddExplosionForce(150.0f, new Vector3(point.x + Random.Range(-gameObject.transform.localScale.x/2f, gameObject.transform.localScale.x/2f), 
			                                                point.y, 
			                                                point.z + Random.Range(0.0f, 1.0f)), 0.0f);
		}
		else
		{
			rigidbody.AddExplosionForce(300.0f, point, 0.0f);
		}

		OnDeath();

		state = State.Dead;
	}

	void OnCollisionEnter(Collision collision)
	{
		foreach(ContactPoint point in collision.contacts)
		{
			switch(point.otherCollider.gameObject.tag)
			{
			case "Terrain Obstacle":
				KillFrom (point.point);
				Debug.Log ("DED!");
				break;
			case "Terrain":
				numJumps = 0;
				break;
			default:
				break;
			}
		}
	}

	public void Reset()
	{
		gameObject.transform.localPosition = new Vector3(0, gameObject.transform.localScale.y/2f, 0);
		gameObject.transform.localRotation = Quaternion.identity;

		numJumps = 0;
		downLastFrame = false;
		state = State.Alive;

		rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ; 
	}
}
