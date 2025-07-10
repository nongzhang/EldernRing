using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Unity.Netcode;

namespace SG
{
    //角色状态管理器，一般角色有血量，魔法量，耐力这三种状态
    public class CharacterStatsManager : MonoBehaviour
    {
        CharacterManager characterManager;
        [Header("Stamina Regeneration")]
        [SerializeField] float staminaRegenerationAmount = 2;         //耐力恢复量
        private float staminaRegenerationTimer = 0;                   //耐力恢复计时器
        private float staminaTickTimer = 0;
        [SerializeField] float staminaRegenerationDelay = 2;          //延迟几秒后开始恢复耐力

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

            //生命值计算公式（基于生命力属性值）
            health = vitality * 15;
            return Mathf.RoundToInt(health);
        }

        public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
        {
            float stamina = 0;

            //耐力值计算公式（基于耐力属性值）
            stamina = endurance * 10;
            return Mathf.RoundToInt(stamina);
        }

        public virtual void RegenerateStamina()
        {
            if (!characterManager.IsOwner)
            {
                return;
            }

            //在冲刺期间不会恢复耐力
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
                    if (staminaTickTimer >= 0.1)    //每过0.1s,恢复一定量的耐力
                    {
                        staminaTickTimer = 0;
                        characterManager.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                    }
                }
            }
        }

        //重置耐力恢复计时器。因为耐力是在动作结束之后2s开始恢复的，如果我们在这期间有任何打断它的行为(比如任何耗费耐力的动作)，那么就重新开始计时
        public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float currentStaminaAmount)
        {
            //只有当这个动作消耗耐力时，我们才会重置恢复
            //如果我们已经在恢复耐力，那么我们就无需重置
            if (currentStaminaAmount < previousStaminaAmount)
            {
                staminaRegenerationTimer = 0;
            }
        }
    }
}
