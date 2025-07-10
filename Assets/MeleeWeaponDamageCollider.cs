using Sg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Attacking Character")]
        public CharacterManager characterCausingDamage;   //在计算伤害时，这个字段用于查询攻击者的伤害加成、特效等信息
    }
}
