using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;
using UnityEngine.EventSystems;
namespace XvXR.UI.Input
{ 

/// <summary>
///�����Զ������ߵ�����
/// </summary>
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
        base.Process();//�����Դ��¼�

        ProcessAllRaycast();
    }
    private void ProcessAllRaycast()
    {

           ///�������е�����ģ��
        for (int i = 0; i < inputControllerBases.Length; i++)
        {
                //�����ǰģ��û�м�������ǲ������򲻴���
            if (!inputControllerBases[i].isActiveAndEnabled) {
                continue;
            }

            if (inputControllerBases[i].Is3DInput)
            {
                    //�����3D���������룬�Ǿ���Ҫ��ȡ���ߴ���������UIԪ��
                XvRaycaster customRaycaster = inputControllerBases[i].customRaycaster;
                RaycastResult result = customRaycaster.FirstRaycastResult();//��ȡ��һ��Ԫ��

                Process2DOr3DRaycast(inputControllerBases[i], result);//����ǰUIԪ�ص��߼�


            }
            else
            {
                    //�������Ļ�ϵĵ㣬����Ҫ��ȡ
                PointerEventData pointerEventData = new PointerEventData(eventSystem);
                  //������Ҫ��ȡ�Զ������Ļ�����
                pointerEventData.position = inputControllerBases[i].screenPosition;
                    //���ð�ťΪLeftButton
                pointerEventData.button = PointerEventData.InputButton.Left;

                    //ͨ��EventSystem��ȡ���ߴ�����������
                eventSystem.RaycastAll(pointerEventData, m_RaycastResultCache);
                RaycastResult result = FindFirstRaycast(m_RaycastResultCache);
                Process2DOr3DRaycast(inputControllerBases[i], result);
            }

        }

        // }
    }
    private void Process2DOr3DRaycast(XvInputControllerBase inputControllerBase, RaycastResult result)
    {
       
        ///��ȡ������
        var scrollDelta = inputControllerBase.GetScrollDelta();


        var customEventData = inputControllerBase.CustomEventData;
        if (customEventData == null) { return; }

        customEventData.Reset();
        customEventData.delta = Vector2.zero;
        customEventData.scrollDelta = scrollDelta;//��������ֵ


        customEventData.pointerCurrentRaycast = result;//���õ�ǰ������Ϣ
            //��֮֡����ƶ�λ��
        customEventData.screenPositionDeltadelta = inputControllerBase.PositionDeltadelta.magnitude;

        //customEventData.position = inputControllerBase.CustomEventData.position;
        customEventData.position = inputControllerBase.screenPosition;
       



        ProcessPress(customEventData);
        ProcessMove(customEventData);
        ProcessDrag(customEventData);

        // �����¼�
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
  

    #region ��������̧���¼�
    protected void ProcessPress(CustomEventData eventData)
    {
        if (eventData.GetKeyPress())
        {
            //�ж��Ƿ��ǰ���״̬
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
        //��ǰѡ�е�����
        var currentOverGo = eventData.pointerCurrentRaycast.gameObject;
        //����״̬Ϊ����״̬
        eventData.pressPrecessed = true;

        //���õ��״̬
        eventData.eligibleForClick = true;
        //ָ������仯
        eventData.delta = Vector2.zero;
        //������ק״̬
        eventData.dragging = false;
        //�Ƿ�Ӧ��ʹ��������ֵ��
        eventData.useDragThreshold = true;
        //�������λ��
        eventData.pressPosition = eventData.position;
        //���õ�ǰ����ʱ�������
        eventData.pointerPressRaycast = eventData.pointerCurrentRaycast;
            //�����ǰָ���GameObject��ͬ����ȡ��ѡ��ǰѡ����GameObject��
            DeselectIfSelectionChanged(currentOverGo, eventData);
        eventData.button = PointerEventData.InputButton.Left;

            //�ҵ���������¼������岢ִ���¼�������IPointerDownHandler�¼����¼��̳���������ֱ�ӷ��Ͷ�Ӧ�¼�
            var newPressed = ExecuteEvents.ExecuteHierarchy(currentOverGo, eventData, ExecuteEvents.pointerDownHandler);

            //�ҵ�IPointerClickHandler�¼����յ����壬����IPointerClickHandler�¼���Ҫ�����̧���ʱ���ж��Ƿ���ͬһ�����壬����������ǻ�ȡ����ִ���¼�
            var newClick = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);
        // 
        if (newPressed == null)
        {
               
                newPressed = newClick;
        }
        //���û����������ʱ�䣬���ʱ���Time.timeʱ����ͬ
        var time = Time.unscaledTime;

        if (newPressed == eventData.lastPress)
        {
            //���õ��������
            if (time < (eventData.clickTime))
            {
                ++eventData.clickCount;
            }
            else
            {
                eventData.clickCount = 1;
            }
            //���õ��ʱ��
            eventData.clickTime = time;
        }
        else
        {
            eventData.clickCount = 1;
        }
            //�ܹ�����pointerDownHandler�¼�������
            eventData.pointerPress = newPressed;
            //�ܹ�����IPointerClickHandler�¼������壬�洢�����������̧���ʱ���ж��Ƿ���Ҫִ�е���ʱ���¼�
            eventData.pointerClick = newClick;

        if (newPressed != null)
        {
            //Debug.LogError("newPressed==" + newPressed.name);
        }
        if (newClick != null)
        {
           // Debug.LogError("newClick==" + newClick.name);
        }
        //ԭʼָ��ѡ�е�����
        eventData.rawPointerPress = currentOverGo;
        //
        eventData.clickTime = time;

        // ��ȡ�ܹ�������ק������
        eventData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(currentOverGo);

        if (eventData.pointerDrag != null)
        {
            //��ʼ����ק�¼�
            ExecuteEvents.Execute(eventData.pointerDrag, eventData, ExecuteEvents.initializePotentialDrag);
        }
    }

    protected void ProcessPressUp(CustomEventData eventData)
    {

        //��ȡ����ǰ����
        var currentOverGo = eventData.pointerCurrentRaycast.gameObject;



        if (eventData.pointerPress != null)
        {
            //Ϊ���µ�����ִ�����̧���¼�
            ExecuteEvents.Execute(eventData.pointerPress, eventData, ExecuteEvents.pointerUpHandler);

        }
        //���������Ƿ�����Ƶ�������ͬһԪ���ϡ�����
        var pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

        // ִ�а�ť����¼�
        if (eventData.pointerClick != null && eventData.pointerClick == pointerUpHandler && eventData.eligibleForClick)
        {
           ExecuteEvents.Execute(eventData.pointerClick, eventData, ExecuteEvents.pointerClickHandler);
        }
        else if (eventData.pointerDrag != null && eventData.dragging)
        {
            //�����ǰ���̧���ʱ����ͬһ��Ԫ�ز�������ק״̬��˵����drop�¼�
            ExecuteEvents.ExecuteHierarchy(currentOverGo, eventData, ExecuteEvents.dropHandler);
        }
        //���ð���״̬Ϊfalse
        eventData.pressPrecessed = false;

        eventData.eligibleForClick = false;
        //��갴��ʱ���Ԫ����Ϊ��
        eventData.pointerPress = null;
        eventData.rawPointerPress = null;

        //�����ǰ���϶������壬��������ק״̬�������϶��¼�
        if (eventData.pointerDrag != null && eventData.dragging)
        {
            ExecuteEvents.Execute(eventData.pointerDrag, eventData, ExecuteEvents.endDragHandler);
        }

        eventData.dragging = false;
        eventData.pointerDrag = null;

        //����ָ�����/�˳���ˢ��״̬��
        //������������ǽ�����Ƶ���ǰ���ڰ�������������������Ķ����ϣ������ھͻ�õ�����
        if (currentOverGo != eventData.pointerEnter)
        {
            HandlePointerExitAndEnter(eventData, null);
            HandlePointerExitAndEnter(eventData, currentOverGo);
        }
    }

    #endregion
    #region �����ƶ��¼�
    new protected void ProcessMove(PointerEventData eventData)
    {
        var hoverGO = eventData.pointerCurrentRaycast.gameObject;
        if (eventData.pointerEnter != hoverGO)
        {
            HandlePointerExitAndEnter(eventData, hoverGO);
        }

    }
    #endregion

    #region �����϶��¼�
    protected bool ShouldStartDrag(CustomEventData eventData)
    {

        bool isDrag = (eventData.screenPositionDeltadelta > 5);

      //  Debug.LogError("isDrag  ==" + isDrag + "  " + eventData.screenPositionDeltadelta);
        return isDrag;
    }

    protected void ProcessDrag(CustomEventData eventData)
    {
        eventData.button = PointerEventData.InputButton.Left;
        //Debug.LogError("�϶�:" + eventData.pointerDrag != null + " " + !eventData.dragging + "  " + ShouldStartDrag(eventData));
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
            //Debug.LogError(eventData.pointerDrag.name);

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

