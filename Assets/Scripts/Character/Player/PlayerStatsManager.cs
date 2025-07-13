using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        PlayerManager playerManager;

        protected override void Awake()
        {
            base.Awake();
            playerManager = GetComponent<PlayerManager>();
        }

        protected override void Start()
        {
            base.Start();
            CalculateHealthBasedOnVitalityLevel(playerManager.characterNetworkManager.vitality.Value);
            CalculateStaminaBasedOnEnduranceLevel(playerManager.characterNetworkManager.endurance.Value);
        }

    }
}
