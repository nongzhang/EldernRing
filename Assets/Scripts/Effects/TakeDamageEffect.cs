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
        public float physicalDamage = 0;    //��δ�����ᱻ�ֳɣ���׼��������ӿ������������˺�
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightDamage = 0;  //�����˺�
        public float holyDamage = 0;   //��ʥ�˺�

        [Header("Final Damage")]
        private int finalDamageDealt = 0;       //�����ɵģ����㻤�׺��������Կ��Լ������˺�

        [Header("Poise")]
        public float poiseDamage = 0;
        public bool poiseIsBroken = false;

        //�����˺�

        [Header("Animation")]
        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        [Header("Sound FX")]
        public bool willPlayDamageSFX = true;
        public AudioClip elementalDamageSoundFX;

        [Header("Direction Damage Taken From")]
        public float angleHitFrom;             //�ܻ��Ƕ�  �����ܵ�����ʱ�ò����ĸ����˺�����(��ǰˤ�����������ҵȵ�)
        public Vector3 contactPoint;           //�ܻ���   ����ѪҺ��Ч��λ�� 

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
