using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileScript : MonoBehaviour
{
	public float velocity = 3.0f; // double doesnt work with Vector3 line 18.
	public double hitBoxRadius = 0.25f;
	public GameObject target = null;
	public int damage;

	public bool useDamageArea = false; // projectile causes damage in area when hit
	public int damageAreaRadius = 3;
	public string projectileOfTeam = "";

	[SerializeField]
	private baseStats stats;


	baseStats Stats
	{
		get { return stats; } // changed

	}

	void Start()
	{
		Object.Destroy(gameObject, 8.0f);
	}
	

	// Update is called once per frame
	void Update()
	{
		if (target)
		{
			Component damageable = target.GetComponent(typeof(IDamageable));

			gameObject.transform.LookAt(target.transform);
			gameObject.transform.Translate(Vector3.forward * velocity * Time.deltaTime);
			if (Vector3.Distance(gameObject.transform.position, target.transform.position) <= hitBoxRadius)
			{
				if (useDamageArea) // this is not working
				{
					//  var Properties = target.GetComponent<properties>();
					var possibleEnemy = Physics.OverlapSphere(transform.position, damageAreaRadius);
					for (var i = 0; i < possibleEnemy.Length; i++)
					{
						//  if (possibleEnemy[i].gameObject.layer == 8)
						// {
						//  var properties_target = possibleEnemy[i].gameObject.GetComponent<properties>();
						//  if (projectileOfTeam != properties_target.team)
						// {
						//  Component damageable = target.GetComponent(typeof(IDamageable));
						//  GameFunctions.Attack(damageable, stats.BaseDamage);

						var Properties = possibleEnemy[i].transform.parent.parent.parent.GetComponent<unit>(); //  gameObject.GetComponent<unit>();
						//   var Properties = target.GetComponent<unit>();
						Properties.Stats.CurrentHealth -= damage;

						//properties_target.currentHealth -= damage;
						//  }
						// }
					}
				}
				else
				{
				   // Component damageable = target.GetComponent(typeof(IDamageable));
				  //  GameFunctions.Attack(damageable, stats.BaseDamage);
					
					GameFunctions.Attack(damageable,damage);
				//	var Properties = target.GetComponent<unit>();
				//	if(Properties == null)
				//		Debug.Log("Target no unit component");
				//	Properties.Stats.CurrentHealth  -= damage;
				}
				Destroy(gameObject);
			}
		}
		else
		{
			Debug.Log("projectile no target");
			Destroy(gameObject);
		}
	}
}