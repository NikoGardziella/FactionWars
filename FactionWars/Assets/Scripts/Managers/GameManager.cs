using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;


public class GameManager : MonoBehaviour
{

	public GameObject skeleton;
	public GameObject spawnZone;
	int count = 0;

	private static GameManager instance;
	[SerializeField]
	private List<GameObject> objects;
	[SerializeField]
	private List<PlayerStats> players;
	[SerializeField]
	Text enemyScore;
	[SerializeField]
	private Text endGameText;
	[SerializeField]
	private PlayerStats playerInfo;
	[SerializeField]
	private CameraMovement camMovement;

	private CaptureZone captureZone;
	public CaptureZone CaptureZone
	{
		get { return captureZone; }
		set { captureZone = value; }
	}


	private Card card;
	public Card Card
	{
		get { return card; }
		set { card = value; }
	}
	public CameraMovement CamMovement
	{
		get { return camMovement; }
		set { camMovement = value; }
	}

	public PlayerStats PlayerInfo
	{
		get { return playerInfo; }
		set { playerInfo = value; }
	}

	public Text EndGameText
	{
		get { return endGameText; }
		set { endGameText = value; }
	}



	public List<PlayerStats> Players
	{
		get { return players; }
		set { players = value; }
	}

	public List<GameObject> Objects
	{
		get { return objects; }
	}

	

	public static GameManager Instance {get { return instance; } }

	private void Awake()
	{

	//	InvokeRepeating("spawnSkeleton", 2.0f, 15.0f);

		if (instance != this)
			instance = this;

		
		//PhotonNetwork.Instantiate("skeleton", Vector3.up, Quaternion.identity,0);


		GameObject go = PhotonNetwork.Instantiate(GameConstants.GAME_PLAYER, Vector3.zero, Quaternion.identity, 0);
		if (!go)
		{
			Debug.Log("failed to instantiate ingameplayer");
		}
		else
			Debug.Log("Ingameplayer instantiated");
		go.transform.position = GameConstants.PLAYER_START;
		go.transform.rotation = GameConstants.PLAYER_ROT;
		go.tag = GameConstants.PLAYER_TAG;
		PlayerStats ps = go.GetComponent<PlayerStats>();
		ps.enabled = true;
		playerInfo = ps;

		ps.TextScore = go.transform.GetChild(2).GetChild(1).GetChild(1).GetComponent<Text>(); 

		go.transform.GetChild(1).gameObject.SetActive(true);
		go.transform.GetChild(2).gameObject.SetActive(true);
		ps.PlayersDeck.Cards = AccountInfo.Deck;
		Debug.Log("Players Deck: " + ps.PlayersDeck.Cards);

		for (int i = 0; i < go.transform.GetChild(3).childCount; i++)
		{
			go.transform.GetChild(3).GetChild(i).gameObject.tag = GameConstants.PLAYER_TAG;
		}

		//GameObject keep = PhotonNetwork.Instantiate(GameConstants.PLAYER_KEEP, Vector3.zero,Quaternion.identity, 0);
		//keep.tag = GameConstants.PLAYER_TAG;
		//keep.transform.SetParent(go.transform.GetChild(3), false);

		//GameObject towerRight = PhotonNetwork.Instantiate(GameConstants.PLAYER_TOWER, GameConstants.PLAYER_TOWER_RIGHT, Quaternion.identity, 0);
		//towerRight.tag = GameConstants.PLAYER_TAG;
		//towerRight.transform.SetParent(go.transform.GetChild(3), false);

		//GameObject towerLeft = PhotonNetwork.Instantiate(GameConstants.PLAYER_TOWER, GameConstants.PLAYER_TOWER_RIGHT, Quaternion.identity, 0);
		//towerLeft.GetComponent<Structure>().LeftTower = true;
		//towerLeft.tag = GameConstants.PLAYER_TAG;
		//towerLeft.transform.SetParent(go.transform.GetChild(3), false);

		//////Players.Add(ps);
		//////Objects.Add(go.transform.GetChild(3).GetChild(0).gameObject);
		//////Objects.Add(go.transform.GetChild(3).GetChild(1).gameObject);
		//////Objects.Add(go.transform.GetChild(3).GetChild(2).gameObject);

	}

	internal void UpdateZones(string team, bool leftzoneBool)
	{
		if (team == "blue")
		{
			if (!leftzoneBool)
				instance.players[0].LeftZone = true;
			else
				instance.players[0].RightZone = true;
		}
		else if (team == "red")
		{
			
			if (!leftzoneBool)
				instance.players[0].LeftZone = false;
			else
				instance.players[0].RightZone = false;
		}
	}

	void spawnSkeleton()
	{
		count = 0;
		Collider[] hitColliders = Physics.OverlapBox(spawnZone.transform.position, transform.localScale / 2, Quaternion.identity);
		foreach (var ht in hitColliders)
		{
			if (ht.transform.CompareTag(GameConstants.NEUTRAL_TAG))
				count++;
			//else
			//	Debug.Log("no neutrals found");
		}
		if(count == 0)
		{
			PhotonNetwork.Instantiate("skeleton", spawnZone.transform.position, Quaternion.identity, 0);
			PhotonNetwork.Instantiate("skeleton", spawnZone.transform.position, Quaternion.identity, 0);			//Instantiate(skeleton, spawnZone.transform.localPosition, Quaternion.identity);
		}

	}

	void Update()
	{


		if (Input.GetMouseButtonDown(0))
		{

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			LayerMask mask = LayerMask.GetMask("Object");
			//Vector3 origin = Camera.main.transform.position;
			//Vector3 direction = Camera.main.transform.forward;
			//	Ray ray = new Ray(origin, direction);
			//Debug.DrawRay(origin, direction * 100f, Color.red);
			if (Physics.Raycast(ray, out RaycastHit hit, 1000, mask))
			{
				if (hit.transform.CompareTag("gem"))
				{
					playerInfo.AddResource(5);
				//	Debug.Log("hit gem gamemanager.c");
					Destroy(hit.transform.gameObject);
				}
			}
		} 

		objects = FindAllObjects();
		players = FindAllPlayerStats();

		//spawnSkeleton();

		/*if (PlayerStats.FindObjectOfType<PlayerStats>().OnDragging == true)
		{
			Debug.Log("OnDragging == true    camMovement.canDrag = false");
			camMovement.GetComponent<CameraMovement>().canDrag = false;
		}
		else if (PlayerStats.FindObjectOfType<PlayerStats>().OnDragging == false)
		{
			//Debug.Log("OnDragging == false            camMovement.canDrag = true");
			camMovement.GetComponent<CameraMovement>().canDrag = true;
			//camMovement.canDrag = true;
		}
		if (GameObject.Find("InGamePlayer(Clone)"))
			Debug.Log("GameObject.Find(InGamePlayer(Clone) "); */
		if (playerInfo.OnDragging)
			camMovement.canDrag = false;
		else if (!playerInfo.OnDragging)
			camMovement.canDrag = true;




	}

	public static void RemoveObjectFromList(GameObject go)
	{
		foreach  (GameObject g in Instance.Objects)
		{
			Component component = g.GetComponent(typeof(IDamageable));
			if (component)
			{
				if((component as IDamageable).HitTargets.Contains(go))
				{
					(component as IDamageable).HitTargets.Remove(go);
					if((component as IDamageable).Target == go)
					{
						(component as IDamageable).Target = null;
					}
				}
			}
		}

		Instance.Objects.Remove(go);

	}
	public static void RemoveObjectFromList(GameObject go, bool leftTower)
	{
		foreach (GameObject g in Instance.Objects)
		{
			Component component = g.GetComponent(typeof(IDamageable));
			if (component)
			{
				if ((component as IDamageable).HitTargets.Contains(go))
				{
					(component as IDamageable).HitTargets.Remove(go);
					if ((component as IDamageable).Target == go)
					{
						(component as IDamageable).Target = null;
					}
				}
			}
		}
		if (!go.CompareTag(GameConstants.PLAYER_TAG))
		{
			if (leftTower)
			{
				instance.players[0].LeftZone = true;
			}
			else
			{
				instance.players[0].RightZone = true;
			}
			instance.players[0].Score++;
		}
		else
		{
			if (leftTower)
			{
				instance.players[1].LeftZone = true;
			}
			else
			{
				instance.players[1].RightZone = true;
			}
			instance.players[1].Score++;
		}
		Instance.Objects.Remove(go);

	}

	public static List<GameObject> GetAllEnemies(Vector3 pos, List<GameObject> objects, string tag, float range)
	{
		List<GameObject> sentObjects = new List<GameObject>();
		foreach (GameObject g in objects)
		{
			if(!g.CompareTag(tag) && Vector3.Distance(pos, g.transform.position) <= range)
			{
				sentObjects.Add(g);
			}
		}
		
		return sentObjects;
	}

	public static List<GameObject> GetAllEnemies(Vector3 pos, List<GameObject> objects, string tag)
	{
		List<GameObject> sentObjects = new List<GameObject>();
		foreach (GameObject g in objects)
		{
			if (!g.CompareTag(tag))
			{
				sentObjects.Add(g);
			}
		}

		return sentObjects;
	}

	public static void AddObject(GameObject go)
	{
		Instance.Objects.Add(go);
	}

	public void GoToLogin()
	{
		levelManager.LoadLevel(1);
	}

	List<GameObject> FindAllObjects()
	{
		List<GameObject> gotObjects = new List<GameObject>();


		GameObject[] players = GameObject.FindGameObjectsWithTag(GameConstants.PLAYER_TAG);
		foreach (GameObject go in players)
		{
			if (go.GetComponent<Structure>() != null || go.GetComponent<unit>() != null)
				gotObjects.Add(go);
		}

		GameObject[] enemies = GameObject.FindGameObjectsWithTag(GameConstants.ENEMY_TAG);
		foreach (GameObject go in enemies)
		{
			if (go.GetComponent<Structure>() != null || go.GetComponent<unit>() != null)
				gotObjects.Add(go);
		}
		GameObject[] neutrals = GameObject.FindGameObjectsWithTag(GameConstants.NEUTRAL_TAG);
		foreach (GameObject go in neutrals)
		{
			if (go.GetComponent<Structure>() != null || go.GetComponent<unit>() != null)
				gotObjects.Add(go);
		}

		return gotObjects;
	}

	List<PlayerStats> FindAllPlayerStats()
	{
		List<PlayerStats> gotObjects = new List<PlayerStats>();


		GameObject[] players = GameObject.FindGameObjectsWithTag(GameConstants.PLAYER_TAG);
		foreach (GameObject go in players)
		{
			if(go.GetComponent<PlayerStats>() != null)
				gotObjects.Add(go.GetComponent<PlayerStats>());
		}

		GameObject[] enemies = GameObject.FindGameObjectsWithTag(GameConstants.ENEMY_TAG);
		foreach (GameObject go in enemies)
		{
			if (go.GetComponent<PlayerStats>() != null)
				gotObjects.Add(go.GetComponent<PlayerStats>());
		}

		/* GameObject[] neutrals = GameObject.FindGameObjectsWithTag(GameConstants.NEUTRAL_TAG);
		foreach (GameObject go in neutrals)
		{
			if (go.GetComponent<PlayerStats>() != null)
				gotObjects.Add(go.GetComponent<PlayerStats>());
		} */
		return gotObjects;
	}

}


