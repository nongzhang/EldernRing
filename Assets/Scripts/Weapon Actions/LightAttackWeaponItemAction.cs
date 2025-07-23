using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
    public class LightAttackWeaponItemAction : WeaponItemAction
    {
        [SerializeField] string light_Attack_01 = "Main_Light_Attack_01";
        [SerializeField] string light_Attack_02 = "Main_Light_Attack_02";
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
            //��ǰ�������ڹ����� ���ҿ�����������ô��ִ����������
            if (playerManager.playerCombatManager.canComboWithMainHandWeapon && playerManager.isPerformingAction)
            {
                playerManager.playerCombatManager.canComboWithMainHandWeapon = false;

                //������һ������������������һ����������
                if (playerManager.playerCombatManager.lastAttackAnimationPerformed == light_Attack_01)
                {
                    playerManager.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack02, light_Attack_02, true);
                }
                else
                {
                    playerManager.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01, light_Attack_01, true);
                }
            }
            else if (!playerManager.isPerformingAction)  //����������ǵ�ǰû���ڹ�������ִ��һ����ͨ�Ĺ���
            {
                playerManager.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01, light_Attack_01, true);
            }
        }
    }
}