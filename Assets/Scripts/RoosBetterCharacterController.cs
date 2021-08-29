using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RoosBetterCharacterController : MonoBehaviour
{
    CharacterController controller;
    [Header("Choose your type of controller")]
    [Tooltip("First person by default")]
    public bool isThirdPerson = false;

    [Space(10)]
    [Header("Global Variables")]
    [Tooltip("Main camera transform")]
    public Transform playerCamera = null;
    public float acceleration = 3f;
    public float gravityMultiplyer = 1.5f;

    [Space(10)]
    [Header("Third person Variables")]
    public float speed = 5f;
    public float sneakSpeed = 3f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    [Space(10)]
    [Header("First person Variables")]
    public float jumpHeight = 2f;
    public float walkSpeed = 4f;
    public float runSpeed = 8f; 
    public float mouseSensitivity = 100f;
    

    [Space(10)]
    [Tooltip("The transform that will check to see if it is touching the ground")]
    public Transform groundCheck;
    public float groundCheckTolerance = 0.2f;
    [Tooltip("Mask layer(s) that character can travel on")]
    public LayerMask groundMask;
    [Space(10)]
    [Tooltip("Check this to make character swim when they are in the water. Uncheck to sink")]
    public bool useSwimming = false;
    [Tooltip("Drag water gameObject here, player will then swim")]
    public Transform waterLevel = null;
    [Tooltip("Point of bouyancy of the body")]
    public Transform floatPoint = null;
    public float swimSpeed = 3f;

    // private variable group
    bool isMakingSound = false;
    float xRotation = 0f;
    Vector3 velocity; // used for gravity calcs
    // bool isGrounded = true; moved inside of first person region - left here untill testing successfull;
    float gravity = -9.81f;
    float currentSpeed = 0f; //  for lerping

    public static event Action<Vector3> PlayerMovingSound;

    // Start is called before the first frame update
    void Start()
    {
        if (floatPoint == null) floatPoint = transform;
        if (playerCamera == null) playerCamera = Camera.main.transform;
        if (groundCheck == null) groundCheck = transform;
        controller = GetComponent <CharacterController>();

        StartCoroutine(MakeSound());
    }

    IEnumerator MakeSound()
	{
		while (true)
		{
            yield return new WaitForSeconds(0.3f);
            if (isMakingSound) PlayerMovingSound?.Invoke(transform.position);
        }  
	}

    // Update is called once per frame
    void Update()
    {
        #region Third person logic
        if (isThirdPerson)
		{
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            float TPspeedToUse = (Input.GetKey(KeyCode.LeftControl)) ? sneakSpeed : speed;

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y; // get rotation required plus camera angle
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); // smooth rotation
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                currentSpeed = Mathf.Lerp(currentSpeed, TPspeedToUse, Time.deltaTime * acceleration); // smooth speed transitions
                Vector3 moveDirection = transform.rotation * Vector3.forward;
                controller.Move(moveDirection * currentSpeed * Time.deltaTime);

                isMakingSound = (currentSpeed > sneakSpeed * 1.1f) ? true : false; // added 10% for a lerp buffer
            }
            return;
        }
		#endregion

		#region First person logic

		Cursor.lockState = CursorLockMode.Locked;

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
        float speedToUse = (Input.GetKey(KeyCode.LeftShift)) ? runSpeed : walkSpeed; // check for run walk

        // if using player swimming, make sure you choose a float point and water object
		if (useSwimming)
		{
            if (floatPoint.position.y < waterLevel.position.y)
            {
                activeGravity = 0f;
                velocity.y = 0f;
                speedToUse = swimSpeed;
            }
        }

        currentSpeed = Mathf.Lerp(currentSpeed, speedToUse, Time.deltaTime * acceleration); // smooth speed transitions
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // gravity
        bool isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckTolerance, groundMask);
        if (isGrounded && velocity.y < 0f) velocity.y = 0;

        // jump
        if (Input.GetButtonDown("Jump") && isGrounded) velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
      

        velocity.y += activeGravity * gravityMultiplyer * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        isMakingSound = (currentSpeed > walkSpeed * 1.1f) ? true : false;
        #endregion
    }
}
