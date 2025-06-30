using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    [System.Serializable]
    public class CharacterSaveData
    {
        [Header("Character Name")]
        public string characterName = "Character";

        [Header("Time Played")]
        public float secondPlayed;

        //ʹ��json���棬���Բ�����Vector3
        [Header("World Coordinates")]
        public float xPos;
        public float yPos;
        public float zPos;
    }
}
