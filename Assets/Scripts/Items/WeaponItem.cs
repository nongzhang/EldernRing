using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ
{
    public class WeaponItem : Item
    {
        //�������������� ��������Ŀǰʹ�õ��������ı乥������������Զ�������ͽ�ս�����Ĺ���������Ȼ��ͬ��

        [Header("Weapon Model")]
        public GameObject weaponModel;

        [Header("Weapon Requirements")]  //װ�����������������ֵ,���������ݣ�����������
        public int strengthREQ = 0;
        public int dexREQ = 0;
        public int intREQ = 0;
        public int faithREQ = 0;

        [Header("Weapon Base Damage")]  //�����������˺�
        public int physicalDamage = 0;
        public int magicDamage = 0;
        public int fireDamage = 0;
        public int holyDamage = 0;
        public int lightningDamage = 0;

        [Header("Weapon Poise")]  //������ʧ��ֵ,���Լ�ʵ��
        public float poiseDamage = 10;

        [Header("Attack Modifiers")]  //�ܶ��������������ĵڶ��ι����ȵ�һ���˺��ߣ�������Ҫһ���˺�������
        public float light_Attack_01_Modifier = 1.0f;
        public float light_Attack_02_Modifier = 1.2f;
        public float heavy_Attack_01_Modifier = 1.4f;
        public float heavy_Attack_02_Modifier = 1.6f;
        public float charge_Attack_01_Modifier = 2.0f;
        public float charge_Attack_02_Modifier = 2.2f;

        [Header("Stamina Costs Modifiers")]          //��������ʱ���ĵ�����
        public int baseStaminaCost = 20;
        public float lightAttackStaminaCostMultiplier = 0.9f;

        [Header("Actions")]
        public WeaponItemAction oneHandRB_Action;      //��������Ҽ��Ķ��� �����Ҽ������     �ṥ��
        public WeaponItemAction oneHandRT_Action;      //                �����Ұ��������    �ع���
        //item based actions(�������������ṥ�����Ҽ����ع�����shift+�����ս����)

        //Ash of War

        //����������������
    }
}