using SG;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    PlayerControls playerControls;
    [SerializeField] Vector2 movementInput;

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

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();   //Lambda±Ì¥Ô Ω
            //playerControls.PlayerMovement.Movement.performed += OnMovementPerformed;
        }
        playerControls.Enable();
        //SceneManager.activeSceneChanged += OnSceneChange;
    }

    //private void OnMovementPerformed(InputAction.CallbackContext context)
    //{
    //    movementInput = context.ReadValue<Vector2>();
    //}

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += OnSceneChange;
        instance.enabled = false;
    }

    //private void OnDisable()
    //{
    //    SceneManager.activeSceneChanged -= OnSceneChange;
    //}

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }
}
