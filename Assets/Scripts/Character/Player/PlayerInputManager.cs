using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace NZ
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;
        public PlayerManager playerManager;
        PlayerControls playerControls;

        [Header("CAMERA MOVEMENT INPUT")]
        [SerializeField] Vector2 cameraInput;
        public float cameraHorizontalInput;
        public float cameraVerticalInput;

        [Header("LOCK ON INPUT")]
        [SerializeField] bool lockOnInput;
        [SerializeField] bool lockOn_LeftInput;
        [SerializeField] bool lockOn_RightInput;
        [SerializeField] Vector2 lockOn_MouseInput;
        private Coroutine lockOnCoroutine;
        private Vector2 lastMousePosition = Vector2.zero;
        private bool isMouseMovedLeft = false;


        [Header("PLAYER MOVEMENT INPUT")]
        [SerializeField] Vector2 movementInput;
        public float horizontalInput;
        public float verticalInput;
        public float moveAmount;

        [Header("PLAYER ACTION INPUT")]
        [SerializeField] bool dodgeInput = false;
        [SerializeField] bool sprintInput = false;
        [SerializeField] bool jumpInput = false;  

        [Header("BUMPER INPUTS")]
        [SerializeField] bool RB_Input = false;           //�ֱ��Ҽ��������Ҽ�

        [Header("TRIGGER INPUTS")]
        [SerializeField] bool RT_Input = false;
        [SerializeField] bool Hold_RT_Input = false;

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
            //SceneManager.activeSceneChanged += OnSceneChange;   
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.activeSceneChanged += OnSceneChange;    //������¼�����Start��������Ϊ���ս�����Ϸʱ���ڲ˵����棬�Ҳ�������Input��Ч��ֻ�е����糡����ʱ���������������Ч
            instance.enabled = false;
            if (playerControls != null)
            {
                playerControls.Disable();
            }
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
            {
                instance.enabled = true;

                if (playerControls != null)
                {
                    playerControls.Enable();
                }
            }
            else
            {
                instance.enabled = false;
                if (playerControls != null)
                {
                    playerControls.Disable();
                }
            }
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();
                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();   //Lambda���ʽ
                                                                                                                   //playerControls.PlayerMovement.Movement.performed += OnMovementPerformed;
                //playerControls.PlayerCamera.Mouse.performed += i => cameraInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
                playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
                playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
                playerControls.PlayerActions.RB.performed += i => RB_Input = true;

                //Lock on
                playerControls.PlayerActions.LockOn.performed += i => lockOnInput = true;
                playerControls.PlayerActions.SeekLeftLockOnTarget.performed += i => lockOn_LeftInput = true;
                playerControls.PlayerActions.SeekRightLockOnTarget.performed += i => lockOn_RightInput = true;
                playerControls.PlayerActions.SeekLockTargetByMouse.performed += i => lockOn_MouseInput = i.ReadValue<Vector2>();
                playerControls.PlayerActions.SeekLockTargetByMouse.performed += OnMouseMove;

                //��ס���룬��sprintInput��Ϊtrue
                playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
                //�ɿ�(�ͷ�)���룬��sprintInput��Ϊfalse
                playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
            }
            playerControls.Enable();
            //SceneManager.activeSceneChanged += OnSceneChange;
        }

        //private void OnMovementPerformed(InputAction.CallbackContext context)
        //{
        //    movementInput = context.ReadValue<Vector2>();
        //}

        

        //private void OnDisable()
        //{
        //    SceneManager.activeSceneChanged -= OnSceneChange;
        //}

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if (focus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }
        }

        private void Update()
        {
            HandleAllInput(); 
        }

        private void HandleAllInput()
        {
            HandleLockOnInput();
            HandleLockOnSwitchTargetInput();
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
            HandleDodgeInput();
            HandleSprintInput();
            HandleJumpInput();
            HandRBInput();
        }

        private void HandleLockOnInput()
        {
            
            if (playerManager.playerNetworkManager.isLockOn.Value)     //playerManager.playerNetworkManager.isLockOn.Value
            {
                //lockOnInput = false;
                if (playerManager.playerCombatManager.currentTarget == null)
                {
                    return;
                }
                //Ŀ���Ƿ��Ѿ�����
                if (playerManager.playerCombatManager.currentTarget.isDead.Value)
                {
                    playerManager.playerNetworkManager.isLockOn.Value = false;

                    //����Ѱ���µ�Ŀ��
                    //ȷ����ͬһʱ�̲������ж��Э��
                    if (lockOnCoroutine != null)
                        StopCoroutine(lockOnCoroutine);
                    lockOnCoroutine = StartCoroutine(PlayerCamera.instance.WaitThenFindNewTarget());
                }

                
            }
            

            //���������״̬�������ٰ����������ǽ���
            if (lockOnInput && playerManager.playerNetworkManager.isLockOn.Value)
            {
                lockOnInput = false;
                PlayerCamera.instance.ClearLockOnTargets();
                playerManager.playerNetworkManager.isLockOn.Value = false;
                return;
            }

            if (lockOnInput && !playerManager.playerNetworkManager.isLockOn.Value)
            {
                lockOnInput = false;

                PlayerCamera.instance.HandleLocatingLocalTargets();
                if (PlayerCamera.instance.NearestLockOnTarget != null)
                {
                    playerManager.playerCombatManager.SetTarget(PlayerCamera.instance.NearestLockOnTarget);
                    playerManager.playerNetworkManager.isLockOn.Value = true;
                }
            }
        }

        private void HandleLockOnSwitchTargetInput()
        {
            if (lockOn_LeftInput)
            {
                lockOn_LeftInput = false;
                if (playerManager.playerNetworkManager.isLockOn.Value)
                {
                    PlayerCamera.instance.HandleLocatingLocalTargets();

                    if (PlayerCamera.instance.LeftLockOnTarget != null)
                    {
                        playerManager.playerCombatManager.SetTarget(PlayerCamera.instance.LeftLockOnTarget);
                    }
                }
            }

            if (lockOn_RightInput)  // || lockOn_MouseInput.x > 20
            {
                lockOn_RightInput = false;
                if (playerManager.playerNetworkManager.isLockOn.Value)
                {
                    PlayerCamera.instance.HandleLocatingLocalTargets();

                    if (PlayerCamera.instance.RightLockOnTarget != null)
                    {
                        playerManager.playerCombatManager.SetTarget(PlayerCamera.instance.RightLockOnTarget);
                    }
                }
            }
        }

        void OnMouseMove(InputAction.CallbackContext context)
        {
            Vector2 currentMousePosition = context.ReadValue<Vector2>();

            // �ж�����Ƿ������ƶ�
            if (currentMousePosition.x < lastMousePosition.x)
            {
                isMouseMovedLeft = true;
            }
            else
            {
                isMouseMovedLeft = false;
            }

            // �����������λ��
            lastMousePosition = currentMousePosition;

            // �������ֵ
            Debug.Log($"Mouse moved left: {isMouseMovedLeft}");
        }

        private void HandlePlayerMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));
            if (moveAmount <= 0.5 && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1;
            }
            if(playerManager == null)
            {
                return;
            }

            if (!playerManager.playerNetworkManager.isLockOn.Value || playerManager.playerNetworkManager.isSprinting.Value)
            {
                playerManager.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, playerManager.playerNetworkManager.isSprinting.Value);
            }
            else
            {
                playerManager.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput, playerManager.playerNetworkManager.isSprinting.Value);
            }
            
        }

        private void HandleCameraMovementInput()
        {
            cameraHorizontalInput = cameraInput.x;
            cameraVerticalInput = cameraInput.y;
        }

        private void HandleDodgeInput()
        {
            if(dodgeInput)
            {
                dodgeInput = false;
                playerManager.playerLocomotionManager.AttemptToPerformDodge();
            }
        }

        private void HandleSprintInput()
        {
            if (sprintInput)
            {
                playerManager.playerLocomotionManager.HandleSprinting();
            }
            else
            {
                playerManager.playerNetworkManager.isSprinting.Value = false;
            }
        }

        private void HandleJumpInput()
        {
            if (jumpInput)
            {
                jumpInput = false;

                //���UI�˵��򿪣���ôʲô��Ҫ����ֱ�ӷ��ء�����ʹ���ֱ�ӳ�� SouthButton Gamepadͬʱ������Ծ�Ͳ˵�����ʱ��Xbox��A,PS��X

                playerManager.playerLocomotionManager.AttemptToPerformJump();
            }
        }

        private void HandRBInput()
        {
            if (RB_Input)
            {
                RB_Input = false;

                playerManager.playerNetworkManager.SetCharacterActionHand(true);

                playerManager.playerCombatManager.PerformWeaponBasedAction(playerManager.playerInventoryManager.currentRightHandWeapon.oneHandRB_Action, playerManager.playerInventoryManager.currentRightHandWeapon);
            }
        }

    }
}


