using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSequenceFrame : MonoBehaviour
{
    private Image m_Image;
    public List<Sprite> m_Sprites;
    public int m_FPS = 30;
    public bool m_Loop = true;
    private float m_Time;
    private bool m_IsAnimationComplete = false; // 动画是否已完成

    void Start()
    {
        m_Image = GetComponent<Image>();
        if (m_Image == null)
        {
            return;
        }

        if (m_Sprites != null && m_Sprites.Count > 0)
        {
            m_Image.sprite = m_Sprites[0];
        }
        Initialize();
    }

    public void Initialize()
    {
        m_Time = 0.0f;
        m_IsAnimationComplete = false;
    }


    void Update()
    {
        if (m_IsAnimationComplete && !m_Loop)
        {
            return;
        }

        if (m_Sprites == null || m_Sprites.Count == 0)
        {
            return;
        }

        m_Time += Time.deltaTime;
        int frame;
        if (m_Loop)
        {
            // 循环播放：使用模运算
            frame = Mathf.FloorToInt(m_Time * m_FPS) % m_Sprites.Count;
        }
        else
        {
            // 非循环播放：限制在最后一帧
            int totalFrame = Mathf.FloorToInt(m_Time * m_FPS);
            frame = Mathf.Clamp(totalFrame, 0, m_Sprites.Count - 1);
            // 检查动画是否播放完成
            if (totalFrame >= m_Sprites.Count - 1)
            {
                m_IsAnimationComplete = true;
                frame = m_Sprites.Count - 1; // 确保停在最后一帧
            }
        }
        m_Image.sprite = m_Sprites[frame];
    }

    /// <summary>
    /// 重新开始播放动画
    /// </summary>
    public void RestartAnimation()
    {
        Initialize();
        if (m_Sprites != null && m_Sprites.Count > 0)
        {
            m_Image.sprite = m_Sprites[0];
        }
    }

    /// <summary>
    /// 检查动画是否播放完成（仅在非循环模式下有意义）
    /// </summary>
    /// <returns>动画是否完成</returns>
    public bool IsAnimationComplete()
    {
        return m_IsAnimationComplete;
    }

    private void OnDisable()
    {
        RestartAnimation();
    }

    void OnDestroy()
    {
        m_Sprites = null;
    }
}
