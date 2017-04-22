namespace PrimordialOoze
{
	using UnityEngine;


	public class Microbe : MonoBehaviour
	{
		[SerializeField]
		private float sightDistance = 5f;

		[Header("Movement")]
		[SerializeField]
		private float acceleration = 5;

		[SerializeField]
		private float deceleration = 2;

		[SerializeField]
		private float maxSpeed = 5;

		private GamePhysics gamePhysics;
		private bool isMoving;


		#region Properties
		public float Acceleration
		{
			get { return this.acceleration; }
			set { this.acceleration = value; }
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


		public bool IsMoving
		{
			get { return this.isMoving; }
			set { this.isMoving = value; }
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


		public void Awake()
		{
			this.gamePhysics = GetComponent<GamePhysics>();
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


		public void Move(Vector2 direction)
		{
			Move(direction.x, direction.y);
		}


		public void Move(float x, float y)
		{
			x = x * this.acceleration;
			y = y * this.acceleration;

			float xSpeedNextFrame = Mathf.Abs(this.gamePhysics.Velocity.x + (x * Time.deltaTime));
			if (xSpeedNextFrame > this.MaxSpeed)
				x = 0;

			float ySpeedNextFrame = Mathf.Abs(this.gamePhysics.Velocity.y + (y * Time.deltaTime));
			if (ySpeedNextFrame > this.MaxSpeed)
				y = 0;

			this.gamePhysics.AddForce(x, y);
			this.isMoving = true;
		}


		public void Start()
		{
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
			}
		}
	}
}

#endif
#endregion