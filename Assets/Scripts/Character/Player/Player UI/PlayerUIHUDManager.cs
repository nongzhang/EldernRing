using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NZ
{
    public class PlayerUIHUDManager : MonoBehaviour
    {
        [Header("STAT BARS")]
        [SerializeField] UI_StatBar healthBar;
        [SerializeField] UI_StatBar staminaBar;

        [Header("QUICK SLOTS")]
        [SerializeField] Image rightWeaponQuickSlotIcon;
        [SerializeField] Image leftWeaponQuickSlotIcon;

        public void RefreshHUD()
        {
            healthBar.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(true);
            staminaBar.gameObject.SetActive(false);
            staminaBar.gameObject.SetActive(true);
        }

        public void SetNewHealthValue(int oldValue, int newValue)
        {
            healthBar.SetStat(newValue);
        }

        public void SetMaxHealthValue(int maxHealth)
        {
            healthBar.SetMaxStat(maxHealth);
        }

        public void SetNewStaminaValue(float oldValue, float newValue)
        {
            staminaBar.SetStat(Mathf.RoundToInt(newValue));
        }

        public void SetMaxStaminaValue(int maxStamina)
        {
            staminaBar.SetMaxStat(maxStamina);
        }

        public void SetRightWeaponQuickSlotIcon(int weaponID)
        {
            //这种方法要求提供一个武器的 ID，并从数据库中查找与该 ID 匹配的武器。然后可以用它来获取武器的图标等信息
            //优点： 由于你已经存储了当前武器的 ID，你不必等待玩家获得武器后才开始获取它。你可以在需要时预先从数据库获取相关数据
            //缺点： 这种方法不如直接获取武器的方式直观。
            WeaponItem weapon = WorldItemDataBase.Instance.GetWeaponByID(weaponID);

            if (weapon == null || weapon.itemIcon == null)
            {
                Debug.Log("weapon is null");
                rightWeaponQuickSlotIcon.enabled = false; 
                rightWeaponQuickSlotIcon.sprite = null;
                return;
            }

            //如果当前等级(属性)不满足使用该武器，那么武器右下角会显示叉号

            rightWeaponQuickSlotIcon.sprite = weapon.itemIcon;
            rightWeaponQuickSlotIcon.enabled = true;
        }

        public void SetLeftWeaponQuickSlotIcon(int weaponID)
        {
            //这种方法要求提供一个武器的 ID，并从数据库中查找与该 ID 匹配的武器。然后可以用它来获取武器的图标等信息
            //优点： 由于你已经存储了当前武器的 ID，你不必等待玩家获得武器后才开始获取它。你可以在需要时预先从数据库获取相关数据
            //缺点： 这种方法不如直接获取武器的方式直观。
            WeaponItem weapon = WorldItemDataBase.Instance.GetWeaponByID(weaponID);

            if (weapon == null || weapon.itemIcon == null)
            {
                Debug.Log("weapon is null");
                leftWeaponQuickSlotIcon.enabled = false;
                leftWeaponQuickSlotIcon.sprite = null;
                return;
            }

            //如果当前等级(属性)不满足使用该武器，那么武器右下角会显示叉号

            leftWeaponQuickSlotIcon.sprite = weapon.itemIcon;
            leftWeaponQuickSlotIcon.enabled = true;
        }
    }
}
