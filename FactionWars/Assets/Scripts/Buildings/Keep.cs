using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Keep : Structure
{
	public GameOver gameOver;
	public PlayerStats playerStats;
	public GameObject photonPlayer;
	private void Awake()
	{
		gameOver = FindObjectOfType<GameOver>();
	//	photonPlayer = GameObject.FindGameObjectWithTag("PhotonPlayer");
	//	gameOver = photonPlayer.gameObject.GetComponent<GameOver>();
	}

	void OnDestroy()
	{
		gameOver = FindObjectOfType<GameOver>();
		Debug.Log("keep has died");
		if (!gameObject.CompareTag("Player"))
		{
			Debug.Log("Player keep has died");
			gameOver.YouWon();
		}
		else if (gameObject.CompareTag("Player"))
		{
			Debug.Log("Enemy keep has died");
			gameOver.YouLost();
			
		}

	}


	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(Stats.CurrentHealth);
			stream.SendNext(Stats.HealthBar.fillAmount);
			stream.SendNext(LeftTower);
		}
		else
		{
			Stats.CurrentHealth = (float)stream.ReceiveNext();
			Stats.HealthBar.fillAmount = (float)stream.ReceiveNext();
			LeftTower = (bool)stream.ReceiveNext();
		}
	}

}
