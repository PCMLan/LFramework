using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum UIEventType
{
    PointerClick,
    PointerDown,
    PointerEnter,
    PointerExit,
    PointerUp,
    BeginDrag,
    Drag,
    EndDrag,
    Press,
    Scroll,
    Select,
    Deselect,
    UpdateSelected,
    Submit,
    Drop,
    InitializePotentialDrag,
    Cancel,
}


// 目前实现了UnityEngine.EventSystems内所有接口
public class UIEventListener :
    MonoBehaviour,
    IDisposable,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerClickHandler,
    IInitializePotentialDragHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IDropHandler,
    IScrollHandler,
    IUpdateSelectedHandler,
    ISelectHandler,
    IDeselectHandler,
    IMoveHandler,
    ISubmitHandler,
    ICancelHandler
{

    private class UIEvent : UnityEvent<PointerEventData> { }

    private Dictionary<UIEventType, UIEvent> m_EventDic = new Dictionary<UIEventType, UIEvent>();

    private PointerEventData m_PointerEventData;

    private bool m_IsDown = false;

    private bool m_IsPress = false;

    private bool m_IsDrag = false;

    private float m_DownTime = 0f;

    private float m_PressDurationTime = 1f;

    private int m_ValidDragPointreId = -1;


    public static UIEventListener GetOrAddListener(GameObject go)
    {
        UIEventListener listener = null;
        if (go != null)
        {
            listener = go.GetComponent<UIEventListener>();
            if (listener == null)
            {
                listener = go.AddComponent<UIEventListener>();
            }
        }
        return listener;
    }

    public void AddListener(UIEventType eventType, UnityAction<PointerEventData> listener)
    {
        if (!m_EventDic.ContainsKey(eventType))
        {
            m_EventDic[eventType] = new UIEvent();
        }
        m_EventDic[eventType].AddListener(listener);
    }

    public void RemoveListener(UIEventType eventType, UnityAction<PointerEventData> listener)
    {
        if (m_EventDic.TryGetValue(eventType, out UIEvent uiEvent))
        {
            uiEvent.RemoveListener(listener);
        }
    }

    public void RemoveAllListener()
    {
        foreach (var v in m_EventDic.Values)
        {
            v.RemoveAllListeners();
            v.Invoke(null);
        }
        m_EventDic.Clear();
    }

    private void TryCall(UIEventType eventType, PointerEventData eventData)
    {
        if (m_EventDic.TryGetValue(eventType, out UIEvent uiEvent))
        {
            uiEvent.Invoke(eventData);
        }
    }

    void Update()
    {
        if (!m_IsDown)
        {
            return;
        }

        m_DownTime += Time.unscaledDeltaTime;
        if (m_DownTime < m_PressDurationTime)
        {
            return;
        }

        m_IsPress = true;
        TryCall(UIEventType.Press, m_PointerEventData);
        m_IsDown = false;
        m_PointerEventData = null;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        TryCall(UIEventType.PointerEnter, eventData);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (m_IsPress) //长按不触发点击
        {
            m_IsPress = false;
            return;
        }

        if (m_IsDrag)
        {
            return;
        }

        TryCall(UIEventType.PointerClick, eventData);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        TryCall(UIEventType.PointerDown, eventData);
        m_DownTime = 0f;
        m_IsDown = true;
        m_PointerEventData = eventData;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        TryCall(UIEventType.PointerUp, eventData);
        m_IsDown = false;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        TryCall(UIEventType.PointerExit, eventData);
        m_IsDown = false;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (m_ValidDragPointreId != -1)
        {
            return;
        }
        m_IsDrag = true;
        m_ValidDragPointreId = eventData.pointerId;
        TryCall(UIEventType.BeginDrag, eventData);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId != m_ValidDragPointreId)
        {
            return;
        }
        TryCall(UIEventType.Drag, eventData);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        m_IsDrag = false;
        if (eventData.pointerId != m_ValidDragPointreId)
        {
            return;
        }
        m_ValidDragPointreId = -1;
        TryCall(UIEventType.EndDrag, eventData);
    }

    void IScrollHandler.OnScroll(PointerEventData eventData)
    {
        TryCall(UIEventType.Scroll, eventData);
    }

    void ICancelHandler.OnCancel(BaseEventData eventData)
    {
        TryCall(UIEventType.Cancel, eventData as PointerEventData);
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        TryCall(UIEventType.Select, eventData as PointerEventData);
    }

    void IUpdateSelectedHandler.OnUpdateSelected(BaseEventData eventData)
    {
        TryCall(UIEventType.UpdateSelected, eventData as PointerEventData);
    }

    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
        TryCall(UIEventType.Deselect, eventData as PointerEventData);
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        TryCall(UIEventType.Drop, eventData);
    }

    void IInitializePotentialDragHandler.OnInitializePotentialDrag(PointerEventData eventData)
    {
        TryCall(UIEventType.InitializePotentialDrag, eventData);
    }


    void ISubmitHandler.OnSubmit(BaseEventData eventData)
    {
        TryCall(UIEventType.Submit, eventData as PointerEventData);
    }

    void IMoveHandler.OnMove(AxisEventData eventData)
    {

    }

    void IDisposable.Dispose()
    {
        RemoveAllListener();
    }

}
