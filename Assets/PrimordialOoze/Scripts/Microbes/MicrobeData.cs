namespace PrimordialOoze
{
	using System;
	using UnityEngine;


	[Serializable]
	public class MicrobeData
	{
		[Header("Stats")]
		[SerializeField]
		private float sightDistance = 5f;

		[SerializeField]
		private int strength = 5;

		[Header("Attack")]
		[SerializeField]
		private float attackSpeed = 15f;

		[SerializeField]
		private float attackDuration = 0.5f;

		[SerializeField]
		private float attackCooldown = 0.25f;

		[Header("Movement")]
		[SerializeField]
		private float acceleration = 5;

		[SerializeField]
		private float deceleration = 2;

		[SerializeField]
		private float maxSpeed = 5;

		[Header("Health")]
		[SerializeField]
		private int maxHealth = 100;

		[SerializeField]
		private int currentHealth;

		private MapData map;
		private MicrobeData parentMicrobeData;


		#region Properties
		public float Acceleration
		{
			get { return this.acceleration; }
			set { this.acceleration = value; }
		}


		public float AttackCooldown
		{
			get { return this.attackCooldown; }
			set { this.attackCooldown = value; }
		}


		public float AttackDuration
		{
			get { return this.attackDuration; }
			set { this.attackDuration = value; }
		}


		public float AttackSpeed
		{
			get { return this.attackSpeed; }
			set { this.attackSpeed = value; }
		}


		public int CurrentHealth
		{
			get { return this.currentHealth; }
			set { this.currentHealth = value; }
		}


		public float Deceleration
		{
			get { return this.deceleration; }
			set { this.deceleration = value; }
		}


		public MapData Map
		{
			get
			{
				if (this.map == null)
					this.map = new MapData(this);
				return this.map;
			}
			set { this.map = value; }
		}


		public int MaxHealth
		{
			get { return this.maxHealth; }
			set { this.maxHealth = value; }
		}


		public float MaxSpeed
		{
			get { return this.maxSpeed; }
			set { this.maxSpeed = value; }
		}


		public MicrobeData ParentMicrobeData
		{
			get { return this.parentMicrobeData; }
			set { this.parentMicrobeData = value; }
		}


		public float SightDistance
		{
			get { return this.sightDistance; }
			set { this.sightDistance = value; }
		}


		public int Strength
		{
			get { return this.strength; }
			set { this.strength = value; }
		}
		#endregion


		public MicrobeData Clone()
		{
			MicrobeData clone = new MicrobeData();
			clone.sightDistance = this.sightDistance;
			clone.strength = this.strength;
			clone.attackSpeed = this.attackSpeed;
			clone.attackDuration = this.attackDuration;
			clone.attackCooldown = this.attackCooldown;
			clone.acceleration = this.acceleration;
			clone.deceleration = this.deceleration;
			clone.maxSpeed = this.maxSpeed;
			clone.maxHealth = this.maxHealth;
			clone.currentHealth = this.currentHealth;

			return clone;
		}
	}
}