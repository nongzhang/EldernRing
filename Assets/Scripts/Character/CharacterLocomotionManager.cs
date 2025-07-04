using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SG
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        public static CharacterLocomotionManager instance;
        CharacterManager characterManager;

        [Header("Ground CHeck & Jumping")]
        [SerializeField] protected float gravityForce = -5.55f;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float groundCheckSphereRadius = 1;
        [SerializeField] protected Vector3 yVelocity;                  //角色被向上或向下拉的“力”（跳跃或下落时）,CC这个组件的isGrounded的检测是上一个动作结束后，所以我们必须给它加一个持续向下的力
        [SerializeField] protected float groundedYVelocity = -20;      //角色贴地时所施加的“吸附地面”的力
        [SerializeField] protected float fallStartYVelocity = -5;      //当角色离地开始下落时的初始下落“力”（随着下落时间增加，数值变大） 这会使玩家体验更优
        protected bool fallingVelocityHasBeenSet = false;
        [SerializeField] protected float inAirTime = 0;

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected virtual void Start()
        {
            //SceneManager.activeSceneChanged += OnSceneChange;
            //instance.enabled = false;

        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
            {
                instance.enabled = true;
            }
            else
            {
                instance.enabled = false;
            }
        }

        protected virtual void Update()
        {
            if (SceneManager.GetActiveScene().name == "Scene_Main_Menu_01")
            {
                return;
            }
            HandleGroundCheck();

            if (characterManager.isGrounded)
            {
                //如果我们没有尝试跳跃或向上移动
                if (yVelocity.y < 0)
                {
                    inAirTime = 0;
                    fallingVelocityHasBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }   
            }
            else
            {
                //如果我们没有跳跃，下落速度也没有设定
                if (!characterManager.isJumping && !fallingVelocityHasBeenSet)
                {
                    fallingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }
                inAirTime += Time.deltaTime;
                characterManager.animator.SetFloat("InAirTime", inAirTime);
                yVelocity.y += gravityForce * Time.deltaTime; 
            }
            //应该始终有某种力作用于 Y 方向的速度。
            characterManager.characterController.Move(yVelocity * Time.deltaTime);

        }

        protected void HandleGroundCheck()
        {
            characterManager.isGrounded = Physics.CheckSphere(characterManager.transform.position, groundCheckSphereRadius, groundLayer);
        }

        protected void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(characterManager.transform.position, groundCheckSphereRadius);
        }

        protected void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }
    }
}

