using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ
{
    [System.Serializable]
    public class CharacterSaveData
    {
        [Header("SCENE INDEX")]
        public int sceneIndex = 1;

        [Header("Character Name")]
        public string characterName = "Character";

        [Header("Time Played")]
        public float secondPlayed;

        //使用json保存，所以不能用Vector3
        [Header("World Coordinates")]
        public float xPos;
        public float yPos;
        public float zPos;

        [Header("Resources")]
        public int currentHealth;
        public float currentStamina;

        [Header("Stats")]
        public int vitality;
        public int endurance;
    }
}
