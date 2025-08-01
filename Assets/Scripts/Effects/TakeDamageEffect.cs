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

            //检查伤害来自的方向
            //播放伤害动画
            PlayDirectionalBasedDamageAnimation(characterManager);
            //检查积累情况（位置，流血）
            //播放伤害音效
            PlayDamageSFX(characterManager);
            //播放伤害特效
            PlayDamageVFX(characterManager);
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
            Debug.Log("finalDamageDealt: "+ finalDamageDealt);
            character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;
        }

        private void PlayDamageVFX(CharacterManager character)
        {
            //如果我们有火焰伤害，播放火焰粒子特效
            //如果有闪电伤害，播放闪电例子特效
            character.characterEffectManager.PlayBloodSplatterVFX(contactPoint);
        }

        private void PlayDamageSFX(CharacterManager character)
        {
            AudioClip physicalDamageSFX = WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.physicalDamageSFX);
            character.characterSoundFXManager.PlaySoundFX(physicalDamageSFX);
            //如果火焰伤害大于零，播放燃烧音效
            //如果闪电伤害大于零，播放滋滋声······
        }

        private void PlayDirectionalBasedDamageAnimation(CharacterManager characterManager)
        {
            if (!characterManager.IsOwner)
            {
                return;
            }

            if (characterManager.isDead.Value)
                return;

            poiseIsBroken = true;
            if (angleHitFrom >= 145 && angleHitFrom <=180)
            {
                damageAnimation = characterManager.characterAnimatorManager.GetRandomAnimationFromList(characterManager.characterAnimatorManager.Forward_Medium_Damage);
                //播放向前的动画
            }
            else if (angleHitFrom <= -145 &&  angleHitFrom >=-180)
            {
                //播放向前的动画
                damageAnimation = characterManager.characterAnimatorManager.GetRandomAnimationFromList(characterManager.characterAnimatorManager.Forward_Medium_Damage);
            }
            else if (angleHitFrom >=-45 && angleHitFrom <=45)
            {
                //播放向后的动画
                damageAnimation = characterManager.characterAnimatorManager.GetRandomAnimationFromList(characterManager.characterAnimatorManager.Backward_Medium_Damage);
            }
            else if (angleHitFrom >=-144 && angleHitFrom <= -45)
            {
                //播放向左的动画
                damageAnimation = characterManager.characterAnimatorManager.GetRandomAnimationFromList(characterManager.characterAnimatorManager.Left_Medium_Damage);
            }
            else if ((angleHitFrom >= 45 && angleHitFrom <= 144))
            {
                //播放向右的动画
                damageAnimation = characterManager.characterAnimatorManager.GetRandomAnimationFromList(characterManager.characterAnimatorManager.Right_Medium_Damage);
            }

            //如果韧性被打破，则播放一个受击踉跄动画。
            if (poiseIsBroken)
            {
                characterManager.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;
                characterManager.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);
            }
        }
    }
}
