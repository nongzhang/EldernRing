using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ
{
    public class PlayerInventoryManager : CharacterInventoryManager
    {
        public WeaponItem currentRightHandWeapon;
        public WeaponItem currentLeftHandWeapon;

        [Header("Quick Slot")]  //��ϵ��Ϸ�����ֶ�����װ������װ���Թ��л�
        public WeaponItem[] weaponInRightHandSlot = new WeaponItem[3];
        public int rightHandWeaponIndex = 0;
        public WeaponItem[] weaponInLeftHandSlot = new WeaponItem[3];
        public int leftHandWeaponIndex = 0;

    }
}
