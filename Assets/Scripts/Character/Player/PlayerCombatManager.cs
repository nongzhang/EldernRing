using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
namespace NZ
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager playerManager;
        public WeaponItem currentWeaponBeingUsed;

        protected override void Awake()
        {
            base.Awake();

            playerManager = GetComponent<PlayerManager>();
        }

        public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
        {
            if (playerManager.IsOwner)
            {
                //执行动作
                weaponAction.AttemptToPerformAction(playerManager, weaponPerformingAction);

                //通知服务器我们已执行该操作，因此我们也要从他们的视角执行它
                playerManager.playerNetworkManager.NotifyTheServerOfWeaponActionServerRpc(NetworkManager.Singleton.LocalClientId, weaponAction.actionID, weaponPerformingAction.itemID);
            }
        }

        public virtual void DrainStaminaBasedOnAttack()
        {
            if (!playerManager.IsOwner)
                return;
            if (currentWeaponBeingUsed == null)
            {
                return;
            }

            float staminaDeducted = 0;

            switch (currentAttackType)
            {
                case AttackType.LightAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                default:
                    break;
            }
            Debug.Log("Stamina Deducted: " + staminaDeducted);
            playerManager.playerNetworkManager.currentStamina.Value -= Mathf.RoundToInt(staminaDeducted);
        }

        public override void SetTarget(CharacterManager newTarget)
        {
            base.SetTarget(newTarget);

            if (playerManager.IsOwner)
            {
                PlayerCamera.instance.SetLockCameraHeight();
            }
        }
    }
}
