using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : BaseMovement
{
    [SerializeField]
    private AnimatorController myAnim;
    private Vector3 tempMovement;

    [SerializeField]
    private float sprintSpeedMultiplier = 2f; // Adjust as needed
    private bool isSprinting;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        // Detect sprint input
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
        }

        // Calculate movement direction
        tempMovement = Input.GetAxis("Horizontal") * Camera.main.transform.right + Input.GetAxis("Vertical") * Camera.main.transform.forward;
        tempMovement.y = 0f; //Ensure no vertical movement
    }

    void FixedUpdate()
    {
        PlayerMove();
        ChangeAnimation();
    }

    void PlayerMove()
    {
        Move(tempMovement);

        // Adjust movement speed based on sprinting state
        float speedMultiplier = isSprinting ? sprintSpeedMultiplier : 1f;
        Move(tempMovement * speedMultiplier);

    }

    void ChangeAnimation()
    {
        if (myAnim)
        {
            // Set "Running" animation based on movement magnitude and sprinting state
            bool isRunning = tempMovement.magnitude > 0f && !isSprinting;
            myAnim.ChangeAnimBoolValue("Running", isRunning);
            myAnim.ChangeAnimBoolValue("Sprinting", isSprinting);

            // Set rotation based on movement direction
            if (tempMovement.magnitude > 0f)
            {
                float rot = Mathf.Atan2(-tempMovement.z, tempMovement.x) * Mathf.Rad2Deg + 90f;
                transform.rotation = Quaternion.Euler(0f, rot, 0f);
            }
        }
    }
}