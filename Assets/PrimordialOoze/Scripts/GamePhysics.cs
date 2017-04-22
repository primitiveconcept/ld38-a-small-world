namespace PrimordialOoze
{
	using Assets.PrimordialOoze.Scripts.Extensions.Vectors;
	using UnityEngine;


	public class GamePhysics : MonoBehaviour
	{
		private Rigidbody2D rigidbody2D;
		private Vector2 forcesApplied;
		private Vector2 positionLastFrame;


		#region Properties
		public float CurrentSpeed
		{
			get
			{
				float xSpeed = this.CurrentXSpeed;
				float ySpeed = this.CurrentYSpeed;
				return xSpeed > ySpeed
							? xSpeed
							: ySpeed;
			}
		}


		public float CurrentXSpeed
		{
			get { return Mathf.Abs(this.Velocity.x); }
		}


		public float CurrentYSpeed
		{
			get { return Mathf.Abs(this.Velocity.y); }
		}


		public Vector2 Velocity
		{
			get { return this.rigidbody2D.velocity; }
			set { this.rigidbody2D.velocity = value; }
		}
		#endregion

		
		public void AddForce(float x, float y)
		{
			this.rigidbody2D.AddForce(new Vector2(x, y));
		}
		

		public void Awake()
		{
			this.rigidbody2D = GetComponent<Rigidbody2D>();
			if (this.rigidbody2D == null)
			{
				this.rigidbody2D = this.gameObject.AddComponent<Rigidbody2D>();
			}

			this.rigidbody2D.isKinematic = false;
			this.rigidbody2D.gravityScale = 0;
			this.rigidbody2D.freezeRotation = true;
			this.rigidbody2D.angularDrag = 0;
			this.rigidbody2D.drag = 0;
		}


		public void SetHorizontalMovement(float value)
		{
			this.Velocity = Velocity.SetX(value);
		}


		public void SetVerticalMovement(float value)
		{
			this.Velocity = Velocity.SetY(value);
		}


		public void SetMovement(Vector2 value)
		{
			this.Velocity = value;
		}


		public void StopMovement()
		{
			this.Velocity = Vector2.zero;
		}
	}
}


#region Editor
#if UNITY_EDITOR

namespace PrimordialOoze
{
	using UnityEditor;
	using UnityEngine;


	[CustomEditor(typeof(GamePhysics))]
	public class GamePhysicsInspector : Editor
	{
		private GamePhysics gamePhysics;


		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (Application.isPlaying)
			{
				if (this.gamePhysics == null)
					this.gamePhysics = target as GamePhysics;

				this.gamePhysics.Velocity = EditorGUILayout.Vector2Field("Velocity", this.gamePhysics.Velocity);
				EditorGUILayout.FloatField("Current Speed", this.gamePhysics.CurrentSpeed);
			}
		}
	}
}

#endif
#endregion