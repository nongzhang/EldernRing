using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ
{
    public class CharacterCombatManager : MonoBehaviour
    {
        [Header("Attack Target")]
        public CharacterManager currentTarget;

        [Header("Attack Type")]
        public AttackType currentAttackType;

        [Header("Lock On Transform")]
        public Transform LockOnTransform;

        protected virtual void Awake()
        {

        }
    }
}