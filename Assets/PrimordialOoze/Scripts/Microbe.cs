namespace PrimordialOoze
{
	using System;
	using System.Collections;
	using UnityEngine;


	public class Microbe : MonoBehaviour,
							IDamageable
	{
		[Header("Stats")]
		[SerializeField]
		private float sightDistance = 5f;

		[SerializeField]
		private float strength = 5f;
		
		[Header("Attack")]
		[SerializeField]
		private GameObject attackDamageField;

		[SerializeField]
		private float attackDuration = 0.5f;

		[SerializeField]
		private float attackCooldown = 0.25f;

		[SerializeField]
		private float attackSpeed = 10f;

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

		[SerializeField]
		private bool isInvulnerable;

		[SerializeField]
		private float invulnerabilityDuration = 2;

		[SerializeField]
		private float invulnerabilityTimeLeft;

		private SpriteRenderer spriteRenderer;
		private Animator animator;
		private GamePhysics gamePhysics;
		private bool isMoving;
		private bool isAttacking;
		private bool isCoolingDown;

		public event Action<IDamageable> Damaged;
		public event Action Killed;


		#region Properties
		public float Acceleration
		{
			get { return this.acceleration; }
			set { this.acceleration = value; }
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


		public float Strength
		{
			get { return this.strength; }
			set { this.strength = value; }
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


		public GamePhysics GamePhysics
		{
			get { return this.gamePhysics; }
		}


		public float InvulnerabilityDuration
		{
			get { return this.invulnerabilityDuration; }
			private set { this.invulnerabilityDuration = value; }
		}


		public float InvulnerabilityTimeLeft
		{
			get { return this.invulnerabilityTimeLeft; }
			set { this.invulnerabilityTimeLeft = value; }
		}


		public bool IsAttacking
		{
			get { return this.isAttacking; }
			set { this.isAttacking = value; }
		}


		public bool IsInvulnerable
		{
			get { return this.isInvulnerable; }
			private set { this.isInvulnerable = value; }
		}


		public bool IsMoving
		{
			get { return this.isMoving; }
			set { this.isMoving = value; }
		}


		public int MaxHealth
		{
			get { return this.maxHealth; }
			private set { this.maxHealth = value; }
		}


		public float MaxSpeed
		{
			get { return this.maxSpeed; }
			set { this.maxSpeed = value; }
		}


		public float SightDistance
		{
			get { return this.sightDistance; }
			set { this.sightDistance = value; }
		}
		#endregion


		public void Attack(float x, float y)
		{
			if (this.isAttacking
				|| this.isCoolingDown
				|| this.attackDamageField == null)
			{
				return;
			}
				
			this.isAttacking = true;
			this.gamePhysics.SetMovement(new Vector2(x,y).normalized * this.attackSpeed);
			this.attackDamageField.SetActive(true);
			StartCoroutine(ApplyAttack(x, y));
		}


		public void Awake()
		{
			this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();
			this.animator = GetComponentInChildren<Animator>();
			this.gamePhysics = GetComponent<GamePhysics>();
			this.currentHealth = this.maxHealth;
			if (this.attackDamageField != null)
				this.attackDamageField.SetActive(false);
		}


		public int TakeDamage(int amount)
		{
			amount = IDamageableExtensions.TakeDamage(this, amount);
			if (amount > 0
				&& this.Damaged != null)
			{
				this.Damaged(this);
				if (this.currentHealth == 0
					&& this.Killed != null)
				{
					this.Killed();
				}
			}

			return amount;
		}


		public void FixedUpdate()
		{
			if (!this.isMoving
				&& this.gamePhysics.Velocity != Vector2.zero)
			{
				Vector2 newSpeed = Vector2.Lerp(
					this.gamePhysics.Velocity,
					Vector2.zero,
					this.deceleration * Time.deltaTime);
				this.gamePhysics.SetMovement(newSpeed);
			}

			this.isMoving = false;
		}


		public void Move(Vector2 direction, float speed = 0)
		{
			Move(direction.x, direction.y, speed);
		}


		public void Move(float x, float y, float speed = 0)
		{
			if (this.isAttacking)
				return;

			if (speed == 0)
			{
				x = x * this.acceleration;
				y = y * this.acceleration;
			}
			else
			{
				x = x * speed;
				y = y * speed;
			}

			if (speed == 0)
			{
				float xSpeedNextFrame = Mathf.Abs(this.gamePhysics.Velocity.x + (x * Time.deltaTime));
				if (xSpeedNextFrame > this.MaxSpeed)
					x = 0;

				float ySpeedNextFrame = Mathf.Abs(this.gamePhysics.Velocity.y + (y * Time.deltaTime));
				if (ySpeedNextFrame > this.MaxSpeed)
					y = 0;
			}

			this.gamePhysics.AddForce(x, y);
			this.isMoving = true;
		}


		public void Start()
		{
		}


		private IEnumerator ApplyAttack(float x, float y)
		{
			this.isMoving = true;
			yield return new WaitForSeconds(this.attackDuration);

			this.isAttacking = false;
			this.isCoolingDown = true;
			this.gamePhysics.SetMovement(new Vector2(x, y) * this.MaxSpeed);
			this.attackDamageField.SetActive(false);
			StartCoroutine(RemoveCooldown());
		}


		private IEnumerator RemoveCooldown()
		{
			yield return new WaitForSeconds(this.attackCooldown);

			this.isCoolingDown = false;
		}
	}
}


#region Editor
#if UNITY_EDITOR

namespace PrimordialOoze
{
	using UnityEditor;
	using UnityEngine;


	[CustomEditor(typeof(Microbe))]
	public class MicrobeInspector : Editor
	{
		private Microbe microbe;


		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			if (Application.isPlaying)
			{
				if (this.microbe == null)
					this.microbe = target as Microbe;

				EditorGUILayout.Toggle("Is Moving", this.microbe.IsMoving);
				EditorGUILayout.Toggle("Is Attacking", this.microbe.IsAttacking);
			}
		}
	}
}

#endif
#endregion