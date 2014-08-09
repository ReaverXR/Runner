using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Track : MonoBehaviour {

	private const int INITIAL_CHUNKS = 5;
	private const float TRACK_MOVEMENT_PER_SECOND = 9;

	public List<GameObject> terrainChunks;
	public GameObject blankTerrainChunk;

	private List<GameObject> upcomingTerrain, toDestroy;
	private GameObject lastTerrain;

	// Use this for initialization
	void Start () {
	
		// Initialize stuffs.
		upcomingTerrain = new List<GameObject> ();
		toDestroy = new List<GameObject>();

		// Load terrain chunks.
		PreloadTerrainPrefabs ();

		SetUpLevel();

	}

	void SetUpLevel()
	{
		AddSpecficTerrain(blankTerrainChunk);

		// Select a few terrain chunks to start.
		for (int i = 0; i < INITIAL_CHUNKS-1; i++)
			AddRandomTerrain ();
	}

	// Update is called once per frame
	void Update () {
	
		UpdateTerrain();

		CleanUp();

	}

	void UpdateTerrain()
	{
		int toAdd = 0;

		foreach (GameObject go in upcomingTerrain) 
		{
			go.transform.position = new Vector3( go.transform.position.x - Time.deltaTime*TRACK_MOVEMENT_PER_SECOND, go.transform.position.y, go.transform.position.z );

			if(go.transform.position.x <= -go.transform.localScale.x)
			{
				toDestroy.Add(go);

				toAdd++;
			}
		}

		for(int i = 0; i < toAdd; i++)
			AddRandomTerrain();
	}

	void CleanUp()
	{
		foreach(GameObject go in toDestroy)
		{
			if(upcomingTerrain.Contains(go))
				upcomingTerrain.Remove(go);

			Destroy(go);
		}

		toDestroy.Clear();
	}

	void PreloadTerrainPrefabs()
	{
		Object[] chunks = Resources.LoadAll("Prefabs/Terrain Chunks/");

		foreach (Object obj in chunks) 
		{
			terrainChunks.Add( obj as GameObject );

			Debug.Log ("TERRAIN: " + obj.name + " loaded.");
		}
	}

	void AddRandomTerrain()
	{
		GameObject newTerrainChunk = terrainChunks [Random.Range (0, terrainChunks.Count)];
		 
		AddSpecficTerrain(newTerrainChunk);
	}

	void AddSpecficTerrain(GameObject newTerrainChunk)
	{
		GameObject terrain = Instantiate(newTerrainChunk) as GameObject;

		terrain.transform.parent = this.gameObject.transform;
		
		if (upcomingTerrain.Count == 0) 
		{
			terrain.transform.localPosition = new Vector3(0, -terrain.transform.localScale.y/2f, 0);
		}
		else
		{
			terrain.transform.localPosition = new Vector3( lastTerrain.transform.localPosition.x + lastTerrain.transform.localScale.x/2 + terrain.transform.localScale.x/2,
			                                              -terrain.transform.localScale.y/2f,
			                                                      0);
		}
		
		lastTerrain = terrain;
		upcomingTerrain.Add(terrain);
	}

	public void Reset()
	{
		foreach(GameObject go in upcomingTerrain)
			Destroy (go);

		upcomingTerrain.Clear();

		foreach(GameObject go in toDestroy)
		{
			if(go != null)
				Destroy(go);
		}

		toDestroy.Clear();

		SetUpLevel();
	}
}
