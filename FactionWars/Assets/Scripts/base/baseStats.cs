using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public class baseStats
{
	[SerializeField]
	private float currentHealth;
	[SerializeField]
	private float maxHealth;
	[SerializeField]
	private float range;
	[SerializeField]
	private float baseDamage;
	[SerializeField]
	private float attackDelay;
	[SerializeField]
	private float currentAttackDelay;
	[SerializeField]
	private float movementSpeed;
	[SerializeField]
	private Image healthBar;
	[SerializeField] 
	private SphereCollider detectionObject;
	[SerializeField]
	private GameConstants.OBJECT_TYPE objectType;
	[SerializeField]
	private GameConstants.OBJECT_ATTACKABLE objectAttackable;
	[SerializeField]
	private GameConstants.UNIT_TYPE unitType;


	public GameConstants.OBJECT_ATTACKABLE ObjectAttackable
	{
		get { return objectAttackable; }
	}


	public GameConstants.OBJECT_TYPE ObjectType
	{
		get { return objectType; }
	}

	public GameConstants.UNIT_TYPE UnitType
	{
		get { return unitType; }
	}
	public  SphereCollider DetectionObject
	{
		get { return detectionObject; }
	}

	public Image HealthBar
	{
		get { return healthBar; }
		set { healthBar = value; }
	}

	public float MovementSpeed
	{
		get { return movementSpeed; }
		set { movementSpeed = value; }
	}

	public float CurrentAttackDelay
	{
		get { return currentAttackDelay; }
		set { currentAttackDelay = value; }
	}

	public float AttackDelay
	{
		get { return attackDelay; }
	}

	public  float BaseDamage
	{
		get { return baseDamage; }
	}

	public float Range
	{
		get { return range; }
	}

	public float MaxHealth
	{
		get { return maxHealth; }
	}

	public float percetHealth
	{
		get { return currentHealth / maxHealth; }
	}

	public float CurrentHealth
	{
		get { return currentHealth; }
		set
		{
			if (value <= 0)
				currentHealth = 0;
			else if (value >= maxHealth)
				currentHealth = maxHealth;
			else
				currentHealth = value;
		}
	}

	public void UpdateStats()
	{
		if(healthBar != null)
			healthBar.fillAmount = percetHealth;
		detectionObject.radius = range;
		if(currentAttackDelay < attackDelay)
		{
			currentAttackDelay += Time.deltaTime;
		}
	}

}


