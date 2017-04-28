namespace PrimordialOoze
{
	using UnityEngine;
	using UnityEngine.SceneManagement;


	public class SceneLoading : MonoBehaviour
	{
		public void LoadScene(int sceneIndex)
		{
			Debug.Log("Loading Game...");
			SceneManager.LoadScene(sceneIndex);
		}
	}
}
