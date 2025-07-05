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

                playerManager.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, playerManager.playerNetworkManager.isSprinting.Value);  //δ����ʱ��ˮƽ�����ƶ�ֵΪ0
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
            //���ǵ��ƶ������ǻ�������泯���ӽǺ����ǵ��ƶ�����
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
        /// ��ҿ��ƵĽ�ɫ�����������ڼ��Ծɿ�����ˮƽ�����˶���ֻ���˶����ٶȺ�С
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

            //������������ʱ���޷�ʹ�ó�̡��������ƶ�״̬ʱ������ʹ�ó�̡������ھ�ֹ״̬ʱ������ʹ�ó��
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
            //���ܶ�������������������ɫ���˶��У���ô���Ƿ����������ɫ��ֹ����ô���Ǻ󳷲�
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
            //����ִ��ĳ��ͨ�ö��������罻�������������,��ʱ��������Ծ(δ��������ս��ϵͳ���߼����ܻᷢ���仯,��������)
            if (playerManager.isPerformingAction)
            {
                return;
            }

            //����ֵ�ľ�ʱ��������Ծ
            if (playerManager.playerNetworkManager.currentStamina.Value <= 0)
            {
                return;
            }

            //������Ծ״̬ʱ����������Ծ
            if (playerManager.isJumping)
            {
                return;
            }

            //������ڵ���״̬ʱ�������ڿ��У���������Ծ
            if (!playerManager.isGrounded)
            {
                return;
            }

            //

            playerManager.playerAnimatorManager.PlayTargetActionAnimation("Main_Jump_01", false);  //isPerformingAction��Ϊfalse, ԭ���ǿ���������
            playerManager.isJumping = true;
            playerManager.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;
            jumpDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
            jumpDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
            jumpDirection.y = 0;

            if (jumpDirection != Vector3.zero)
            {
                //����ڳ���У���Ծ������ȫ����
                if (playerManager.playerNetworkManager.isSprinting.Value)
                {
                    jumpDirection *= 1;
                }
                //����ڱ����У���Ծ�����ǰ����
                else if (PlayerInputManager.instance.moveAmount > 0.5)
                {
                    jumpDirection *= 0.5f;
                }
                //����������У���Ծ�������ķ�֮һ����
                else if (PlayerInputManager.instance.moveAmount <= 0.5f)
                {
                    jumpDirection *= 0.25f;
                }
            }
            
        }

        public void ApplyJumpingVelocity()
        {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);     //������������Ծ�߶Ⱥ�������С�������ɫ������ʱ����Ĵ�ֱ���ٶȡ�
        }
    }

}

