using UnityEngine;

public class FirelightsAnimatorManager : MonoBehaviour
{
    [SerializeField] TutorialState _tutorialStatus;
    [SerializeField] string _name;

    private void OnEnable()
    {
        if(_tutorialStatus.TutorialDefeated.Find(name => name == _name) == _name)
        {
            GetComponentInChildren<ParticleSystem>().Play();
        }
    }
}
