using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace SG
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
        CharacterManager characterManager;
        [Header("Position")]
        public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public Vector3 networkPositionVelocity;
        public float networkPositionSmoothTime = 0.1f;
        public float networkRotationSmoothTime = 0.1f;

        [Header("Animator")]
        public NetworkVariable<float> horizontalMovement = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> veryicalMovement = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> moveAmount = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Flags")]
        public NetworkVariable<bool> isSprinting = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);

        [Header("Stats")]
        public NetworkVariable<int> endurance = new NetworkVariable<int>(10, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> currentStamina = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> maxStamina = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }

        //Server RPC��һ���ͻ��˵��÷��͸�����˵ķ���,����ķ��������������host��,һ̨�ͻ���
        [ServerRpc]
        public void NotifyTheServerOfActionAnimationServerRpc(ulong clientID, string animationId, bool applyRootMotion)
        {
            if (IsServer)
            {
                PlayActionAnimationForAllClientClientRpc(clientID, animationId, applyRootMotion);
            }
        }

        [ClientRpc]
        public void PlayActionAnimationForAllClientClientRpc(ulong clientID, string animationId, bool applyRootMotion)
        {
            //ȷ��������Ϣ���Ǹ���ɫ����ִ���������(��˱��ⲥ�Ŷ�������)
            if (clientID != NetworkManager.Singleton.LocalClientId)
            {
                PerformActionAnimationFromServer(animationId, applyRootMotion);
            }
        }

        private void PerformActionAnimationFromServer(string animationID, bool applyRootMotion)
        {
            characterManager.applyRootMotion = applyRootMotion;
            characterManager.animator.CrossFade(animationID, 0.2f);
        }
    }
}


