using UnityEngine;
using System.Collections;

public class CFead : CObj2D
{
	public enum eFeadState
	{
		STOP,
		FEAD_IN,
		FEAD_MIDDLE,
		FEAD_MIDDLE_STOP,
		FEAD_OUT,
	};
	public enum eFeadType
	{
		NONE,
		WHITEFEAD,
		BLACKFEAD,
	};


	// --変数宣言--
	public float m_fInTime, m_fMiddleTime, m_fOutTime;
	protected float m_fChangeTime;
	protected bool m_bMiddleStop;
	protected int m_nFeadType;


	//**関数***************************************************************************
	//	概要	:	変数初期化
	//*********************************************************************************
	void Awake()
	{
		m_bMiddleStop = false;
		m_fChangeTime = 0.0f;
		m_nFeadType = (int)eFeadType.NONE;

		Initialize();
	}

	//**関数***************************************************************************
	//	概要	:	生成時処理
	//*********************************************************************************
	public override bool Create(GameObject parent)
	{
		m_nState = (int)eFeadState.STOP;

		if(Initialize())
			return base.Create(parent);

		return base.Create(parent);
	}


	//**関数***************************************************************************
	//	概要	:	初期処理
	//*********************************************************************************
	public override bool Initialize()
	{
		// サイズ調整
		float x = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
		float y = gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
		gameObject.GetComponent<CObj2D>().SetLocalScale(Screen.width / x / 2, Screen.height / y / 2, 1.0f);

		return base.Initialize();
	}


	//**関数***************************************************************************
	//	概要	:	更新
	//*********************************************************************************
	void Update()
	{ 
		// 使用中でなければ処理なし
		if(! m_bUseFlg || CCommon.GetPauseFlg()) return;
	
		float	fAlpha = 1.0f;

		m_fChangeTime += Time.deltaTime;

		switch(m_nState)
		{
		// フェードイン
		case (int)eFeadState.FEAD_IN:
			fAlpha = GetColor().a;
			fAlpha += 1.0f * CCommon.TimeUpdate(m_fInTime);
			SetColor_Alpha(fAlpha);

			// フェード停止状態へ移行
			if(m_fChangeTime >= m_fInTime)
			{
				m_fChangeTime = 0.0f;
				m_nState = (int)eFeadState.FEAD_MIDDLE;
				SetColor_Alpha(1.0f);
			}
			break;

		// 中間
		case (int)eFeadState.FEAD_MIDDLE:
			if(m_fChangeTime >= m_fMiddleTime)
			{
				m_fChangeTime = 0.0f;

				if(m_bMiddleStop)
					m_nState = (int)eFeadState.FEAD_MIDDLE_STOP;
				else
					m_nState = (int)eFeadState.FEAD_OUT;

			}
			break;

		// 中間停止
		case (int)eFeadState.FEAD_MIDDLE_STOP:

			break;

		case (int)eFeadState.FEAD_OUT:
			fAlpha = GetColor().a;
			fAlpha -= 1.0f * CCommon.TimeUpdate(m_fOutTime);
			SetColor_Alpha(fAlpha);

			if(m_fChangeTime >= m_fOutTime)
			{
				m_bUseFlg = false;
				m_nState = (int)eFeadState.STOP;
			}
			break;

		default:
			break;
		}
	}


	//**関数***************************************************************************
	//	概要	:	フェード開始
	//*********************************************************************************
	public virtual bool FeadStart(eFeadType eType)
	{
		// 動作中は処理しない
		if (m_bUseFlg) return false;

		m_nFeadType = (int)eType;
		m_nState = (int)eFeadState.FEAD_IN;
		m_fChangeTime = 0.0f;

		// TODO 色に応じた処理
		if (m_nFeadType == (int)eFeadType.BLACKFEAD)
			m_nFeadType = (int)eFeadType.BLACKFEAD;
		else
			m_nFeadType = (int)eFeadType.WHITEFEAD;

		m_bUseFlg = true;
		return true;
	}

	//**関数***************************************************************************
	//	概要	:	フェード中間から開始
	//*********************************************************************************
	public virtual bool FeadStartMid(eFeadType eType)
	{
		// 動作中は処理しない
		if (m_bUseFlg) return false;

		m_nFeadType = (int)eType;
		m_nState = (int)eFeadState.FEAD_MIDDLE;
		m_fChangeTime = 0.0f;

		// TODO 色に応じた処理
		if (m_nFeadType == (int)eFeadType.BLACKFEAD)
			m_nFeadType = (int)eFeadType.BLACKFEAD;
		else
			m_nFeadType = (int)eFeadType.WHITEFEAD;

		m_bUseFlg = true;
		return true;
	}


	//**関数***************************************************************************
	//	概要	:	中間停止状態から開始
	//*********************************************************************************
	public virtual bool MiddleStart()
	{
		if (!m_bMiddleStop || m_nState != (int)eFeadState.FEAD_MIDDLE_STOP)
			return false;

		m_nState = (int)eFeadState.FEAD_OUT;
		m_fChangeTime = 0.0f;

		return true;
	}


	//**関数***************************************************************************
	//	概要	:	中間停止フラグのゲットセット
	//*********************************************************************************
	public virtual bool GetMiddleStop() { return m_bMiddleStop; }
	public virtual void SetMiddleStop(bool bFlg) { m_bMiddleStop = bFlg; }


	//**関数***************************************************************************
	//	概要	:	フェードタイムセット
	//*********************************************************************************
	public virtual bool SetFeadTime(float fIn, float fMiddle, float fOut)
	{
		// 動作中は変更を受け付けない
		if (m_bUseFlg)
			return false;

		m_fInTime = fIn;
		m_fMiddleTime = fMiddle;
		m_fOutTime = fOut;
		return true;
	}
}