using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePadTest : MonoBehaviour
{
    [SerializeField] bool leftRight = false;
    [SerializeField] bool rightTrigger = false;

    PlayerControls playerControls;



    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.Test.LeftTrigger.performed += i => leftRight = true;
            playerControls.Test.RightTrigger.performed += i => rightTrigger = true;
        }
        playerControls.Enable();
    }

    private void Update()
    {
        HandleLeftTrigger();
        HandleRightTrigger();
    }

    private void HandleLeftTrigger()
    {
        if (leftRight)
        {
            leftRight = false;
            Debug.Log("������Trigger����");
        }
    }

    private void HandleRightTrigger()
    {
        if (rightTrigger)
        {
            rightTrigger = false;
            Debug.Log("������Trigger����");
        }
    }
}
