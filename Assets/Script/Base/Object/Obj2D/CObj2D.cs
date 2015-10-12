using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CObj2D : CObject 
{
	public enum e2DAnimType
	{
		NONE,
		ONCE,
		LOOP,
		STOP,
	};

	public string TexHierarchy;
	protected List<float> m_fAnimWait = new List<float>();	// Time Unit Second
	
	protected float m_fAnimTime;
	protected int 	m_nAnimNum;
	protected int	m_nAnimState;
	protected e2DAnimType	m_eAnimType;
	protected bool	m_bAnimEnd = false;

	// アニメーション
	protected string m_szTexName;


	//**関数***************************************************************************
	//	概要	:	変数初期化
	//*********************************************************************************
	void Awake()
	{
		m_fAnimTime = 0.0f;
		m_nAnimState = 0;
		m_nAnimNum = 0;
		m_eAnimType = e2DAnimType.NONE;
		m_bAnimEnd = false;

		m_szTexName = null;
	}


	//**関数***************************************************************************
	//	概要	:	生成時処理
	//*********************************************************************************
	public override bool Create(GameObject parent)
	{
		if(Initialize())
			return base.Create(parent);

		return base.Create(parent);
	}
	public override bool Create(GameObject parent, Vector3 Localpos)
	{
		if (Initialize())
			return base.Create(parent, Localpos);

		return base.Create(parent , Localpos);
	}
	public override bool Create(GameObject parent, Vector3 Localpos, Vector3 Localrot, Vector3 Localscale)
	{
		if (Initialize())
			return base.Create(parent, Localpos, Localrot, Localscale);

		return base.Create(parent, Localpos,Localrot,Localscale);
	}


	//**関数***************************************************************************
	//	概要	:	初期化
	//*********************************************************************************
	public virtual bool Initialize()
	{
		m_szTexName = gameObject.GetComponent<SpriteRenderer>().name;

		return true;
	}

	//**関数***************************************************************************
	//	概要	:	更新処理
	//*********************************************************************************
	void Update()
	{
		if (CCommon.GetPauseFlg ())
			return;

		AnimUpdate ();
	}


	//**関数***************************************************************************
	//	概要	:	アニメーション更新
	//*********************************************************************************
	virtual protected void AnimUpdate()
	{
		if (m_eAnimType == e2DAnimType.NONE || m_eAnimType == e2DAnimType.STOP || m_bAnimEnd)
			return;

		m_fAnimTime += CCommon.TimeUpdate (m_fAnimWait [m_nAnimState]);

		if (m_fAnimTime >= m_fAnimWait [m_nAnimState]) {
			m_fAnimTime = 0.0f;
			m_nAnimState ++;
			if(m_nAnimState >= m_nAnimNum)
			{
				if(m_eAnimType == e2DAnimType.ONCE)
					AnimEnd();
				else if(m_eAnimType == e2DAnimType.LOOP)
					m_nAnimState = 0;
			}

			// テクスチャ変更
			TexNameSet();
		}
	}


	//**関数***************************************************************************
	//	概要	:	アニメーション終了
	//*********************************************************************************
	virtual protected void AnimEnd ()
	{
		m_bAnimEnd = true;
	}


	//**関数***************************************************************************
	//	概要	:	アニメーション情報セット
	//*********************************************************************************	
	public void SetAnimInfo(string texName , e2DAnimType eType , List<float> fWaitList)
	{
		if (e2DAnimType.NONE == eType)
			return;

		m_nAnimNum = fWaitList.Count;
		m_fAnimWait.Clear ();
		m_fAnimWait = fWaitList;
		m_nAnimState = 0;
		m_fAnimTime = 0.0f;
		m_eAnimType = eType;

		// テクスチャ変更
		m_szTexName = texName;
		TexNameSet();

		if (eType != e2DAnimType.NONE || eType != e2DAnimType.STOP)
			m_bAnimEnd = false;
	}


	//**関数***************************************************************************
	//	概要	:	テクスチャ1箇所を指定し、そこで停止させる
	//*********************************************************************************
	public void SetAnimOneScene(string texName, int nPoint)
	{
		m_nAnimNum = 0;
		m_fAnimWait.Clear ();
		m_nAnimState = nPoint;
		m_fAnimTime = 0.0f;
		m_eAnimType = e2DAnimType.STOP;
		m_bAnimEnd = true;
		
		// テクスチャ変更
		m_szTexName = texName;
		TexNameSet();
	}


	//**関数***************************************************************************
	//	概要	:	テクスチャアニメーションセット
	//*********************************************************************************
	protected void TexNameSet()
	{
		string szName;

		if (m_nAnimState >= 10)
			szName = m_szTexName + '_' + (m_nAnimState / 10).ToString() + (m_nAnimState % 10);
		else
			szName = m_szTexName + '_' + (m_nAnimState % 10).ToString();

		gameObject.GetComponent<SpriteRenderer>().sprite = CCommon.GetSprite(TexHierarchy + m_szTexName, szName);
	}


	//**関数***************************************************************************
	//	概要	:	アニメーション終了フラグを取得する。
	//*********************************************************************************
	public bool GetAnimEnd()
	{
		return m_bAnimEnd;
	}


	//**関数***************************************************************************
	//	概要	:	色変化
	//*********************************************************************************
	public void SetColor(Color color)
	{
		gameObject.GetComponent<SpriteRenderer>().color = color;
	}
	public void SetColor(float r, float g, float b, float a)
	{
		Color colorBuf = gameObject.GetComponent<SpriteRenderer>().color;
		colorBuf.r = r;
		colorBuf.g = g;
		colorBuf.b = b;
		colorBuf.a = a;
		gameObject.GetComponent<SpriteRenderer>().color = colorBuf;
	}
	public void SetColor(float r, float g, float b)
	{
		Color colorBuf = gameObject.GetComponent<SpriteRenderer>().color;
		colorBuf.r = r;
		colorBuf.g = g;
		colorBuf.b = b;
		gameObject.GetComponent<SpriteRenderer>().color = colorBuf;
	}
	public void SetColor_Red(float r)
	{
		Color colorBuf = gameObject.GetComponent<SpriteRenderer>().color;
		colorBuf.r = r;
		gameObject.GetComponent<SpriteRenderer>().color = colorBuf;
	}
	public void SetColor_Green(float g)
	{
		Color colorBuf = gameObject.GetComponent<SpriteRenderer>().color;
		colorBuf.g = g;
		gameObject.GetComponent<SpriteRenderer>().color = colorBuf;
	}
	public void SetColor_Blue(float b)
	{
		Color colorBuf = gameObject.GetComponent<SpriteRenderer>().color;
		colorBuf.b = b;
		gameObject.GetComponent<SpriteRenderer>().color = colorBuf;
	}
	public void SetColor_Alpha(float a)
	{
		Color colorBuf = gameObject.GetComponent<SpriteRenderer>().color;
		colorBuf.a = a;
		gameObject.GetComponent<SpriteRenderer>().color = colorBuf;
	}


	//**関数***************************************************************************
	//	概要	:	色取得
	//*********************************************************************************
	public Color GetColor()
	{
		return gameObject.GetComponent<SpriteRenderer>().color;
	}

}
