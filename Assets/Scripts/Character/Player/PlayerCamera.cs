using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace NZ
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera instance;
        public PlayerManager playerManager;
        public Camera cameraObject;
        [SerializeField] Transform cameraPivotTransform;

        //改变这些来调整相机的表现
        [Header("Camera Settings")]
        private float cameraSmoothSpeed = 1.0f;                      //这个值越大，相机到达它的位置的时间越长
        [SerializeField] float leftAndRightRotationSpeed = 220;      //
        [SerializeField] float upAndDownRotationSpeed = 220;
        [SerializeField] float minimumPivot = -30;                   //相机向下看能看到的最低点
        [SerializeField] float maximumPivot = 60;                    //相机向上看能看到的最高点
        [SerializeField] float cameraCollisionRadius = 0.2f;
        [SerializeField] LayerMask colliderWithLayers;

        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        private Vector3 cameraObjectPosition;      //在碰撞时将相机对象移动到此位置
        [SerializeField] float leftAndRightLookAngle;
        [SerializeField] float upAndDownLookAngle;
        private float cameraZPosition;                      //计算相机碰撞需要的值
        private float targetCameraZPosition;                     //计算相机碰撞需要的值

        [Header("Lock On")]
        [SerializeField] private float lockOnRadius = 20;
        [SerializeField] private float minimumViewableAngle = -50;
        [SerializeField] private float maximumViewableAngle = 50;
        [SerializeField] float lockOnTargetFollowSpeed = 0.2f;
        [SerializeField] float setCameraHeightSpeed = 1;
        [SerializeField] float unlockedCameraHeight = 0.6f;        //1.65f?
        [SerializeField] float lockedCameraHeight = 0.95f;          //2.0f?
        //[SerializeField] private float maximumLockOnDistance = 20;      //需在实际游戏场景中测试
        private Coroutine cameraLockOnHeightCoroutine;
        private List<CharacterManager> availableTargets = new List<CharacterManager>();
        private CharacterManager nearestLockOnTarget;
        private CharacterManager leftLockOnTarget;
        private CharacterManager rightLockOnTarget;
        private Vector3 velocity = Vector3.zero;


        public CharacterManager NearestLockOnTarget { get => nearestLockOnTarget; set => nearestLockOnTarget = value; }
        public CharacterManager LeftLockOnTarget { get => leftLockOnTarget; set => leftLockOnTarget = value; }
        public CharacterManager RightLockOnTarget { get => rightLockOnTarget; set => rightLockOnTarget = value; }

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
            //围绕玩家旋转
            HandleRotation();
            //处理和场景中其他对象碰撞的问题
        }

        private void HandleFollowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, playerManager.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }

        private void HandleRotation()
        {
            //当我们锁定时，强制相机围绕着目标旋转
            if (playerManager.playerNetworkManager.isLockOn.Value)
            {
                //旋转当前这个GameObjec，水平左右旋转
                Vector3 rotationDirection = playerManager.playerCombatManager.currentTarget.characterCombatManager.LockOnTransform.position - transform.position;  //从相机指向目标点的向量
                rotationDirection.Normalize();
                rotationDirection.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lockOnTargetFollowSpeed);

                //竖直方向上旋转
                rotationDirection = playerManager.playerCombatManager.currentTarget.characterCombatManager.LockOnTransform.position - cameraPivotTransform.position;
                rotationDirection.Normalize();

                targetRotation = Quaternion.LookRotation(rotationDirection);
                cameraPivotTransform.rotation = Quaternion.Slerp(cameraPivotTransform.rotation, targetRotation, lockOnTargetFollowSpeed);

                //将当前的旋转值保存到我们用于计算视角的变量中，以防止解锁目标后视角突然跳动
                leftAndRightLookAngle = transform.eulerAngles.y;
                upAndDownLookAngle = transform.eulerAngles.x;
            }
            //否则围绕玩家自身旋转
            else
            {
                //普通旋转
                //鼠标左右移动控制相机水平移动
                leftAndRightLookAngle += PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed * Time.deltaTime;

                //鼠标上下移动控制相机竖直移动
                upAndDownLookAngle -= PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed * Time.deltaTime;
                upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

                Vector3 cameraRotation = Vector3.zero;
                Quaternion targetRotation;

                //旋转 player camera left&right
                cameraRotation.y = leftAndRightLookAngle;
                targetRotation = Quaternion.Euler(cameraRotation);
                transform.rotation = targetRotation;

                //旋转 camera pivot up&down
                cameraRotation = Vector3.zero;
                cameraRotation.x = upAndDownLookAngle;
                targetRotation = Quaternion.Euler(cameraRotation);
                cameraPivotTransform.localRotation = targetRotation;
            }
            
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

        public void HandleLocatingLocalTargets()
        {
            float shortestDistance = Mathf.Infinity;
            float shorestDistanceOfRightTarget = Mathf.Infinity;
            float shortestDistanceOfLeftTarget = -Mathf.Infinity;

            Collider[] colliders = Physics.OverlapSphere(playerManager.transform.position, lockOnRadius, WorldUtilityManager.Instance.GetCharacterLayers());
            
            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager lockOnTarget = colliders[i].GetComponent<CharacterManager>();
                Debug.Log(lockOnTarget.gameObject.name);
                if (lockOnTarget != null)
                {
                    Vector3 lockOnTargetDirection = lockOnTarget.transform.position - playerManager.transform.position;
                    //float distanceFromTarget = lockOnTargetDirection.x * lockOnTargetDirection.x +lockOnTargetDirection.y * lockOnTargetDirection.y + lockOnTargetDirection.z * lockOnTargetDirection.z;
                    float distanceFromTarget = Vector3.Distance(lockOnTarget.transform.position, playerManager.transform.position);
                    float viewableAngle = Vector3.Angle(lockOnTargetDirection, cameraObject.transform.forward);

                    if (lockOnTarget.isDead.Value)
                        continue;
                    //把自身当作目标，跳过
                    if (lockOnTarget.transform.root == playerManager.transform.root)
                        continue;
                    //如果目标超过最大锁定距离，跳过，检查下一个潜在目标
                    //if (distanceFromTarget > maximumLockOnDistance * maximumLockOnDistance)
                    //    continue;

                    //最后，如果目标在视野之外，或被环境遮挡，则检查下一个潜在目标。
                    if (viewableAngle > minimumViewableAngle && viewableAngle < maximumViewableAngle)
                    {
                        RaycastHit hit;

                        if (Physics.Linecast(playerManager.playerCombatManager.LockOnTransform.position, 
                            lockOnTarget.characterCombatManager.LockOnTransform.position, out hit,WorldUtilityManager.Instance.GetEnviroLayers()))
                        {
                            continue;
                        }
                        else
                        {
                            availableTargets.Add(lockOnTarget);
                        }
                    }
                }
            }
            //我们现在要在潜在目标中进行筛选，以决定首先锁定哪个目标
            for (int k = 0; k < availableTargets.Count; k++)
            {
                if (availableTargets[k] != null)
                {
                    //float distanceFromTarget = playerManager.transform.position.x * availableTargets[k].transform.position.x +
                    //    playerManager.transform.position.y * availableTargets[k].transform.position.y + playerManager.transform.position.z * availableTargets[k].transform.position.z;
                    float distanceFromTarget = Vector3.Distance(playerManager.transform.position, availableTargets[k].transform.position);

                    if (distanceFromTarget < shortestDistance)
                    {
                        shortestDistance = distanceFromTarget;
                        NearestLockOnTarget = availableTargets[k];
                    }
                    //如果在搜索目标时我们已经处于锁定状态，就在当前目标的基础上搜索最近的左侧或右侧目标
                    if (playerManager.playerNetworkManager.isLockOn.Value)
                    {
                        Vector3 relativeEnemyPosition = playerManager.transform.InverseTransformDirection(availableTargets[k].transform.position);  //目标点指向玩家的向量

                        //Vector3 directionToTarget = availableTargets[k].transform.position - playerManager.transform.position;
                        //Vector3 relativeEnemyPosition = playerManager.transform.InverseTransformDirection(directionToTarget);


                        var distanceFromLeftTarget = relativeEnemyPosition.x;
                        var distanceFromRightTarget = relativeEnemyPosition.x;

                        if (availableTargets[k] == playerManager.playerCombatManager.currentTarget)
                            continue;

                        if (relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortestDistanceOfLeftTarget)
                        {
                            //Debug.Log("Switch to left target");
                            shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                            leftLockOnTarget = availableTargets[k];
                        }
                        else if (relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shorestDistanceOfRightTarget)
                        {
                            shorestDistanceOfRightTarget = distanceFromRightTarget;
                            rightLockOnTarget = availableTargets[k];
                        }
                    }
                }
                else
                {
                    ClearLockOnTargets();
                    playerManager.playerNetworkManager.isLockOn.Value = false;
                }
            }
        }


        public void SetLockCameraHeight()
        {
            if (cameraLockOnHeightCoroutine != null)
            {
                StopCoroutine(cameraLockOnHeightCoroutine);
            }
            cameraLockOnHeightCoroutine = StartCoroutine(SetCameraHeight());
        }

        /// <summary>
        /// 清除当前我们锁定的目标(只有一个)
        /// </summary>
        public void ClearLockOnTargets()
        {
            nearestLockOnTarget = null;
            leftLockOnTarget = null;
            rightLockOnTarget = null;
            availableTargets.Clear();
        }

        public IEnumerator WaitThenFindNewTarget()
        {
            while (playerManager.isPerformingAction)
            {
                yield return null;
            }

            ClearLockOnTargets();
            HandleLocatingLocalTargets();

            if (nearestLockOnTarget != null)
            {
                playerManager.playerCombatManager.SetTarget(nearestLockOnTarget);
                playerManager.playerNetworkManager.isLockOn.Value = true;
            }
            yield return null;
        }

        private IEnumerator SetCameraHeight()
        {
            float duration = 1;
            float timer = 0;

            //Vector3 velocity = Vector3.zero;
            Vector3 newLockedCameraHeight = new Vector3(cameraPivotTransform.transform.localPosition.x, lockedCameraHeight);
            Vector3 newUnlockCameraHeight = new Vector3(cameraPivotTransform.transform.localPosition.x, unlockedCameraHeight);

            while (timer < duration)
            {
                timer += Time.deltaTime;
                if (playerManager != null)
                {
                    if (playerManager.playerCombatManager.currentTarget != null)
                    {
                        cameraPivotTransform.transform.localPosition =
                            Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedCameraHeight, ref velocity, setCameraHeightSpeed);
                        cameraPivotTransform.transform.localRotation =
                            Quaternion.Slerp(cameraPivotTransform.transform.localRotation, Quaternion.Euler(0, 0, 0), setCameraHeightSpeed);
                    }
                    else
                    {
                        cameraPivotTransform.transform.localPosition =
                            Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockCameraHeight, ref velocity, setCameraHeightSpeed);
                    }
                }

                yield return null;
            }

            if (playerManager != null)
            {
                if (playerManager.playerCombatManager.currentTarget != null)
                {
                    cameraPivotTransform.transform.localPosition = newLockedCameraHeight;
                    cameraPivotTransform.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    cameraPivotTransform.transform.localPosition = newUnlockCameraHeight;
                }
            }

            yield return null;
        }
    }
}

