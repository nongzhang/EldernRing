using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NZ.Utility;

namespace NZ
{
    public class WorldCharacterEffectsManager : Singleton<WorldCharacterEffectsManager>
    {
        //public static WorldCharacterEffectsManager instance;

        [Header("Damage")]
        public TakeDamageEffect takeDamageEffect;

        [Header("VFX")]
        public GameObject bloodSplatterVFX;

        [SerializeField] List<InstantCharacterEffect> instantCharacterEffects;

        //private void Awake()
        //{
        //    if (instance == null)
        //    {
        //        instance = this;
        //    }
        //    else
        //    {
        //        Destroy(gameObject);
        //    }
        //    GenerateEffectIDs();
        //}
        public override void Awake()
        {
            base.Awake();
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
