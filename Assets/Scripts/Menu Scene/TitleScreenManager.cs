using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

namespace SG
{
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager Instance;

        [Header("Menus")]
        [SerializeField] GameObject titleScreenMainMenu;
        [SerializeField] GameObject titleScreenLoadMenu;

        [Header("Buttons")]
        [SerializeField] Button loadMenuReturnButton;
        [SerializeField] Button mainMenuLoadGameButton;
        [SerializeField] Button mainMenuNewGameButton;

        [Header("Pop Ups")]
        [SerializeField] GameObject noCharacterSlotsPopUp;
        [SerializeField] Button noCharacterSlotsOkeyButton;

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
        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        public void StartNewGame()
        {
            WorldSaveGameManager.instance.AttemptCreateNewGame();
            
        }

        public void OpenLoadGameMenu()
        {
            titleScreenMainMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);
            loadMenuReturnButton.Select();
        }

        public void CloseLoadGameMenu()
        {
            titleScreenLoadMenu.SetActive(false);
            titleScreenMainMenu.SetActive(true);
            mainMenuLoadGameButton.Select();
        }

        public void DisplayNoFreeCharacterSlotsPopUp()
        {
            noCharacterSlotsPopUp.SetActive(true);
            noCharacterSlotsOkeyButton.Select();
        }

        public void CloseNoFreeCharacterSlotsPopUp()
        {
            noCharacterSlotsPopUp.SetActive(false);
            mainMenuNewGameButton.Select();
        }
    }
}

