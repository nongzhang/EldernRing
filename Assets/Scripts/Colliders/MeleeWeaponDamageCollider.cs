using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace NZ
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Attacking Character")]
        public CharacterManager characterCausingDamage;   //��ĳ����ɫ�ܵ��˺�ʱ���������ָ���������˺��ġ������ߡ��� CharacterManager����ɫ������������

        [Header("Weapon Attack Modifiers")]
        public float light_Attack_01_Modifier;
        public float heavy_Attack_01_Modifier;
        public float charge_Attack_01_Modifier;

        protected override void Awake()
        {
            base.Awake();
            //if (damageCollider == null)
            //{
            //    damageCollider = GetComponent<Collider>();
            //}
            damageCollider.enabled = false;   //��ս�������˺���ײ���ڿ�ʼʱ���ֹرգ�ֻ�л�����ض�֡�ڼ俪��
            
            //Debug.Log(characterCausingDamage.gameObject.name);
        }

        protected override void OnTriggerEnter(Collider other)
        {
            //Debug.Log("A :" + other.gameObject.name);
            if (characterCausingDamage == null)
            {
                characterCausingDamage = GetComponentInParent<CharacterManager>();
            }
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();  //��ɫ�����ؽڵ���ײ���Ƚ϶࣬����һ��������������CharacterManager�ű��ǹ����ڸ�����Player���ϵ�

            if (damageTarget != null)
            {
                //Debug.Log("B :" + other.gameObject.name);
                if (damageTarget == characterCausingDamage)    //���ǲ�����Լ�����˺�
                    return;
                contactPoint = other.ClosestPointOnBounds(this.transform.position);    //��ӽ��ĵ���Ϊ��ײ��
                //Debug.Log("C :" + other.gameObject.name);
                DamageTarget(damageTarget);
            }
        }

        protected override void DamageTarget(CharacterManager damageTarget)
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
            damageEffect.angleHitFrom = Vector3.SignedAngle(characterCausingDamage.transform.forward, damageTarget.transform.forward, Vector3.up);

            switch (characterCausingDamage.characterCombatManager.currentAttackType)
            {
                case AttackType.LightAttack01:
                    ApplyAttackDamageModifiers(light_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.HeavyAttack01:
                    ApplyAttackDamageModifiers(heavy_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.ChargedAttack01:
                    ApplyAttackDamageModifiers(charge_Attack_01_Modifier, damageEffect);
                    break;
                default:
                    break;
            }

            //damageTarget.characterEffectManager.ProcessInstantEffect(damageEffect);
            if (characterCausingDamage.IsOwner)      //characterCausingDamage.IsOwner
            {
                damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(damageTarget.NetworkObjectId,
                    characterCausingDamage.NetworkObjectId,
                    damageEffect.physicalDamage,
                    damageEffect.magicDamage,
                    damageEffect.fireDamage,
                    damageEffect.holyDamage,
                    damageEffect.poiseDamage,
                    damageEffect.angleHitFrom,
                    damageEffect.contactPoint.x,
                    damageEffect.contactPoint.y,
                    damageEffect.contactPoint.z);
            }
        }

        private void ApplyAttackDamageModifiers(float modifier, TakeDamageEffect damage)
        {
            damage.physicalDamage *= modifier;
            damage.magicDamage *= modifier;
            damage.fireDamage *= modifier;
            damage.lightDamage *= modifier;
            damage.holyDamage *= modifier;

            //��������������ػ�����ôӦ����Ӧ�ñ�������������Щ��������֮���ٶ����һ����������
        }
    }
}
