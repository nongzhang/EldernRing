using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SG
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
                saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecidedCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                //如果有存档文件，那么就会从文件中获取信息，否则禁用它，不会显示存档文件，只有图标
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot01.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_02)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecidedCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                //如果有存档文件，那么就会从文件中获取信息，否则禁用它，不会显示存档文件，只有图标
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot02.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_03)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecidedCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                //如果有存档文件，那么就会从文件中获取信息，否则禁用它，不会显示存档文件，只有图标
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot02.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_04)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecidedCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                //如果有存档文件，那么就会从文件中获取信息，否则禁用它，不会显示存档文件，只有图标
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot04.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_05)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecidedCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                //如果有存档文件，那么就会从文件中获取信息，否则禁用它，不会显示存档文件，只有图标
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot05.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_06)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecidedCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                //如果有存档文件，那么就会从文件中获取信息，否则禁用它，不会显示存档文件，只有图标
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot06.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_07)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecidedCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                //如果有存档文件，那么就会从文件中获取信息，否则禁用它，不会显示存档文件，只有图标
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot07.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_08)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecidedCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                //如果有存档文件，那么就会从文件中获取信息，否则禁用它，不会显示存档文件，只有图标
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot08.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_09)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecidedCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                //如果有存档文件，那么就会从文件中获取信息，否则禁用它，不会显示存档文件，只有图标
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot09.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_010)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecidedCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                //如果有存档文件，那么就会从文件中获取信息，否则禁用它，不会显示存档文件，只有图标
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot10.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }  

        }

        public void LoadGameFromCharacterSlot()
        {
            WorldSaveGameManager.instance.currentCharacterSlotBeingUsed = characterSlot;
            WorldSaveGameManager.instance.LoadGame();
        }
    }
}
