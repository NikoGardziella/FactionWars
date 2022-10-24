using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using PlayFab;
using PlayFab.ClientModels;
using Photon.Pun;


public class GameOver : MonoBehaviour
{
	public AccountStats accountStats;
	public GameObject gameOverObject;
	public Text endGameText;
	public AccountInfo info;
	public LobbyManager lobbyManager;
	public string username;
	public string password;

	private void Awake()
	{
		//lobbyManager = FindObjectOfType<LobbyManager>();

		info = FindObjectOfType<AccountInfo>(); // 10.4 removed GameObject.
		accountStats = FindObjectOfType<AccountStats>();
		
	}

	/*	[SerializeField]
		private Text endGameText;
		public Text EndGameText
		{
			get { return endGameText; }
			set { endGameText = value; }
		}

		[SerializeField]
	/	private GameObject gameOverObject;
		public GameObject GameOverObject
		{
			get { return gameOverObject; }
			set { gameOverObject = value; }
		} */

		public void GameOverButton()
		{
		Debug.Log("GameOverBUtton pressed");
		//PlayFabClientAPI.ForgetAllCredentials();
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.LeaveLobby();
			 //lobbyManager.OnLeftLobby();
			//lobbyManager.OnJoinedRoom();
		//AccountInfo.Login(info.setUsername, info.setPassword);
		/*	gameObject.SetActive(false);
			gameOverObject.SetActive(false);
			endGameText.gameObject.SetActive(false);
			accountStats.inGame = false;
			StateMachine.ChangeState(); */
		accountStats.inGame = false;
	//accountInfo.OnLogin(result);
			levelManager.LoadLevel(GameConstants.MAIN_SCENE);
		} 

	public void YouLost()
	{
		Debug.Log("you lost");
		gameOverObject.SetActive(true);
		endGameText.gameObject.SetActive(true);
		endGameText.text = "You Lost";

	}

	public void YouWon()
	{
		Debug.Log("you won");
		gameOverObject.SetActive(true);
		if (gameOverObject.activeInHierarchy)
			Debug.Log("Game over object is active");
		else
		{
			Debug.Log("Game over object is NOT active");
		}
		if (gameOverObject)
			Debug.Log("game over object found");
		else
			Debug.Log("game over object NOT found");
		gameObject.SetActive(true);
		//gameObject.transform.parent.gameObject.SetActive(true);
		accountStats.trophies += 1;
		Debug.Log("trophies: "+ accountStats.trophies);
		UpdateTrophies();
		info.SetTrophies(accountStats.trophies);
		endGameText.gameObject.SetActive(true);
		endGameText.text = "You Won";
	}

	public void UpdateTrophies()
	{
		List<StatisticUpdate> newUpdate = new List<StatisticUpdate>
		{
			new StatisticUpdate
			{
				StatisticName="Trophies",
				Value = accountStats.trophies
			}
		};

	PlayFabClientAPI.UpdatePlayerStatistics(
	new UpdatePlayerStatisticsRequest
	{
		
		Statistics = newUpdate
	},
	onSuccessCallback => { Debug.Log("Trophies updated"); },
	onFailureCallback => { Debug.Log("Trophies update error"); }
);
	}

}

