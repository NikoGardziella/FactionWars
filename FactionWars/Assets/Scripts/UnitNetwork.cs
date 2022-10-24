using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class UnitNetwork : MonoBehaviour
{

	[SerializeField]
	GameObject unitLocation;
	[SerializeField]
	GameObject agentLocation;
	[SerializeField]
	Vector3 newUnitPosition;
	[SerializeField]
	Vector3 oldAgentPosition;
	[SerializeField]
	Quaternion newUnitRotation;


	private void Update()
	{
		
	
			unitLocation.transform.rotation = Quaternion.Lerp(unitLocation.transform.rotation, newUnitRotation, 0.1f);
			unitLocation.transform.position = Vector3.Lerp(unitLocation.transform.position, newUnitPosition, .1f);
		
		//if (newUnitPosition != unitLocation.transform.position)
		//{
		//	unitLocation.transform.position  = Vector3.Lerp(unitLocation.transform.position, newUnitPosition, .1f);
		//}

		if (oldAgentPosition != agentLocation.transform.localPosition)
		{
			agentLocation.transform.localPosition = Vector3.Lerp(agentLocation.transform.localPosition, oldAgentPosition, .1f);
		}
	}

	/*private void Awake()
	{
		if(newUnitPosition != unitLocation.transform.position)
		{
			unitLocation.transform.position  = Vector3.Lerp(unitLocation.transform.position, newUnitPosition, .1f);
		}
	} */




	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{

		if (stream.IsWriting)
		{

			stream.SendNext(unitLocation.transform.position);
			stream.SendNext(unitLocation.transform.rotation);

			stream.SendNext(agentLocation.transform.localPosition);
		}
		else
		{
			newUnitPosition = (Vector3)stream.ReceiveNext();
			newUnitRotation = (Quaternion)stream.ReceiveNext();
			newUnitRotation.y -= 180f;
			//newUnitRotation.w *= -1f;
			newUnitPosition = -newUnitPosition; // this is the weirdest possible solutution.

			oldAgentPosition = (Vector3)stream.ReceiveNext();
		}
	}




} 
