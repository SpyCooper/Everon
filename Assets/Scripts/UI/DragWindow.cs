using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragWindow : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler
{
    // allows a game object (mainly a UI object) to be dragged around the screen

    private RectTransform dragRectTransform;
    private Canvas canvas;

    // on Awake
    private void Awake()
    {
        // sets the dragRectTransform
        if(dragRectTransform == null)
        {
            dragRectTransform = transform.parent.GetComponent<RectTransform>();
        }
        
        // sets the canvas
        if(canvas == null)
        {
            // loops upwards until the Canvas is found
            Transform testCanvasTransform = transform.parent;
            while(testCanvasTransform != null)
            {
                canvas = testCanvasTransform.GetComponent<Canvas>();
                if(canvas != null)
                {
                    break;
                }
                testCanvasTransform = testCanvasTransform.parent;
            }
        }
    }

    // used when dragging begins
    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    // used during dragging
    public void OnDrag(PointerEventData eventData)
    {
        // the window follows the mouse
        dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    
    // used when dragging is over
    public void OnEndDrag(PointerEventData eventData)
    {
    }

    // when clicked the window is shown in the top
    public void OnPointerDown(PointerEventData eventData)
    {
        dragRectTransform.SetAsLastSibling();
    }
    
}
