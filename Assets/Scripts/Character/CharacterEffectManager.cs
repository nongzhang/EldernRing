using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ
{
    public class CharacterEffectManager : MonoBehaviour
    {
        //����ʱЧ���������ܵ��˺������Ƶ�

        //�������Ч���������ж���

        //����̬Ч��������װ����Ʒ�ȴ�����buff

        CharacterManager characterManager;

        [Header("VFX")]
        [SerializeField] GameObject bloodSplatterVFX;

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }

        public virtual void ProcessInstantEffect(InstantCharacterEffect instantCharacterEffect)
        {
            instantCharacterEffect.ProcessEffect(characterManager);
        }

        public void PlayBloodSplatterVFX(Vector3 contactPoint)
        {
            if (bloodSplatterVFX != null)
            {
                GameObject bloodSplatter = Instantiate(bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
            else
            {
                GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.Instance.bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
        }
    }
}
