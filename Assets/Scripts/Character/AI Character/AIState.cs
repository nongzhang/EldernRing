using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ
{
    public class AIState : ScriptableObject
    {
        public virtual AIState Tick(AICharacterManager aICharacterManager)
        {
            Debug.Log("We are running this state");
            return this;
        }
    }
}
