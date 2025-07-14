using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ
{
    public class WorldSoundFXManager : MonoBehaviour
    {
        public static WorldSoundFXManager instance;
        [Header("Action Sounds")]
        public AudioClip rollSFX;

        [Header("Damage Sounds")]
        public AudioClip[] physicalDamageSFX;
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
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public AudioClip ChooseRandomSFXFromArray(AudioClip[] audioClips)
        {
            int index = Random.Range(0, audioClips.Length);
            return audioClips[index];
        }
    }
}
