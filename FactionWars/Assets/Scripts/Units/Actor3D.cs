using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Actor3D : MonoBehaviour
{

	[SerializeField]
	Animator anim;
	[SerializeField]
	bool isFlying;
	[SerializeField]
	private NavMeshAgent agent;
	[SerializeField]
	public bool isRunning = false;

	public NavMeshAgent Agent
	{
		get { return agent; }

	}



	private void Awake()
	{
		isRunning = true;
		//	agent = GetComponent<NavMeshAgent>();
		//	anim = GetComponent<Animator>();
	}


	private void Update()
	{
		if (!isFlying)
		{
			//anim.SetBool("isWalking", agent.velocity == Vector3.zero ? false : true); commented 10.5
			if (Mathf.Abs(agent.velocity.z) >= Mathf.Abs(agent.velocity.x))
			{
				//anim.SetFloat("TargetZ", -agent.velocity.z);
				//anim.SetFloat("TargetX", 0);
			}
			else if (Mathf.Abs(agent.velocity.z) >= Mathf.Abs(agent.velocity.x))
			{
				//anim.SetFloat("TargetX", agent.velocity.x);
				//anim.SetFloat("TargetZ", 0);
			}
		}
		
	}
}
