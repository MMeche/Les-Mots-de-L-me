#if UNITY_EDITOR
using Mono.Cecil;
#endif
using UnityEngine;

public class TutorialEnemyDataHolder : EnemyDataHolder
{
    
    [SerializeField] TutorialState _tutorialStatus;

    private void OnEnable()
    {
        if(_tutorialStatus.TutorialDefeated.Find(name => name== EnemyStatusData.CharName)== null)
        { 
            _tutorialStatus.TutorialDefeated.Add(EnemyStatusData.CharName);      
        }

        
    }
}
