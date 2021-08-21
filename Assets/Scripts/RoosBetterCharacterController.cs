using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoosBetterCharacterController : MonoBehaviour
{
    CharacterController controller;
    float xRotation = 0f;

    public float jumpHeight = 2f;
    public float walkSpeed = 4f;
    public float runSpeed = 8f;
    public float swimSpeed = 3f;
    public float acceleration = 3f;
    public float mouseSensitivity = 100f;
    public Transform playerCamera;
    public Transform floatPoint;

    Vector3 velocity;
    public bool isGrounded = true;
    float gravity = -9.81f;
    public float gravityMultiplyer = 1.5f;
    public Transform groundCheck;
    public float groundCheckTolerance = 0.2f;
    public LayerMask groundMask;
    public Transform waterLevel = null;

    float currentSpeed = 0f;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent <CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float activeGravity = gravity; // set gravity to normal for this frame


        // mouse movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY; // needs to be minus so look is not inverted;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // player movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float playerSpeed = (Input.GetKey(KeyCode.LeftShift)) ? runSpeed : walkSpeed; // check for run walk

        if(floatPoint.position.y < waterLevel.position.y) // check if player is swimming
		{
            activeGravity = 0f;
            velocity.y = 0f;
            playerSpeed = swimSpeed;
		}

        currentSpeed = Mathf.Lerp(currentSpeed, playerSpeed, Time.deltaTime * acceleration); // smooth transitions
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // gravity
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckTolerance, groundMask);
        if(isGrounded && velocity.y < 0f) velocity.y = -1f;

        // jump
        if (Input.GetButtonDown("Jump") && isGrounded) velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
      

        velocity.y += activeGravity * gravityMultiplyer * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
