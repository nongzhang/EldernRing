using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ
{
    //虽然角色攻击对方，但实质是武器造成伤害，所以我们需要有一个武器管理类
    public class WeaponManager : MonoBehaviour
    {
        public MeleeWeaponDamageCollider meleeWeaponDamageCollider;

        private void Awake()
        {
            meleeWeaponDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
        }

        public void SetWeaponDamage(CharacterManager characterWieldingWeapon, WeaponItem weapon)
        {
            meleeWeaponDamageCollider.physicalDamage = weapon.physicalDamage;
            meleeWeaponDamageCollider.magicDamage = weapon.magicDamage;
            meleeWeaponDamageCollider.fireDamage = weapon.fireDamage;
            meleeWeaponDamageCollider.lightDamage = weapon.lightningDamage;
            meleeWeaponDamageCollider.holyDamage = weapon.holyDamage;

            meleeWeaponDamageCollider.light_Attack_01_Modifier = weapon.light_Attack_01_Modifier;
        }
    }
}
