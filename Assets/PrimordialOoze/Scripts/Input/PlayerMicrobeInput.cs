namespace PrimordialOoze
{
	using PrimordialOoze.Extensions.Vectors;
	using UnityEngine;
	using UnityStandardAssets.CrossPlatformInput;


	public class PlayerMicrobeInput : MicrobeInput
	{
		public const string Fire1 = "Fire1";
		public const string Fire2 = "Fire2";
		public const string HorizontalAxis = "Horizontal";
		public const string MouseScrollwheel = "Mouse ScrollWheel";
		public const string VerticalAxis = "Vertical";


		#region Properties
		/// <summary>
		/// Current world position of the mouse pointer.
		/// </summary>
		public static Vector3 MousePosition
		{
			get
			{
				return Camera.main.ScreenToWorldPoint(
					Input.mousePosition
						.SetZ(0));
			}
		}


		public PrimaryAttack PrimaryAttack
		{
			get { return GetComponent<PrimaryAttack>(); }
		}


		public SecondaryAttack SecondaryAttack
		{
			get { return GetComponent<SecondaryAttack>(); }
		}
		#endregion


		public override void ProcessAttackInput()
		{
			if (CrossPlatformInputManager.GetButtonDown(Fire1)
				|| CrossPlatformInputManager.GetButtonDown(Fire2))
			{
				Vector3 target = new Vector3(
						CrossPlatformInputManager.GetAxisRaw(HorizontalAxis),
						CrossPlatformInputManager.GetAxisRaw(VerticalAxis))
					.normalized;

				if (target.x == 0
					&& target.y == 0)
				{
					target = (MousePosition - this.transform.position).normalized;
				}

				if (CrossPlatformInputManager.GetButtonDown(Fire1))
				{
					PrimaryAttack primaryAttack = this.PrimaryAttack;
					if (primaryAttack != null)
						primaryAttack.Attack(target.x, target.y);
				}

				else if (CrossPlatformInputManager.GetButtonDown(Fire2))
				{
					SecondaryAttack secondaryAttack = this.SecondaryAttack;
					if (secondaryAttack != null)
						secondaryAttack.Attack(target.x, target.y);
				}
			}
		}


		public override void ProcessDirectionalInput()
		{
			float xMovement =
				CrossPlatformInputManager.GetAxisRaw(HorizontalAxis);

			float yMovement =
				CrossPlatformInputManager.GetAxisRaw(VerticalAxis);

			if (xMovement == 0
				&& yMovement == 0)
			{
				return;
			}

			this.Microbe.Move(xMovement, yMovement);
		}


		public void ProcessMicrobeScalerInput()
		{
			float mouseWheel = CrossPlatformInputManager.GetAxis(MouseScrollwheel);
			if (mouseWheel == 0)
				return;

			Debug.Log(mouseWheel);

			MicrobeScaler microbeScaler = GetComponent<MicrobeScaler>();
			if (microbeScaler == null)
				return;

			mouseWheel = mouseWheel * Game.MicrobeMap.transform.localScale.x;

			microbeScaler.ChangeScale(1 + mouseWheel);
		}


		public override void Update()
		{
			base.Update();

			ProcessMicrobeScalerInput();
		}
	}
}


#region Editor
#if UNITY_EDITOR

namespace PrimordialOoze
{
	using UnityEditor;
	using UnityStandardAssets.CrossPlatformInput;


	[CustomEditor(typeof(PlayerMicrobeInput))]
	public class PlayerInputInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			EditorGUILayout.FloatField(
				"Horizontal Axis",
				CrossPlatformInputManager.GetAxisRaw(PlayerMicrobeInput.HorizontalAxis));
			EditorGUILayout.FloatField(
				"Vertical Axis",
				CrossPlatformInputManager.GetAxisRaw(PlayerMicrobeInput.VerticalAxis));
		}
	}
}

#endif
#endregion