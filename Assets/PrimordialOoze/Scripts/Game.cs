namespace PrimordialOoze
{
	using System.Collections.Generic;
	using System.Linq;
	using PrimordialOoze.Extensions.Colors;
	using PrimordialOoze.Extensions.Coroutines;
	using UnityEngine;
	using UnityEngine.SceneManagement;
	using UnityEngine.UI;


	public class Game : MonoBehaviour
	{
		private static Game _instance;

		[SerializeField]
		private SightUI sightUI;

		[SerializeField]
		private Text primaryAttackText;

		[SerializeField]
		private Text secondaryAttackText;

		[SerializeField]
		private Text hintText;

		[SerializeField]
		private Text tooltipText;

		[SerializeField]
		private ScrollingBackground scrollingBackground;

		[SerializeField]
		private Color overworldColor;

		[SerializeField]
		private Color insideMicrobeColor;

		private MicrobeMap microbeMap;

		[SerializeField]
		private Microbe playerPrefab;

		[SerializeField]
		private List<Microbe> overworldMicrobes;


		#region Properties
		public static ScrollingBackground ScrollingBackground
		{
			get { return Instance.scrollingBackground; }
		}


		public static Color InsideMicrobeColor
		{
			get { return Instance.insideMicrobeColor; }
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


		public static Color OverworldColor
		{
			get { return Instance.overworldColor; }
		}


		public static List<Microbe> OverworldMicrobes
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


		public static SightUI SightUi
		{
			get { return Instance.sightUI; }
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


		public static void HideTooltip()
		{
			Instance.tooltipText.gameObject.SetActive(false);
		}


		public static void SetPrimaryAttackText(string text)
		{
			if (string.IsNullOrEmpty(text))
				Instance.primaryAttackText.transform.parent.gameObject.SetActive(false);
			if (string.IsNullOrEmpty(text))
				Instance.primaryAttackText.transform.parent.gameObject.SetActive(true);

			if (Instance.primaryAttackText.text != text)
			{
				Instance.primaryAttackText.text = text;
				Instance.StartCoroutine(Instance.primaryAttackText.Flicker(
					Color.yellow,
					Instance.primaryAttackText.color,
					null,
					1f,
					0.1f));
			}
		}


		public static void SetSecondaryAttackText(string text)
		{
			if (string.IsNullOrEmpty(text))
				Instance.secondaryAttackText.transform.parent.gameObject.SetActive(false);
			if (string.IsNullOrEmpty(text))
				Instance.secondaryAttackText.transform.parent.gameObject.SetActive(true);

			if (Instance.secondaryAttackText.text != text)
			{
				Instance.secondaryAttackText.text = text;
				Instance.StartCoroutine(Instance.secondaryAttackText.Flicker(
					Color.yellow,
					Instance.secondaryAttackText.color,
					null,
					1f,
					0.1f));
			}
		}


		public static void ShowHintText(string text, float seconds)
		{
			Instance.hintText.text = text;
			Instance.WaitForSeconds(
				seconds,
				() =>
					{
						Instance.hintText.text = "";
					});
		}


		public static void ShowTooltip(string text, Vector2 position)
		{
			if (Instance.tooltipText.text != text)
				Instance.tooltipText.text = text;
			Instance.tooltipText.transform.position = position;
			Instance.tooltipText.gameObject.SetActive(true);
		}


		public void Awake()
		{
			EnforceSingleInstance();
		}


		public void Start()
		{
			this.sightUI.gameObject.SetActive(true);
			this.sightUI.UpdateSight(PlayerMicrobe.SightDistance);
			this.overworldMicrobes = FindObjectsOfType<Microbe>().ToList();
			this.overworldMicrobes.Remove(Game.PlayerMicrobe);

			ShowHintText("Eliminate all enemy microbes. Dash Attack to imobilize, then Inject to enter them.", 8f);
		}


		#region Helper Methods
		private void EnforceSingleInstance()
		{
			if (_instance == null)
			{
				//If I am the first instance, make me the Singleton
				_instance = this;
				//DontDestroyOnLoad(this.gameObject);
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