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
                //������ĳЩ����ʱ���õ����л���׷��Ŀ��״̬
                Debug.Log("We have a target");
                return this;
            }
            else
            {
                //���˼�����ѰĿ��״̬
                Debug.Log("searching a  target");
                aICharacterManager.aICharacterCombatManager.FindATargetViaLineOdSight(aICharacterManager);
                return this;
            }

            
        }
    } 
}
