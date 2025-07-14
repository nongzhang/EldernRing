using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ
{
    public class WorldGameSessionManager : MonoBehaviour
    {
        public static WorldGameSessionManager Instance;
        [Header("Active Player In Session")]
        public List<PlayerManager> playerManagers = new List<PlayerManager>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void AddPlayerToActivePlayersList(PlayerManager player)
        {
            if (!playerManagers.Contains(player))
            {
                playerManagers.Add(player);
            }

            for (int i = playerManagers.Count - 1; i > -1; i--)
            {
                if (playerManagers[i] == null)
                {
                    playerManagers.RemoveAt(i);
                }
            }
        }

        public void RemovePlayerFromActivePlayersList(PlayerManager player)
        {
            if (playerManagers.Contains(player))
            {
                playerManagers.Remove(player);
            }

            for (int i = playerManagers.Count - 1; i > -1; i--)
            {
                if (playerManagers[i] == null)
                {
                    playerManagers.RemoveAt(i);
                }
            }
        }
    }
}
