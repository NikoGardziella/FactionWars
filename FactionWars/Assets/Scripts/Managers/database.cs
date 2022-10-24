using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class database : MonoBehaviour
{
	private static database instance;
	[SerializeField]
	private List<CardStats> cards;
	[SerializeField]
	private List<CatalogItem> catalogCards;
	[SerializeField]
	private List<StoreItem> cardStoreItems;
	[SerializeField]
	private List<StoreItem> chestStoreItems;
	[SerializeField]
	private bool updated = false;

	public static bool Updated
	{
		get { return Instance.updated; }
		set { Instance.updated = value; }
	}



	public List<CatalogItem> CatalogCards
	{
		get { return catalogCards; }
		set { catalogCards = value; }
	}


	public List<CardStats> Cards
	{
		get { return cards; }
		set { cards = value; }
	}


	public List<StoreItem> CardStoreItems
	{
		get { return cardStoreItems; }
	}


	public static database Instance
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

	public static void UpdateDatabase()
	{
		GetCatalogItemsRequest request = new GetCatalogItemsRequest()
		{
			CatalogVersion = GameConstants.CATALOG_ITEMS
		};
		Debug.Log("UpdateDatabase");
		PlayFabClientAPI.GetCatalogItems(request, OnUpdateDatabase, GameFunctions.OnAPIError);
	}
	
	public static CatalogItem GetCatalogItem(ItemInstance item) // 13.4 something wrong here
	{
		Debug.Log("GetCatalogItem ITEM: " + item + "    Instance.CatalogCards: " + Instance.CatalogCards);
		foreach (CatalogItem c in Instance.CatalogCards)
		{
			Debug.Log(item.ItemId + " " + c.ItemId);
			if (item.ItemId == c.ItemId)
			{
				Debug.Log("IF" + item.ItemId + " " + c.ItemId);
				return c;
			}

		}
		Debug.Log(string.Format("ERROR! Item {0} was not found", item.ItemId));
		return null;
	}

	public static CardStats GetCardInfo(ItemInstance item, int i)
	{
		CatalogItem ci = GetCatalogItem(item);
		return GameFunctions.CreateCard(ci, i);
	}

	static void OnUpdateDatabase(GetCatalogItemsResult result)
	{
		GetStoreItems(GameConstants.STORE_CHEST); // 13.4 moved up
		GetStoreItems(GameConstants.STORE_CARDS); // 13.4 moved up
		Debug.Log("OnUpdateDatabase");
		//Debug.Log("on Update database: result.Catalog.Count" + result.Catalog.Count);
		for (int i = 0; i < result.Catalog.Count; i++) // 12.4 changed to <=
		{
			//Debug.Log("OnUpdateDatabase for loop "+ i);
			if (result.Catalog[i].ItemClass == GameConstants.ITEM_CARDS)
			{
				Debug.Log("OnUpdateDatabase result.Catalog[i].ItemClass: " + result.Catalog[i].ItemClass);
				Debug.Log("OnUpdateDatabase result.Catalog[i] : " + result.Catalog[i]);
				Instance.CatalogCards.Add(result.Catalog[i]);
				Instance.Cards.Add(GameFunctions.CreateCard(result.Catalog[i], i)); // changed from card to Card 7.4 ERROR ___
			}
		}

	}


	static void GetStoreItems(string id)
	{
		Debug.Log("GET storetems: id" + id);
		GetStoreItemsRequest request = new GetStoreItemsRequest()
		{
			CatalogVersion = GameConstants.CATALOG_ITEMS,
			StoreId = id
		};

		PlayFabClientAPI.GetStoreItems(request,GotStoreItems , GameFunctions.OnAPIError);
	}

	static void GotStoreItems(GetStoreItemsResult result)
	{
	
		Debug.Log("got store items: result.StoreId" + result.StoreId);
		if(result.StoreId == GameConstants.STORE_CARDS)
		{
			Debug.Log("Got Store items CARDS");
			Instance.cardStoreItems = result.Store;
		}
		else if(result.StoreId == GameConstants.STORE_CHEST)
		{
			Debug.Log("Got Store items CHEST");
			Instance.chestStoreItems = result.Store;
			Updated = true;
		}

	}


}
