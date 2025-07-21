using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack Action")]
    public class HeavyAttckWeaponItemAction : WeaponItemAction
    {
        [SerializeField] string heavy_Attack_01 = "Main_Heavy_Attack_01";
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
            PerformHeavyAttack(playerManager, weaponPerformingAction);
        }

        private void PerformHeavyAttack(PlayerManager playerManager, WeaponItem weaponPerformingAction)
        {
            if (playerManager.playerNetworkManager.isUsingRightHand.Value)
            {
                playerManager.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01, heavy_Attack_01, true);
            }

            if (playerManager.playerNetworkManager.isUsingLeftHand.Value)
            {

            }
        }
    }
}
