using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
namespace NZ
{
    public class CharacterCombatManager : NetworkBehaviour
    {
        CharacterManager characterManager;

        [Header("Attack Target")]
        public CharacterManager currentTarget;

        [Header("Attack Type")]
        public AttackType currentAttackType;

        [Header("Lock On Transform")]
        public Transform LockOnTransform;

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }

        public virtual void SetTarget(CharacterManager newTarget)
        {
            if (characterManager.IsOwner)
            {
                if (newTarget != null)
                {
                    currentTarget = newTarget;
                    characterManager.characterNetworkManager.currentTargetNetworkObjectID.Value = newTarget.GetComponent<NetworkObject>().NetworkObjectId;
                }
                else
                {
                    currentTarget = null;
                }
            }
        }
    }
}