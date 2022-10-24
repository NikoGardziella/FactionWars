using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{

	[SerializeField]
	CardStats card;
	[SerializeField]
	private int maxCards = 4;

	public CardStats Card
	{
		get
		{
			return card;
		}
		set
		{
			card = value;
		}
	}

	public void AddToDeck()
	{
		if (UiManager.CardsInDeck < maxCards)
		{
		//	UiManager.CurrentCount++;
			AccountInfo.Deck.Add(card);
			GetComponent<Button>().interactable = false;
			UiManager.UpdateDeckinfo(UiManager.CurrentCount, card);
			UiManager.CurrentCost += card.Cost;
		}
		else
			Debug.Log("Deck is full");
	}
}
