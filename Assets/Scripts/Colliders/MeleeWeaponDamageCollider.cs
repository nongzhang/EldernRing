using Sg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Attacking Character")]
        public CharacterManager characterCausingDamage;   //当某个角色受到伤害时，这个变量指向造成这次伤害的“攻击者”的 CharacterManager（角色管理器）对象

        [Header("Weapon Attack Modifiers")]
        public float light_Attack_01_Modifier;

        protected override void Awake()
        {
            base.Awake();
            //if (damageCollider == null)
            //{
            //    damageCollider = GetComponent<Collider>();
            //}
            damageCollider.enabled = false;   //近战武器的伤害碰撞器在开始时保持关闭，只有挥舞的特定帧期间开启
            
            //Debug.Log(characterCausingDamage.gameObject.name);
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (characterCausingDamage == null)
            {
                characterCausingDamage = GetComponentInParent<CharacterManager>();
            }
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();  //角色各个关节的碰撞器比较多，所以一般会碰到这个，而CharacterManager脚本是挂载在父物体Player身上的

            if (damageTarget != null)
            {
                Debug.Log("A :" + other.gameObject.name);
                if (damageTarget == characterCausingDamage)    //我们不会对自己造成伤害
                    return;
                contactPoint = other.ClosestPointOnBounds(this.transform.position);    //最接近的点作为碰撞点
                Debug.Log("B :" + other.gameObject.name);
                DamageTarget(damageTarget);
            }
        }

        protected override void DamageTarget(CharacterManager damageTarget)
        {
            //我们不想在一个攻击中伤害同一个目标两次，所以我们把受伤害的目标加到一个列表，这个列表会在造成伤害前进行检查
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

            switch (characterCausingDamage.characterCombatManager.currentAttackType)
            {
                case AttackType.LightAttack01:
                    ApplyAttackDamageModifiers(light_Attack_01_Modifier, damageEffect);
                    break;
                default:
                    break;
            }

            //damageTarget.characterEffectManager.ProcessInstantEffect(damageEffect);
            if (characterCausingDamage.IsOwner)
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

            //如果攻击是蓄力重击，那么应该在应用本函数计算完这些基本倍率之后，再额外乘一次蓄力倍率
        }
    }
}
