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

		private float fire1HoldTime;
		private float fire2HoldTime;


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
		#endregion


		public void Start()
		{
			var primaryAttack = GetComponent<PrimaryAttack>();
			if (primaryAttack != null)
				Game.SetPrimaryAttackText(primaryAttack.AttackName);
			else
				Game.SetPrimaryAttackText("(None)");

			var secondaryAttack = GetComponent<SecondaryAttack>();
			if (secondaryAttack != null)
				Game.SetSecondaryAttackText(secondaryAttack.AttackName);
			else
				Game.SetSecondaryAttackText("(None)");
		}


		public override void ProcessAttackInput()
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

			if (CrossPlatformInputManager.GetButton(Fire1))
			{
				PrimaryAttack primaryAttack = this.PrimaryAttack;
				if (primaryAttack != null)
					primaryAttack.Attack(target.x, target.y);
			}

			else if (CrossPlatformInputManager.GetButton(Fire2))
			{
				SecondaryAttack secondaryAttack = this.SecondaryAttack;
				if (secondaryAttack != null)
					secondaryAttack.Attack(target.x, target.y);
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
			if (CrossPlatformInputManager.GetButtonDown(Fire1))
				this.fire1HoldTime = Time.deltaTime;
			if (CrossPlatformInputManager.GetButtonDown(Fire2))
				this.fire2HoldTime = Time.deltaTime;
			if (CrossPlatformInputManager.GetButton(Fire1))
				this.fire1HoldTime += Time.deltaTime;
			if (CrossPlatformInputManager.GetButton(Fire2))
				this.fire2HoldTime += Time.deltaTime;
			if (CrossPlatformInputManager.GetButtonUp(Fire1))
				this.fire1HoldTime = 0;
			if (CrossPlatformInputManager.GetButtonUp(Fire2))
				this.fire2HoldTime = 0;

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