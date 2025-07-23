using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack Action")]
    public class HeavyAttckWeaponItemAction : WeaponItemAction
    {
        [SerializeField] string heavy_Attack_01 = "Main_Heavy_Attack_01";
        [SerializeField] string heavy_Attack_02 = "Main_Heavy_Attack_02";
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
            //��ǰ�������ڹ����� ���ҿ�����������ô��ִ����������
            if (playerManager.playerCombatManager.canComboWithMainHandWeapon && playerManager.isPerformingAction)
            {
                playerManager.playerCombatManager.canComboWithMainHandWeapon = false;

                //������һ������������������һ����������
                if (playerManager.playerCombatManager.lastAttackAnimationPerformed == heavy_Attack_01)
                {
                    playerManager.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.HeavyAttack02, heavy_Attack_02, true);
                }
                else
                {
                    playerManager.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.HeavyAttack01, heavy_Attack_01, true);
                }
            }
            else if (!playerManager.isPerformingAction)  //����������ǵ�ǰû���ڹ�������ִ��һ����ͨ�Ĺ���
            {
                playerManager.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.HeavyAttack01, heavy_Attack_01, true);
            }
        }
    }
}
