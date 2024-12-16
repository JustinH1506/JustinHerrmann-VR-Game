using UnityEngine;

public class Goal : MonoBehaviour
{
	[SerializeField] private GameObject winScreen = null;

	/// <summary>
	/// Opens the win screen
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			winScreen.SetActive(true);
		}
	}
}
