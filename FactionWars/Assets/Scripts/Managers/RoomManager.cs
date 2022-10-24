using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;


public class RoomManager : MonoBehaviourPunCallbacks
{
	[SerializeField]
	private int multiplayerSceneIndex;
	[SerializeField]
	private int playerCount;
	[SerializeField]
	private int minPlayerToStart;
	[SerializeField]
	private Text timerToStartDispaly;
	[SerializeField]
	private int roomSize;

	public AccountStats acc;

	public PhotonView myPhotonView;

	private bool readyToCountDown;
	private bool readyToStart;
	private bool startingGame;

	private float timerToStartGame;
	private float notFullGameTimer;
	private float fullGameTimer;

	[SerializeField]
	private int maxWaitTime;
	[SerializeField]
	private float maxFullGameWaitTime;

	private void Start()
	{
		fullGameTimer = maxFullGameWaitTime;
		notFullGameTimer = maxWaitTime;
		timerToStartGame = maxWaitTime;
		PlayerCountUpdate();
		acc = FindObjectOfType<AccountStats>();
	}

	private void PlayerCountUpdate()
	{
		playerCount = PhotonNetwork.PlayerList.Length;
		//roomSize = PhotonNetwork.CurrentRoom.MaxPlayers; // useless, since alway 2?

		if (playerCount == roomSize)
		{
			Debug.Log("playerCount == roomSize: Ready to start");
			readyToStart = true;
			StartGame();
		}
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		PlayerCountUpdate();
		if (PhotonNetwork.IsMasterClient)
			myPhotonView.RPC("RPC_SendTimer", RpcTarget.Others, timerToStartGame);
		//base.OnPlayerEnteredRoom(newPlayer);
	}
	[PunRPC]
	private void RPC_SendTimer(float timeIn)
	{
		timerToStartGame = timeIn;
		notFullGameTimer = timeIn;
		if(timeIn < fullGameTimer)
		{
			fullGameTimer = timeIn;
		}
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		PlayerCountUpdate();
	}

	private void Update()
	{
		//WaitingForMorePlayer();
	}

	void WaitingForMorePlayer()
	{
		if(playerCount <= 1)
		{
			ResetTimer();
		}
		if (readyToStart)
		{
			fullGameTimer -= Time.deltaTime;
			timerToStartGame = fullGameTimer;
		}
		else if (readyToCountDown)
		{
			notFullGameTimer -= Time.deltaTime;
			timerToStartGame = notFullGameTimer;
		}
		if(timerToStartGame <= 0f)
		{
			if (startingGame)
				return;
			StartGame();
		}		
	}

	void ResetTimer()
	{
		timerToStartGame = maxWaitTime;
		notFullGameTimer = maxWaitTime;
		fullGameTimer = maxFullGameWaitTime;
	}


	public override void OnJoinedRoom()
	{
		Debug.Log("ROOM MAANGER: joined room" + PhotonNetwork.CurrentRoom.Name);
		StartGame();
	}

	private void StartGame()
	{
		startingGame = true;
		if (!PhotonNetwork.IsMasterClient)
		{
			Debug.LogError("not master client");
			//return;
		}
		//PhotonNetwork.CurrentRoom.IsOpen = false;
		if (readyToStart)
		{
			//acc.startGame = true;
			Debug.Log("change state from RoomManager.cs");
			//StateMachine.ChangeState();
		}
		else
			Debug.Log("Ready to start:" + readyToStart);
	}
}
