namespace PrimordialOoze
{
	using System.Linq;
	using UnityEngine;


	public class Game : MonoBehaviour
	{
		private static Game _instance;

		[SerializeField]
		private SightUI sightUI;

		[SerializeField]
		private GameObject bubblesEffect;

		[SerializeField]
		private MicrobeMap microbeMap;

		[SerializeField]
		private Microbe playerPrefab;

		private Microbe[] overworldMicrobes;


		#region Properties
		public static GameObject BubblesEffect
		{
			get { return Instance.bubblesEffect; }
		}


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


		public static Microbe[] OverworldMicrobes
		{
			get { return Instance.overworldMicrobes; }
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


		public static Microbe FindMicrobe(MicrobeData microbeData)
		{
			Microbe queriedMicrobe = MicrobeMap.Microbes.FirstOrDefault(
				microbe => microbe.Data == microbeData);

			if (queriedMicrobe == null)
			{
				queriedMicrobe = Instance.overworldMicrobes.FirstOrDefault(
					microbe => microbe.Data == microbeData);
			}

			return queriedMicrobe;
		}


		public void Awake()
		{
			EnforceSingleInstance();
		}


		public void Start()
		{
			this.sightUI.gameObject.SetActive(true);
			this.sightUI.UpdateSight(PlayerMicrobe.SightDistance);
			this.overworldMicrobes = FindObjectsOfType<Microbe>();
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