using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace SG
{
    public class CharacterManager : NetworkBehaviour
    {
        [Header("Status")]
        public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [HideInInspector]public CharacterController characterController;
        [HideInInspector]public Animator animator;
        [HideInInspector]public CharacterNetworkManager characterNetworkManager;
        [HideInInspector]public CharacterEffectManager characterEffectManager;
        [HideInInspector]public CharacterAnimatorManager characterAnimatorManager;

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
            characterEffectManager = GetComponent<CharacterEffectManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        }

        protected virtual void Start()
        {
            IgnoreMyOwnColliders();
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

        public virtual IEnumerator ProcessdeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;   
                isDead.Value = true;

                if (!manuallySelectDeathAnimation)   //���û���Զ������������Ͳ���Ĭ�ϵ�
                {
                    characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
                }
            }

            yield return new WaitForSeconds(5);
        }

        //�����ɫ
        public virtual void ReviveCharacter()
        {

        }

        //������������ؽڵ���ײ��֮�����ײ
        protected virtual void IgnoreMyOwnColliders()
        {
            Collider characterControllerCollider = GetComponent<Collider>();
            Collider[] damageableCharacterColliders = GetComponentsInChildren<Collider>();
            List<Collider> ignoreColliders = new List<Collider>();

            foreach (var collider in damageableCharacterColliders)
            {
                ignoreColliders.Add(collider);
            }

            ignoreColliders.Add(characterControllerCollider);

            //foreach (var collider in ignoreColliders)
            //{
            //    foreach (var otherCollider in ignoreColliders)
            //    {
            //        Physics.IgnoreCollision(collider, otherCollider, true);
            //    }
            //}

            for (int i = 0; i < ignoreColliders.Count; i++)
            {
                for (int j = i + 1; j < ignoreColliders.Count; j++)
                {
                    Physics.IgnoreCollision(ignoreColliders[i], ignoreColliders[j], true);
                }
            }
        }
    }
}

