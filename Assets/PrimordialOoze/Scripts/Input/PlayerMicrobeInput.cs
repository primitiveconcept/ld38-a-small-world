namespace PrimordialOoze
{
	using UnityStandardAssets.CrossPlatformInput;
	using MonoBehaviour = UnityEngine.MonoBehaviour;


	public class PlayerMicrobeInput : MicrobeInput
	{
		public const string HorizontalAxis = "Horizontal";
		public const string VerticalAxis = "Vertical";
		

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