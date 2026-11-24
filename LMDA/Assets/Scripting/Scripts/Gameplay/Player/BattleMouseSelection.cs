using UnityEngine;

public class BattleMouseSelection : MonoBehaviour
{
    BattleSelectionActions _selectionController;
    int _targetAnchor;
    void Start()
    {
        _selectionController = FindFirstObjectByType<BattleSelectionActions>();
        _targetAnchor = transform.GetSiblingIndex();
    }

    void OnMouseEnter()
    {
        if (_selectionController.MouseSelection) _selectionController.Move(_targetAnchor);
    }

    void OnMouseDown()
    {
        if(_selectionController.MouseSelection)_selectionController.Chose(_targetAnchor);
    }
}
