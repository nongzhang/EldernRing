using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace NZ
{
    public class AICharacterCombatManager : CharacterCombatManager
    {
        [Header("Detection")]
        [SerializeField] float detectionRadius = 15;
        [SerializeField] float minimumDetectionAngle = -35;
        [SerializeField] float maximumDetectionAngle = 35;

        public void FindATargetViaLineOdSight(AICharacterManager aICharacterManager)
        {
            if (currentTarget != null)
                return;

            Collider[] colliders = Physics.OverlapSphere(aICharacterManager.transform.position, detectionRadius, WorldUtilityManager.Instance.GetCharacterLayers());

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

                if (targetCharacter == null)
                    continue;

                if (targetCharacter == aICharacterManager)
                    continue;

                if (targetCharacter.isDead.Value)
                    continue;

                if (WorldUtilityManager.Instance.CanIDamageThisTarget(aICharacterManager.characterGroup, targetCharacter.characterGroup))
                {
                    Vector3 targetDirection = targetCharacter.transform.position - aICharacterManager.transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, aICharacterManager.transform.forward);

                    if (viewableAngle > minimumDetectionAngle && viewableAngle < maximumDetectionAngle)
                    {
                        if (Physics.Linecast(aICharacterManager.characterCombatManager.LockOnTransform.position, targetCharacter.characterCombatManager.LockOnTransform.position, 
                            WorldUtilityManager.Instance.GetEnviroLayers()))
                        {
                            Debug.DrawLine(aICharacterManager.characterCombatManager.LockOnTransform.position, targetCharacter.characterCombatManager.LockOnTransform.position);
                            Debug.Log("±»µ²×¡ÁË");
                        }
                        else
                        {
                            aICharacterManager.characterCombatManager.SetTarget(targetCharacter);
                        }
                    }
                }
            }
        }
    }
}
