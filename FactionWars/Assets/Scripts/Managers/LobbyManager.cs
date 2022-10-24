using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;



public class LobbyManager : MonoBehaviourPunCallbacks
{

	[SerializeField]
	private GameObject quickStartButton;
	[SerializeField]
	private GameObject quickCancelButton;
	[SerializeField]
	private int roomSize = 2;

	

	private static  LobbyManager instance;

	public static LobbyManager Instance
	{
		get { return  instance; }
		set { instance = value; }
	}

	public UiManager uiManager;


	public string _roomName;
	public int roomId;

	[SerializeField]
	bool battleStarting = false;

	[SerializeField]
	bool firstLoad = false;
	[SerializeField]
	float currTime = 0;
	[SerializeField]
	AccountStats myPlayer;
	[SerializeField]
	List<AccountStats> players = new List<AccountStats>();
	[SerializeField]
	private int playerCount;

	public AccountInfo info;

	public void QuickStart()
	{
		if (UiManager.CardsInDeck != 0)
		{
			quickStartButton.SetActive(false);
			quickCancelButton.SetActive(true);
			PhotonNetwork.JoinRandomRoom();
			Debug.Log("QuickStart");
		}
		else
			uiManager.SystemMessage("No cards in deck");
	}
	public static List<AccountStats> Players
	{
		get { return Instance.players; }
		set { Instance.players = value; }
	}

	private void Awake()
	{
		info = FindObjectOfType<AccountInfo>();
		roomId = 1;
		if (instance != this)
			instance = this;
	}



		List<AccountStats> GetPlayers()
	{
		List<AccountStats> gotPlayers = new List<AccountStats>();

		GameObject[] gos = GameObject.FindGameObjectsWithTag(GameConstants.PHOTON_PLAYER);
		foreach (GameObject go in gos)
		{
			gotPlayers.Add(go.GetComponent<AccountStats>());
		}

		return gotPlayers;
	}

	private void Update()
	{

		if (battleStarting)
			PlayerCountUpdate();
	/*	if (!firstLoad)
		{
			
			players = GetPlayers();
			if (PhotonNetwork.connectionState == ConnectionState.Connected && PhotonNetwork.insideLobby)
			{
				Debug.Log("connection state: " + PhotonNetwork.connectionState);
				RoomOptions roomOptions = new RoomOptions()
				{
					IsVisible = true,
					MaxPlayers = 20 // 28.8 was 20
				};
			//	if (PhotonNetwork.JoinRandomRoom())
			//		Debug.Log("Joined random room");
			//	else
			//	{
				//	_roomName = AccountInfo.Instance.setUsername;
				
				if (PhotonNetwork.JoinOrCreateRoom(GameConstants.ROOM_ONE, roomOptions, TypedLobby.Default))
					Debug.Log("created room:" + roomId);

				// 29.8 was PhotonNetwork.JoinOrCreateRoom(GameConstants.ROOM_ONE, roomOptions, TypedLobby.Default);
				Debug.Log("count of rooms:" + PhotonNetwork.countOfRooms);
				Debug.Log("count of players in room:" + PhotonNetwork.countOfPlayers);
				
			//	}
			}
			else if(PhotonNetwork.connectionState == ConnectionState.Connected)
			{
				PhotonNetwork.JoinLobby();
				
				Debug.Log("is in lobby: " + PhotonNetwork.InLobby);
			}
		}
		else
		{
			players = GetPlayers();
			if (players == null)
				Debug.Log("Players not found");
			if (myPlayer.looking)
			{
				if (currTime < GameConstants.LOOKING_TIMER)
				{
					currTime += Time.deltaTime;
					AccountStats acc = (GameFunctions.FoundPlayer(myPlayer.trophies, players.ToArray()));
					if (acc != null)
					{
						
						RoomOptions roomOptions = new RoomOptions()
						{
							
							IsVisible = true,
							MaxPlayers = 2 // 28.8 was 20
						};
						//PhotonNetwork.LeaveRoom();
						
						if (PhotonNetwork.JoinOrCreateRoom("GameArea" + roomId, roomOptions, TypedLobby.Default))
							Debug.Log("joinOrcreated gamearea" + roomId);
						string roomName = "GameArea";
						//Debug.Log("roomName: " + roomName);
						acc.gameObject.GetComponent<PhotonView>().RPC("ChangeRoomName", PhotonTargets.All, roomName+roomId);
						if(!myPlayer.inGame)
							myPlayer.levelName = roomName+roomId; // move hgher
															  //levelManager.LoadLevel(GameConstants.GAME_SCENE);
						
					}
				}
				else
				{
					myPlayer.looking = false;
					currTime = 0;
				}
			}
			else if (myPlayer == null)
				return;
		}*/
	}

	private void PlayerCountUpdate()
	{
		playerCount = PhotonNetwork.PlayerList.Length;
		//roomSize = PhotonNetwork.CurrentRoom.MaxPlayers; // useless, since alway 2?

		if (playerCount == roomSize)
		{
			Debug.Log("playerCount == roomSize: Ready to start");
			myPlayer.startGame = true;
		}
	}


	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		Debug.Log("failed to join room");
		//base.OnJoinRoomFailed(returnCode, message);
		CreateRoom();
	}

	

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.Log("failed to join room" + message + returnCode);
		CreateRoom();
	//	base.OnJoinRandomFailed(returnCode, message);
	}

	void CreateRoom()
	{
		Debug.Log("Creating room");
		int randomRoomNumber = Random.Range(0, 10000);
		RoomOptions roomOps = new RoomOptions()
		{
			IsVisible = true,
			IsOpen = true,
			MaxPlayers = (byte)roomSize,

		};
		PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOps);
		Debug.Log("created room: " + randomRoomNumber);
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		Debug.Log("failed to create room.. trying again");
		CreateRoom();
		//base.OnCreateRoomFailed(returnCode, message);
	}

	public void QuickCancel()
	{
		quickCancelButton.SetActive(false);
		quickStartButton.SetActive(true);
		PhotonNetwork.LeaveRoom();
	}

	/*void OnPhotonJoinRoomFailed()
	{
		Debug.Log("OnPhotonRoomFailed");
		Debug.Log("count of rooms:" + PhotonNetwork.countOfRooms);
		Debug.Log("count of players in room:" + PhotonNetwork.countOfPlayers);
		//PhotonNetwork.CreateRoom(GameConstants.ROOM_ONE);
	} */
	
	public override void OnJoinedRoom()
	{
		Debug.Log("LOBBY MANAGER: OnJoinedRoom: " + PhotonNetwork.CurrentRoom);
		Debug.Log("count of rooms:" + PhotonNetwork.CountOfRooms);
		Debug.Log("count of players in room:" + PhotonNetwork.CountOfPlayers);
		firstLoad = true;
		GameObject go = PhotonNetwork.Instantiate(GameConstants.PHOTON_PLAYER, Vector3.zero, Quaternion.identity, 0);
		AccountStats stats = go.GetComponent<AccountStats>();
		//stats.levelName = GameConstants.ROOM_ONE;
		//stats.levelName = "Room " + roomId; // this starts the game, Why =? 29.8
		//Debug.Log("onjoined levelname: " + stats.levelName);
		go.name = AccountInfo.Instance.Info.AccountInfo.PlayFabId;
		stats.me = true;
		stats.trophies = AccountInfo.Instance.Info.PlayerStatistics[0].Value;

		UserDataRecord record = new UserDataRecord();
		if (AccountInfo.Instance.Info.UserData.TryGetValue(GameConstants.DATA_LEVEL, out record))
		{
			stats.level = int.Parse(record.Value);
		}
		else
			Debug.Log("Error: Try get values");
		myPlayer = stats;
		players.Add(stats);
		battleStarting = true;
		//StateMachine.ChangeState();
		//myPlayer.ChangeRoomName("hello");
	}

	public void OnConnectedToPhoton()
	{
		Debug.Log("OnConnectedToPhoton");
	}

	public override void OnLeftRoom()
	{
		Debug.LogError("Left room");
	}



	public void OnPhotonCreateRoomFailed(object[] codeAndMsg)
	{
		Debug.Log("OnPhotonCreateRoomFailed: " + codeAndMsg);
		//base.OnCreateRoomFailed(codeAndMsg);
		
	}



/*	public void OnPhotonJoinRoomFailed(object[] codeAndMsg)
	{
		Debug.Log("failed to join room" + codeAndMsg);
		Debug.Log("count of rooms:" + PhotonNetwork.CountOfRooms);
		Debug.Log("count of players in room:" + PhotonNetwork.CountOfPlayers);
		if(PhotonNetwork.CountOfPlayers % 2 != 0)
			roomId = roomId + 1;
		Debug.Log("roomid: " + roomId );
		if(PhotonNetwork.countOfRooms == 1) 
		{
			if (PhotonNetwork.connectionState == ConnectionState.Connected && PhotonNetwork.insideLobby)
			{
				
				RoomOptions roomOptions = new RoomOptions()
				{
					IsVisible = true,
					MaxPlayers = 2 // 28.8 was 20
				};
				Debug.Log("creating ROOM TWO");
				PhotonNetwork.JoinOrCreateRoom(GameConstants.ROOM_TWO, roomOptions, TypedLobby.Default); // 29.8 was PhotonNetwork.JoinOrCreateRoom(GameConstants.ROOM_ONE, roomOptions, TypedLobby.Default);
				Debug.Log("count of rooms:" + PhotonNetwork.countOfRooms);
				Debug.Log("count of players in room:" + PhotonNetwork.countOfPlayers);
			}
		} 
	}*/

	public void OnCreatedRoom()
	{
		Debug.Log("Created room:" + this);
		Debug.Log("count of rooms:" + PhotonNetwork.CountOfRooms);
		Debug.Log("count of players in room:" + PhotonNetwork.CountOfPlayers);
	}

	public void OnJoinedLobby()
	{
		Debug.Log("OnJoinedLobby");
		Debug.Log("count of rooms:" + PhotonNetwork.CountOfRooms);
		Debug.Log("count of players in room:" + PhotonNetwork.CountOfPlayers);
		//OnJoinedRoom();
	}

	public void OnLeftLobby()
	{
		Debug.Log("OnLeftLobby");
	}

	public void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.Log("OnFailedToConnectToPhoton:" + cause);
		GetPlayers();
	}

	public void OnConnectionFail(DisconnectCause cause)
	{
		Debug.Log("OnConnectionFail");
	}

	public void OnDisconnectedFromPhoton()
	{
		//OnJoinedRoom();
		Debug.Log("OnDisconnectedFromPhoton");

	}


	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		Debug.Log("OnPhotonInstantiate:" + info);
	}

	public void OnReceivedRoomListUpdate()
	{
		Debug.Log("OnReceivedRoomListUpdate");
	}

	/*public void IPunCallbacks.OnJoinedRoom()
	{
		Debug.Log("IPunCallbacks. OnJoinedRoom"); /
	} */

/*	public void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		Debug.Log("OnPhotonPlayerConnected:" + newPlayer);
	} */

/*	public void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{
		Debug.Log("photon player disconnected:" + otherPlayer);
	}*/

	public void OnPhotonRandomJoinFailed(object[] codeAndMsg)
	{
		throw new System.NotImplementedException();
	}

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.AutomaticallySyncScene = true;
		quickStartButton.SetActive(true);
		Debug.Log("OnConnectedToMaster");
	}

	public void OnPhotonMaxCccuReached()
	{
		throw new System.NotImplementedException();
	}

	public void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
	{
		Debug.Log("OnPhotonCustomRoomPropertiesChanged");
	}

	public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
	{
		Debug.Log("OnPhotonPlayerPropertiesChanged");
	}

	public void OnUpdatedFriendList()
	{
		throw new System.NotImplementedException();
	}

	public void OnCustomAuthenticationFailed(string debugMessage)
	{
		throw new System.NotImplementedException();
	}

	public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
	{
		throw new System.NotImplementedException();
	}

	public void OnWebRpcResponse(OperationResponse response)
	{
		throw new System.NotImplementedException();
	}

	public void OnOwnershipRequest(object[] viewAndPlayer)
	{
		throw new System.NotImplementedException();
	}

	public void OnLobbyStatisticsUpdate()
	{
		Debug.Log("OnLobbyStatisticsUpdate");
	}

/*	public void OnPhotonPlayerActivityChanged(PhotonPlayer otherPlayer)
	{
		throw new System.NotImplementedException();
	} */

	public void OnOwnershipTransfered(object[] viewAndPlayers)
	{
		throw new System.NotImplementedException();
	} 
}
