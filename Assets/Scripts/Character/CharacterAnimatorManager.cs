using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
