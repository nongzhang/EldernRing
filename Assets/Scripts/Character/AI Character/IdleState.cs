using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ {
    [CreateAssetMenu(menuName ="A.I/States/Idle")]
    public class IdleState :AIState
    {
        public override AIState Tick(AICharacterManager aICharacterManager)
        {
            if (aICharacterManager.characterCombatManager.currentTarget != null)
            {
                //当满足某些条件时，让敌人切换到追击目标状态
                Debug.Log("We have a target");
                return this;
            }
            else
            {
                //敌人继续搜寻目标状态
                Debug.Log("searching a  target");
                aICharacterManager.aICharacterCombatManager.FindATargetViaLineOdSight(aICharacterManager);
                return this;
            }

            
        }
    } 
}
