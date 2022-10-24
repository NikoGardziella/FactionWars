using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class StoreSlot : MonoBehaviour
{
	[SerializeField]
	private StoreItem item;
	[SerializeField]
	private Text coinsText;
	[SerializeField]
	private Text gemsText;

	public StoreItem Item
	{
		get { return item; }
		set { item = value; }
	}


	private void Update()
	{
		if(item != null)
		{
			uint price = 0;
			if (item.VirtualCurrencyPrices.TryGetValue(GameConstants.COIN_CODE, out price))
			{
				coinsText.text = string.Format("{0} C", price);
			}
			if (item.VirtualCurrencyPrices.TryGetValue(GameConstants.GEM_CODE, out price))
			{
				gemsText.text = string.Format("{0} G", price);
			}
			
		}
	}


	public void BuyWithCoins()
	{
		uint price = 0;
		if (item.VirtualCurrencyPrices.TryGetValue(GameConstants.COIN_CODE, out price))
		{
			PurchaseItemRequest request = new PurchaseItemRequest()
			{
				ItemId = item.ItemId,
				VirtualCurrency = GameConstants.COIN_CODE,
				Price = (int)price
			};
			for (int i = 0; i < AccountInfo.Cards.Count; i++)
			{
				Debug.Log("itemid: " + item.ItemId + "accountinfo card: " + AccountInfo.Cards[i].Name);
				if (item.ItemId == AccountInfo.Cards[i].Name)
				{
				//	uiManager.SystemMessage("Card already inventory");
					Debug.Log("Card already inventory");
					return;
				}
			}
			PlayFabClientAPI.PurchaseItem(request, OnBoughtItem, GameFunctions.OnAPIError);
		}
	}
	public void BuyWithGems()
	{
		uint price = 0;
		if (item.VirtualCurrencyPrices.TryGetValue(GameConstants.GEM_CODE, out price))
		{
			PurchaseItemRequest request = new PurchaseItemRequest()
			{
				ItemId = item.ItemId,
				VirtualCurrency = GameConstants.GEM_CODE,
				Price = (int)price
			};
			for (int i = 0; i < AccountInfo.Cards.Count; i++) 
			{
				Debug.Log("itemid: " + item.ItemId + "accountinfo card: " + AccountInfo.Cards[i].Name);
				if (item.ItemId == AccountInfo.Cards[i].Name)
				{
				//	uiManager.SystemMessage("Card already inventory");
					Debug.Log("Card already inventory");
					return ;
				}
			}
			PlayFabClientAPI.PurchaseItem(request, OnBoughtItem, GameFunctions.OnAPIError);
		}

	}

	void OnBoughtItem(PurchaseItemResult result)
	{
		AccountInfo.GetAccountInfo();
	}
}
