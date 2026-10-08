using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("Assets/UI/LongPressButton", 31)]
public class LongPressButton : Button
{
    private bool m_IsPress = false;
    private bool m_IsPointDown = false;

    private float m_InitTime = 0f;
    private float m_IntervalTime = 0.5f;
    private float m_DealyOnBeginToLongPressedUpdateTime = 0.1f;

    public PointerEventData CurClickEventData { get; private set; }

    public class LongPressEvent : UnityEvent { }
    public class DragEvent<Vector2> : UnityEvent<Vector2> { }

    public LongPressEvent OnLongPress { get; private set; }

    public LongPressEvent OnLongPressEnd { get; private set; }

    public UnityAction OnPointUpEvent { get; private set; }

    public UnityAction<float> OnBeginToLongPressedUpdate { get; set; }

    public void AddClickListener(UnityAction listener)
    {
        onClick.AddListener(listener);
    }

    public void AddLongPressListener(UnityAction listener)
    {
        if (listener != null)
        {
            if (OnLongPress == null)
            {
                OnLongPress = new LongPressEvent();
            }
            OnLongPress.AddListener(listener);
        }
    }

    public void AddLongPressEndListener(UnityAction listener)
    {
        if (listener != null)
        {
            if (OnLongPressEnd == null)
            {
                OnLongPressEnd = new LongPressEvent();
            }
            OnLongPressEnd.AddListener(listener);
        }
    }

    public void AddBeginToLongPressedUpdateListener(UnityAction<float> listener)
    {
        OnBeginToLongPressedUpdate = listener;
    }

    public void AddPointUpListener(UnityAction listener)
    {
        OnPointUpEvent = listener;
    }

    public void SetIntervalTime(float time)
    {
        m_IntervalTime = time;
    }

    public float GetIntervalTime()
    {
        return m_IntervalTime;
    }

    public float GetBeginToLongPressedTime()
    {
        return m_IntervalTime - m_DealyOnBeginToLongPressedUpdateTime;
    }

    private void Update()
    {
        if (!m_IsPointDown)
        {
            return;
        }

        float durationTime = Time.unscaledTime - m_InitTime;
        if (durationTime > m_IntervalTime)
        {
            m_IsPress = true;
            m_IsPointDown = false;
            OnLongPress?.Invoke();
        }
        else
        {
            if (durationTime > m_DealyOnBeginToLongPressedUpdateTime)
            {
                OnBeginToLongPressedUpdate?.Invoke(Time.unscaledDeltaTime);
            }
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (m_IsPress)
        {
            return;
        }

        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }
        if (!IsActive() || !IsInteractable())
        {
            return;
        }
        onClick?.Invoke();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        m_IsPointDown = true;
        m_IsPress = false;
        CurClickEventData = eventData;
        m_InitTime = Time.unscaledTime;
        //TODO 播放点击音效  OnClickAudio();
        //目前先这样播放音效 后面要封装支持多样化点击音效
        //GameCtrlMgr.GetInstance().SoundCtrl.PlayEffect("button_1");
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        OnPointUpEvent?.Invoke();
        m_IsPointDown = false;
        CurClickEventData = null;
        if (m_IsPress)
        {
            OnLongPressEnd?.Invoke();
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        m_IsPress = false;
        m_IsPointDown = false;
    }

    public void RemoveAllListener()
    {
        OnLongPress?.RemoveAllListeners();
        OnLongPressEnd?.RemoveAllListeners();
        onClick?.RemoveAllListeners();
        OnBeginToLongPressedUpdate = null;
    }
}
