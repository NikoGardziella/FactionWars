using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AccountStats : MonoBehaviour
{
	public AccountInfo accountInfo;
	public bool me = false;
	public bool looking = false;
	public int trophies; // Match Makng Rating
	public int level = 1;
	public bool inGame = false;
	public bool startGame = false;
	public string levelName = GameConstants.ROOM_ONE;
	public string myUsername;
	public RoomManager myRoomManager;

	private void Awake()
	{
		accountInfo = FindObjectOfType<AccountInfo>();
		myUsername = accountInfo.setUsername; // remove ?
	}

	private void LateUpdate()
	{
		//levelName = GameConstants.ROOM_ONE;
		if(levelName != GameConstants.ROOM_ONE && !inGame && me && startGame) // 9,.5 level !=""
		{
			
			inGame = true;
			//Debug.Log("Loaded levelname: " + levelName);
			Debug.Log("change state from Accountstats");
			StateMachine.ChangeState();
			//levelManager.LoadLevel(GameConstants.GAME_SCENE);
		}


	}
	
	[PunRPC]
	public void ChangeRoomName(string roomName)
	{
		levelName = roomName;
	//	Debug.Log("changin room to:" + roomName);
	//	StateMachine.ChangeState();
		//levelManager.LoadLevel(GameConstants.GAME_SCENE);
	}

	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			
			//Debug.Log("Stream is writingh");
			// Me. Send my data to other players
			stream.SendNext(looking);
			stream.SendNext(trophies);
			stream.SendNext(level);
			stream.SendNext(levelName);
		}
		else
		{
			//Other network player. Receive data
			looking = (bool)stream.ReceiveNext();
			trophies = (int)stream.ReceiveNext();
			level = (int)stream.ReceiveNext();
			levelName = (string)stream.ReceiveNext();
			//Debug.Log("Stream is receivinng");
		}
	}
}
