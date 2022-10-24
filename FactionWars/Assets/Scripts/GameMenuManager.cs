using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameMenuManager : MonoBehaviour
{
	public AccountStats accountStats;
	public GameObject gameMenu;
	//public GameObject closeMenu;
	public GameObject goMainMenuConfirm;
	public GameObject confirmYes;
	public GameObject confirmNo;


	private void Awake()
	{
		accountStats = FindObjectOfType<AccountStats>();
	}
	public void OpenGameMenu()
	{
		gameMenu.SetActive(true);
	}

	public void CloseGameMenu()
	{
		gameMenu.SetActive(false);
	}

	public void goMainMenu()
	{
		goMainMenuConfirm.SetActive(true);
	}

	public void ConfirmYes()
	{
		gameMenu.SetActive(false);
		goMainMenuConfirm.SetActive(false);
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.LeaveLobby();
		accountStats.inGame = false;
		SceneManager.LoadScene(GameConstants.MAIN_SCENE);
	}

	public void ConfirmNo()
	{
		goMainMenuConfirm.SetActive(false);
		
	}

}
