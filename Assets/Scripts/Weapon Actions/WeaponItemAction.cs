using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ
{
    [CreateAssetMenu(menuName ="Character Actions/Weapon Actions/Test Action")]
    public class WeaponItemAction : ScriptableObject
    {
        public int actionID;

        public virtual void AttemptToPerformAction(PlayerManager playerManager, WeaponItem weaponPerformingAction)
        {
            if (playerManager.IsOwner)
            {
                playerManager.playerNetworkManager.currentWeaponBeingUsed.Value = weaponPerformingAction.itemID;
            }

            Debug.Log("The Action has Fired");
        }
    }
}
