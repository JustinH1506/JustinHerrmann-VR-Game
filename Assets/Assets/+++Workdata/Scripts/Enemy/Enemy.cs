using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
	#region Variables

	[SerializeField] private GameObject loseScreen = null;
	
	[SerializeField] private float moveSpeed = 0f;

	[SerializeField] private float totalLevelLength = 160f;

	[SerializeField] private float radius = 0f;

	private float angle = 0f;

	private Rigidbody rb = null;

	#endregion

	#region Methods

	

	#endregion
	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		
		float currentPosition = transform.position.x / totalLevelLength;
			
		angle = currentPosition * 360 * Mathf.Deg2Rad;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			other.gameObject.SetActive(false);
			
			loseScreen.SetActive(true);

			return;
		}

		moveSpeed = -moveSpeed;
	}

	private void Update()
	{

		if (Input.GetKeyDown(KeyCode.R))
		{
			Time.timeScale = 1f;

			SceneManager.LoadScene(0);
		}
		
		angle += moveSpeed * Time.deltaTime;  // increment angle to move the enemy around the circle
		Vector3 enemyPosition = new Vector3(
			Mathf.Cos(angle) * radius,
			0f,
			Mathf.Sin(angle) * radius
		);
		rb.MovePosition(enemyPosition);
		
		transform.LookAt(Vector3.zero);
	}
}
