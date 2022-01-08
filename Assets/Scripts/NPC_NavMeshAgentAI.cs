using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// script attached to all NPC entities in the game
public class NPC_NavMeshAgentAI : MonoBehaviour 
{

	private Terrain terrain;
	private NavMeshAgent npcAgent;
	private Animation npcAnimation;
	private Vector3 randomPosition;
	private GameObject target;
	private float npcAgentSpeed;
	private float DistanceFromTarget;
	private float speedFactor;
	private int terrainWidth;
	private int terrainLength;
	private int terrainPosX;
	private int terrainPosZ;
	private AudioSource audioSrc;
	private bool isPlaying;

	// Use this for initialization
	private void Start () 
	{
		npcAgent = GetComponent<NavMeshAgent>();
		npcAgentSpeed = npcAgent.speed;

		npcAnimation = GetComponent<Animation>();
		npcAnimation["walk"].speed = 0.5f;	

		target = GameObject.Find ("Player");

		speedFactor = 6.0f;

		terrain = GameObject.Find ("DINOISLAND").GetComponent<Terrain>();
		terrainWidth = (int)terrain.terrainData.size.x;
		terrainLength = (int)terrain.terrainData.size.z;
		terrainPosX = (int)terrain.transform.position.x;
		terrainPosZ = (int)terrain.transform.position.z;

		audioSrc = GetComponent<AudioSource>();

		StartCoroutine (SetAgentRandomPath ());
	}

	private IEnumerator SetAgentRandomPath ()
	{
		while (true)
		{
			int posX = Random.Range ((terrainPosX / 2), ((terrainPosX + terrainWidth) / 2));
			int posZ = Random.Range ((terrainPosZ / 2), ((terrainPosZ + terrainLength) / 2));

			randomPosition = new Vector3 (posX, transform.position.y, posZ);

			yield return new WaitForSeconds (8.0f);
		}
	}

	// Update is called once per frame
	private void Update () 
	{
		DistanceFromTarget = Vector3.Distance (target.transform.position, transform.position);

		SetupAgentLocomotion ();
		SetupAgentAnimation ();
	}

	private void SetupAgentLocomotion ()
	{
		if (DistanceFromTarget > 150.0f)
		{
			if (npcAgent.speed > npcAgentSpeed)
			{
				npcAgent.speed = npcAgentSpeed / speedFactor;
			}
			npcAgent.SetDestination (randomPosition);
		}
		else
		{
			if (gameObject.tag == "NPC")
			{
				if (npcAgent.speed <= npcAgentSpeed) 
				{
					npcAgent.speed = npcAgentSpeed * speedFactor;	
				}
				npcAgent.SetDestination (target.transform.position);	
			}
			else
			{
				if (npcAgent.speed > npcAgentSpeed)
				{
					npcAgent.speed = npcAgentSpeed / speedFactor;
				}
				npcAgent.SetDestination (randomPosition);
			}
		}
	}

	private void SetupAgentAnimation ()
	{
		if (AgentDone ())
		{
			npcAnimation.CrossFade ("idle");
		}
		else 
		{
			if (DistanceFromTarget > 150.0f)
			{
				npcAnimation.CrossFade ("walk");
			}
			else
			{
				if (gameObject.tag == "NPC")
				{
					npcAnimation.CrossFade ("run");	

					if (DistanceFromTarget < 60.0f)
					{
						if (!this.GetComponent<NPC_InventoryStorage>().isSoundActivated)
						{
							audioSrc.loop = false;
							audioSrc.Play ();

							this.GetComponent<NPC_InventoryStorage>().isSoundActivated = true;
						}
					}
					else
					{
						if (this.GetComponent<NPC_InventoryStorage>().isSoundActivated)
						{
							this.GetComponent<NPC_InventoryStorage>().isSoundActivated = false;
						}
					}
				}
				else
				{
					npcAnimation.CrossFade ("walk");
				}
			}
		}
	}

	private bool AgentDone ()
	{
		return npcAgent.remainingDistance<=npcAgent.stoppingDistance;
	}
}
