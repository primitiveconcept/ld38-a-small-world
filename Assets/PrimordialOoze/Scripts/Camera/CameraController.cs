namespace PrimordialOoze
{
	using UnityEngine;


	[RequireComponent(typeof(Camera))]
	public class CameraController : MonoBehaviour
	{
		[Space(10)]
		[Header("Dimension")]
		public bool Camera3D = false;

		[Space(10)]
		[Header("Distances")]
		/// How far ahead from the Player the camera is supposed to be		
		public float HorizontalLookDistance = 3;
		/// Vertical Camera Offset	
		public Vector3 CameraOffset;
		/// Minimal distance that triggers look ahead
		public float LookAheadTrigger = 0.1f;
		/// How high (or low) from the Player the camera should move when looking up/down
		public float ManualUpDownLookDistance = 3;

		[Space(10)]
		[Header("Movement Speed")]
		/// How fast the camera goes back to the Player
		public float ResetSpeed = 0.5f;
		/// How fast the camera moves
		public float CameraSpeed = 0.3f;

		[Space(10)]
		[Header("Camera Zoom")]
		public bool ZoomWithMovement = false;
		[Range(1, 20)]
		public float MinimumZoom = 7f;
		[Range(1, 20)]
		public float MaximumZoom = 7f;
		public float ZoomSpeed = 0.4f;

		// Private variables

		protected Transform target;
		protected GamePhysics targetController;
		// TODO: Just get rid of level bounds.
		protected Bounds levelBounds;
		protected CameraBounds[] cameraBounds;

		protected float xMin;
		protected float xMax;
		protected float yMin;
		protected float yMax;

		protected float offsetZ;
		protected Vector3 lastTargetPosition;
		protected Vector3 currentVelocity;
		protected Vector3 lookAheadPos;

		protected float shakeFrequency;
		protected float shakeIntensity;
		protected float shakeDuration;

		protected float currentZoom;
		protected new Camera camera;

		protected Vector3 lookDirectionModifier = new Vector3(0, 0, 0);
		protected float originalOrthographicSize;


		#region Properties
		/// <summary>
		/// True if the camera should follow the player.
		/// </summary>
		public bool FollowsPlayer { get; set; }


		public float OriginalOrthographicSize
		{
			get { return this.originalOrthographicSize; }
		}
		#endregion


		/// <summary>
		/// Every frame, we move the camera if needed
		/// </summary>
		public void FixedUpdate()
		{
			// if the camera is not supposed to follow the player, we do nothing
			if (!FollowsPlayer)
				return;

			HandleAutoZoom();

			// if the player has moved since last update
			float xMoveDelta = (this.target.position - this.lastTargetPosition).x;

			Vector3 aheadTargetPos = GetLookAheadPosition(xMoveDelta);

			Vector3 newCameraPosition = Vector3.SmoothDamp(
				transform.position,
				aheadTargetPos,
				ref this.currentVelocity,
				CameraSpeed);

			Vector3 shakeFactorPosition = new Vector3(0, 0, 0);

			// If shakeDuration is still running.
			if (this.shakeDuration > 0)
			{
				//shakeFactorPosition = Random.insideUnitSphere * shakeIntensity * shakeDuration;

				shakeFactorPosition = new Vector3(
					0,
					Mathf.Sin(Time.time * this.shakeFrequency) * this.shakeIntensity * this.shakeDuration,
					0);
				this.shakeDuration -= Time.deltaTime;
			}
			newCameraPosition = newCameraPosition + shakeFactorPosition;

			if (Camera3D == false)
			{
				float posX, posY, posZ = 0f;
				// Clamp to level boundaries
				if (this.levelBounds.size != Vector3.zero)
				{
					posX = Mathf.Clamp(newCameraPosition.x, this.xMin, this.xMax);
					posY = Mathf.Clamp(newCameraPosition.y, this.yMin, this.yMax);
				}
				else
				{
					posX = newCameraPosition.x;
					posY = newCameraPosition.y;
				}
				posZ = newCameraPosition.z;
				// We move the actual transform
				transform.position = new Vector3(posX, posY, posZ);
			}
			else
			{
				transform.position = newCameraPosition;
			}

			this.lastTargetPosition = this.target.position;
		}


		public Vector3 GetLookAheadPosition(float xMoveDelta = 0)
		{
			bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > LookAheadTrigger;

			if (updateLookAheadTarget)
			{
				this.lookAheadPos = HorizontalLookDistance * Vector3.right * Mathf.Sign(xMoveDelta);
			}
			else
			{
				this.lookAheadPos = Vector3.MoveTowards(this.lookAheadPos, Vector3.zero, Time.deltaTime * ResetSpeed);
			}

			Vector3 aheadTargetPos = this.target.position + this.lookAheadPos + Vector3.forward * this.offsetZ
									+ this.lookDirectionModifier
									+ CameraOffset;

			return aheadTargetPos;
		}


		/// <summary>
		/// Moves the camera down
		/// </summary>
		public void LookDown()
		{
			this.lookDirectionModifier = new Vector3(0, -ManualUpDownLookDistance, 0);
		}


		/// <summary>
		/// Moves the camera up
		/// </summary>
		public void LookUp()
		{
			this.lookDirectionModifier = new Vector3(0, ManualUpDownLookDistance, 0);
		}


		/// <summary>
		/// Resets the look direction modifier
		/// </summary>
		public void ResetLookUpDown()
		{
			this.lookDirectionModifier = new Vector3(0, 0, 0);
		}


		/// <summary>
		/// Set camera to original zoom level, disabling speed-based zoom.
		/// </summary>
		public void ResetZoom()
		{
			SetStaticZoom(this.originalOrthographicSize);
		}


		/// <summary>
		/// Set camera to a specific zoom level, and disable speed-based zoom.
		/// </summary>
		/// <param name="zoom"></param>
		public void SetStaticZoom(float zoom)
		{
			this.ZoomWithMovement = false;
			this.MinimumZoom = zoom;
			this.MaximumZoom = zoom;
			this.camera.orthographicSize = zoom;
		}


		/// <summary>
		/// Use this method to shake the camera.
		/// </summary>
		/// <param name="intensity">Magnitude of shake.</param>
		/// <param name="frequency">How fast it shakes.</param>
		/// <param name="duration">How long it should shake.</param>
		public void Shake(float intensity, float frequency, float duration)
		{
			this.shakeIntensity = intensity;
			this.shakeFrequency = frequency;
			this.shakeDuration = duration;
		}


		#region Helper Methods
		/// <summary>
		/// Gets the levelbounds coordinates to lock the camera into the level
		/// </summary>
		private void GetCameraBounds()
		{
			if (this.levelBounds.size == Vector3.zero)
			{
				return;
			}

			Bounds lockedBounds = this.levelBounds;
			foreach (CameraBounds bounds in this.cameraBounds)
			{
				if (!bounds.gameObject.activeInHierarchy
					|| !bounds.enabled)
				{
					continue;
				}

				if (bounds.Bounds.Contains(this.target.position))
				{
					lockedBounds = bounds.Bounds;
					break;
				}
			}

			// camera size calculation (orthographicSize is half the height of what the camera sees.
			float cameraHeight = Camera.main.orthographicSize * 2f;
			float cameraWidth = cameraHeight * Camera.main.aspect;

			this.xMin = lockedBounds.min.x + (cameraWidth / 2);
			this.xMax = lockedBounds.max.x - (cameraWidth / 2);
			this.yMin = lockedBounds.min.y + (cameraHeight / 2);
			this.yMax = lockedBounds.max.y - (cameraHeight / 2);
		}


		/// <summary>
		/// Handles the zoom of the camera based on the main character's speed
		/// </summary>
		private void HandleAutoZoom()
		{
			if (this.ZoomWithMovement)
			{
				float characterSpeed = Mathf.Max(
					Mathf.Abs(this.targetController.CurrentXSpeed),
					Mathf.Abs(this.targetController.CurrentYSpeed));
				float currentVelocity = 0f;

				this.currentZoom = Mathf.SmoothDamp(
					this.currentZoom,
					(characterSpeed / 10) * (MaximumZoom - MinimumZoom) + MinimumZoom,
					ref currentVelocity,
					ZoomSpeed);

				this.camera.orthographicSize = this.currentZoom;
			}

			GetCameraBounds();
		}


		/// <summary>
		/// Initialization
		/// </summary>
		private void Start()
		{
			// we get the camera component
			this.camera = GetComponent<Camera>();

			// We make the camera follow the player
			FollowsPlayer = true;
			this.currentZoom = MinimumZoom;

			// player and level bounds initialization
			this.target = FindObjectOfType<PlayerMicrobeInput>().transform;
			if (this.target.GetComponent<GamePhysics>() == null)
				return;
			this.targetController = this.target.GetComponent<GamePhysics>();

			this.cameraBounds = FindObjectsOfType<CameraBounds>();

			// we store the target's last position
			this.lastTargetPosition = this.target.position;
			this.offsetZ = (transform.position - this.target.position).z;
			transform.parent = null;

			//lookDirectionModifier=new Vector3(0,0,0);
			this.originalOrthographicSize = this.camera.orthographicSize;

			HandleAutoZoom();
		}
		#endregion
	}
}