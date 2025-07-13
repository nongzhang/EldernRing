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
