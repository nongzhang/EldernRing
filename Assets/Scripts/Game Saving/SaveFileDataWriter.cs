using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SG
{
    public class SaveFileDataWriter
    {
        public string saveDataDirectoryPath = "";
        public string saveFileName = "";

        //在创建新存档前，我们必须检查是否有一个角色得存档已经存在(最多10个存档)
        public bool CheckToSeeIfFileExists()
        {
            if (File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DeleteSaveFile()
        {
            File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
        }

        public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
        {
            string  savePath = Path.Combine(saveDataDirectoryPath,saveFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log("Creating save file, at save path: "+savePath);

                string dataToStore = JsonUtility.ToJson(characterData,true);

                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter fileWriter = new StreamWriter(stream))
                    {
                        fileWriter.Write(dataToStore);
                    }
                }
            }
            catch (System.Exception ex)
            {

                Debug.LogError("Error whilst trying to save character data, game not saved" + savePath + "\n" + ex);
            }
        }

        public CharacterSaveData LoadSaveFile()
        {
            CharacterSaveData characterData = null;

            string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

            if (File.Exists(loadPath))
            {
                try
                {
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(loadPath,FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }
                    characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
                }
                catch (System.Exception ex)
                {
                    throw;
                }
            }

            return characterData;
        }
    }
}
