using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
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

        [Header("Weapon Base Poise Damage")]  //������ʧ��ֵ,���Լ�ʵ��
        public float poiseDamage = 10;

        [Header("Stamina Costs")]          //��������ʱ���ĵ�����
        public int baseStaminaCost = 20;


        //item based actions(�������������ṥ�����Ҽ����ع�����shift+�����ս����)

        //Ash of War

        //����������������
    }
}