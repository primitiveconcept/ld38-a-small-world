namespace PrimordialOoze
{
	using UnityEngine;


    [ExecuteInEditMode]
	public class CameraBounds : MonoBehaviour
	{
		[Space(10)]
		[Header("Camera Bounds")]
		/// The camera limits, camera won't go beyond this point.
		public Bounds Bounds = new Bounds(Vector3.zero, Vector3.one * 10);

        public bool AnchorToGameObject = false;


        // TODO: Make Bounds respect local space, and axe this.
        public void Update()
        {
            if (this.AnchorToGameObject)
            {
                this.Bounds.center = this.transform.position;
            }
        }

	}
}