using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace NZ
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager characterManager;
        int vertical;
        int horizontal;

        [Header("Damage Animations")]
        public string lastDamageAnimationPlayed;

        [Header("Damage Animations")]
        [SerializeField] string hit_Forward_Medium_01 = "hit_Forward_Medium_01";
        [SerializeField] string hit_Forward_Medium_02 = "hit_Forward_Medium_02";

        [SerializeField] string hit_Backward_Medium_01 = "hit_Backward_Medium_01";
        [SerializeField] string hit_Backward_Medium_02 = "hit_Backward_Medium_02";

        [SerializeField] string hit_Left_Medium_01 = "hit_Left_Medium_01";
        [SerializeField] string hit_Left_Medium_02 = "hit_Left_Medium_02";

        [SerializeField] string hit_Right_Medium_01 = "hit_Right_Medium_01";      //Medium��ʾ�������ȣ����ݲ�ͬ���ǿ�Ȳ��Ų�ͬ���ȵĶ���
        [SerializeField] string hit_Right_Medium_02 = "hit_Right_Medium_02";

        private List<string> forward_Medium_Damage = new List<string>();
        private List<string> backward_Medium_Damage = new List<string>();
        private List<string> left_Medium_Damage = new List<string>();
        private List<string> right_Medium_Damage = new List<string>();

        public List<string> Forward_Medium_Damage { get => forward_Medium_Damage; set => forward_Medium_Damage = value; }
        public List<string> Backward_Medium_Damage { get => backward_Medium_Damage; set => backward_Medium_Damage = value; }
        public List<string> Left_Medium_Damage { get => left_Medium_Damage; set => left_Medium_Damage = value; }
        public List<string> Right_Medium_Damage { get => right_Medium_Damage; set => right_Medium_Damage = value; }

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");   //��ʹ��string����ת��Ϊint, ��������ʡ�ڴ�
        }

        protected virtual void Start()
        {
            Forward_Medium_Damage.Add(hit_Forward_Medium_01);
            Forward_Medium_Damage.Add(hit_Forward_Medium_02);
            Backward_Medium_Damage.Add(hit_Backward_Medium_01);
            Backward_Medium_Damage.Add(hit_Backward_Medium_02);
            Right_Medium_Damage.Add(hit_Right_Medium_01);
            Right_Medium_Damage.Add(hit_Right_Medium_02);
            Left_Medium_Damage.Add(hit_Left_Medium_01);
            Left_Medium_Damage.Add(hit_Left_Medium_02);
        }

        public string GetRandomAnimationFromList(List<string> animationList)
        {
            List<string> finalList = new List<string>();
            foreach (string animation in animationList)
            {
                finalList.Add(animation);
            }

            finalList.Remove(lastDamageAnimationPlayed);

            for (int i = 0; i < finalList.Count; i++)
            {
                if (finalList[i] == null)
                {
                    finalList.RemoveAt(i);
                }
            }

            int randomValue = Random.Range(0, finalList.Count);
            return finalList[randomValue];
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
            Debug.Log("Playing Animation: " + targetAnimation);
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

        public virtual void PlayTargetAttackActionAnimation(AttackType attackType,
            string targetAnimation, bool isPerformingAction, bool applyRootMotion = true,
            bool canRotate = false, bool canMove = false)
        {
            //��¼��һ��ִ�еĹ����������������У�
            //��¼��ǰ�Ĺ������ͣ��ṥ�����ع����ȣ�
            //���ݵ�ǰ�������¶�����
            //�ж���ι����Ƿ���Ա��м�
            //֪ͨ���磺�����ڹ�����״̬��������ڼ��㷴���˺��ȣ�
            characterManager.characterCombatManager.currentAttackType = attackType;
            characterManager.applyRootMotion = applyRootMotion;
            characterManager.animator.CrossFade(targetAnimation, 0.2f);
            characterManager.isPerformingAction = isPerformingAction;
            characterManager.canRotate = canRotate;
            characterManager.canMove = canMove;

            //���߷�����/����,���ǲ�����һ��������Ȼ��Ҳ������������ҿ����������ڲ������������
            characterManager.characterNetworkManager.NotifyTheServerOfAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }
    }
}
