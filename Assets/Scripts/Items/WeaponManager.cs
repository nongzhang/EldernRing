using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ
{
    //��Ȼ��ɫ�����Է�����ʵ������������˺�������������Ҫ��һ������������
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
            meleeWeaponDamageCollider.light_Attack_02_Modifier = weapon.light_Attack_02_Modifier;

            meleeWeaponDamageCollider.heavy_Attack_01_Modifier = weapon.heavy_Attack_01_Modifier;
            meleeWeaponDamageCollider.heavy_Attack_02_Modifier = weapon.heavy_Attack_02_Modifier;

            meleeWeaponDamageCollider.charge_Attack_01_Modifier = weapon.charge_Attack_01_Modifier;
            meleeWeaponDamageCollider.charge_Attack_02_Modifier = weapon.charge_Attack_02_Modifier;
        }
    }
}
