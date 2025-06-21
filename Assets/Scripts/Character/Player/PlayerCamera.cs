using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SG
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera instance;
        public PlayerManager playerManager;
        public Camera cameraObject;

        //�ı���Щ����������ı���
        [Header("Camera Settings")]
        private float cameraSmoothSpeed = 1.0f;                      //���ֵԽ�������������λ�õ�ʱ��Խ��
        [SerializeField] float leftAndRightRotationSpeed = 220;      //
        [SerializeField] float upAndDownRotationSpeed = 220;
        [SerializeField] float minimumPivot = -30;                   //������¿��ܿ�������͵�
        [SerializeField] float maximumPivot = 60;                    //������Ͽ��ܿ�������ߵ�

        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        [SerializeField] float leftAndRightLookAnglr;
        [SerializeField] float upAndDownLookAngle;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void HandleAllCameraAction()
        {
            HandleFollowTarget();
            //Χ�������ת
            HandleRotation();
            //����ͳ���������������ײ������
        }

        private void HandleFollowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, playerManager.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }

        private void HandleRotation()
        {
            //����������ʱ��ǿ�����Χ����Ŀ����ת
            //����Χ�������ת
        }
    }
}

