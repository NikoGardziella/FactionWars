using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Structure : MonoBehaviour, IDamageable
{
	public GameObject attackGameObjectArrow;
	public GameObject attackGameObjectBall;

	[SerializeField]
	private baseStats stats;
	[SerializeField]
	private List<GameObject> hitTargets;
	[SerializeField]
	GameObject target;
	[SerializeField]
	private bool leftTower;

	public int priority;

	public int Priority
	{
		get { return priority; }
		set { priority = value; }
	}

	public bool LeftTower
	{
		get { return leftTower; }
		set { leftTower = value; }
	}

	public baseStats Stats
	{
		get
		{
			return stats;
		}
	}

	public List<GameObject> HitTargets
	{
		get
		{
			return hitTargets;
		}
	}

	public GameObject Target
	{
		get
		{
			return target;
		}
		set
		{
			target = value;
		}
	}

	void IDamageable.TakeDamage(float amount)
	{
		stats.CurrentHealth -= amount;
	}
	private void Start()
	{
		List<GameObject> objects = GameManager.Instance.Objects;
		if(objects != null)
		{
			objects = GameManager.GetAllEnemies(transform.position, objects, gameObject.tag);
			target = GameFunctions.GetNearestTarget(objects, stats.DetectionObject, gameObject.tag);
		}
	}
	void Update()
	{
		if (stats.CurrentHealth > 0)
		{
			stats.UpdateStats();
			Attack();
		}
		else
		{

			print(gameObject.name + "has Died");
			GameManager.RemoveObjectFromList(gameObject, leftTower); // why lefttower?
 			Destroy(gameObject);
		}
	}
	void Attack()
	{
		if (target != null)
		{
			if (stats.CurrentAttackDelay >= stats.AttackDelay)
			{
				Component damageable = target.GetComponent(typeof(IDamageable));

				if (damageable)
				{
					if (hitTargets.Contains(target))
					{
						float distance = Vector3.Distance(target.transform.position, gameObject.transform.position);

						if (distance < stats.Range) // 14.5 added
						{
							if (GameFunctions.CanAttack(gameObject.tag, target.tag, damageable, stats))
							{
								rangedAttack(damageable, stats.BaseDamage, target);
								//GameFunctions.Attack(damageable, stats.BaseDamage);
								stats.CurrentAttackDelay = 0;
							}
						}

						//else
							//Debug.Log("STRUCTURE: can attack false");
					}
					//else
					//	Debug.Log("STRUCTURE: hitTargets does not Contain" + target);
				}
				//else
				//	Debug.Log("STRUCTURE: damageable" + damageable);
			}
		}
		else
		{
			List<GameObject> objects = GameManager.Instance.Objects;
			if (objects != null)
			{
				objects = GameManager.GetAllEnemies(transform.position, objects, gameObject.tag);
				target = GameFunctions.GetNearestTarget(objects, stats.DetectionObject, gameObject.tag);
			}
		}
	}
	public void rangedAttack(Component damageable, float baseDamage, GameObject target)
	{
		var myInfo = gameObject.GetComponent<Structure>();
		Vector3 pos = transform.position;
		pos.y = pos.y + 2f;
		var shoot = Instantiate(attackGameObjectArrow, pos, Quaternion.identity);
		var shootInfo = shoot.GetComponent<projectileScript>();
		shootInfo.target = target;
		//shootInfo.projectileOfTeam = myInfo.team;

	}
	public void OnTriggerEnter(Collider other)
	{
		if (!other.transform.CompareTag(gameObject.tag)) // (!other.transform.parent.CompareTag(gameObject.tag))
		{
			Component damageable = other.transform.gameObject.GetComponent(typeof(IDamageable)); // Component damageable = other.transform.parent.gameObject.GetComponent(typeof(IDamageable));
			if (damageable)
			{
				if (!hitTargets.Contains(damageable.gameObject))
				{
					hitTargets.Add(damageable.gameObject);
				}
			}
		}
	}

	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(stats.CurrentHealth);
			stream.SendNext(stats.HealthBar.fillAmount);
			stream.SendNext(leftTower);
		}
		else
		{
			stats.CurrentHealth = (float)stream.ReceiveNext();
			stats.HealthBar.fillAmount = (float)stream.ReceiveNext();
			leftTower = (bool)stream.ReceiveNext();
		}
	}

	public void OnTriggerStay(Collider other)
	{
		if (!other.gameObject.CompareTag(gameObject.tag))
		{
			if (hitTargets.Count > 0)
			{
				GameObject go = GameFunctions.GetNearestTarget(hitTargets, stats.DetectionObject, gameObject.tag, stats.Range);

				if (go != null)
				{
					target = go;
				}
				else if (go == null)
					return ;
			}
		}
	}
}
