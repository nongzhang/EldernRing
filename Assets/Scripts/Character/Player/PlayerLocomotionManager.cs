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
        [SerializeField] float sprintingSpeed = 6.5f;
        [SerializeField] float rotationSpeed = 15;
        [SerializeField] int sprintingStaminaCost = 2;

        [Header("Jump")]
        private float jumpStaminaCost = 25f;
        [SerializeField] float jumpHeight = 4.0f;
        [SerializeField] float jumpForwardSpeed = 5;
        [SerializeField] float freeFallSpeed = 2;
        private Vector3 jumpDirection;

        [Header("Dodge")]
        private Vector3 roleDirection;
        private float dodgeStaminaCost = 25f;
        

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

                playerManager.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, playerManager.playerNetworkManager.isSprinting.Value);  //未锁定时，水平方向移动值为0
            }
        }

        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
            HandleJumpingMovement();
            HandleFreeFallMovement();
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

            if (playerManager.playerNetworkManager.isSprinting.Value)
            {
                playerManager.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
            }
            else
            {
                if (PlayerInputManager.instance.moveAmount > 0.5)
                {
                    playerManager.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
                }
                else if (PlayerInputManager.instance.moveAmount <= 0.5)
                {
                    playerManager.characterController.Move(moveDirection * walkSpeed * Time.deltaTime);
                }
            }     
        }

        private void HandleJumpingMovement()
        {
            if (playerManager.isJumping)
            {
                playerManager.characterController.Move(jumpDirection * jumpForwardSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        /// 玩家控制的角色可以在下落期间仍旧可以在水平方向运动，只是运动的速度很小
        /// </summary>
        private void HandleFreeFallMovement()
        {
            if (!playerManager.isGrounded)
            {
                Vector3 freeFallDirection;

                freeFallDirection = PlayerCamera.instance.transform.forward * PlayerInputManager.instance.verticalInput;
                freeFallDirection += PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontalInput;
                freeFallDirection.y = 0;

                playerManager.characterController.Move(freeFallDirection * freeFallSpeed * Time.deltaTime);
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

        public void HandleSprinting()
        {
            if (playerManager.isPerformingAction)
            {
                playerManager.playerNetworkManager.isSprinting.Value = false;
            }

            //当耐力条用完时，无法使用冲刺。当处于移动状态时，可以使用冲刺。当处于静止状态时，不能使用冲刺
            if (playerManager.playerNetworkManager.currentStamina.Value<=0)
            {
                playerManager.playerNetworkManager.isSprinting.Value = false;
                return;
            }

            if (moveAmount >= 0.5f)
            {
                playerManager.playerNetworkManager.isSprinting.Value = true;
            }
            else
            {
                playerManager.playerNetworkManager.isSprinting.Value = false;
            }

            if (playerManager.playerNetworkManager.isSprinting.Value)
            { 
                playerManager.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
            }
        }

        public void AttemptToPerformDodge()
        {
            if (playerManager.isPerformingAction)
            {
                return;
            }
            if (playerManager.playerNetworkManager.currentStamina.Value <= 0)
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
            playerManager.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;
        }

        public void AttemptToPerformJump()
        {
            //正在执行某种通用动作，比如交互、捡物、翻滚等,此时不允许跳跃(未来当加入战斗系统后，逻辑可能会发生变化,比如跳劈)
            if (playerManager.isPerformingAction)
            {
                return;
            }

            //耐力值耗尽时不允许跳跃
            if (playerManager.playerNetworkManager.currentStamina.Value <= 0)
            {
                return;
            }

            //正在跳跃状态时，不允许跳跃
            if (playerManager.isJumping)
            {
                return;
            }

            //如果不在地面状态时，比如在空中，不允许跳跃
            if (!playerManager.isGrounded)
            {
                return;
            }

            //

            playerManager.playerAnimatorManager.PlayTargetActionAnimation("Main_Jump_01", false);  //isPerformingAction设为false, 原因是可能有跳劈
            playerManager.isJumping = true;
            playerManager.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;
            jumpDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
            jumpDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
            jumpDirection.y = 0;

            if (jumpDirection != Vector3.zero)
            {
                //如果在冲刺中，跳跃方向是全距离
                if (playerManager.playerNetworkManager.isSprinting.Value)
                {
                    jumpDirection *= 1;
                }
                //如果在奔跑中，跳跃方向是半距离
                else if (PlayerInputManager.instance.moveAmount > 0.5)
                {
                    jumpDirection *= 0.5f;
                }
                //如果在行走中，跳跃方向是四分之一距离
                else if (PlayerInputManager.instance.moveAmount <= 0.5f)
                {
                    jumpDirection *= 0.25f;
                }
            }
            
        }

        public void ApplyJumpingVelocity()
        {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);     //根据期望的跳跃高度和重力大小，计算角色在起跳时所需的垂直初速度。
        }
    }

}

