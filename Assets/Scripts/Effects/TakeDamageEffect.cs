using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace NZ
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
    public class TakeDamageEffect : InstantCharacterEffect
    {
        [Header("Character Causing Damage")]
        public CharacterManager characterCausingDamage;

        [Header("Damage")]
        public float physicalDamage = 0;    //在未来将会被分成，标准，打击，挥砍，穿刺四种伤害
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightDamage = 0;  //闪电伤害
        public float holyDamage = 0;   //神圣伤害

        [Header("Final Damage")]
        private int finalDamageDealt = 0;       //玩家造成的，计算护甲和其他所以抗性减免后的伤害

        [Header("Poise")]
        public float poiseDamage = 0;
        public bool poiseIsBroken = false;

        //持续伤害

        [Header("Animation")]
        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        [Header("Sound FX")]
        public bool willPlayDamageSFX = true;
        public AudioClip elementalDamageSoundFX;

        [Header("Direction Damage Taken From")]
        public float angleHitFrom;             //受击角度  决定受到攻击时该播放哪个受伤害动画(向前摔倒，向左，向右等等)
        public Vector3 contactPoint;           //受击点   决定血液特效的位置 

        public override void ProcessEffect(CharacterManager characterManager)
        {
            base.ProcessEffect(characterManager);

            if(characterManager.isDead.Value)
                return;
            CalculateDamage(characterManager);
        }

        private void CalculateDamage(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            if(characterCausingDamage != null)
            {

            }

            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightDamage + holyDamage);
            if (finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }
            Debug.Log(finalDamageDealt);
            character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;
        }
    }
}
