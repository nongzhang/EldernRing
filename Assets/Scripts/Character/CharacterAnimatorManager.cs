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
            //#endregion 

            //characterManager.animator.SetFloat("Horizontal", snappedHorizontal);
            //characterManager.animator.SetFloat("Vertical", snappedVertical);
        }

        //在播放目标行动动画时，禁用移动和旋转
        public virtual void PlayTargetActionAnimation(
            string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, 
            bool canRotate = false, bool canMove = false)
        {
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
    }
}
