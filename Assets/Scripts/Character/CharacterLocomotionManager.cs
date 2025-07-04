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
        [SerializeField] protected Vector3 yVelocity;                  //��ɫ�����ϻ��������ġ���������Ծ������ʱ��,CC��������isGrounded�ļ������һ�������������������Ǳ��������һ���������µ���
        [SerializeField] protected float groundedYVelocity = -20;      //��ɫ����ʱ��ʩ�ӵġ��������桱����
        [SerializeField] protected float fallStartYVelocity = -5;      //����ɫ��ؿ�ʼ����ʱ�ĳ�ʼ���䡰��������������ʱ�����ӣ���ֵ��� ���ʹ����������
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
                //�������û�г�����Ծ�������ƶ�
                if (yVelocity.y < 0)
                {
                    inAirTime = 0;
                    fallingVelocityHasBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }   
            }
            else
            {
                //�������û����Ծ�������ٶ�Ҳû���趨
                if (!characterManager.isJumping && !fallingVelocityHasBeenSet)
                {
                    fallingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }
                inAirTime += Time.deltaTime;
                characterManager.animator.SetFloat("InAirTime", inAirTime);
                yVelocity.y += gravityForce * Time.deltaTime; 
            }
            //Ӧ��ʼ����ĳ���������� Y ������ٶȡ�
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

