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
        public float lightDamage = 0;  //闪电伤害
        public float holyDamage = 0;   //神圣伤害

        [Header("Contact Point")]
        public Vector3 contactPoint;

        [Header("Characters Damaged")]
        protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();

        protected virtual void Awake()
        {

        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();  //角色各个关节的碰撞器比较多，所以一般会碰到这个，而CharacterManager脚本是挂载在父物体Player身上的

            if (damageTarget != null)
            {
                contactPoint = other.ClosestPointOnBounds(this.transform.position);    //最接近的点作为碰撞点

                DamageTarget(damageTarget);
            }
        }

        protected virtual void DamageTarget(CharacterManager damageTarget)
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
            charactersDamaged.Clear();  //这样，在下一次攻击中，我们又可以对角色造成伤害
            
        }
    }
}
