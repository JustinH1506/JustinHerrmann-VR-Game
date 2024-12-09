using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerActions : MonoBehaviour
{
	#region Variables

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

	[SerializeField] private Animator anim = null;

	private Vector3 controllerPos = new Vector3();

	private Vector3 endpos = new Vector3();

	private float controllerAngle = 0f;

	private float moveInput;

	private bool startCalculating = false;

	private Rigidbody rb = null;

	private PlayerActionMap _playerActions;
	
	#endregion

	#region Methods
	
	private void OnEnable()
	{
		_playerActions.Enable();

		_playerActions.Player.Move.performed += CalculateEndpointPosition;
		_playerActions.Player.Move.canceled += StartVRMovement;

		_playerActions.Laptop.Move.performed += CalculateEndpointPosition;
		_playerActions.Laptop.Move.canceled += StartVRMovement;
	}

	private void OnDisable()
	{
		_playerActions.Disable();
		
		_playerActions.Player.Move.performed -= CalculateEndpointPosition;
		_playerActions.Player.Move.canceled -= StartVRMovement;

		_playerActions.Laptop.Move.performed -= CalculateEndpointPosition;
		_playerActions.Laptop.Move.canceled -= StartVRMovement;
	}

	private void Awake()
	{
		_playerActions = new PlayerActionMap();
		
		rb = GetComponent<Rigidbody>();

		anim = GetComponent<Animator>();
		
		float currentPosition = transform.position.x / totalLevelLength;
			
		angle = currentPosition * 360 * Mathf.Deg2Rad;
	}
	

	private void FixedUpdate()
	{
		MovePlayer();
	}

	private void Update()
	{
		if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
		{
			Jump();
		}
	}

	private void MovePlayer()
	{
		//moveInput = -moveInput;

		if (!IsGrounded())
		{
			anim.SetBool("IsGrounded", true);
		}

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

	void CalculateEndpointPosition(InputAction.CallbackContext context)
	{
		Debug.Log("Starts Calculating");
		
		controllerPos = controllerLeft.transform.position;
			
		controllerAngle = controllerLeft.transform.eulerAngles.y;
			
		endpos = endpoint.position - controllerPos;
	}

	private void StartVRMovement(InputAction.CallbackContext context)
	{
		Debug.Log("Should Start moving");
			
		Vector3 newEndPos = endpoint.position - controllerPos;

		Vector3 endDifference = newEndPos - endpos;

		endDifference = Quaternion.AngleAxis(-controllerAngle, Vector3.up) * endDifference; 
			
		if (endDifference.x >= maxControllerMoveAmount)
		{
			moveInput = -1f;
			anim.SetFloat("MoveInput", moveInput);
		}
		else if(endDifference.x <= -maxControllerMoveAmount)
		{
			moveInput = 1f;
			anim.SetFloat("MoveInput", moveInput);
		}

		if (endDifference.y >= maxControllerMoveAmount && IsGrounded())
		{
			Jump();
			anim.SetBool("IsGrounded", false);
		}
		else if(endDifference.y <= maxControllerMoveAmount && (endDifference.x < maxControllerMoveAmount && endDifference.x > - maxControllerMoveAmount))
		{
			moveInput = 0f;
			rb.velocity = Vector2.zero;
		}
	}

	#endregion
}
