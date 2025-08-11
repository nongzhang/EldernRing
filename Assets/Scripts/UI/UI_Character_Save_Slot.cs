using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace NZ
{
    public class UI_Character_Save_Slot : MonoBehaviour
    {
        SaveFileDataWriter saveFileDataWriter;

        [Header("Game Slot")]
        public CharacterSlot characterSlot;

        [Header("Character Info")]
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI timePlayed;

        private void OnEnable()
        {
            LoadSaveSlots();
        }

        private void LoadSaveSlots()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            if (characterSlot == CharacterSlot.CharacterSlot_01)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.Instance.DecidedCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                //如果有存档文件，那么就会从文件中获取信息，否则禁用它，隐藏存档条
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot01.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_02)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.Instance.DecidedCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                //如果有存档文件，那么就会从文件中获取信息，否则禁用它，不会显示存档文件，只有图标
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot02.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_03)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.Instance.DecidedCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                //如果有存档文件，那么就会从文件中获取信息，否则禁用它，不会显示存档文件，只有图标
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot03.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_04)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.Instance.DecidedCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                //如果有存档文件，那么就会从文件中获取信息，否则禁用它，不会显示存档文件，只有图标
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot04.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_05)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.Instance.DecidedCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                //如果有存档文件，那么就会从文件中获取信息，否则禁用它，不会显示存档文件，只有图标
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot05.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_06)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.Instance.DecidedCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                //如果有存档文件，那么就会从文件中获取信息，否则禁用它，不会显示存档文件，只有图标
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot06.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_07)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.Instance.DecidedCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                //如果有存档文件，那么就会从文件中获取信息，否则禁用它，不会显示存档文件，只有图标
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot07.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_08)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.Instance.DecidedCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                //如果有存档文件，那么就会从文件中获取信息，否则禁用它，不会显示存档文件，只有图标
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot08.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_09)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.Instance.DecidedCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                //如果有存档文件，那么就会从文件中获取信息，否则禁用它，不会显示存档文件，只有图标
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot09.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_10)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.Instance.DecidedCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                //如果有存档文件，那么就会从文件中获取信息，否则禁用它，不会显示存档文件，只有图标
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot10.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }  

        }

        public void LoadGameFromCharacterSlot()
        {
            WorldSaveGameManager.Instance.currentCharacterSlotBeingUsed = characterSlot;
            WorldSaveGameManager.Instance.LoadGame();
        }

        public void SelectCurrentSlot()
        {
            TitleScreenManager.Instance.SelectCharacterSlot(characterSlot);
        }
    }
}
