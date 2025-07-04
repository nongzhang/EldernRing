using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class WorldCharacterEffectsManager : MonoBehaviour
    {
        public static WorldCharacterEffectsManager instance;

        [SerializeField] List<InstantCharacterEffect> instantCharacterEffects;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            GenerateEffectIDs();
        }

        private void GenerateEffectIDs()
        {
            for (int i = 0; i < instantCharacterEffects.Count; i++)
            {
                instantCharacterEffects[i].instantEffectID  = i;
            }
        }
    }
}
