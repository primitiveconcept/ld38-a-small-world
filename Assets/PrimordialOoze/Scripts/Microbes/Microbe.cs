namespace PrimordialOoze
{
	using System;
	using System.Collections;
	using PrimordialOoze.Extensions.Colors;
	using PrimordialOoze.Extensions.Coroutines;
	using UnityEngine;


	public class Microbe : MonoBehaviour,
							IDamageable,
							IInjectable
	{
		public const string AttackAnimation = "Attack";
		public const string DeathAnimation = "Death";
		public const string IdleAnimation = "Idle";
		public const string InjectAnimation = "Inject";

		[SerializeField]
		private MicrobeData data;

		[SerializeField]
		private bool isInvulnerable;

		[SerializeField]
		private float invulnerabilityDuration = 0.5f;

		[SerializeField]
		private float invulnerabilityTimeLeft;

		[SerializeField]
		private bool orientMovement = false;

		[SerializeField]
		private bool attacksBackward = false;

		private SpriteRenderer spriteRenderer;
		private Animator animator;
		private GamePhysics gamePhysics;
		private Color originalColor;
		private Color currentColor;
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


		public bool AttacksBackward
		{
			get { return this.attacksBackward; }
			set { this.attacksBackward = value; }
		}


		public float AttackSpeed
		{
			get { return this.data.AttackSpeed; }
			set { this.data.AttackSpeed = value; }
		}


		public Color CurrentColor
		{
			get { return this.currentColor; }
			set { this.currentColor = value; }
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


		public MicrobeInput Input
		{
			get { return GetComponent<MicrobeInput>(); }
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
			private set
			{
				this.data.MaxHealth = value;
				UpdateScale();
			}
		}


		public float MaxSpeed
		{
			get { return this.data.MaxSpeed; }
			set { this.data.MaxSpeed = value; }
		}


		public bool OrientMovement
		{
			get { return this.orientMovement; }
			set { this.orientMovement = value; }
		}


		public Color OriginalColor
		{
			get { return this.originalColor; }
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


		public static Vector3 GetScaleForMaxHealth(
			Vector3 originalScale,
			int maxHealth)
		{
			float scalar = maxHealth / 100f;
			Vector3 scale = originalScale * scalar;

			return scale;
		}


		public void Awake()
		{
			this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();
			this.animator = GetComponentInChildren<Animator>();
			this.gamePhysics = GetComponent<GamePhysics>();
			this.originalScale = this.transform.localScale;
			this.originalColor = this.spriteRenderer.color;
			this.currentColor = this.spriteRenderer.color;
			this.CurrentHealth = this.MaxHealth;
			this.Killed += OnKilled;
			Initialize();
		}


		public bool CanBeInjectedBy(Microbe injector)
		{
			if (this.GetCurrentHealthPercent() <= 0.5f
				&& injector.transform.localScale.x <= this.transform.localScale.x / 2)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		public void CompleteInjection(Microbe injector)
		{
			Game.MicrobeMap.SetCurrentMicrobe(this.Data);
			if (this.Data.ParentMicrobeData != null)
				Destroy(this.gameObject);
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

			if (this.orientMovement
				&& this.isMoving
				&& !this.isAttacking)
			{
				RotateToward(
					this.gamePhysics.Velocity.x,
					this.gamePhysics.Velocity.y);
			}

			this.isMoving = false;
		}


		public void Initialize()
		{
			UpdateOpacity();
			UpdateScale();
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


		public void RotateAwayFrom(float x, float y)
		{
			float zRotation = Mathf.Atan2(-y, -x) * Mathf.Rad2Deg;
			transform.localRotation = Quaternion.Euler(0f, 0f, zRotation - 90);
		}


		public void RotateToward(float x, float y)
		{
			float zRotation = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
			transform.localRotation = Quaternion.Euler(0f, 0f, zRotation - 90);
		}


		public int TakeDamage(int amount)
		{
			amount = this.DeductHealth(amount);
			if (amount > 0)
			{
				StartCoroutine(
					this.spriteRenderer.Flicker(
						Color.clear,
						this.currentColor,
						UpdateOpacity,
						this.invulnerabilityDuration));

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


		public void UpdateCameraBasedOnScaled()
		{
			Camera.main.orthographicSize = 15 * this.transform.localScale.x;
		}


		public void UpdateOpacity()
		{
			float alpha = 0.25f + (this.GetCurrentHealthPercent() * 0.75f);
			this.spriteRenderer.color = this.currentColor.SetAlpha(alpha);
		}


		public void UpdateScale()
		{
			this.transform.localScale =
				GetScaleForMaxHealth(this.originalScale, this.MaxHealth);

			if (GetComponent<PlayerMicrobeInput>() != null)
				UpdateCameraBasedOnScaled();
		}


		public IEnumerator WaitForCooldownEnd()
		{
			yield return new WaitForSeconds(this.AttackCooldown);

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