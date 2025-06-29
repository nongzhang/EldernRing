using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace SG
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager characterManager;
        int vertical;
        int horizontal;
        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");   //��ʹ��string����ת��Ϊint, ��������ʡ�ڴ�
        }
        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement,bool isSprinting)
        {
            float horizontalAmount = horizontalMovement;
            float verticalAmount = verticalMovement;
            if (isSprinting)
            {
                verticalAmount = 2;   //������������棬��ֱ����ֵΪ2ʱ��ʾ���
            }

            characterManager.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
            characterManager.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);

            #region ��һ��ʵ�ַ�ʽ
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
            #endregion 

            //characterManager.animator.SetFloat("Horizontal", snappedHorizontal);
            //characterManager.animator.SetFloat("Vertical", snappedVertical);

            //if (isSprinting)
            //{
            //    snappedVertical = 2;   //������������棬��ֱ����ֵΪ2ʱ��ʾ���
            //}

            //characterManager.animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
            //characterManager.animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
            //#endregion
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
