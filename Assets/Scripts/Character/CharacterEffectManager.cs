using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ
{
    public class CharacterEffectManager : MonoBehaviour
    {
        //处理即时效果，比如受到伤害，治疗等

        //处理持续效果，比如中毒等

        //处理静态效果，比如装备饰品等带来的buff

        CharacterManager characterManager;

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }

        public virtual void ProcessInstantEffect(InstantCharacterEffect instantCharacterEffect)
        {
            instantCharacterEffect.ProcessEffect(characterManager);
        }
    }
}
