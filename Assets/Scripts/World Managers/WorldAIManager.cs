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
            //���ý�ɫ��Ϸ���� ��������ͬ������״̬
            //���ֻ��������Լ�λ������Ķ���ͨ������ɫ����Ϊ���򣬲��ڲ���Ҫʱ����Զ����ҵĽ�ɫ
        }
    }
}
