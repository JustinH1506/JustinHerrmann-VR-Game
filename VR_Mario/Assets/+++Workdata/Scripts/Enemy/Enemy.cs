using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
	[SerializeField] private GameObject loseScreen = null;
	
	[SerializeField] private float moveSpeed = 0f;

	[SerializeField] private float totalLevelLength = 160f;

	[SerializeField] private float radius = 0f;

	private float angle = 0f;

	private Rigidbody rb = null;

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

			Time.timeScale = 0f;

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
		
		// 	
		// float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
		// float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
		//
		// Vector3 playerZPosition = new Vector3(transform.position.x, transform.position.y, y);
		//
		// transform.position = playerZPosition;
		//
		// transform.LookAt(Vector3.zero);
		
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
