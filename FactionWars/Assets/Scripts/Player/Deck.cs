using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Deck
{
	[SerializeField]
	int maxCards = 4;
	[SerializeField]
	private List<CardStats> cards;
	[SerializeField]
	private List<CardStats> hand;
	[SerializeField]
	private CardStats nextCard;

	public CardStats NextCard
	{
		get { return nextCard; }
		set { nextCard = value; }
	}
	public List<CardStats> Hand
	{
		get { return hand; }
	}
	public List<CardStats> Cards
	{
		get { return cards; }
		set { cards = value; }
	}

	public void Start()
	{
		nextCard = cards[0];
	}

	public CardStats DrawCard()
	{
		CardStats cs = nextCard;
		hand.Add(nextCard);
		//cards.Remove(nextCard); removed 28.9
		nextCard = cards[Random.Range(0, cards.Count)];
		return cs;
	}

	public void RemoveHand(int index)
	{
		foreach (CardStats cs in hand)
		{
			if(cs.Index == index)
			{
				hand.Remove(cs);
				cards.Add(cs);
				break;
			}
		}
	}

}
