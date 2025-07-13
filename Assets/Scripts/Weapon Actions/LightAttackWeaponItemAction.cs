using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
    public class LightAttackWeaponItemAction : WeaponItemAction
    {
        [SerializeField] string light_Attack_01 = "Main_Light_Attack_01";
        public override void AttemptToPerformAction(PlayerManager playerManager, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerManager, weaponPerformingAction);
            if (!playerManager.IsOwner)
            {
                return;
            }
            if (playerManager.playerNetworkManager.currentStamina.Value <= 0)
            {
                return;
            }
            if (!playerManager.isGrounded)
            {
                return;
            }
            PerformLightAttack(playerManager, weaponPerformingAction);
        }

        private void PerformLightAttack(PlayerManager playerManager, WeaponItem weaponPerformingAction)
        {
            if (playerManager.playerNetworkManager.isUsingRightHand.Value)
            {
                playerManager.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01, light_Attack_01, true);
            }

            if (playerManager.playerNetworkManager.isUsingLeftHand.Value)
            {

            }
        }
    }
}