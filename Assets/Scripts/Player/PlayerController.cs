using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;
    public Transform cam;

    [Space(10)]
    public CharacterController controller;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private InputManager inputManager;
    private TimeManager timeManager;

    private void Start()
    {
        inputManager = InputManager.Instance;
        timeManager = TimeManager.Instance;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movement = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move = cam.forward * move.z + cam.right * move.x;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (inputManager.IsPlayerJumped() && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (inputManager.IsPlayerTimeSlow())
        {
            timeManager.SlowDown(0.25f, 2f);
        }
    }
}