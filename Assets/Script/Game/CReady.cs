using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CReady : CObj2D 
{
	protected readonly Vector3 PUNCH_VEC = new Vector3(1.3f , 1.3f , 0.0f);		// パンチングサイズ
	protected readonly float PUNCH_DURATION = 0.3f;								// パンチング時間
	
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
	
	protected bool isTweenEnd = false;
	
	void Awake()
	{
		SetReady((int)eReadyState.READY, 1.2f);
		DOTween.Init();
	}


	//**関数***************************************************************************
	//	概要	:	更新
	//*********************************************************************************
	void Update () 
	{
		isTweenEnd = false;
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
					isTweenEnd = true;
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

		DOTween.Init();
		transform.DOScale(PUNCH_VEC, 0.0f);
		transform.DOPunchScale(PUNCH_VEC, PUNCH_DURATION, 3, 1.0f);
		gameObject.GetComponent<SpriteRenderer>().sprite = CCommon.GetSprite(TexHierarchy + "Ready", "Ready_" + m_nState.ToString());
		m_fLimTime = fLimTime;
	}
	
	public bool GetTweenEnd() {
		return isTweenEnd;
	}
}
