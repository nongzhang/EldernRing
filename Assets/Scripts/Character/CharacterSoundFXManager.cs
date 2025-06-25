using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class CharacterSoundFXManager : MonoBehaviour
    {
        private AudioSource audioSource;

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayRollSoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.Instance.rollSFX);
        }
    }
}
