using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace SG
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;
        protected override void Awake()
        {
            base.Awake();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
        }

        protected override void Update()
        {
            base.Update();
            if (!IsOwner)
            {
                return;
            }
            playerLocomotionManager.HandleAllMovement();
            playerStatsManager.RegenerateStamina();
        }

        protected override void LateUpdate()
        {
            if (!IsOwner)
            {
                return;
            }
            base.LateUpdate();
            PlayerCamera.instance.HandleAllCameraAction();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner)
            {
                PlayerCamera.instance.playerManager = this;
                PlayerInputManager.instance.playerManager = this;
                WorldSaveGameManager.instance.playerManager = this;

                playerNetworkManager.vitality.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
                playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxStaminaValue;

                //当状态改变时更新UI状态条（血量，耐力值）
                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHUDManager.SetNewStaminaValue;
                playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.instance.playerUIHUDManager.SetNewHealthValue;
                playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;
 
            }
        }

        public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
            currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
            currentCharacterData.xPos = transform.position.x;
            currentCharacterData.yPos = transform.position.y;
            currentCharacterData.zPos = transform.position.z;

            currentCharacterData.currentHealth = playerNetworkManager.currentHealth.Value;
            currentCharacterData.currentStamina = playerNetworkManager.currentStamina.Value;

            currentCharacterData.vitality = playerNetworkManager.vitality.Value; 
            currentCharacterData.endurance = playerNetworkManager.endurance.Value;
        }

        public void LoadGameFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            playerNetworkManager.characterName.Value = currentCharacterData.characterName;
            Vector3 myPosition = new Vector3(currentCharacterData.xPos, currentCharacterData.yPos, currentCharacterData.zPos);
            transform.position = myPosition;

            playerNetworkManager.vitality.Value = currentCharacterData.vitality;
            playerNetworkManager.endurance.Value = currentCharacterData.endurance;

            playerNetworkManager.maxHealth.Value = playerStatsManager.CalculateHealthBasedOnVitalityLevel(playerNetworkManager.vitality.Value);
            playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
            playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(currentCharacterData.endurance);
            playerNetworkManager.currentStamina.Value = currentCharacterData.currentStamina;
            PlayerUIManager.instance.playerUIHUDManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
        }
    }
}
