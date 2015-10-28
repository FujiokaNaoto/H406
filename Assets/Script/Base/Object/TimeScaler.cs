//**プログラムヘッダ***************************************************************
//	プログラム概要	:	時間をかけて拡大縮小
//*********************************************************************************
using UnityEngine;
using System.Collections;

public class TimeScaler : MonoBehaviour 
{
	// --変数宣言--
	public Vector3 m_TargetScale;

	private Vector3 m_StartScale;
	private float m_fChangeTime;
	private bool m_bScaleFlg;


	//**関数***************************************************************************
	//	概要	:	初期化
	//*********************************************************************************
	void Awake()
	{
		m_fChangeTime = 0.0f;
		m_bScaleFlg = false;
	}


	//**関数***************************************************************************
	//	概要	:	初期化
	//*********************************************************************************
	void Start()
	{
	}


	//**関数***************************************************************************
	//	概要	:	更新
	//*********************************************************************************
	void Update()
	{
		// 拡縮を行うか否かの判定
		if (!m_bScaleFlg)
			return;

		// 時間準拠での更新処理
		Vector3 vecBuf = CCommon.TimeUpdate(m_fChangeTime) * (m_TargetScale - m_StartScale);
		Vector3 scaleBuf = transform.localScale + vecBuf;

		if (vecBuf.x < 0 && scaleBuf.x <= m_TargetScale.x)
		{
			scaleBuf = m_TargetScale;
			m_bScaleFlg = false;
		}
		else if (vecBuf.x >= 0 && scaleBuf.x >= m_TargetScale.x)
		{
			scaleBuf = m_TargetScale;
			m_bScaleFlg = false;
		}

		transform.localScale = scaleBuf;
	}

	//**関数***************************************************************************
	//	概要	:	ターゲット値設定
	//*********************************************************************************
	public void SetTargetScale(Vector3 vec , float fChangeTime , bool bStartFlg)
	{
		m_StartScale = transform.localScale;
		m_TargetScale = vec;
		m_fChangeTime = fChangeTime;
		m_bScaleFlg = bStartFlg;
	}
	public void SetTargetScale(float x, float y, float z , float fChangeTime , bool bStartFlg)
	{
		m_StartScale = transform.localScale;
		m_TargetScale.x = x;
		m_TargetScale.y = y;
		m_TargetScale.z = z;

		m_fChangeTime = fChangeTime;
		m_bScaleFlg = bStartFlg;
	}


	//**関数***************************************************************************
	//	概要	:	拡縮開始
	//			:	すでに拡縮中は実行不可
	//*********************************************************************************
	public bool ScaleStart()
	{
		if (m_bScaleFlg)
			return false;

		m_bScaleFlg = true;
		return true;
	}


	//**関数***************************************************************************
	//	概要	:	フラグ取得
	//*********************************************************************************
	public bool GetScaleFlg() { return m_bScaleFlg; }
}
