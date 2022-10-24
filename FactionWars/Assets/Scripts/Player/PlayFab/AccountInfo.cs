using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class AccountInfo : MonoBehaviourPunCallbacks
{
	public GameObject trophieAmmount;
	public AccountStats accountStats;
	public LoginManager loginManager;
	[SerializeField]
	private string[] deckInfo;
	private static AccountInfo instance;
	[SerializeField]
	private GetPlayerCombinedInfoResultPayload info;
	[SerializeField]
	private List<CardStats> cards = new List<CardStats>();
	[SerializeField]
	private List<CardStats> deck = new List<CardStats>();

	public string setUsername;
	public string setPassword;

	public GameOver gameOver;

	private string playFabId = "";
	public GetPlayerCombinedInfoResultPayload Info
	{
		get { return info; }
		set { info = value; }
	}

	public static List<CardStats> Cards
	{
		get { return Instance.cards; }
		set { Instance.cards = value; }
	}
	public static List<CardStats> Deck
	{
		get { return Instance.deck; }
		set { Instance.deck = value; }
	}


	public static AccountInfo Instance
	{
		get { return instance; }
		set { instance = value; }
	}

	private void Awake()
	{
		if (instance != this)
			instance = this;
		DontDestroyOnLoad(gameObject);
	}

	private void Update()
	{
		if (Info != null && Cards.Count == 0 && database.Updated) //  29.4 changed from instance.cards.count // 3.5 was Cards.Count
		{
		//	Debug.Log("adding card");
			AddCards();
		}
	}


	public static void Register(string username, string email, string password)
	{
		RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest()
		{
			DisplayName = username,
			TitleId = PlayFabSettings.TitleId,
			Email = email,
			Username = username,
			Password = password
		};

		PlayFabClientAPI.RegisterPlayFabUser(request, OnRegister, OnAPIError);

	}

	public static void Login(string username, string password)
	{
		Instance.setUsername = username;
		Instance.setPassword = password;
		LoginWithPlayFabRequest request = new LoginWithPlayFabRequest()
		{
			TitleId = PlayFabSettings.TitleId,
			Username = username,
			Password = password
		};

		PlayFabClientAPI.LoginWithPlayFab(request, OnLogin, OnAPIError);
	}

	static void OnRegister(RegisterPlayFabUserResult result)
	{
		Instance.SetUpAccount();
		Debug.Log("Registered with:" + result.PlayFabId);
	}
	static void OnLogin(LoginResult result)
	{
		Debug.Log("Login with:" + result.PlayFabId);
		Instance.playFabId = result.PlayFabId;
		GetAccountInfo(result.PlayFabId); // added: result.PlayFabId 10.4
		instance.GetTrophies();
		database.UpdateDatabase();
		PhotonNetwork.ConnectUsingSettings();
		/*	GetPhotonAuthenticationTokenRequest request = new GetPhotonAuthenticationTokenRequest()
			{
				PhotonApplicationId = "09d9f56d - 8fbb - 42e7 - 95c6 - 8061a1bd96f0"


			};
			PlayFabClientAPI.GetPhotonAuthenticationToken(request, OnPhotonAuthSuccess, GameFunctions.OnAPIError); 
			//PhotonNetwork.ConnectUsingSettings(); */
		levelManager.LoadLevel(GameConstants.MAIN_SCENE);
	}

	public void GetTrophies()
	{
		PlayFabClientAPI.GetPlayerStatistics(
			new GetPlayerStatisticsRequest(),
			OnGetStatistic,
			error => Debug.Log(error.GenerateErrorReport())
			);
	}

	void OnGetStatistic(GetPlayerStatisticsResult result)
	{
		Debug.Log("received the following statistis:");
		foreach (var eachStat in result.Statistics)
			accountStats.trophies = eachStat.Value;
			//Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
	}

	static void OnPhotonAuthSuccess(GetPhotonAuthenticationTokenResult result)
	{
		AuthenticationValues customAuth = new AuthenticationValues();
		Debug.Log("OnPhotonAuthSuccess" + Instance.playFabId);
		customAuth.UserId = instance.playFabId;
		customAuth.AuthType = CustomAuthenticationType.Custom;
		customAuth.AddAuthParameter("username", Instance.playFabId);
		customAuth.AddAuthParameter("Token", result.PhotonCustomAuthenticationToken);
		//	customAuth.Token = result.PhotonCustomAuthenticationToken;

		PhotonNetwork.AuthValues = customAuth;
		PhotonNetwork.ConnectUsingSettings(); // PhotonNetwork.ConnectUsingSettings(GameConstants.VERSION);
	}

	public static void GetAccountInfo()
	{
		GetPlayerCombinedInfoRequestParams paramInfo = new GetPlayerCombinedInfoRequestParams()
		{
			GetTitleData = true,
			GetUserInventory = true,
			GetUserAccountInfo = true,
			GetUserVirtualCurrency = true,
			GetPlayerProfile = true,
			GetPlayerStatistics = true,
			GetUserData = true,
			GetUserReadOnlyData = true
		};
		Debug.Log("GetAccountinfo. no parameter");
		GetPlayerCombinedInfoRequest request = new GetPlayerCombinedInfoRequest()
		{
			PlayFabId = Instance.info.AccountInfo.PlayFabId,
			InfoRequestParameters = paramInfo
		};
		PlayFabClientAPI.GetPlayerCombinedInfo(request, OnAccountInfo, GameFunctions.OnAPIError);
	}
	public static void GetAccountInfo(string playfabId)
	{
		GetPlayerCombinedInfoRequestParams paramInfo = new GetPlayerCombinedInfoRequestParams()
		{
			GetTitleData = true,
			GetUserInventory = true,
			GetUserAccountInfo = true,
			GetUserVirtualCurrency = true,
			GetPlayerProfile = true,
			GetPlayerStatistics = true,
			GetUserData = true,
			GetUserReadOnlyData = true
		};
		Debug.Log("GetAccountinfo");
		GetPlayerCombinedInfoRequest request = new GetPlayerCombinedInfoRequest()
		{
			PlayFabId = playfabId,
			InfoRequestParameters = paramInfo
		};
		PlayFabClientAPI.GetPlayerCombinedInfo(request, OnAccountInfo, GameFunctions.OnAPIError);
	}

	public static void UpdateLeaderBoards(string key)
	{
		GetLeaderboardRequest request = new GetLeaderboardRequest()
		{
			StatisticName = key,
			StartPosition = 0
		};
		PlayFabClientAPI.GetLeaderboard(request, GotLeaderBoards, GameFunctions.OnAPIError);

	}



	public static void GotLeaderBoards(GetLeaderboardResult result)
	{
		UiManager.LeaderBoardEntries = result.Leaderboard;
	}


	public static void AddToDeck()
	{
		string deckContents = "";
		//int index = 0;
		foreach (CardStats item in Deck)
		{
			Debug.Log(item.Name + "addding to deck");
		//	if(index < Deck.Count - 1)
			deckContents += item.Name + ",";
		//	index++;
		}
		deckContents.TrimEnd(',');

		Dictionary<string, string> data = new Dictionary<string, string>
		{
			{ GameConstants.DATA_DECK, deckContents }
		};
		UpdateUserDataRequest request = new UpdateUserDataRequest()
		{
			Data = data
		};
		PlayFabClientAPI.UpdateUserData(request, GotData, GameFunctions.OnAPIError);
	}

	private static void GotData(UpdateUserDataResult result)
	{
		Debug.Log("Updated data!");
		//AddDeckInfo();
	}

	static void OnAccountInfo(GetPlayerCombinedInfoResult result)
	{
		Instance.Info = result.InfoResultPayload;
		
		AddCards();
		Debug.Log("Updated account info :" + instance.info.UserInventory.Count);
	}

	void SetUpAccount()
	{
		Dictionary<string, string> data = new Dictionary<string, string>
		{
			{ GameConstants.DATA_EXP, "1" },
			{ GameConstants.DATA_MAX_EXP, "100" },
			{ GameConstants.DATA_LEVEL, "1" }
		};


		UpdateUserDataRequest request = new UpdateUserDataRequest()
		{
			Data = data
		};
		PlayFabClientAPI.UpdateUserData(request, UpdateDataInfo, GameFunctions.OnAPIError);
	}

	void UpdateDataInfo(UpdateUserDataResult result)
	{
		Debug.Log("UpdateDataInfo");
		List<StatisticUpdate> stats = new List<StatisticUpdate>();
		StatisticUpdate trophies = new StatisticUpdate
		{
			StatisticName = GameConstants.STAT_TROPHIES,
			Value = 0
		};
		stats.Add(trophies);

		UpdatePlayerStatisticsRequest request = new UpdatePlayerStatisticsRequest()
		{
			Statistics = stats
		};
		PlayFabClientAPI.UpdatePlayerStatistics(request, UpdateStatInfo, GameFunctions.OnAPIError);
	}

	void UpdateStatInfo(UpdatePlayerStatisticsResult result)
	{
		Debug.Log("UpdateStatInfo");
	}

	static void AddCards()
	{
		Debug.Log("AddCards: Instance.Info.UserInventory.Count" + Instance.Info.UserInventory.Count);
		for (int i = 0; i < Instance.Info.UserInventory.Count; i++)
		{
		//	Debug.LogError("Instance.Info.UserInventory[i].ItemClass:" + Instance.Info.UserInventory[i].ItemClass);
			if (Instance.Info.UserInventory[i].ItemClass == GameConstants.ITEM_CARDS)
			{
				Debug.Log("Add Cards" + Instance.Info.UserInventory[i]);
				Cards.Add(database.GetCardInfo(Instance.Info.UserInventory[i], i));
			}
		}
		AddDeckInfo();

		/*UserDataRecord temp; // this is not working
		if (Instance.Info.UserData.TryGetValue(GameConstants.DATA_DECK, out temp))
		{
			//	if (temp == null)
			//		return;
				Instance.deckInfo = temp.Value.Split(',');
				Debug.Log("deckinfo: " + instance.deckInfo[0]);
				Debug.Log("deckinfo: " + instance.deckInfo[1]);
				Debug.Log("deckinfo: " + instance.deckInfo[2]);
				Debug.Log("deckinfo: " + instance.deckInfo[3]);
				Debug.Log("deckinfo: " + instance.deckInfo[4]);
				Debug.Log("deckinfo: " + instance.deckInfo[5]);
				Debug.Log("deckinfo: " + instance.deckInfo[6]);
				Debug.Log("deckinfo: " + instance.deckInfo[7]);
				Debug.Log("deckinfo: " + instance.deckInfo[8]);
				for (int i = 0; i < Instance.deckInfo.Length - 1; i++)
				{
					for (int j = 0; j < Instance.Info.UserInventory.Count; j++)
					{
						if(Instance.deckInfo[i] == Instance.Info.UserInventory[j].ItemId)
						{
							Debug.Log("adding to deck:" + Instance.Info.UserInventory[j]);
							Deck.Add(database.GetCardInfo(Instance.Info.UserInventory[j], j));
							break ;
						}

					}
				}
			}
			else
			{
				Debug.Log("failed to get deck");
			}*/
	}

	static void AddDeckInfo() // this adds extra card when confirming the deck
	{
		for (int i = 0; i < Deck.Count; i++)
		{
			//DeckSlot = null;
		}
		Debug.Log("add deck info");
		// this is not working
		if (Instance.Info.UserData.TryGetValue(GameConstants.DATA_DECK, out UserDataRecord temp))
		{

			string temp1 = temp.Value.TrimEnd(',');
			Instance.deckInfo = temp1.Split(',');

			for (int i = 0; i < Instance.Info.UserInventory.Count; i++) //  Instance.Info.UserInventory.Count
			{
				//if (Instance.deckInfo[i] == null)
				//	return;
				for (int j = 0; j < Instance.Info.UserInventory.Count; j++)
				{
					if (Instance.deckInfo[i] == Instance.Info.UserInventory[j].ItemId) // ItemId
					{
					//	Debug.Log("")
						Debug.Log("adding to deck:" + Instance.Info.UserInventory[j]);
						//if (Deck[i].InDeck == -1)
						//if(Deck[j] == null)
							Deck.Add(database.GetCardInfo(Instance.Info.UserInventory[j], j));
						break;
					}
					else
					{
						Debug.Log("ERROR: Instance.deckInfo[i] == Instance.Info.UserInventory[j].ItemId");
					//	break;
					}
				}
			}
		}
		else
		{
			Debug.Log("failed to get deck");
		}
	}



	public override void OnConnectedToMaster()
	{
		Debug.Log("Connected to " + PhotonNetwork.CloudRegion + " server");
	}

	public static void OnAPIError(PlayFabError error)
	{
		Debug.Log("error:" + error.Error);
		if (error.ErrorMessage == "The display name entered is not available.")
			instance.SendSystemMessage("Username is already taken");
		if (error.Error == PlayFabErrorCode.EmailAddressNotAvailable)
			instance.SendSystemMessage("Email is already taken");
		if (error.Error == PlayFabErrorCode.InvalidEmailOrPassword)
			instance.SendSystemMessage("Invalid username or password");
		if (error.ErrorMessage == "Invalid username or password")
			instance.SendSystemMessage(" Invalid username or password");
		if(error.HttpCode == 1007)
			instance.SendSystemMessage("Invalid Username ");
		Debug.LogError(error);
	}

	public void SetTrophies(int trophies)
	{
		//trophieAmmount.transform.GetComponent<Text>().text = trophies.ToString();
	}

	

	public void SendSystemMessage(string message)
	{
		loginManager.SystemMessage(message);
	}
}

