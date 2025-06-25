using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace SG
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager characterManager;
        float vertical;
        float horizontal;
        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }
        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement)
        {
            characterManager.animator.SetFloat("Horizontal", horizontalMovement,0.1f, Time.deltaTime);
            characterManager.animator.SetFloat("Vertical", verticalMovement, 0.1f, Time.deltaTime);

            //�����������������ô�ã����߻��ʱ��������̫��������ʹ�����·�ʽ
            //float snappedHorizontal = 0;
            //float snappedVertical = 0;

            //#region Horizontal
            //if (horizontalMovement > 0 && horizontalMovement <= 0.5f)
            //{
            //    snappedHorizontal = 0.5f;
            //}
            //else if (horizontalMovement > 0.5f && horizontalMovement <= 1)
            //{
            //    snappedHorizontal = 1;
            //}
            //else if (horizontalMovement < 0 && horizontalMovement >= -0.5f)
            //{
            //    snappedHorizontal = -0.5f;
            //}
            //else if (horizontalMovement < -0.5f && horizontalMovement >= -1)
            //{
            //    snappedHorizontal = -1;
            //}
            //else
            //{
            //    snappedHorizontal = 0;
            //}
            //#endregion 

            //#region Vertical
            //if (verticalMovement > 0 && verticalMovement <= 0.5f)
            //{
            //    snappedVertical = 0.5f;
            //}
            //else if (verticalMovement > 0.5f && verticalMovement <= 1)
            //{
            //    snappedVertical = 1;
            //}
            //else if (verticalMovement < 0 && verticalMovement >= -0.5f)
            //{
            //    snappedVertical = -0.5f;
            //}
            //else if (verticalMovement < -0.5f && verticalMovement >= -1)
            //{
            //    snappedVertical = -1;
            //}
            //else
            //{
            //    snappedVertical = 0;
            //}
            //#endregion 

            //characterManager.animator.SetFloat("Horizontal", snappedHorizontal);
            //characterManager.animator.SetFloat("Vertical", snappedVertical);
        }

        //�ڲ���Ŀ���ж�����ʱ�������ƶ�����ת
        public virtual void PlayTargetActionAnimation(
            string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, 
            bool canRotate = false, bool canMove = false)
        {
            //������������ֹ��ɫ����ͼִ���¶���
            //���磺�����ɫ�ܵ��˺�����ʼ�����ܻ������������־λ�ͻ����ó� true����ʾ�㴦��stunned(Ӳֱ)״̬��
            //֮�����ǿ������������� AI ִ�ж���ǰ���ȼ�������־λ����������stunned(Ӳֱ)�ڼ������²�����
            characterManager.applyRootMotion = applyRootMotion;
            characterManager.animator.CrossFade(targetAnimation, 0.2f);
            characterManager.isPerformingAction = isPerformingAction;
            characterManager.canRotate = canRotate;
            characterManager.canMove = canMove;

            //���߷�����/����,���ǲ�����һ��������Ȼ��Ҳ������������ҿ����������ڲ������������
            characterManager.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }
    }
}
