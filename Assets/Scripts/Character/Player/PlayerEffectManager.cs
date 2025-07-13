using NZ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NZ
{
    public class PlayerEffectManager : CharacterEffectManager
    {
        [Header("Debug Delete Later")]
        [SerializeField] InstantCharacterEffect effectToTest;
        [SerializeField] bool processEffect = false;

        private void Update()
        {
            if (processEffect)
            {
                processEffect = false;
                //当我们实例化它时，原始的不受影响
                TakeStaminDamageEffect effect = Instantiate(effectToTest) as TakeStaminDamageEffect;
                effect.staminaDamage = 5.5f;

                //
                ProcessInstantEffect(effect);
            }
        }
    }
}
