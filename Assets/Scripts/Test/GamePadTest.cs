using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePadTest : MonoBehaviour
{
    [SerializeField] bool leftTrigger = false;
    [SerializeField] bool rightTrigger = false;
    [SerializeField] bool leftArrowKey = false;
    [SerializeField] bool rightArrowKey= false;
    [SerializeField] Vector2 lockOn_MouseInput;

    private Vector2 lastMousePosition = Vector2.zero;
    

    PlayerControls playerControls;


    private bool isMouseMovedLeft = false;
    private bool isMouseMovedRight = false;


    public float stillThreshold = 0.01f; // 可调阈值，避免微小抖动误判
    public bool isMouseStill;

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            //playerControls.Test.LeftTrigger.performed += i => leftTrigger = true;
            //playerControls.Test.RightTrigger.performed += i => rightTrigger = true;
            //playerControls.PlayerActions.SeekLockTargetByMouse.performed += OnMouseMove;


            //鼠标控制
            //playerControls.PlayerActions.SeekLeftLockTargetByMouse.performed += i => isMouseMovedLeft = true;
            //playerControls.PlayerActions.SeekRightLockTargetByMouse.performed += i => isMouseMovedRight = true;


            //键盘控制
            playerControls.PlayerActions.SeekLeftLockOnTarget.performed += i => leftArrowKey = true;
            playerControls.PlayerActions.SeekRightLockOnTarget.performed += i => rightArrowKey = true;
        }
        playerControls.Enable();
    }

    private void Update()
    {
        HandleLeftTrigger();
        HandleRightTrigger();
        //MouseInputTest();

        //HandleLeftArrowKey();
        //HandleRightArrowKey();


        Vector2 currentMousePosition = Mouse.current.position.ReadValue();
        float distance = Vector2.Distance(currentMousePosition, lastMousePosition);

        isMouseStill = distance < stillThreshold;

        lastMousePosition = currentMousePosition;

        HandleMouseMove();
    }

    private void HandleLeftTrigger()
    {
        if (leftTrigger)
        {
            leftTrigger = false;
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

    private void HandleLeftArrowKey()
    {
        if (leftArrowKey)
        {
            leftArrowKey = false;
            Debug.Log("触发左方向键了");
        }
    }

    private void HandleRightArrowKey()
    {
        if (rightArrowKey)
        {
            rightArrowKey = false;
            Debug.Log("触发右方向键了");
        }
    }


    private void HandleMouseMove()
    {
        if (isMouseMovedLeft && isMouseStill)
        {
            isMouseMovedLeft = false;
            Debug.Log("鼠标向左移动了");
        }

        if (isMouseMovedRight && isMouseStill)
        {
            isMouseMovedRight = false;
            Debug.Log("鼠标向右移动了");
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
        //Debug.Log("Mouse.current.position.ReadValue: " + Mouse.current.position.ReadValue().x);

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
