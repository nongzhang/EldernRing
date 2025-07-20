using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePadTest : MonoBehaviour
{
    [SerializeField] bool leftRight = false;
    [SerializeField] bool rightTrigger = false;
    [SerializeField] Vector2 lockOn_MouseInput;

    private Vector2 lastMousePosition = Vector2.zero;
    private bool isMouseMovedLeft = false;

    PlayerControls playerControls;



    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.Test.LeftTrigger.performed += i => leftRight = true;
            playerControls.Test.RightTrigger.performed += i => rightTrigger = true;
            playerControls.PlayerActions.SeekLockTargetByMouse.performed += OnMouseMove;
        }
        playerControls.Enable();
    }

    private void Update()
    {
        HandleLeftTrigger();
        HandleRightTrigger();
        //MouseInputTest();
    }

    private void HandleLeftTrigger()
    {
        if (leftRight)
        {
            leftRight = false;
            Debug.Log("触发左Trigger键了");
        }
    }

    private void HandleRightTrigger()
    {
        if (rightTrigger)
        {
            rightTrigger = false;
            Debug.Log("触发右Trigger键了");
        }
    }

    //private void MouseInputTest()
    //{
    //    if (lockOn_MouseInput.x != 0)
    //    {
    //        Debug.Log("lockOn_MouseInput.x: " + lockOn_MouseInput.x);
    //    }
        
    //    if (lockOn_MouseInput.y != 0)
    //    {
    //        Debug.Log("lockOn_MouseInput.x: " + lockOn_MouseInput.y);
    //    }
        
    //}

    void OnMouseMove(InputAction.CallbackContext context)
    {
        Vector2 currentMousePosition = context.ReadValue<Vector2>();

        // 判断鼠标是否向左移动
        if (currentMousePosition.x < lastMousePosition.x)
        {
            isMouseMovedLeft = true;
        }
        else
        {
            isMouseMovedLeft = false;
        }

        // 更新最后的鼠标位置
        lastMousePosition = currentMousePosition;

        // 输出布尔值
        Debug.Log($"Mouse moved left: {isMouseMovedLeft}");
    }
}
