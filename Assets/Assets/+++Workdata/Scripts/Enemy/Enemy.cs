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

	/// <summary>
	/// Gets some components and calculates the angle value
	/// </summary>
	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		
		float currentPosition = transform.position.x / totalLevelLength;
			
		angle = currentPosition * 360 * Mathf.Deg2Rad;
	}

	/// <summary>
	/// Calls lose if the player gets touched. sets move speed of enemies when hitting a wall. 
	/// </summary>
	/// <param name="other"></param>
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

	/// <summary>
	/// Calculates the movement in a circular area for the enemy
	/// </summary>
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
	
	#endregion
}
