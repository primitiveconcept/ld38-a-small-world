namespace PrimordialOoze
{
	using System;
	using System.Collections;
	using PrimordialOoze.Extensions.Colors;
	using UnityEngine;


	public class Microbe : MonoBehaviour,
							IDamageable
	{
		public const string AttackAnimation = "Attack";
		public const string CameraIdleAnimation = "CameraIdle";
		public const string CameraZoomInAnimation = "CameraZoomIn";
		public const string IdleAnimation = "Idle";
		public const string InjectAnimation = "Inject";

		[SerializeField]
		private MicrobeData data;

		[SerializeField]
		private bool isInvulnerable;

		[SerializeField]
		private float invulnerabilityDuration = 2;

		[SerializeField]
		private float invulnerabilityTimeLeft;

		private SpriteRenderer spriteRenderer;
		private Animator animator;
		private GamePhysics gamePhysics;
		private Vector3 originalScale;
		private bool isMoving;
		private bool isAttacking;
		private bool isCoolingDown;

		public event Action<IDamageable> Damaged;
		public event Action Killed;


		#region Properties
		public float Acceleration
		{
			get { return this.data.Acceleration; }
			set { this.data.Acceleration = value; }
		}


		public Animator Animator
		{
			get { return this.animator; }
		}


		public float AttackCooldown
		{
			get { return this.data.AttackCooldown; }
			set { this.data.AttackCooldown = value; }
		}


		public float AttackDuration
		{
			get { return this.data.AttackDuration; }
			set { this.data.AttackDuration = value; }
		}


		public float AttackSpeed
		{
			get { return this.data.AttackSpeed; }
			set { this.data.AttackSpeed = value; }
		}


		public int CurrentHealth
		{
			get { return this.data.CurrentHealth; }
			set
			{
				this.data.CurrentHealth = value;
				UpdateOpacity();
			}
		}


		public MicrobeData Data
		{
			get { return this.data; }
			set
			{
				this.data = value;
				Initialize();
			}
		}


		public float Deceleration
		{
			get { return this.data.Deceleration; }
			set { this.data.Deceleration = value; }
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


		public bool IsCoolingDown
		{
			get { return this.isCoolingDown; }
			set { this.isCoolingDown = value; }
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
			get { return this.data.MaxHealth; }
			private set { this.data.MaxHealth = value; }
		}


		public float MaxSpeed
		{
			get { return this.data.MaxSpeed; }
			set { this.data.MaxSpeed = value; }
		}


		public Vector3 OriginalScale
		{
			get { return this.originalScale; }
		}


		public float SightDistance
		{
			get { return this.data.SightDistance; }
			set { this.data.SightDistance = value; }
		}


		public SpriteRenderer SpriteRenderer
		{
			get { return this.spriteRenderer; }
		}


		public int Strength
		{
			get { return this.data.Strength; }
			set { this.data.Strength = value; }
		}
		#endregion


		public void Awake()
		{
			this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();
			this.animator = GetComponentInChildren<Animator>();
			this.gamePhysics = GetComponent<GamePhysics>();
			this.originalScale = this.transform.localScale;
			this.CurrentHealth = this.MaxHealth;
			this.Killed += OnKilled;
		}


		public void FixedUpdate()
		{
			if (!this.isMoving
				&& this.gamePhysics.Velocity != Vector2.zero)
			{
				Vector2 newSpeed = Vector2.Lerp(
					this.gamePhysics.Velocity,
					Vector2.zero,
					this.Deceleration * Time.deltaTime);
				this.gamePhysics.SetMovement(newSpeed);
			}

			this.isMoving = false;
		}


		public void Initialize()
		{
			UpdateOpacity();
		}


		public void Move(Vector2 direction, float speed = 0)
		{
			Move(direction.x, direction.y, speed);
		}


		public void Move(float x, float y, float speed = 0)
		{
			if (this.isAttacking)
			{
				return;
			}

			if (speed == 0)
			{
				x = x * this.Acceleration;
				y = y * this.Acceleration;
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


		public void OnKilled()
		{
			MicrobeData microbeParent = Game.MicrobeMap.CurrentMicrobe;
			if (microbeParent != null
				&& microbeParent.Map.Microbes.Contains(this.data))
			{
				microbeParent.Map.Microbes.Remove(this.data);
			}
		}


		public int TakeDamage(int amount)
		{
			amount = this.DeductHealth(amount);
			if (amount > 0)
			{
				if (this.Damaged != null)
					this.Damaged(this);

				if (this.CurrentHealth == 0
					&& this.Killed != null)
				{
					Debug.Log("Killed.");
					this.Killed();
				}
			}

			return amount;
		}


		public void Update()
		{
			this.CountdownInvulnerabilityTimeLeft();
		}


		public IEnumerator WaitForCooldownEnd()
		{
			yield return new WaitForSeconds(this.AttackCooldown);

			this.isCoolingDown = false;
		}


		#region Helper Methods
		private void UpdateOpacity()
		{
			float alpha = 0.5f + (this.GetCurrentHealthPercent() / 2);
			this.spriteRenderer.color = this.spriteRenderer.color.SetAlpha(alpha);
		}
		#endregion
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

				if (GUILayout.Button("Load Internal Map"))
				{
					Game.MicrobeMap.SetCurrentMicrobe(this.microbe.Data);
				}
			}
		}
	}
}

#endif
#endregion