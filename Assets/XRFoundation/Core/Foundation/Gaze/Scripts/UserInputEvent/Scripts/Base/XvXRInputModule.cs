using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;
using UnityEngine.EventSystems;
namespace XvXR.UI.Input
{ 


[RequireComponent(typeof(EventSystem))]
    [DisallowMultipleComponent]
public class XvXRInputModule : MixedRealityInputModule
    {

      //  

    private XvInputControllerBase[] inputControllerBases;

    protected override void Awake()
    {
        base.Awake();
        inputControllerBases = FindObjectsOfType<XvInputControllerBase>();

         EventSystem.current.sendNavigationEvents = false;
    }
    public override void Process()
    {
        base.Process();
        ProcessAllRaycast();
    }
    private void ProcessAllRaycast()
    {

        for (int i = 0; i < inputControllerBases.Length; i++)
        {
            if (!inputControllerBases[i].isActiveAndEnabled) {
                continue;
            }

            if (inputControllerBases[i].Is3DInput)
            {
                XvRaycaster customRaycaster = inputControllerBases[i].customRaycaster;
                RaycastResult result = customRaycaster.FirstRaycastResult();

                Process2DOr3DRaycast(inputControllerBases[i], result);


            }
            else
            {
                PointerEventData pointerEventData = new PointerEventData(eventSystem);
                pointerEventData.position = inputControllerBases[i].screenPosition;
                pointerEventData.button = PointerEventData.InputButton.Left;
                eventSystem.RaycastAll(pointerEventData, m_RaycastResultCache);
                RaycastResult result = FindFirstRaycast(m_RaycastResultCache);
                Process2DOr3DRaycast(inputControllerBases[i], result);
            }

        }

    }
    private void Process2DOr3DRaycast(XvInputControllerBase inputControllerBase, RaycastResult result)
    {
       
        var scrollDelta = inputControllerBase.GetScrollDelta();


        var customEventData = inputControllerBase.CustomEventData;
        if (customEventData == null) { return; }

        customEventData.Reset();
        customEventData.delta = Vector2.zero;
        customEventData.scrollDelta = scrollDelta;


        customEventData.pointerCurrentRaycast = result;
        customEventData.screenPositionDeltadelta = inputControllerBase.PositionDeltadelta.magnitude;

        customEventData.position = inputControllerBase.screenPosition;
       



        ProcessPress(customEventData);
        ProcessMove(customEventData);
        ProcessDrag(customEventData);

        if (result.isValid && !Mathf.Approximately(scrollDelta.sqrMagnitude, 0.0f))
        {
            var scrollHandler = ExecuteEvents.GetEventHandler<IScrollHandler>(result.gameObject);
            ExecuteEvents.ExecuteHierarchy(scrollHandler, customEventData, ExecuteEvents.scrollHandler);
        }
        if (eventSystem.sendNavigationEvents)
        {
            SendSubmitEventToSelectedObject();
        }
        SendUpdateEventToSelectedObject();
    }
  

    #region mouse down event
    protected void ProcessPress(CustomEventData eventData)
    {
        if (eventData.GetKeyPress())
        {
            if (!eventData.pressPrecessed)
            {
                ProcessPressDown(eventData);

            }
            // Debug.LogError(eventData.eligibleForClick);
        }
        else if (eventData.pressPrecessed)
        {

            ProcessPressUp(eventData);
        }
    }
    //protected void DeselectIfSelectionChanged(GameObject currentOverGo, BaseEventData pointerEvent)
    //{
    //    // Selection tracking
    //    var selectHandlerGO = ExecuteEvents.GetEventHandler<ISelectHandler>(currentOverGo);
    //    // if we have clicked something new, deselect the old thing
    //    // leave 'selection handling' up to the press event though.
    //    if (selectHandlerGO != eventSystem.currentSelectedGameObject)
    //        eventSystem.SetSelectedGameObject(null, pointerEvent);
    //}
    protected void ProcessPressDown(CustomEventData eventData)
    {
        var currentOverGo = eventData.pointerCurrentRaycast.gameObject;
        eventData.pressPrecessed = true;
        eventData.eligibleForClick = true;
        eventData.delta = Vector2.zero;
        eventData.dragging = false;
        eventData.useDragThreshold = true;
        eventData.pressPosition = eventData.position;
        eventData.pointerPressRaycast = eventData.pointerCurrentRaycast;
            DeselectIfSelectionChanged(currentOverGo, eventData);
        eventData.button = PointerEventData.InputButton.Left;
            var newPressed = ExecuteEvents.ExecuteHierarchy(currentOverGo, eventData, ExecuteEvents.pointerDownHandler);
            var newClick = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);
        if (newPressed == null)
        {
               
                newPressed = newClick;
        }
        var time = Time.unscaledTime;

        if (newPressed == eventData.lastPress)
        {
            if (time < (eventData.clickTime))
            {
                ++eventData.clickCount;
            }
            else
            {
                eventData.clickCount = 1;
            }
            eventData.clickTime = time;
        }
        else
        {
            eventData.clickCount = 1;
        }
            eventData.pointerPress = newPressed;
            eventData.pointerClick = newClick;

        if (newPressed != null)
        {
        }
        if (newClick != null)
        {
           // Debug.LogError("newClick==" + newClick.name);
        }
        eventData.rawPointerPress = currentOverGo;
        //
        eventData.clickTime = time;

        eventData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(currentOverGo);

        if (eventData.pointerDrag != null)
        {
            ExecuteEvents.Execute(eventData.pointerDrag, eventData, ExecuteEvents.initializePotentialDrag);
        }
    }

    protected void ProcessPressUp(CustomEventData eventData)
    {

        var currentOverGo = eventData.pointerCurrentRaycast.gameObject;



        if (eventData.pointerPress != null)
        {
            ExecuteEvents.Execute(eventData.pointerPress, eventData, ExecuteEvents.pointerUpHandler);

        }
        var pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

        if (eventData.pointerClick != null && eventData.pointerClick == pointerUpHandler && eventData.eligibleForClick)
        {
           ExecuteEvents.Execute(eventData.pointerClick, eventData, ExecuteEvents.pointerClickHandler);
        }
        else if (eventData.pointerDrag != null && eventData.dragging)
        {
            ExecuteEvents.ExecuteHierarchy(currentOverGo, eventData, ExecuteEvents.dropHandler);
        }
        eventData.pressPrecessed = false;
        eventData.eligibleForClick = false;
        eventData.pointerPress = null;
        eventData.rawPointerPress = null;

        if (eventData.pointerDrag != null && eventData.dragging)
        {
            ExecuteEvents.Execute(eventData.pointerDrag, eventData, ExecuteEvents.endDragHandler);
        }

        eventData.dragging = false;
        eventData.pointerDrag = null;

        if (currentOverGo != eventData.pointerEnter)
        {
            HandlePointerExitAndEnter(eventData, null);
            HandlePointerExitAndEnter(eventData, currentOverGo);
        }
    }

    #endregion
    #region dual move event
    new protected void ProcessMove(PointerEventData eventData)
    {
        var hoverGO = eventData.pointerCurrentRaycast.gameObject;
        if (eventData.pointerEnter != hoverGO)
        {
            HandlePointerExitAndEnter(eventData, hoverGO);
        }

    }
    #endregion

    #region dual drag event
    protected bool ShouldStartDrag(CustomEventData eventData)
    {

        bool isDrag = (eventData.screenPositionDeltadelta > 5);

      //  Debug.LogError("isDrag  ==" + isDrag + "  " + eventData.screenPositionDeltadelta);
        return isDrag;
    }

    protected void ProcessDrag(CustomEventData eventData)
    {
        eventData.button = PointerEventData.InputButton.Left;
        if (eventData.pointerDrag != null && !eventData.dragging && ShouldStartDrag(eventData))
        {
            ExecuteEvents.Execute(eventData.pointerDrag, eventData, ExecuteEvents.beginDragHandler);
            eventData.dragging = true;


        }




        if (eventData.dragging && eventData.pointerDrag != null)
        {
            if (eventData.pointerPress != eventData.pointerDrag)
            {
                ExecuteEvents.Execute(eventData.pointerPress, eventData, ExecuteEvents.pointerUpHandler);

                eventData.eligibleForClick = false;
                eventData.pointerPress = null;
                eventData.rawPointerPress = null;
            }
            ExecuteEvents.Execute(eventData.pointerDrag, eventData, ExecuteEvents.dragHandler);

        }
    }

    #endregion

    new protected bool SendSubmitEventToSelectedObject()
    {
        if (eventSystem.currentSelectedGameObject == null)
            return false;

        var data = GetBaseEventData();
        if (input.GetButtonDown("Submit"))
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.submitHandler);

        if (input.GetButtonDown("Cancel"))
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.cancelHandler);
        return data.used;
    }
    new protected bool SendUpdateEventToSelectedObject()
    {
        if (eventSystem.currentSelectedGameObject == null)
            return false;

        var data = GetBaseEventData();
        ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.updateSelectedHandler);
        return data.used;
    }
}
}

