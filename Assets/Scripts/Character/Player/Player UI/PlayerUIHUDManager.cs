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
            //���ַ���Ҫ���ṩһ�������� ID���������ݿ��в������ ID ƥ���������Ȼ�������������ȡ������ͼ�����Ϣ
            //�ŵ㣺 �������Ѿ��洢�˵�ǰ������ ID���㲻�صȴ���һ��������ſ�ʼ��ȡ�������������ҪʱԤ�ȴ����ݿ��ȡ�������
            //ȱ�㣺 ���ַ�������ֱ�ӻ�ȡ�����ķ�ʽֱ�ۡ�
            WeaponItem weapon = WorldItemDataBase.Instance.GetWeaponByID(weaponID);

            if (weapon == null || weapon.itemIcon == null)
            {
                Debug.Log("weapon is null");
                rightWeaponQuickSlotIcon.enabled = false; 
                rightWeaponQuickSlotIcon.sprite = null;
                return;
            }

            //�����ǰ�ȼ�(����)������ʹ�ø���������ô�������½ǻ���ʾ���

            rightWeaponQuickSlotIcon.sprite = weapon.itemIcon;
            rightWeaponQuickSlotIcon.enabled = true;
        }

        public void SetLeftWeaponQuickSlotIcon(int weaponID)
        {
            //���ַ���Ҫ���ṩһ�������� ID���������ݿ��в������ ID ƥ���������Ȼ�������������ȡ������ͼ�����Ϣ
            //�ŵ㣺 �������Ѿ��洢�˵�ǰ������ ID���㲻�صȴ���һ��������ſ�ʼ��ȡ�������������ҪʱԤ�ȴ����ݿ��ȡ�������
            //ȱ�㣺 ���ַ�������ֱ�ӻ�ȡ�����ķ�ʽֱ�ۡ�
            WeaponItem weapon = WorldItemDataBase.Instance.GetWeaponByID(weaponID);

            if (weapon == null || weapon.itemIcon == null)
            {
                Debug.Log("weapon is null");
                leftWeaponQuickSlotIcon.enabled = false;
                leftWeaponQuickSlotIcon.sprite = null;
                return;
            }

            //�����ǰ�ȼ�(����)������ʹ�ø���������ô�������½ǻ���ʾ���

            leftWeaponQuickSlotIcon.sprite = weapon.itemIcon;
            leftWeaponQuickSlotIcon.enabled = true;
        }
    }
}
