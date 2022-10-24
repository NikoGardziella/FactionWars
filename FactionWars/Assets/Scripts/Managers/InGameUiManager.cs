using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUiManager : MonoBehaviour
{
	private static InGameUiManager instance;

	public static InGameUiManager Instance
	{
		get { return instance; }
		set { instance = value; }
	}
		
	[SerializeField]
	private Text currResource;
	[SerializeField]
	private Text maxResource;
	[SerializeField]
	private Text score;
	[SerializeField]	
	private Transform handParent;
	[SerializeField]
	private Card nextCard;
	[SerializeField]
	private List<Image> allResources;


	public static Text CurrResource
	{
		get { return Instance.currResource; }
	}
	public static Text MaxResource
	{
		get { return Instance.maxResource; }
	}
	public static Text Score
	{
		get { return Instance.score; }
	}
	public static Transform Handparent
	{
		get { return Instance.handParent; }
	}
	public static Card NextCard
	{
		get { return Instance.nextCard; }
	}
	public static List<Image> AllResources
	{
		get { return Instance.allResources; }
		set { Instance.allResources = value; }
	}

}
