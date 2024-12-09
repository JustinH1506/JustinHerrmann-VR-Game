using UnityEngine;

public class Goal : MonoBehaviour
{
	[SerializeField] private GameObject winScreen = null;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			winScreen.SetActive(true);

			Time.timeScale = 0f;
		}
	}
}
