using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NZ
{
    public class WorldItemDataBase : MonoBehaviour
    {
        public static WorldItemDataBase Instance;

        public WeaponItem unarmedWeapon;  //武器缺失的情形
        [SerializeField] List<WeaponItem> weapons = new List<WeaponItem>();

        //游戏中所有物品的列表
        [Header("Item")]
        private List<Item> items = new List<Item>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            foreach (var weapon in weapons)
            {
                items.Add(weapon);
            }

            for (int i= 0; i < items.Count; i++)
            {
                items[i].itemID = i;
            }
        }

        public WeaponItem GetWeaponByID(int ID)
        {
            return weapons.FirstOrDefault(weapon => weapon.itemID == ID);
        }
    }
}
