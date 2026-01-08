using UnityEngine;
using TMPro; // Added for the interaction text
using UnityEngine.UI; // Added for crosshair

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 9f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundLayerMask = -1;

    [Header("Camera")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float mouseSensitivity = 200f;

    [Header("Interaction System")]
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask interactableLayer; // Set this to 'Interactable' in Inspector
    [SerializeField] private TextMeshProUGUI interactText; // Your "Bấm E để tương tác" text

    private Rigidbody rb;
    private bool isGrounded;
    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();

        if (interactText != null) interactText.gameObject.SetActive(false);
    }

    void Update()
    {
        CheckGrounded();
        HandleJump();
        HandleLook();
        HandleInteraction(); // New function added here
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleInteraction()
    {
        RaycastHit hit;
        // Shoot a beam from the camera forward
        bool hitSomething = Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, interactDistance, interactableLayer);

        if (hitSomething)
        {
            // Try to find NPC data or a Door script on the object we hit
            NPC npc = hit.collider.GetComponent<NPC>();
            TransitionScene door = hit.collider.GetComponent<TransitionScene>();

            if (npc != null || door != null)
            {
                if (interactText != null) interactText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (npc != null) npc.OnInteract();
                    if (door != null) door.EnterDoor();
                }
                return; // Exit so we don't hide the text
            }
        }

        // Hide text if we aren't looking at anything interactable
        if (interactText != null) interactText.gameObject.SetActive(false);
    }

    // --- YOUR ORIGINAL FUNCTIONS REMAIN UNCHANGED BELOW ---
    private void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayerMask);
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float currentSpeed = Input.GetKey(KeyCode.LeftControl) ? sprintSpeed : walkSpeed;
        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
        Vector3 movement = moveDirection.normalized * currentSpeed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void HandleLook()
    {
        if (playerCamera == null) return;
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}