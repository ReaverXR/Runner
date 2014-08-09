using UnityEngine;
using System.Collections;

public class GameStateManager : MonoBehaviour {

	private const float TIME_TO_RESTART = 2.0f;

	public Character character;
	public Track track;

	public float deathTime = 0.0f;

	void Start()
	{
		character.OnDeath += OnDeath;
	}

	void Awake()
	{

	}

	void Update()
	{
		if(deathTime > 0.0f && Time.time >= deathTime + TIME_TO_RESTART)
			RestartLevel();
	}

	public void RestartLevel()
	{
		track.Reset();
		character.Reset();
		deathTime = 0.0f;
	}

	void OnDeath()
	{
		deathTime = Time.time;
	}
}
