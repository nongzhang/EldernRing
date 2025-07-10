using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class WeaponItem : Item
    {
        //动画控制器覆盖 （基于你目前使用的武器来改变攻击动画，比如远程武器和近战武器的攻击动画显然不同）

        [Header("Weapon Model")]
        public GameObject weaponModel;

        [Header("Weapon Requirements")]  //装备武器所必须的属性值,力量，敏捷，智力，信仰
        public int strengthREQ = 0;
        public int dexREQ = 0;
        public int intREQ = 0;
        public int faithREQ = 0;

        [Header("Weapon Base Damage")]  //基于武器的伤害
        public int physicalDamage = 0;
        public int magicDamage = 0;
        public int fireDamage = 0;
        public int holyDamage = 0;
        public int lightningDamage = 0;

        [Header("Weapon Base Poise Damage")]  //武器的失衡值,可自己实现
        public float poiseDamage = 10;

        [Header("Stamina Costs")]          //武器攻击时消耗的耐力
        public int baseStaminaCost = 20;


        //item based actions(比如鼠标左键是轻攻击，右键是重攻击，shift+左键是战技等)

        //Ash of War

        //武器被弹开的声音
    }
}