using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ
{
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        PlayerManager playerManager;
        public WeaponModelInstantiationSlot rightHandSlot;
        public WeaponModelInstantiationSlot leftHandSlot;

        [SerializeField] WeaponManager rightWeaponManager;
        [SerializeField] WeaponManager leftWeaponManager;

        private GameObject rightHandWeaponModel;
        private GameObject leftHandWeaponModel;
        protected override void Awake()
        {
            base.Awake();
            playerManager = GetComponent<PlayerManager>();
        }

        protected override void Start()
        {
            base.Start();
            LoadWeaponsOnBothHands();
        }

        private void InitializeWeaponSlot()
        {
            WeaponModelInstantiationSlot[] weaponModelInstantiationSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

            foreach (var item in weaponModelInstantiationSlots)
            {
                if (item.weaponModelSlot == WeaponModelSlot.RightHand)
                {
                    rightHandSlot = item;
                }
                else if (item.weaponModelSlot == WeaponModelSlot.LeftHand)
                {
                    leftHandSlot = item;
                }
            }
        }

        public void LoadWeaponsOnBothHands()
        {
            LoadRightWeapon();
            LoadLeftWeapon();
        }

        public void SwitchRightWeapon()
        {
            if (!playerManager.IsOwner)
                return;
            playerManager.playerAnimatorManager.PlayTargetActionAnimation("Swap_Right_Weapon_01", false,false,true,true);

            //�����л��߼��������װ����������ʱ��ֻ���������������л��������ֻװ��һ������ʱ�������л����յ������ۣ�ֻ�����������Ϳ���֮���л�

            WeaponItem selectedWeapon = null;

            //�����˫�ֵĻ����л��ɵ����ٽ����л�
            playerManager.playerInventoryManager.rightHandWeaponIndex += 1;
            if (playerManager.playerInventoryManager.rightHandWeaponIndex < 0 || playerManager.playerInventoryManager.rightHandWeaponIndex > 2)
            {
                playerManager.playerInventoryManager.rightHandWeaponIndex = 0;

                //��������Ƿ�װ������һ������
                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int firstWeaponPosition = 0;

                for (int i = 0; i < playerManager.playerInventoryManager.weaponInRightHandSlot.Length; i++)
                {
                    if (playerManager.playerInventoryManager.weaponInRightHandSlot[i].itemID != WorldItemDataBase.Instance.unarmedWeapon.itemID)
                    {
                        weaponCount++;
                        if (firstWeapon == null)
                        {
                            firstWeapon = playerManager.playerInventoryManager.weaponInRightHandSlot[i];
                            firstWeaponPosition = i;
                        }
                    }
                }
                if (weaponCount <= 1)
                {
                    playerManager.playerInventoryManager.rightHandWeaponIndex = -1;
                    selectedWeapon = WorldItemDataBase.Instance.unarmedWeapon;
                    playerManager.playerNetworkManager.currentRightHandWeaponID.Value = selectedWeapon.itemID;
                }
                else
                {
                    playerManager.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                    playerManager.playerNetworkManager.currentRightHandWeaponID.Value = firstWeapon.itemID;
                }
                return;
            }

            foreach (WeaponItem item in playerManager.playerInventoryManager.weaponInRightHandSlot)
            {
                if (playerManager.playerInventoryManager.weaponInRightHandSlot[playerManager.playerInventoryManager.rightHandWeaponIndex].itemID != WorldItemDataBase.Instance.unarmedWeapon.itemID)
                {
                    selectedWeapon = playerManager.playerInventoryManager.weaponInRightHandSlot[playerManager.playerInventoryManager.rightHandWeaponIndex];
                    playerManager.playerNetworkManager.currentRightHandWeaponID.Value = playerManager.playerInventoryManager.weaponInRightHandSlot[playerManager.playerInventoryManager.rightHandWeaponIndex].itemID;
                    return;
                }
            }

            if (selectedWeapon == null && playerManager.playerInventoryManager.rightHandWeaponIndex <= 2)
            {
                SwitchRightWeapon();
            }
        }


        public void SwitchLeftWeapon()
        {
            if (!playerManager.IsOwner)
                return;
            playerManager.playerAnimatorManager.PlayTargetActionAnimation("Swap_Left_Weapon_01", false, false, true, true);

            //�����л��߼��������װ����������ʱ��ֻ���������������л��������ֻװ��һ������ʱ�������л����յ������ۣ�ֻ�����������Ϳ���֮���л�

            WeaponItem selectedWeapon = null;

            //�����˫�ֵĻ����л��ɵ����ٽ����л�
            playerManager.playerInventoryManager.leftHandWeaponIndex += 1;
            if (playerManager.playerInventoryManager.leftHandWeaponIndex < 0 || playerManager.playerInventoryManager.leftHandWeaponIndex > 2)
            {
                playerManager.playerInventoryManager.leftHandWeaponIndex = 0;

                //��������Ƿ�װ������һ������
                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int firstWeaponPosition = 0;

                for (int i = 0; i < playerManager.playerInventoryManager.weaponInLeftHandSlot.Length; i++)
                {
                    if (playerManager.playerInventoryManager.weaponInLeftHandSlot[i].itemID != WorldItemDataBase.Instance.unarmedWeapon.itemID)
                    {
                        weaponCount++;
                        if (firstWeapon == null)
                        {
                            firstWeapon = playerManager.playerInventoryManager.weaponInLeftHandSlot[i];
                            firstWeaponPosition = i;
                        }
                    }
                }
                if (weaponCount <= 1)
                {
                    playerManager.playerInventoryManager.leftHandWeaponIndex = -1;
                    selectedWeapon = WorldItemDataBase.Instance.unarmedWeapon;
                    playerManager.playerNetworkManager.currentLeftHandWeaponID.Value = selectedWeapon.itemID;
                }
                else
                {
                    playerManager.playerInventoryManager.leftHandWeaponIndex = firstWeaponPosition;
                    playerManager.playerNetworkManager.currentLeftHandWeaponID.Value = firstWeapon.itemID;
                }
                return;
            }

            foreach (WeaponItem item in playerManager.playerInventoryManager.weaponInLeftHandSlot)
            {
                if (playerManager.playerInventoryManager.weaponInLeftHandSlot[playerManager.playerInventoryManager.leftHandWeaponIndex].itemID != WorldItemDataBase.Instance.unarmedWeapon.itemID)
                {
                    selectedWeapon = playerManager.playerInventoryManager.weaponInLeftHandSlot[playerManager.playerInventoryManager.leftHandWeaponIndex];
                    playerManager.playerNetworkManager.currentLeftHandWeaponID.Value = playerManager.playerInventoryManager.weaponInLeftHandSlot[playerManager.playerInventoryManager.leftHandWeaponIndex].itemID;
                    return;
                }
            }

            if (selectedWeapon == null && playerManager.playerInventoryManager.leftHandWeaponIndex <= 2)
            {
                SwitchLeftWeapon();
            }
        }

        public void LoadRightWeapon()
        {
            if (playerManager.playerInventoryManager.currentRightHandWeapon != null)
            {
                rightHandSlot.UnloadWeapon();

                rightHandWeaponModel = Instantiate(playerManager.playerInventoryManager.currentRightHandWeapon.weaponModel);
                rightHandSlot.LoadWeapon(rightHandWeaponModel);
                rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
                rightWeaponManager.SetWeaponDamage(playerManager, playerManager.playerInventoryManager.currentRightHandWeapon);
            }
        }

        public void LoadLeftWeapon()
        {
            if (playerManager.playerInventoryManager.currentLeftHandWeapon != null)
            {
                //�Ƴ�������
                leftHandSlot.UnloadWeapon();

                //����������
                leftHandWeaponModel = Instantiate(playerManager.playerInventoryManager.currentLeftHandWeapon.weaponModel);
                leftHandSlot.LoadWeapon(leftHandWeaponModel);
                leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
                leftWeaponManager.SetWeaponDamage(playerManager, playerManager.playerInventoryManager.currentLeftHandWeapon);
            }
        }

        //�������ڼ����˺�����ײ���Ŀ����͹رգ���Animator Event�ϵ���
        public void OpenDamageCollider()
        {
            if (playerManager.playerNetworkManager.isUsingRightHand.Value)
            {
                rightWeaponManager.meleeWeaponDamageCollider.EnableDamageCollider();
            }
            else if (playerManager.playerNetworkManager.isUsingLeftHand.Value)
            {
                leftWeaponManager.meleeWeaponDamageCollider.EnableDamageCollider();
            }
        }

        public void CloseDamageCollider()
        {
            if (playerManager.playerNetworkManager.isUsingRightHand.Value)
            {
                rightWeaponManager.meleeWeaponDamageCollider.DisableDamageCollider();
            }
            else if (playerManager.playerNetworkManager.isUsingLeftHand.Value)
            {
                leftWeaponManager.meleeWeaponDamageCollider.DisableDamageCollider();
            }
        }
    }
}