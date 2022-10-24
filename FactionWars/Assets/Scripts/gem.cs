using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gem : MonoBehaviour
{
	public PlayerStats playerStats;
	int reward = 5;

	private void Start()
	{
		playerStats = gameObject.GetComponent<PlayerStats>();
	}

	/*void OnMouseDown()
	{
		playerStats.AddResource(reward);
		Destroy(gameObject);
	}*/
	private void Update()
	{
	/*	if (Input.GetMouseButtonDown(0))
		{

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			Vector3 origin = Camera.main.transform.position;
			Vector3 direction = Camera.main.transform.forward;
			//	Ray ray = new Ray(origin, direction);
			Debug.DrawRay(origin, direction * 100f, Color.red);
			if (Physics.Raycast(ray, out RaycastHit hit))
			{
				if (hit.transform.CompareTag("gem"))
				{
					Debug.Log("hit gem gem.c");
					playerStats.AddResource(reward);
					Debug.Log("Resource added");
					
				}
				//Destroy(hit.transform.gameObject);
			}
		} */
	}



}
