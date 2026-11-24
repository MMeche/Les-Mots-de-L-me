using UnityEngine;
using UnityEngine.EventSystems;

public class JournalGridSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        
        GameObject dropped = eventData.pointerDrag;
        JournalDraggableItem draggableItem = dropped.GetComponent<JournalDraggableItem>();
        if (transform.childCount == 1)
        {
            transform.GetChild(0).SetParent(draggableItem.parentAfterDrag);
        }
        draggableItem.parentAfterDrag = transform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
