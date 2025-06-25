using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager playerManager;

        [HideInInspector]public float verticalMovement;
        [HideInInspector]public float horizontalMovement;
        [HideInInspector]public float moveAmount;

        [Header("Movement Settings")]
        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;
        [SerializeField] float walkSpeed = 2;
        [SerializeField] float runningSpeed = 5;
        [SerializeField] float rotationSpeed = 15;

        [Header("Dodge")]
        private Vector3 roleDirection;

        protected override void Awake()
        {
            base.Awake();
            playerManager = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            base.Update();
            if (playerManager.IsOwner)
            {
                playerManager.characterNetworkManager.veryicalMovement.Value = verticalMovement;
                playerManager.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
                playerManager.characterNetworkManager.moveAmount.Value = moveAmount;
            }
            else
            {
                verticalMovement = playerManager.characterNetworkManager.veryicalMovement.Value;
                horizontalMovement = playerManager.characterNetworkManager.horizontalMovement.Value;
                moveAmount = playerManager.characterNetworkManager.moveAmount.Value;

                playerManager.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);  //未锁定时，水平方向移动值为0
            }
        }

        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
        }

        private void GetMovementValue()
        {
            verticalMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;
            moveAmount = PlayerInputManager.instance.moveAmount;
        }

        private void HandleGroundedMovement()
        {
            if (!playerManager.canMove)
            {
                return;
            }
            GetMovementValue();  
            //我们的移动方向是基于相机面朝的视角和我们的移动输入
            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0.00f;

            if (PlayerInputManager.instance.moveAmount > 0.5)
            {
                playerManager.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
            }
            else if (PlayerInputManager.instance.moveAmount <= 0.5)
            {
                playerManager.characterController.Move(moveDirection * walkSpeed * Time.deltaTime);
            }
        }

        private void HandleRotation()
        {
            if (!playerManager.canRotate)
            {  return; }
            targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection = targetRotationDirection + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;

            if(targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
            }

            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }

        public void AttemptToPerformDodge()
        {
            if (playerManager.isPerformingAction)
            {
                return;
            }
            //闪避动作分两种情况，如果角色在运动中，那么就是翻滚。如果角色静止，那么就是后撤步
            if (PlayerInputManager.instance.moveAmount > 0)
            {
                roleDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
                roleDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
                roleDirection.y = 0;
                roleDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(roleDirection);
                playerManager.transform.rotation = playerRotation;

                playerManager.playerAnimatorManager.PlayTargetActionAnimation("Roll_Forward_01", true, true);
            }
            else
            {
                playerManager.playerAnimatorManager.PlayTargetActionAnimation("Back_Step_01", true, true);
            }
            
        }
    }

}

