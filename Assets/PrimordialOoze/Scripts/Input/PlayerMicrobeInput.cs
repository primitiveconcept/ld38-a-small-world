namespace PrimordialOoze
{
	using Assets.PrimordialOoze.Scripts.Extensions.Vectors;
	using UnityEngine;
	using UnityStandardAssets.CrossPlatformInput;
	using MonoBehaviour = UnityEngine.MonoBehaviour;


	public class PlayerMicrobeInput : MicrobeInput
	{
		public const string Fire1 = "Fire1";
		public const string HorizontalAxis = "Horizontal";
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
		#endregion


		public override void ProcessAttackInput()
		{
			if (CrossPlatformInputManager.GetButtonDown(Fire1))
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

				Debug.Log("Attack: " + target.x + ", " + target.y);
				this.Microbe.Attack(target.x, target.y);
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