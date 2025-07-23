using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ
{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        PlayerManager playerManager;
        protected override void Awake()
        {
            base.Awake();
            playerManager = GetComponent<PlayerManager>();
        }

        private void OnAnimatorMove()
        {
            if (playerManager.applyRootMotion)
            {
                Vector3 velocity = playerManager.animator.deltaPosition;
                playerManager.characterController.Move(velocity);
                playerManager.transform.rotation *= playerManager.animator.deltaRotation;
            }
        }

        public override void EnableCanDoCombo()
        {
            if (playerManager.playerNetworkManager.isUsingRightHand.Value)
            {
                playerManager.playerCombatManager.canComboWithMainHandWeapon = true;
            }
            else 
            { 
                //启用副手连击动作
            }
        }
            

        public override void DisableCanDoCombo()
        {
            playerManager.playerCombatManager.canComboWithMainHandWeapon = false;
        }
    }
}
