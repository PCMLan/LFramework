using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStateItem : MonoBehaviour
{
    public List<GameObject> m_Items;
    private int m_State;
    private bool m_Initialized;

    public int State
    {
        get
        {
            return m_State;
        }
        set
        {
            int state = Mathf.Clamp(value, 0, m_Items.Count - 1);
            if (!m_Initialized || m_State != state)
            {
                for (int i = 0; i < m_Items.Count; ++i)
                {
                    if (m_Items[i] != null)
                    {
                        m_Items[i].FastSetActive(i == state);
                    }
                }
                m_State = state;
                m_Initialized = true;
            }
        }
    }

    public GameObject ActiveItem
    {
        get
        {
            return m_Items[m_State];
        }
    }

    public Text ActiveText
    {
        get
        {
            return m_Items[m_State].GetComponent<Text>();
        }
    }

    public GameObject GetItemByIndex(int stateIndex)
    {
        if (m_Items == null || m_Items.Count <= 0)
        {
            return null;
        }
        if (stateIndex < 0 || stateIndex > m_Items.Count - 1)
        {
            return null;
        }
        return m_Items[stateIndex];
    }
}
