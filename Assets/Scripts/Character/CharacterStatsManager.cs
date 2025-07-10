using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Unity.Netcode;

namespace SG
{
    //��ɫ״̬��������һ���ɫ��Ѫ����ħ����������������״̬
    public class CharacterStatsManager : MonoBehaviour
    {
        CharacterManager characterManager;
        [Header("Stamina Regeneration")]
        [SerializeField] float staminaRegenerationAmount = 2;         //�����ָ���
        private float staminaRegenerationTimer = 0;                   //�����ָ���ʱ��
        private float staminaTickTimer = 0;
        [SerializeField] float staminaRegenerationDelay = 2;          //�ӳټ����ʼ�ָ�����

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }

        protected virtual void Start()
        {

        }

        public int CalculateHealthBasedOnVitalityLevel(int vitality)
        {
            float health = 0;

            //����ֵ���㹫ʽ����������������ֵ��
            health = vitality * 15;
            return Mathf.RoundToInt(health);
        }

        public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
        {
            float stamina = 0;

            //����ֵ���㹫ʽ��������������ֵ��
            stamina = endurance * 10;
            return Mathf.RoundToInt(stamina);
        }

        public virtual void RegenerateStamina()
        {
            if (!characterManager.IsOwner)
            {
                return;
            }

            //�ڳ���ڼ䲻��ָ�����
            if (characterManager.characterNetworkManager.isSprinting.Value)
            {
                return;
            }

            if (characterManager.isPerformingAction)
                return;

            staminaRegenerationTimer += Time.deltaTime;

            if (staminaRegenerationTimer >= staminaRegenerationDelay)
            {
                if (characterManager.characterNetworkManager.currentStamina.Value < characterManager.characterNetworkManager.maxStamina.Value)
                {
                    staminaTickTimer += Time.deltaTime;
                    if (staminaTickTimer >= 0.1)    //ÿ��0.1s,�ָ�һ����������
                    {
                        staminaTickTimer = 0;
                        characterManager.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                    }
                }
            }
        }

        //���������ָ���ʱ������Ϊ�������ڶ�������֮��2s��ʼ�ָ��ģ�������������ڼ����κδ��������Ϊ(�����κκķ������Ķ���)����ô�����¿�ʼ��ʱ
        public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float currentStaminaAmount)
        {
            //ֻ�е����������������ʱ�����ǲŻ����ûָ�
            //��������Ѿ��ڻָ���������ô���Ǿ���������
            if (currentStaminaAmount < previousStaminaAmount)
            {
                staminaRegenerationTimer = 0;
            }
        }
    }
}
