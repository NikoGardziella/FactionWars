using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;


public class UiManager : MonoBehaviour
{
	// system message
	public GameObject sysMsg;
	public Text sysMsgText;
	//profile menu

	public AccountInfo info;
	[SerializeField]
	private List<GameObject> menus;
	[SerializeField]
	private List<GameObject> profileMenus;
	//[SerializeField]
	//GameObject leadBoardmenus;
	[SerializeField]
	private List<GameObject> leaderBoardMenus;
	[SerializeField]
	private static UiManager instance;
	[SerializeField]
	private Text level;
	[SerializeField]
	private Text gems;
	[SerializeField]
	private Text coins;
	[SerializeField]
	private Text playerName;
	[SerializeField]
	private Text trophies;
	[SerializeField]
	private Image exp;
	[SerializeField]
	private List<Toggle> menuToggle;

	[SerializeField]
	private List<GameObject> leaderboardGameObjects;
	[SerializeField]
	private List<PlayerLeaderboardEntry> leaderboardEntries = new List<PlayerLeaderboardEntry>();
	[SerializeField]
	private string leaderBoardKey = "";
	public List<Toggle> MenuToggle
	{
		get { return Instance.menuToggle; }
		set { Instance.menuToggle = value; }
	}


	//Shop menu
	[SerializeField]
	private List<GameObject> storeContents;


	//deck  menu	
	[SerializeField]
	private List<GameObject> inventoryContents;
	[SerializeField]
	private List<GameObject> deckContents;
	[SerializeField]
	private Text avgCost;
	[SerializeField]
	private float currentCost;
	[SerializeField]
	private int currentCount;
	[SerializeField]
	private int cardsInDeck;
	[SerializeField]
	private int maxCardsInDeck = 4;

	public static List<GameObject> LeaderBoardMenus
	{
		get { return Instance.leaderBoardMenus; }
		set { Instance.leaderBoardMenus = value; }
	}
	public static List<GameObject> StoreContents
	{
		get { return Instance.storeContents; }
		set { Instance.storeContents = value; }
	}
	public static List<GameObject> InventoryContents
	{
		get { return Instance.inventoryContents; }
		set { Instance.inventoryContents = value; }
	}
	public static List<GameObject> DeckContents
	{
		get { return Instance.deckContents; }
		set { Instance.deckContents = value; }
	}
	public static Text AvgCost
	{
		get { return Instance.avgCost; }
		set { Instance.avgCost = value; }
	}
	public static float CurrentCost
	{
		get
		{
			return Instance.currentCost;
		}
		set
		{
			Instance.currentCost = value;
		}
	}
	public static int CurrentCount
	{
		get
		{
			return Instance.currentCount;
		}
		set
		{
			Instance.currentCount = value;
		}
	}
	public static int CardsInDeck
	{
		get
		{
			return Instance.cardsInDeck;
		}
		set
		{
			Instance.cardsInDeck = value;
		}
	}

	public static Text Level
	{
		get { return Instance.level; }
		set { Instance.level = value; }
	}
	public static Text Gems
	{
		get { return Instance.gems; }
		set { Instance.gems = value; }
	}
	public static Text Coins
	{
		get { return Instance.coins; }
		set { Instance.coins = value; }
	}
	public static Text PlayerName
	{
		get { return Instance.playerName; }
		set { Instance.playerName = value; }
	}
	public static Text Trophies
	{
		get { return Instance.trophies; }
		set { Instance.trophies = value; }
	}
	public static Image Exp
	{
		get { return Instance.exp; }
		set { Instance.exp = value; }
	}


	public static UiManager Instance
	{
		get { return instance; }
		set { instance = value; }
	}

	private void Awake()
	{
		if (instance != this)
			instance = this;
		info = FindObjectOfType<AccountInfo>(); // 10.4 removed GameObject.
		GetLeaderboards(0);
	//	AccountInfo.AddDeckInfo();	
	}

	private void Update()
	{

		if (info.Info == null)
			return;
		UpdateText();
		UpdateToggles(menuToggle, menus.ToArray());
		if ((menus[GameConstants.MENU_BATTLE].activeInHierarchy))
		{
			/*profileMenus[0].SetActive(true);
			if (profileMenus[1].activeInHierarchy) // was 1
			{
				UpdateLeaderboards();
			} */
		}
		else if (menus[GameConstants.MENU_SHOP].activeInHierarchy)
		{
			UpdateShopInfo(); // error
		}
		else if (menus[GameConstants.MENU_DECK].activeInHierarchy)
		{
			if(Input.GetKeyDown(KeyCode.D))
			{
				for (int i = 0; i < AccountInfo.Deck.Count; i++)
				{
					Debug.Log("setting deckcontents to null");
					AccountInfo.Deck[i] = null;
				}
			}
			CountDeckCards();
			UpdateInventoryInfo();
			UpdateDeckinfo();
			AvgCost.text = string.Format("Avg cost: {0}", CurrentCost / (AccountInfo.Deck.Count));
		}
		else if ((menus[GameConstants.MENU_CLAN].activeInHierarchy))
		{
			profileMenus[0].SetActive(true);
			if (profileMenus[1].activeInHierarchy)
			{
				UpdateLeaderboards();
			}
		}
		
	}

	public void CountDeckCards()
	{
		cardsInDeck = 0;
		foreach (var Card in DeckContents)
		{
			if (Card.activeInHierarchy)
				cardsInDeck++;
		}
	}

	public void GameOverButton()
	{
		AccountInfo.Login(info.setUsername, info.setPassword);
	}

	public static List<PlayerLeaderboardEntry> LeaderBoardEntries 
	{
		get 
		{
			return Instance.leaderboardEntries;
		}
		set
		{
			Instance.leaderboardEntries = value;
		}
	}

	private void UpdateLeaderboards()
	{
		//Debug.Log("updateleadebaord key: " + leaderBoardKey);
		if (leaderboardEntries.Count == 0 && leaderBoardKey != "")
		{
			Debug.Log("Getting leaderboard with key: " + leaderBoardKey);
			AccountInfo.UpdateLeaderBoards(leaderBoardKey); // key is Leadboard  _// is it right?

		}
		else
		{
			for (int i = 0; i < leaderboardEntries.Count; i++)
			{
				Debug.Log("Updating leadeboard objects, entry:" + leaderboardEntries[i].DisplayName);
				leaderboardGameObjects[i].transform.GetChild(0).GetComponent<Text>().text = leaderboardEntries[i].DisplayName;
				leaderboardGameObjects[i].transform.GetChild(1).GetComponent<Text>().text = leaderboardEntries[i].StatValue.ToString();
				leaderboardGameObjects[i].SetActive(true);

			}
		}
	}

	private void UpdateDeckinfo() 
	{

		currentCost = 0;		
	//	Debug.Log("UpdateDeckinfo: DeckContents.Count: " + DeckContents.Count);
	//	Debug.Log("cards in deck " + cardsInDeck);
	/*	if(cardsInDeck > GameConstants.MAX_HAND_SIZE)
		{
			for (int i = cardsInDeck; i > GameConstants.MAX_HAND_SIZE; i--)
			{
				DeckContents[i].GetComponent<DeckSlot>().Card = null;
				DeckContents[i].GetComponent<DeckSlot>().gameObject.SetActive(false);
			}
		} */
		for (int i = 0; i < DeckContents.Count; i++)  // error? for (int i = 0; i < DeckContents.Count; i++)
		{
		//	Debug.Log("deck count:" + AccountInfo.Deck.Count);
			if (i < AccountInfo.Deck.Count) // if (i < AccountInfo.Deck.Count)
			{
			//	if (i > AccountInfo.Deck.Count)
			//		break;
			//	Debug.Log("deck count:" + AccountInfo.Deck.Count);
				if (AccountInfo.Deck[i] != null) // 	if (AccountInfo.Deck[i] != null)
				{
					DeckContents[i].GetComponent<DeckSlot>().gameObject.SetActive(true);
					DeckContents[i].GetComponent<DeckSlot>().Card = AccountInfo.Deck[i];
					currentCost += AccountInfo.Deck[i].Cost;
				}
				else if (deckContents[i].GetComponent<DeckSlot>().Card.Prefab == null)
				{
					//deckContents[i].SetActive(false);
					//DeckContents[i].GetComponent<DeckSlot>().Card = AccountInfo.
					Debug.Log("AccountInfo.Deck is null. i : " + i);
				}
				else
					Debug.Log("AccountInfo.Deck[i] == null");
			}
			//else
			//	Debug.Log("AccountInfo.Deck.Count: " + AccountInfo.Deck.Count);
		}
		

	}

	private void UpdateInventoryInfo()
	{
		//Debug.Log("Updateinventory: InventoryContents.Count" + InventoryContents.Count);
	//	Debug.Log("AccountInfo.Instance.Info.UserInventory.Count" + AccountInfo.Instance.Info.UserInventory.Count);

		for (int i = 0; i < InventoryContents.Count; i++)  // error?
		{
			
			if (i < AccountInfo.Instance.Info.UserInventory.Count)
			{
				if (AccountInfo.Instance.Info.UserInventory[i].ItemClass == GameConstants.ITEM_CARDS)
				{
					//Debug.Log("AccountInfo.Cards[i];" + AccountInfo.Cards[i]);
					InventoryContents[i].GetComponent<ItemSlot>().Card = AccountInfo.Cards[i];
					InventoryContents[i].SetActive(true);
					InventoryContents[i].GetComponent<Button>().interactable = true;
					InventoryContents[i].transform.GetChild(0).GetComponent<Image>().sprite = AccountInfo.Cards[i].Icon;
					InventoryContents[i].transform.GetChild(1).GetComponent<Text>().text = AccountInfo.Cards[i].Name;
					InventoryContents[i].transform.GetChild(2).GetComponent<Text>().text = string.Format("Cost: {0}", AccountInfo.Cards[i].Cost);
				}
				else
				{
					
					InventoryContents[i].SetActive(false);
				}
			}
			else
			{
				InventoryContents[i].SetActive(false);
			}
		}
	}

	public static void UpdateDeckinfo(int i, CardStats card)
	{
		if (card != null)
		{
			DeckContents[i].GetComponent<DeckSlot>().gameObject.SetActive(true);
			DeckContents[i].GetComponent<DeckSlot>().Card = card;
		}
		else
		{

		}
	}

	public void AddToDeck()
	{
		if(cardsInDeck < GameConstants.MAX_HAND_SIZE)
			AccountInfo.AddToDeck();
	}

	private void UpdateShopInfo()
	{
		Debug.Log("storecontents count:"+ storeContents.Count);
		Debug.Log("database.Instance.CardStoreItems.Count" + database.Instance.CardStoreItems.Count);
		for (int j = 0; j < database.Instance.CardStoreItems.Count; j++) //  storeContents[0].transform.GetChild(0).GetChild(j).childCount
		{
			if(j < database.Instance.CardStoreItems.Count) // 12.10 was: database.Instance.CardStoreItems.Count
			{
				storeContents[0].transform.GetChild(0).GetChild(j).GetComponent<StoreSlot>().Item = database.Instance.CardStoreItems[j];
				storeContents[0].transform.GetChild(0).GetChild(j).gameObject.SetActive(true);
				storeContents[0].transform.GetChild(0).GetChild(j).GetChild(1).GetComponent<Text>().text = database.Instance.CardStoreItems[j].ItemId;
				storeContents[0].transform.GetChild(0).GetChild(j).GetChild(0).GetComponent<Image>().sprite = database.Instance.Cards[j].Icon;
			}
			else
			{
				storeContents[0].transform.GetChild(0).GetChild(j).gameObject.SetActive(false);
			}
		}

	}


	void UpdateText()
	{
		if(info != null)
		{
			playerName.text = info.Info.AccountInfo.Username;

			int g = -1;
			if(info.Info.UserVirtualCurrency != null)
			{
				if(info.Info.UserVirtualCurrency.TryGetValue(GameConstants.COIN_CODE, out g))
				{
					coins.text = g.ToString();
				}
				if (info.Info.UserVirtualCurrency.TryGetValue(GameConstants.GEM_CODE, out g))
				{
					gems.text = g.ToString();
				}
			}
			UserDataRecord record = new UserDataRecord();
			float min = -1;
			float max = -1;

			if(info.Info.UserData != null)
			{
				if (info.Info.UserData.TryGetValue(GameConstants.DATA_EXP, out record))
				{
					min = float.Parse(record.Value);
				}
				if (info.Info.UserData.TryGetValue(GameConstants.DATA_LEVEL, out record))
				{
					level.text = record.Value;
				}
				if (info.Info.UserData.TryGetValue(GameConstants.DATA_MAX_EXP, out record))
				{
					max = float.Parse(record.Value);
				}

				exp.fillAmount = min / max;
			}

			List<StatisticValue> stats = info.Info.PlayerStatistics;

			foreach (StatisticValue item in stats)
			{
				if(item.StatisticName == GameConstants.STAT_TROPHIES)
				{
					trophies.text = item.Value.ToString();
				}
			}
		}
	}

	public void ChangeProfileMenus(int i)
	{
		GameFunctions.ChangeMenu(profileMenus.ToArray(), i);
	}

	public void GetLeaderboards(int i)
	{
		Debug.Log("Getleaderboards i:" + i);
		Debug.Log(LeaderBoardMenus[i].name);
		leaderBoardKey = LeaderBoardMenus[i].name;
		Debug.Log("GetLeaderboards key:" + leaderBoardKey);
		ChangeMenu(i, leaderBoardMenus.ToArray());
	}


	public void StartBattle()
	{
		if (cardsInDeck != 0)
		{
			foreach (AccountStats acc in LobbyManager.Players)
			{
				if (acc.me)
				{
					acc.looking = true;
					// LobbyManager looking

				}
			}
		}
		else
			SystemMessage("No cards in deck");
	}


	public void ChangeMenu(int i, GameObject[] m)
	{
		GameFunctions.ChangeMenu(m, i); // 14.4 menus.ToArray() changed 1st arg
	}

	void UpdateToggles(List<Toggle> togs,GameObject[] m)
	{
		for (int i = 0; i < togs.Count; i++)
		{
			if (togs[i].isOn)
			{
				ChangeMenu(i,m);
			}
		} 
	}
	public void SystemMessage(string message)
	{
		sysMsgText.text = message;
		sysMsg.SetActive(true);
	}

	public void CloseMessage()
	{
		sysMsg.SetActive(false);
	}
}
