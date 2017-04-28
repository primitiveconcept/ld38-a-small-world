namespace PrimordialOoze
{
	using PrimordialOoze.Extensions.Coroutines;
	using UnityEngine;

	[RequireComponent(typeof(AudioSource))]
	public class OneShotAudio : MonoBehaviour
	{
		public void Start()
		{
			this.WaitForSeconds(
				GetComponent<AudioSource>().clip.length,
				() =>
					{
						Destroy(this.gameObject);
					});
		}
	}
}