using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NZ.Utility;

namespace NZ
{
    public class WorldActionManager : Singleton<WorldActionManager>
    {
        //public static WorldActionManager instance;
        [Header("Weapon Item Actions")]
        public WeaponItemAction[] weaponItemActions;

        //private void Awake()
        //{
        //    if (instance == null)
        //    {
        //        instance = this;
        //    }
        //    else
        //    {
        //        Destroy(gameObject);
        //    }

        //    DontDestroyOnLoad(gameObject);
        //}

        public override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            for (int i = 0; i < weaponItemActions.Length; i++)
            {
                weaponItemActions[i].actionID = i;
            }
        }

        public WeaponItemAction GetWeaponItemActionByID(int ID)
        {
            return weaponItemActions.FirstOrDefault(action => action.actionID == ID);
        }

        //��linq�Ż���
        //public WeaponItemAction GetWeaponItemActionByID(int ID)
        //{
        //    if (weaponItemActions == null) return null;

        //    for (int i = 0; i < weaponItemActions.Length; i++)
        //    {
        //        if (weaponItemActions[i].actionID == ID)
        //        {
        //            return weaponItemActions[i];
        //        }
        //    }

        //    return null;
        //}

    }
}
