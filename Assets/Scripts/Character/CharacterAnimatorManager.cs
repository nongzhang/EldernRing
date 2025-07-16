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

        [SerializeField] string hit_Right_Medium_01 = "hit_Right_Medium_01";      //Medium表示动画幅度，根据不同打击强度播放不同幅度的动画
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
            horizontal = Animator.StringToHash("Horizontal");   //不使用string而是转化为int, 这样更节省内存
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
                verticalAmount = 2;   //动画混合树里面，竖直方向值为2时表示冲刺
            }

            characterManager.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
            characterManager.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);

            #region 另一种实现方式
            //如果动画看起来不怎么好，或者混合时看起来不太正常，可使用以下方式
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
            //    snappedVertical = 2;   //动画混合树里面，竖直方向值为2时表示冲刺
            //}

            //characterManager.animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
            //characterManager.animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
            //#endregion
        }

        //在播放目标行动动画时，禁用移动和旋转
        public virtual void PlayTargetActionAnimation(
            string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, 
            bool canRotate = false, bool canMove = false)
        {
            Debug.Log("Playing Animation: " + targetAnimation);
            //可以用它来阻止角色再试图执行新动作
            //例如：当你角色受到伤害并开始播放受击动画。这个标志位就会设置成 true，表示你处于stunned(硬直)状态。
            //之后我们可以在玩家输入或 AI 执行动作前，先检查这个标志位，不让它在stunned(硬直)期间做出新操作。
            characterManager.applyRootMotion = applyRootMotion;
            characterManager.animator.CrossFade(targetAnimation, 0.2f);
            characterManager.isPerformingAction = isPerformingAction;
            characterManager.canRotate = canRotate;
            characterManager.canMove = canMove;

            //告诉服务器/主机,我们播放了一个动画，然后也让所有其他玩家看到我们正在播放这个动画。
            characterManager.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }

        public virtual void PlayTargetAttackActionAnimation(AttackType attackType,
            string targetAnimation, bool isPerformingAction, bool applyRootMotion = true,
            bool canRotate = false, bool canMove = false)
        {
            //记录上一次执行的攻击动作（用于连招）
            //记录当前的攻击类型（轻攻击、重攻击等）
            //根据当前武器更新动画集
            //判断这次攻击是否可以被招架
            //通知网络：“正在攻击”状态被激活（用于计算反击伤害等）
            characterManager.characterCombatManager.currentAttackType = attackType;
            characterManager.applyRootMotion = applyRootMotion;
            characterManager.animator.CrossFade(targetAnimation, 0.2f);
            characterManager.isPerformingAction = isPerformingAction;
            characterManager.canRotate = canRotate;
            characterManager.canMove = canMove;

            //告诉服务器/主机,我们播放了一个动画，然后也让所有其他玩家看到我们正在播放这个动画。
            characterManager.characterNetworkManager.NotifyTheServerOfAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }
    }
}
