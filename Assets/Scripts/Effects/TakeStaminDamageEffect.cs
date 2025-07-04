using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    [CreateAssetMenu(menuName ="Character Effects/Instant Effects/Take Stamina Damage")]
    public class TakeStaminDamageEffect : InstantCharacterEffect
    {
        public float staminaDamage;
        public override void ProcessEffect(CharacterManager characterManager)
        {
            CalculateStaminaDamage(characterManager);
        }

        private void CalculateStaminaDamage(CharacterManager characterManager)
        {
            if (characterManager.IsOwner)
            {
                characterManager.characterNetworkManager.currentStamina.Value -= staminaDamage;
            }
        }
    }
}
