using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    //��Ȼ��ɫ�����Է�����ʵ������������˺�������������Ҫ��һ������������
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField] MeleeWeaponDamageCollider meleeWeaponDamageCollider;

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
        }
    }
}
