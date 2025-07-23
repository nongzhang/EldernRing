using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ
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

        [Header("Weapon Poise")]  //武器的失衡值,可自己实现
        public float poiseDamage = 10;

        [Header("Attack Modifiers")]  //很多武器连续攻击的第二次攻击比第一次伤害高，所以需要一个伤害修正器
        public float light_Attack_01_Modifier = 1.0f;
        public float light_Attack_02_Modifier = 1.2f;
        public float heavy_Attack_01_Modifier = 1.4f;
        public float heavy_Attack_02_Modifier = 1.6f;
        public float charge_Attack_01_Modifier = 2.0f;
        public float charge_Attack_02_Modifier = 2.2f;

        [Header("Stamina Costs Modifiers")]          //武器攻击时消耗的耐力
        public int baseStaminaCost = 20;
        public float lightAttackStaminaCostMultiplier = 0.9f;

        [Header("Actions")]
        public WeaponItemAction oneHandRB_Action;      //单手鼠标右键的动作 单手右肩键动作     轻攻击
        public WeaponItemAction oneHandRT_Action;      //                单手右扳机键动作    重攻击
        //item based actions(比如鼠标左键是轻攻击，右键是重攻击，shift+左键是战技等)

        //Ash of War

        //武器被弹开的声音
    }
}