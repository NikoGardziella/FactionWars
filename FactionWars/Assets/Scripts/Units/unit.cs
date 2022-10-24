using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;

public class unit : MonoBehaviour, IDamageable
{
	public GameObject attackGameObjectArrow;
	public Material redMaterial;
	public Animator animator;
	public GameObject gem;

	[SerializeField]
	private Actor3D agent;
	[SerializeField]
	private GameObject target;
	[SerializeField]
	private baseStats stats;
	[SerializeField]
	private List<GameObject> hitTargets;
	[SerializeField]
	List<Material> currentMats = new List<Material>();
	[SerializeField]
	public int priority;

	public bool IsRunning;
	public bool IsAttacking;

	public int Priority
	{
		get { return priority; }
		set { priority = value; }
	}
	public List<Material> CurrentMats
	{
		get { return currentMats; }
		set { currentMats = value; }
	}
	public List<GameObject> HitTargets
	{
		get { return hitTargets; }
	}


	public baseStats Stats
	{
		get { return stats; }
	}


	public Actor3D Agent
	{
		get { return agent; }
	}
	public GameObject Target
	{
		get { return target; }
		set { target = value; }
	}



	baseStats IDamageable.Stats
	{
		get { return stats; } // changed

	}

	void IDamageable.TakeDamage(float amount)
	{
		stats.CurrentHealth -= amount;
	}


	private void Start()
	{
		
		if (gameObject.tag == "Player")
		{
			Debug.Log("setting photon transform in active");
			gameObject.GetComponent<PhotonTransformView>().enabled = false;
		}
		if (gameObject.tag == "Enemy")
		{
			gameObject.GetComponent<Renderer>();
			gameObject.GetComponent<NavMeshAgent>().enabled = false;
		}



		List<GameObject> objects = GameManager.Instance.Objects;
		objects = GameManager.GetAllEnemies(transform.position, objects, gameObject.tag);
		target = GameFunctions.GetNearestTarget(objects, stats.DetectionObject, gameObject.tag);
		StartCoroutine(SetCollider());
	}

	IEnumerator SetCollider()
	{
		yield return new WaitForSeconds(1f);
		gameObject.GetComponent<BoxCollider>().enabled = true;
	}

	private void Update()
	{
		if (stats.CurrentHealth > 0)
		{
			agent.Agent.speed = stats.MovementSpeed;
			stats.UpdateStats();
			Attack();
			if (target != null)
			{
				//if (IsAttacking != true)
				animator.SetBool("IsRunning", true);

					/*float distance = Vector3.Distance(target.transform.position, gameObject.transform.position);
					if (distance < stats.Range)
					{
						gameObject.GetComponent<PhotonTransformView>().enabled = false;
						gameObject.GetComponent<NavMeshAgent>().enabled = true;
					}*/
					//else if(distance + 1 > stats.Range && gameObject.CompareTag("Enemy"))
					//	gameObject.GetComponent<NavMeshAgent>().enabled = false;
					
				if (gameObject.GetComponent<NavMeshAgent>().enabled)
				{
					/*float distance = Vector3.Distance(target.transform.position, gameObject.transform.position);
					if (distance < stats.Range)
						gameObject.GetComponent<PhotonTransformView>().enabled = false; */
				Vector3 targetPos;
				targetPos = target.GetComponent<BoxCollider>().ClosestPoint(gameObject.GetComponent<BoxCollider>().transform.position); //  target.GetComponent<BoxCollider>().ClosestPoint(target.transform.position);
				//Debug.Log("targetpos:"+targetPos);
				//Debug.Log("unitpos"+ gameObject.transform.position);
				//Debug.Log("target transform" + target.transform.position);		
					
				float distance = Vector3.Distance(target.transform.position, gameObject.transform.position);
				if (distance > stats.Range)
					agent.Agent.SetDestination(targetPos);
				else
					agent.Agent.SetDestination(agent.Agent.transform.position);
				}
			}
			//else
			//	isRunning = false;
		}
		else
		{
			print(gameObject.name + "has Died");
			GameManager.RemoveObjectFromList(gameObject);
			PhotonNetwork.Destroy(gameObject);
			if (gameObject.transform.CompareTag(GameConstants.NEUTRAL_TAG))
			{
				Vector3 newPos = transform.localPosition;
				newPos.y += 2.0f;
				Instantiate(gem, newPos, gem.transform.rotation);
			}
			Destroy(gameObject);
		}
	}

	void Attack()
	{
		if (target != null)
		{

			List<GameObject> objects = GameManager.Instance.Objects;
			objects = GameManager.GetAllEnemies(transform.position, objects, gameObject.tag);
			target = GameFunctions.GetNearestTarget(objects, stats.DetectionObject, gameObject.tag); // error



			//	if (target == null)
			//		Debug.LogError("target null");
			if (stats.CurrentAttackDelay >= stats.AttackDelay)
			{
				//	target = GameFunctions.GetNearestTarget(objects, stats.DetectionObject, gameObject.tag);

				/*	Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity);
					foreach (var ht in hitColliders)
					{
						if (ht.CompareTag(GameConstants.ENEMY_TAG))
						{
							Debug.Log("Found enemy near");
							target = ht.gameObject;
						}
					} */
				;

				Component damageable = target.GetComponent(typeof(IDamageable));

				if (damageable || target.tag == GameConstants.NEUTRAL_TAG)
				{
					//Debug.Log("target:" + target);

					if (target) // if (hitTargets.Contains(target)) 15.6
					{
						float distance = Vector3.Distance(target.transform.position, gameObject.transform.position);
						//	Debug.Log("distance:" + distance + "         stats.range:" + stats.Range);

						if (distance < stats.Range)
						{
							//	Debug.Log("distance 2:" + distance + "         stats.range 2:" + stats.Range);

							if (GameFunctions.CanAttack(gameObject.tag, target.tag, damageable, stats))
							{
								//	Debug.Log(gameObject.tag + "attacking" + target.tag);
								if (stats.UnitType == GameConstants.UNIT_TYPE.RANGE)
									rangedAttack(damageable, stats.BaseDamage, target);
								else if (stats.UnitType == GameConstants.UNIT_TYPE.MELEE)
								{
								//	animator.SetBool("IsRunning", false);
								//	animator.SetBool("IsAttacking", true);
									//	if(target)
									//		GameFunctions.Attack1(damageable, stats.BaseDamage, target);
									//	else
									GameFunctions.Attack(damageable, stats.BaseDamage);
								}
								else
									Debug.Log("attack error: damageable:" + damageable + "stats.basedamage" + stats.BaseDamage);
								stats.CurrentAttackDelay = 0;
							}
							else
								Debug.Log("CanT Attack");
						}
						//else
						//isRunning = true;
					}
					else
						Debug.Log("hitTargets does not Contains(target):" + target);
				}
			}
			
		}
		else
		{
			List<GameObject> objects = GameManager.Instance.Objects;
			objects = GameManager.GetAllEnemies(transform.position, objects, gameObject.tag);
			target = GameFunctions.GetNearestTarget(objects, stats.DetectionObject, gameObject.tag);
		}
	}
	public void rangedAttack(Component damageable, float baseDamage, GameObject target)
	{
		var myInfo = gameObject.GetComponent<unit>();
//		if(myInfo == null)
//			Debug.Log("no my info");
//		Debug.Log("myinfo"+myInfo);
		var shoot = Instantiate(attackGameObjectArrow, transform.position, Quaternion.identity);
	
		var shootInfo = shoot.GetComponent<projectileScript>();
		shootInfo.target = target;
		//shootInfo.damage =tats.baseDamage;
//		Debug.Log("shootinfo target:" + shootInfo.target);
	
		//shootInfo.projectileOfTeam = myInfo.team;

	}

	public void OnTriggerEnter(Collider other) // 13.5 does not work
	{
		//Debug.Log("OnTriggerEnter : " + other);
		if (!other.CompareTag(gameObject.tag)) // 10.5  if (!other.transform.parent.parent.CompareTag(gameObject.tag))
		{
			Component damageable = other.gameObject.GetComponent(typeof(IDamageable)); // Component damageable = other.transform.parent.parent.gameObject.GetComponent(typeof(IDamageable));
			if (damageable)
			{
				if (!hitTargets.Contains(damageable.gameObject))
				{
					hitTargets.Add(damageable.gameObject);
				}
				else
					Debug.Log("hitTargets.Contains(damageable.gameObject");
			}
			else
				return;
			//Debug.Log("not damageable");
		}
	}

	/*public void OnTriggerStay(Collider other)
	{
		Debug.Log("OnTriggerStay" + other);
		if (!other.gameObject.CompareTag(gameObject.tag))
		{
			if (hitTargets.Count > 0)
			{
				GameObject go = GameFunctions.GetNearestTarget(hitTargets, stats.DetectionObject, gameObject.tag, stats.Range);

				if (go != null)
				{
					target = go;
				}
				else
					Debug.Log("go is null");
			}
			else
				Debug.Log("not hit targets");
		}
	}*/

	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			if (stats.CurrentHealth < 0)
			{
				PhotonNetwork.Destroy(gameObject);
				Debug.Log("photon destroyd gameobject");
			}
			/*		if (photonView.isMine)
					{
						stream.SendNext(stats.CurrentHealth);
						stream.SendNext(stats.HealthBar.fillAmount);
					}*/
		}
		else
		{
			//	if (!photonView.isMine)
			//{
			stats.CurrentHealth = (float)stream.ReceiveNext();
			stats.HealthBar.fillAmount = (float)stream.ReceiveNext();
			//}

		}
	}
}