using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using NZ.Utility;

namespace NZ
{
    public class WorldAIManager : Singleton<WorldAIManager>
    {
        //public static WorldAIManager instance;
        //private IEnumerator loadSceneCoroutine;

        [Header("DEBUG")]
        [SerializeField] bool despawnCharacters = false;
        [SerializeField] bool respawnCharacters = false;

        [Header("Characters")]
        [SerializeField] GameObject[] aiCharacters;
        [SerializeField] List<GameObject> spawnedAICharacters;

        //private void Awake()
        //{
        //    if (instance == null)
        //    {
        //        instance = this;
        //    }
        //    else
        //    {
        //        Destroy(gameObject);
        //    }
        //}
        public override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                StartCoroutine(WaitForSceneToLoadThenSpawnCharacters());
            }
        }

        private void Update()
        {
            if (respawnCharacters)
            {
                respawnCharacters = false;
                SpawnAllCharacters();
            }

            if (despawnCharacters)
            {
                despawnCharacters = false;
                DespawnAllCharacters();
            }
        }

        private IEnumerator WaitForSceneToLoadThenSpawnCharacters()
        {
            while (!SceneManager.GetActiveScene().isLoaded)
            {
                yield return null;
            }
            SpawnAllCharacters();
        }

        private void SpawnAllCharacters()
        {
            foreach (var character in aiCharacters)
            {
                GameObject instantiatedCharacter = Instantiate(character);
                instantiatedCharacter.GetComponent<NetworkObject>().Spawn();
                spawnedAICharacters.Add(instantiatedCharacter);
            }
        }

        private void DespawnAllCharacters()
        {
            foreach(var character in spawnedAICharacters)
            {
                character.GetComponent<NetworkObject>().Despawn();
            }
        }

        private void DisableAllCharacters()
        {
            //禁用角色游戏对象， 在网络上同步禁用状态
            //玩家只需关心与自己位置相近的对象。通过将角色划分为区域，并在不需要时禁用远离玩家的角色
        }
    }
}
