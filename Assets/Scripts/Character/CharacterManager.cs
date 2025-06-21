using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace SG
{
    public class CharacterManager : NetworkBehaviour
    {
        public CharacterController characterController;

        CharacterNetworkManager characterNetworkManager;
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);
            characterController = GetComponent<CharacterController>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
        }

        protected virtual void Update()
        {
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

