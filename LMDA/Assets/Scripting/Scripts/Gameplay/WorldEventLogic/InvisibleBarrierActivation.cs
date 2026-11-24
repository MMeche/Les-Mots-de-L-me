using UnityEngine;

public class InvisibleBarrierActivation : MonoBehaviour
{
    [SerializeField] TutorialState _tutorialState;
    CantGoBackDoor _door;
    bool _triggered=false;

    private void OnEnable()
    {
        _door = transform.parent.GetComponent<CantGoBackDoor>();
        _triggered = _tutorialState.InvisibleWall;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_triggered)
        {
            _triggered = true;
            _tutorialState.InvisibleWall = true;
            _door.CloseDoor();
        }
    }
}
