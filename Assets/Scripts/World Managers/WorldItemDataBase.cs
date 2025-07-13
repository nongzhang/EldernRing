using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NZ
{
    public class WorldItemDataBase : MonoBehaviour
    {
        public static WorldItemDataBase Instance;

        public WeaponItem unarmedWeapon;  //����ȱʧ������
        [SerializeField] List<WeaponItem> weapons = new List<WeaponItem>();

        //��Ϸ��������Ʒ���б�
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
