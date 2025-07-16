using NZ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ
{
    public class DamageCollider : MonoBehaviour
    {
        [Header("Collider")]
        [SerializeField]protected Collider damageCollider;
        public Vector3 boxHalfExtents = new Vector3(0.5f, 0.5f, 0.5f);

        [Header("Damage")]
        public float physicalDamage = 0;
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightDamage = 0;  //�����˺�
        public float holyDamage = 0;   //��ʥ�˺�

        [Header("Contact Point")]
        public Vector3 contactPoint;

        [Header("Characters Damaged")]
        protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();

        protected virtual void Awake()
        {

        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();  //��ɫ�����ؽڵ���ײ���Ƚ϶࣬����һ��������������CharacterManager�ű��ǹ����ڸ�����Player���ϵ�

            if (damageTarget != null)
            {
                contactPoint = other.ClosestPointOnBounds(this.transform.position);    //��ӽ��ĵ���Ϊ��ײ��

                DamageTarget(damageTarget);
            }
        }

        protected virtual void DamageTarget(CharacterManager damageTarget)
        {
            //���ǲ�����һ���������˺�ͬһ��Ŀ�����Σ��������ǰ����˺���Ŀ��ӵ�һ���б�����б��������˺�ǰ���м��
            if (charactersDamaged.Contains(damageTarget))
            {
                return;
            }
            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightDamage = lightDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.contactPoint = contactPoint;

            damageTarget.characterEffectManager.ProcessInstantEffect(damageEffect);
        }

        public virtual void EnableDamageCollider()
        {
            //Debug.Log("Damage Collider Enabled");
            damageCollider.enabled = true;
        }

        public virtual void DisableDamageCollider()
        {
            //Debug.Log("Collider disabled");
            damageCollider.enabled = false;
            //Debug.Log("Clearing charactersDamaged list, count before: " + charactersDamaged.Count);
            charactersDamaged.Clear();  //����������һ�ι����У������ֿ��ԶԽ�ɫ����˺�
            
        }
    }
}
