namespace PrimordialOoze
{
	using UnityEngine;


	public class Game : MonoBehaviour
	{
		private static Game _instance;

		[SerializeField]
		private SightUI sightUI;

		[SerializeField]
		private MicrobeMap microbeMap;

		[SerializeField]
		private Microbe playerPrefab;


		#region Properties
		/// <summary>
		/// Singleton design pattern
		/// </summary>
		/// <value>The instance.</value>
		public static Game Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType<Game>();
					if (_instance == null)
					{
						GameObject obj = new GameObject();
						_instance = obj.AddComponent<Game>();
						obj.name = _instance.GetType().Name;
					}
				}
				return _instance;
			}
		}


		public static MicrobeMap MicrobeMap
		{
			get
			{
				if (Instance.microbeMap == null)
					Instance.microbeMap = FindObjectOfType<MicrobeMap>();
				return Instance.microbeMap;
			}
		}


		public static GameObject Player
		{
			get { return FindObjectOfType<PlayerMicrobeInput>().gameObject; }
		}


		public static Microbe PlayerMicrobe
		{
			get
			{
				if (Player != null)
					return Player.GetComponent<Microbe>();
				return null;
			}
		}


		public static Microbe PlayerPrefab
		{
			get { return Instance.playerPrefab; }
		}
		#endregion


		public void Awake()
		{
			EnforceSingleInstance();
		}


		public void Start()
		{
			this.sightUI.gameObject.SetActive(true);
			this.sightUI.UpdateSight(PlayerMicrobe.SightDistance);
		}


		#region Helper Methods
		private void EnforceSingleInstance()
		{
			if (_instance == null)
			{
				//If I am the first instance, make me the Singleton
				_instance = this;
				DontDestroyOnLoad(this.gameObject);
			}
			else
			{
				//If a Singleton already exists and you find
				//another reference in scene, destroy it!
				if (this != _instance)
				{
					Destroy(this.gameObject);
				}
			}
		}
		#endregion
	}
}