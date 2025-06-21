using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SG
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera instance;
        public PlayerManager playerManager;
        public Camera cameraObject;

        //改变这些来调整相机的表现
        [Header("Camera Settings")]
        private float cameraSmoothSpeed = 1.0f;                      //这个值越大，相机到达它的位置的时间越长
        [SerializeField] float leftAndRightRotationSpeed = 220;      //
        [SerializeField] float upAndDownRotationSpeed = 220;
        [SerializeField] float minimumPivot = -30;                   //相机向下看能看到的最低点
        [SerializeField] float maximumPivot = 60;                    //相机向上看能看到的最高点

        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        [SerializeField] float leftAndRightLookAnglr;
        [SerializeField] float upAndDownLookAngle;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void HandleAllCameraAction()
        {
            HandleFollowTarget();
            //围绕玩家旋转
            HandleRotation();
            //处理和场景中其他对象碰撞的问题
        }

        private void HandleFollowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, playerManager.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }

        private void HandleRotation()
        {
            //当我们锁定时，强制相机围绕着目标旋转
            //否则围绕玩家旋转
        }
    }
}

