using System;
using Audio;
using UnityEngine;
using UnityEngine.InputSystem;
using FMOD.Studio; // Assuming FMOD is used based on AudioManager
using FMODUnity; // Assuming FMOD is used based on AudioManager

// Placeholder for FMOD Event GUIDs - Replace with actual GUIDs from your FMOD project

public class Player : MonoBehaviour
{
    [Header("Rotation")]
    public float maxRotationSpeed = 360f; // Degrees per second

    [Header("References")]
    [SerializeField] private Transform bodyTransform; // Assign the 'Body' child object
    [SerializeField] private Transform beamTransform; // Assign the 'Beam' child object

    private Camera mainCamera;
    private PlayerInput playerInput; // Assumes PlayerInput component is on this GameObject
    private InputAction lookAction;
    private InputAction moveAction; // Action for A/D or joystick rotation
    private float currentRotationSpeed = 0f;
    private Rigidbody rb; // Reference to the Rigidbody component

    // FMOD Event Instance for movement sound (if needed for parameter changes)
    private EventInstance movementSoundInstance;

    void Awake()
    {
        mainCamera = Camera.main;
        playerInput = GetComponent<PlayerInput>(); // Get PlayerInput component

        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found on Player GameObject!");
            return;
        }
        // Assumes an Action Map named "Player" and an Action named "Look" exists
        lookAction = playerInput.actions["Look"];
        moveAction = playerInput.actions["Move"]; // Get the Move action

        if (moveAction == null)
        {
             Debug.LogError("Move action not found in Player Input Actions!");
        }
         if (lookAction == null)
        {
             Debug.LogError("Look action not found in Player Input Actions!");
       }

       // Get the Rigidbody component
       rb = GetComponent<Rigidbody>();
       if (rb == null)
       {
           Debug.LogError("Rigidbody component not found on Player GameObject!");
       }
   }

   void Start()
    {
        // Fire movement start sound - Assuming Play3DAudio attached to AudioManager is okay for 2D
        if (AudioManager.Instance != null)
        {
            // You might want to store this instance if you need to stop it or change parameters later
            movementSoundInstance = AudioManager.Instance.Play2DAudio(AudioEvent.PlayerMovement);
        }
        else
        {
            Debug.LogWarning("AudioManager Instance not found. Cannot play PlayerMovement sound.");
        }
    }

    // Update is called once per frame - Good for input reading, audio updates
    void Update()
    {
        // We read input here, but apply physics changes in FixedUpdate
        UpdateAudioParameters(); // Keep audio update here if needed per frame
    }

    // FixedUpdate is called at a fixed interval - Best for physics operations
    void FixedUpdate()
    {
        HandleRotation(); // Apply rotation using Rigidbody
    }
void OnEnable()
    {
        // Ensure the action is enabled when the component is active
        lookAction?.Enable();
        moveAction?.Enable();
    }

    void OnDisable()
    {
        // Ensure the action is disabled when the component is inactive
        lookAction?.Disable();
        moveAction?.Disable();
    }

    void HandleRotation()
    {
        // Apply rotation using Rigidbody in FixedUpdate
        if (moveAction == null || rb == null) return;

        float moveInputX = moveAction.ReadValue<Vector2>().x;
        // Calculate rotation amount for this fixed frame
        float rotationAmountDegrees = -moveInputX * maxRotationSpeed * Time.fixedDeltaTime; // Use fixedDeltaTime

        // Create a rotation Quaternion representing the change
        Quaternion deltaRotation = Quaternion.Euler(0f, 0f, rotationAmountDegrees);

        // Apply the rotation to the Rigidbody
        rb.MoveRotation(rb.rotation * deltaRotation);

        // Update current rotation speed for audio (absolute value in degrees/sec)
        currentRotationSpeed = Mathf.Abs(rotationAmountDegrees / Time.fixedDeltaTime);


        // --- Smooth Rotation towards Mouse (Commented Out - Needs Rigidbody implementation if re-enabled) ---
        // if (lookAction == null) return; // Only proceed with mouse aiming if action exists
        //
        // // Read mouse position from Input Action
        // Vector2 mouseScreenPosition = lookAction.ReadValue<Vector2>();
        //
        // // Convert mouse position to world position
        // // Calculate distance from camera to player's Z plane
        // float distanceToPlane = Mathf.Abs(transform.position.z - mainCamera.transform.position.z);
        // Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, distanceToPlane));
        //
        // // Calculate direction from player to mouse
        // Vector3 directionToMouse = (mouseWorldPosition - transform.position).normalized;
        //
        // // Calculate target angle (using Atan2 for full 360 degrees)
        // // Angle is 0 when pointing right (positive X), 90 when pointing up (positive Y)
        // float targetAngle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg + 90f;
        //
        // // Calculate current angle
        // // Get angle *after* applying keyboard rotation
        // float currentAngle = transform.eulerAngles.z;
        // // Debug.Log("targetAngle: " + targetAngle + ", currentAngle: " + currentAngle); // Keep commented unless debugging
        //
        // // Smoothly rotate towards the mouse target angle
        // float smoothedAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, maxRotationSpeed * Time.deltaTime);
        //
        // // Calculate the rotation applied by smoothing this frame
        // float mouseRotationAmount = Mathf.DeltaAngle(currentAngle, smoothedAngle);
        //
        // // Apply the smoothed rotation
        // transform.rotation = Quaternion.Euler(0f, 0f, smoothedAngle);
        //
        // // Calculate the *total* speed of rotation for this frame (keyboard + mouse smoothing)
        // // Note: keyboardRotationAmount is already scaled by deltaTime
        // float totalRotationThisFrame = (keyboardRotationAmount * Mathf.Rad2Deg) + mouseRotationAmount; // Convert keyboard part to degrees if needed? No, Rotate uses degrees.
        // currentRotationSpeed = Mathf.Abs(totalRotationThisFrame / Time.fixedDeltaTime); // Use fixedDeltaTime if combining
    }

    void UpdateAudioParameters()
    {
        // Send rotation speed to FMOD
        if (AudioManager.Instance != null && movementSoundInstance.isValid())
        {
            // Assuming a parameter named "RotationSpeed" exists in your FMOD event
            AudioManager.Instance.SetGlobalParameter("PlayerRotationSpeed", Mathf.Abs(currentRotationSpeed));
        }
    }
    
    // private void OnTriggerEnter(Collider other) {
    //     Debug.Log("onTriggerEnter");
    //     // Check if the collision is with the 'Body' part and an Enemy
    //     if (other.CompareTag("PlayerBody"))
    //     {
    //         // Trigger Game Over sequence (e.g., show UI, stop game)
    //         FindObjectOfType<GameManager>()?.GameOver(); // Example call
    //         Time.timeScale = 0; // Simple pause
    //     }
    //     // Check if the collision is with the 'Beam' part and an Enemy
    //     else if (other.CompareTag("PlayerBeam"))
    //     {
    //         Debug.Log("Player Beam collided with Enemy - Destroying Enemy!");
    //         // Destroy the enemy GameObject
    //         AudioManager.Instance.Play2DAudio(AudioEvent.DestroyEnemy); // Play at collision point
    //         Destroy(gameObject); // Destroy self (the enemy)
    //         // Optionally play a different sound for beam hit
    //     }
    // }

    // Make sure to stop FMOD sounds when the object is destroyed
    void OnDestroy()
    {
        if (movementSoundInstance.isValid())
        {
            AudioManager.Instance?.StopAudio(movementSoundInstance);
            movementSoundInstance.release();
        }
    }
}