using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ
{
    public class PlayerInventoryManager : CharacterInventoryManager
    {
        public WeaponItem currentRightHandWeapon;
        public WeaponItem currentLeftHandWeapon;

        [Header("Quick Slot")]  //魂系游戏左右手都可以装备三件装备以供切换
        public WeaponItem[] weaponInRightHandSlot = new WeaponItem[3];
        public int rightHandWeaponIndex = 0;
        public WeaponItem[] weaponInLeftHandSlot = new WeaponItem[3];
        public int leftHandWeaponIndex = 0;

    }
}
