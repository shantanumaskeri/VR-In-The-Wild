using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_RandomSpawnOnTerrain : MonoBehaviour 
{

	public Terrain terrain;
	public GameObject[] objectsToPlace;
	public int numberOfObjects;

	private int currentObjects;
	private int terrainWidth;
	private int terrainLength;
	private int terrainPosX;
	private int terrainPosZ;

	// Use this for initialization
	private void Start () 
	{
		terrainWidth = (int)terrain.terrainData.size.x;
		terrainLength = (int)terrain.terrainData.size.z;
		terrainPosX = (int)terrain.transform.position.x;
		terrainPosZ = (int)terrain.transform.position.z;

		StartCoroutine (SpawnRandomObjects ());
	}

	private IEnumerator SpawnRandomObjects ()
	{
		while (true)
		{
			if (currentObjects < numberOfObjects)
			{
				int posX = Random.Range ((terrainPosX / 2), ((terrainPosX + terrainWidth) / 2));
				int posZ = Random.Range ((terrainPosZ / 2), ((terrainPosZ + terrainLength) / 2));
				float posY = Terrain.activeTerrain.SampleHeight (new Vector3 (posX, 0, posZ));
				GameObject newObject = (GameObject)Instantiate (objectsToPlace [Random.Range (0, objectsToPlace.Length)], new Vector3 (posX, posY, posZ), Quaternion.identity);
				newObject.transform.parent = transform;

				currentObjects += 1;
			}

			yield return new WaitForSeconds (4.0f);
		}
	}
}
