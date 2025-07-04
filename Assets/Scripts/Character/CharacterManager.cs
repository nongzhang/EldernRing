using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace SG
{
    public class CharacterManager : NetworkBehaviour
    {
        [HideInInspector]public CharacterController characterController;
        [HideInInspector]public Animator animator;
        [HideInInspector]public CharacterNetworkManager characterNetworkManager;

        [Header("Flag")]
        public bool isPerformingAction = false;
        public bool applyRootMotion = false;
        public bool isJumping = false;
        public bool isGrounded = true;
        public bool canRotate = true;
        public bool canMove = true;

        

        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
        }

        protected virtual void Update()
        {
            animator.SetBool("isGrounded", isGrounded);
            if (IsOwner)
            {
                characterNetworkManager.networkPosition.Value = transform.position; //�����ɫ������������Ƶģ���ô�Ͱ�����λ�ø�ֵ������λ��
                characterNetworkManager.networkRotation.Value = transform.rotation;
            }
            else
            {
                transform.position = Vector3.SmoothDamp
                    (transform.position,
                    characterNetworkManager.networkPosition.Value,
                    ref characterNetworkManager.networkPositionVelocity,
                    characterNetworkManager.networkPositionSmoothTime);   //��������ɫ�Ǳ����˿��ƣ���ô������λ�ø�ֵ����������ռ��λ��
                transform.rotation = Quaternion.Slerp(transform.rotation, characterNetworkManager.networkRotation.Value, characterNetworkManager.networkRotationSmoothTime);
            }
        }

        protected virtual void LateUpdate()
        {

        }

        
    }
}

