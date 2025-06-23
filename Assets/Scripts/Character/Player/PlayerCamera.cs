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
        [SerializeField] Transform cameraPivotTransform;

        //�ı���Щ����������ı���
        [Header("Camera Settings")]
        private float cameraSmoothSpeed = 1.0f;                      //���ֵԽ�������������λ�õ�ʱ��Խ��
        [SerializeField] float leftAndRightRotationSpeed = 220;      //
        [SerializeField] float upAndDownRotationSpeed = 220;
        [SerializeField] float minimumPivot = -30;                   //������¿��ܿ�������͵�
        [SerializeField] float maximumPivot = 60;                    //������Ͽ��ܿ�������ߵ�
        [SerializeField] float cameraCollisionRadius = 0.2f;
        [SerializeField] LayerMask colliderWithLayers;

        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        private Vector3 cameraObjectPosition;      //����ײʱ����������ƶ�����λ��
        [SerializeField] float leftAndRightLookAngle;
        [SerializeField] float upAndDownLookAngle;
        private float cameraZPosition;                      //���������ײ��Ҫ��ֵ
        private float targetCameraZPosition;                     //���������ײ��Ҫ��ֵ
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
            cameraZPosition = cameraObject.transform.localPosition.z;
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


            //��ͨ��ת
            //��������ƶ��������ˮƽ�ƶ�
            leftAndRightLookAngle += PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed * Time.deltaTime;

            //��������ƶ����������ֱ�ƶ�
            upAndDownLookAngle -= PlayerInputManager.instance.cameraVerticalInput *upAndDownRotationSpeed * Time.deltaTime;
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

            Vector3 cameraRotation = Vector3.zero;
            Quaternion targetRotation;

            //��ת player camera left&right
            cameraRotation.y = leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            //��ת camera pivot up&down
            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;
        }

        private void HandleCollisions()
        {
            targetCameraZPosition = cameraZPosition;
            RaycastHit hit;
            Vector3 direction = cameraObject.transform.position = cameraPivotTransform.position;
            direction.Normalize();
            if (Physics.SphereCast(cameraPivotTransform.position,cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), 0))
            {
                float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);

                if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
                {
                    targetCameraZPosition = -cameraCollisionRadius;
                }

                cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
                cameraObject.transform.localPosition = cameraObjectPosition;
            }
        }
    }
}

