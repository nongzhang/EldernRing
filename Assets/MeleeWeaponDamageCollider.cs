using Sg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Attacking Character")]
        public CharacterManager characterCausingDamage;   //�ڼ����˺�ʱ������ֶ����ڲ�ѯ�����ߵ��˺��ӳɡ���Ч����Ϣ
    }
}
