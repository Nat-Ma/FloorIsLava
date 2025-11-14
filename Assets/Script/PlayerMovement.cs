using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
	[Header("Movement Settings")]
	public float moveSpeed = 6f;	// horizontal movement speed
    public float jumpForce = 5f;	// small jump
    public float drag = 2f;			// slows horizontal sliding
    public float angularDrag = 3f;	// prevents spinning/rolling
	private bool isGameOver = false;
	private bool wonGame = false;
	public TMP_Text gameOverText;
	public TMP_Text gameWonText;

    private Rigidbody rb;
	private bool jumpRequested = false;
	private bool isGrounded = false;
	private Vector2 input = Vector2.zero;
	public Transform cameraPivot;

	// Reference to the switcher
	public CharacterSwitcherNewInput switcher;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = drag;
        rb.angularDamping = angularDrag;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ; // only Z
    }

	// Update is called once per frame
	void Update()
	{
		if (switcher == null || switcher.GetActiveCharacter() != gameObject) return;

		float xInput = 0f;
		float yInput = 0f;

		if (isGameOver || wonGame)
			Time.timeScale = 0f; // freezes everything

		if (Keyboard.current != null)
		{
			// WASD
			if (Keyboard.current.aKey.isPressed) xInput -= 1;
			if (Keyboard.current.dKey.isPressed) xInput += 1;
			if (Keyboard.current.wKey.isPressed) yInput += 1;
			if (Keyboard.current.sKey.isPressed) yInput -= 1;

			// Arrow keys
			if (Keyboard.current.leftArrowKey.isPressed) xInput -= 1;
			if (Keyboard.current.rightArrowKey.isPressed) xInput += 1;
			if (Keyboard.current.upArrowKey.isPressed) yInput += 1;
			if (Keyboard.current.downArrowKey.isPressed) yInput -= 1;

			// Restart the game
			// When game is over, wait for Enter key to restart
			if (isGameOver)
			{
				if (Keyboard.current.enterKey.wasPressedThisFrame || 
					Keyboard.current.numpadEnterKey.wasPressedThisFrame)
				{
					Time.timeScale = 1f; // Unpause
					SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current scene
				}
				return; // Skip the rest of Update while game is over
			}

			// Jump
			if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
				jumpRequested = true;
		}

		input.x = xInput;
    	input.y = yInput;

		// Normalize for diagonal movement (this part is correct)
		if (input.magnitude > 1f) input.Normalize();
	}

	void FixedUpdate()
    {
        // --- Move relative to camera ---
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
		right.y = 0;

        Vector3 moveDir = (right * input.x + forward * input.y).normalized;
        rb.AddForce(moveDir * moveSpeed, ForceMode.Force);

		if (jumpRequested && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpRequested = false;
			isGrounded = false;
        }
    }

	void OnCollisionEnter(Collision collision)
	{
		// Any collision with something solid = grounded
        if (!collision.gameObject.CompareTag("Lava"))   
            isGrounded = true;

		if (collision.gameObject.CompareTag("Lava"))
		{
			isGameOver = true;
			Time.timeScale = 0f;
			gameOverText.gameObject.SetActive(true);
		}
		if (collision.gameObject.CompareTag("Level2"))
		{
			// gameWonText.gameObject.SetActive(true);
			switcher.SwitchToNextCharacter();
		}
		if (collision.gameObject.CompareTag("Target"))
		{
			wonGame = true;
			gameWonText.gameObject.SetActive(true);
			Time.timeScale = 0f;
		}
	}

	void OnCollisionExit(Collision collision)
    {
        // Leaves ground
        if (!collision.gameObject.CompareTag("Lava"))
            isGrounded = false;
    }
}
