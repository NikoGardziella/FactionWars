using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
	[SerializeField]
	List<GameObject> managers;
	[SerializeField]
	GameConstants.MANAGER_STATE state;

	private static StateMachine instance;

	public static StateMachine Instance
	{
		get { return instance; }
		set { instance = value; }
	}
	private void Awake()
	{
		if (instance != this)
			instance = this;
	}
	public static void ChangeState()
	{
		print("state changed");
		Instance.state = Instance.state == GameConstants.MANAGER_STATE.IN_GAME
			? GameConstants.MANAGER_STATE.LOBBY 
			: GameConstants.MANAGER_STATE.IN_GAME;
	}

	private void Update()
	{
		switch (state)
		{
			case GameConstants.MANAGER_STATE.LOBBY:
				{
					GameFunctions.ChangeMenu(managers.ToArray(), 0);
					break;
				}
			case GameConstants.MANAGER_STATE.IN_GAME:
				{
					GameFunctions.ChangeMenu(managers.ToArray(), 1);
					break;
				}
		}
	}


}
