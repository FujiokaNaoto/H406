using UnityEngine;
using System.Collections;

public class CReady : CObj2D 
{
	public string texName;
	public enum eReadyState
	{
		NONE,
		GO,
		FINISH,
		READY,
		MAX
	};
	
	protected float m_fLimTime = 0.0f;
	

	void Awake()
	{
		SetReady((int)eReadyState.READY, 1.2f);
	}


	//**関数***************************************************************************
	//	概要	:	更新
	//*********************************************************************************
	void Update () 
	{
		if(m_nState == (int)eReadyState.NONE) return;

		m_fAnimTime += Time.deltaTime;
		if (m_fAnimTime >= m_fLimTime)
		{
			m_fAnimTime = 0.0f;

			switch (m_nState)
			{
				case (int)eReadyState.GO:
				case (int)eReadyState.FINISH:
					SetReady((int)eReadyState.NONE, 1.0f);
					break;

				case (int)eReadyState.READY:
					SetReady((int)eReadyState.GO, 1.0f);
					break;
			}
		}
	}


	//**関数***************************************************************************
	//	概要	:	状態変化
	//*********************************************************************************
	public void SetReady(int nState , float fLimTime)
	{
		m_nState = nState;
		m_fLimTime = fLimTime;

		gameObject.GetComponent<SpriteRenderer>().sprite = CCommon.GetSprite(TexHierarchy + "Ready", "Ready_" + m_nState.ToString());
		m_fLimTime = fLimTime;
	}

}
