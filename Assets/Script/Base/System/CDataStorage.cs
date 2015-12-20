using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CDataStorage : MonoBehaviour
{
    public enum DATACODE
    {
        TIME,
        STAGE,
		SCORE,
		CLOCK,
		CLOCK_X,
		CLOCK_Y,
		MAX
    };

    protected List<int> m_DataList;

    // Use this for initialization
    void Start()
    {
        m_DataList = new List<int>();
    }
    void Update()
    {
    }
    public void SetData(int nData, int nCode)
    {
        while (true)
        {
            if (nCode < m_DataList.Count)
            {
                m_DataList[nCode] = nData;
                break;
            }
            else
                m_DataList.Add(-1);
        }
    }

    public int GetData(int nCode)
    {
        if (nCode < m_DataList.Count)
            return m_DataList[nCode];
        else
            return -1;
    }

    public void AddData(int nAdd,int nCode)
    {
        if (nCode < m_DataList.Count)
            m_DataList[nCode] += nAdd;
    }

    public void DeleteData(int nCode)
    {
        if(nCode < m_DataList.Count)
            m_DataList.Insert(nCode, -1);
    }
}