using SG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    [HideInInspector]public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector]public PlayerLocomotionManager playerLocomotionManager;
    protected override void Awake()
    {
        base.Awake();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
    }

    protected override void Update()
    {
        base.Update();
        if(!IsOwner)
        {
            return;
        }
        playerLocomotionManager.HandleAllMovement();
    }

    protected override void LateUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
        base.LateUpdate();
        PlayerCamera.instance.HandleAllCameraAction();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if(IsOwner)
        {
            PlayerCamera.instance.playerManager = this;
            PlayerInputManager.instance.playerManager = this;
        }
    }
}
