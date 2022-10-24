using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[SerializeField]
	private PlayerStats playerInfo;
	[SerializeField]
	private CardStats cardInfo;
	[SerializeField]
	private Image icon;
	[SerializeField]
	private Text cardName;
	[SerializeField]
	private Text cost;
	[SerializeField]
	private bool canDrag;

	public bool CanDrag
	{

		get { return canDrag; }
		set { canDrag = value; }
	}


	public Text Cost
	{
		get { return cost; }
	}

	public Text CardName
	{
		get { return cardName; }
		set { cardName = value; }
	}

	public Image Icon
	{
		get { return icon; }
		set { icon = value; }
	}

	public CardStats CardInfo
	{
		get { return cardInfo; }
		set { cardInfo = value; }
	}

	public PlayerStats PlayerInfo
	{
		get { return playerInfo; }
		set { playerInfo = value; }
	}

	private void Update()
	{
		icon.sprite = cardInfo.Icon;
		cardName.text = cardInfo.Name;
		cost.text = cardInfo.Cost.ToString();
	}

	public void Start()
	{
	}

	private void SpawnUnit()
	{
		if(playerInfo.GetCurrResource >= cardInfo.Cost)
		{
			playerInfo.PlayersDeck.RemoveHand(cardInfo.Index);
			playerInfo.RemoveResource(cardInfo.Cost);

			Vector3 mousePos = Input.mousePosition;
			Vector3 worldPos;
			Ray ray = Camera.main.ScreenPointToRay(mousePos);
			RaycastHit hit;
			LayerMask mask = LayerMask.GetMask("Ground");
			if (Physics.Raycast(ray, out hit, 1000f, mask))
			{
				Debug.Log("ELSE: wordpos:" + hit.point);
				Debug.Log("rb:"+hit.rigidbody);
				Debug.Log("collider"+hit.collider);
				Debug.Log("gameibject"+hit.transform.gameObject);
				Debug.Log("layer"+hit.transform.gameObject.layer);
				Debug.Log("IF, wordpos:" + hit.point);
				worldPos = hit.point;
				GameFunctions.SpawnUnit(cardInfo.Prefab.name, playerInfo.UnitTransform, worldPos); 		
			}
			else
			{
				Debug.Log("ELSE: wordpos:" + hit.point);
				Debug.Log("rb:"+hit.rigidbody);
				Debug.Log("collider"+hit.collider);
				Debug.Log("gameibject"+hit.transform.gameObject);
				Debug.Log("layer"+hit.transform.gameObject.layer);
			//	wordPos = Camera.main.ScreenToWorldPoint(mousePos);
			}
			
			//or for tandom rotarion use Quaternion.LookRotation(Random.insideUnitSphere)

			/*Vector3 mousePos = Input.mousePosition;
			mousePos.z = Camera.main.nearClipPlane;

			Vector3 pos = Camera.main.ScreenToWorldPoint(mousePos); // 24.5 changed from screentoworldpoint */
			
			Destroy(gameObject);
		}
		else
		{
			transform.SetParent(playerInfo.HandParent);
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!playerInfo.OnDragging && !gameObject.CompareTag("nextCard"))
		{
			if (canDrag)
			{
				GetComponent<CanvasGroup>().blocksRaycasts = false;
				playerInfo.OnDragging = true;
				playerInfo.SpawnZone = true;
				transform.SetParent(GameFunctions.GetCanvas());
			}
		}
	}


	public void OnDrag(PointerEventData eventData)
	{
		if (playerInfo.OnDragging && !gameObject.CompareTag("nextCard"))
		{
			transform.position = Input.mousePosition;
		} 
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (!gameObject.CompareTag("nextCard"))
		{
			transform.SetParent(playerInfo.HandParent);
			GetComponent<CanvasGroup>().blocksRaycasts = true;
			playerInfo.OnDragging = false;
			playerInfo.SpawnZone = false;
		}
		GameObject go = eventData.pointerCurrentRaycast.gameObject;

		if(go != null)
		{

			if (go == playerInfo.LeftArea && playerInfo.LeftZone)
			{
				SpawnUnit();
			}
			else if (go == playerInfo.RightArea && playerInfo.RightZone)
			{
				SpawnUnit();
			}
		}
		else
		{
			Debug.Log("end of drag go = null");
			SpawnUnit();
		}
	}
}
