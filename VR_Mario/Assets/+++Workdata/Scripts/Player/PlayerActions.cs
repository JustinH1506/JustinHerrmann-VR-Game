using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerActions : MonoBehaviour
{
	[SerializeField] private float moveSpeed = 0f;

	[SerializeField] private float jumpForce = 0f;

	[SerializeField] private float angle = 0f;

	[SerializeField] private float totalLevelLength = 32f;

	[SerializeField] private float radius = 0f;

	[SerializeField] private float groundDistance = 0f;

	[SerializeField] private LayerMask groundMask;

	[SerializeField] private bool tesBool = false;

	[SerializeField] private float maxControllerMoveAmount = 0f;
	
	[SerializeField] private float maxMoveTime = 0f;

	[SerializeField] private GameObject controllerLeft = null;

	[SerializeField] private Transform endpoint = null;

	private Vector3 controllerPos = new Vector3();

	private Vector3 endpos = new Vector3();

	private float controllerAngle = 0f;

	private Rigidbody rb = null;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		
		float currentPosition = transform.position.x / totalLevelLength;
			
		angle = currentPosition * 360 * Mathf.Deg2Rad;
	}

	private void FixedUpdate()
	{
		MovePlayer(0f);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.H))
		{
			controllerPos = controllerLeft.transform.position;
			
			controllerAngle = controllerLeft.transform.eulerAngles.y;
			
			endpos = endpoint.position - controllerPos;
		}

		if (Input.GetKeyUp(KeyCode.H))
		{
			Debug.Log(endpoint.position);
			StartVRMovement();
		}
		
		if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
		{
			Jump();
		}
	}

	private void MovePlayer(float newInput)
	{
		float moveInput = Input.GetAxis("Horizontal");

		moveInput = -moveInput;

		if (moveInput > 0.05f && !DirectionCollision(transform.right))
		{
			moveInput = 0f;
		}
		
		if (moveInput < 0.05f && !DirectionCollision(-transform.right))
		{
			moveInput = 0f;
		}
		
		angle += moveInput * moveSpeed * Time.fixedDeltaTime;  // increment angle to move the enemy around the circle
		Vector3 enemyPosition = new Vector3(
			Mathf.Cos(angle) * radius,
			transform.position.y,
			Mathf.Sin(angle) * radius
		);
		rb.MovePosition(enemyPosition);
		
		transform.LookAt(Vector3.zero);
	}

	private void Jump()
	{
		Vector2 directionToJump = new Vector2(0f, jumpForce);

		rb.velocity = directionToJump;
	}

	bool IsGrounded()
	{
		if (Physics.Raycast(transform.position, Vector3.down, groundDistance, groundMask))
		{
			return true;
		}
		else
		{ 
			return false;
		}
	}

	private bool DirectionCollision(Vector3 raycastDirection)
	{
		if (Physics.Raycast(transform.position, raycastDirection, 0.1f))
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	public void StartVRMovement()
	{
		Vector3 newEndPos = endpoint.position - controllerPos;

		Vector3 endDifference = newEndPos - endpos;
		
		Debug.DrawLine(endpoint.position, endpoint.position - endDifference, Color.green, 10f );

		endDifference = Quaternion.AngleAxis(-controllerAngle, Vector3.up) * endDifference; 
		
		Debug.DrawLine(endpoint.position, endpoint.position - endDifference, Color.magenta, 10f );
		
		if (endDifference.x >= maxControllerMoveAmount)
		{
			MovePlayer(1f);
		}
		else if(endDifference.x <= -maxControllerMoveAmount)
		{
			MovePlayer(-1f);
		}

		if (endDifference.y >= maxControllerMoveAmount)
		{
			Jump();
		}
		else if(endDifference.y <= maxControllerMoveAmount && (endDifference.x < maxControllerMoveAmount && endDifference.x > - maxControllerMoveAmount))
		{
			MovePlayer(0f);
			rb.velocity = Vector2.zero;
		}
	}
}
