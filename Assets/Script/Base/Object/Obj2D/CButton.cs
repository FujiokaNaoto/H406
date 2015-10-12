//**プログラムヘッダ***************************************************************
//	プログラム概要	:	ボタンクラス
//*********************************************************************************
using UnityEngine;
using System.Collections;

public class CButton : CObj2D 
{
	// --変数宣言--
	protected readonly Color DEFAULT_COLOR = new Color(0.87f, 0.87f, 0.87f, 1.0f);
	protected readonly Color ACTIVE_COLOR = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	protected readonly Color UNUSE_COLOR = new Color(0.2f, 0.2f ,0.2f, 1.0f);

	public enum eButtonState
	{
		DEFAULT,			// 使用可能、押されていない
		ACTIVE,				// 使用可能、押された
		UNUSE,				// 未使用
	};

	protected bool m_bButtonRock;					// ボタン反応フラグ
	protected float m_fRockTime;					// ボタン反応時の時間	秒
	protected readonly double REACT_TIME = 0.03;	// ボタン反応許容時間	秒

	protected bool m_bTriClick;


	//**関数***************************************************************************
	//	概要	:	初期化
	//*********************************************************************************
	void Awake()
	{
		SetState(eButtonState.DEFAULT);
		m_fRockTime = 0.0f;
		m_bButtonRock = false;
		m_bTriClick = false;
	}


	//**関数***************************************************************************
	//	概要	:	更新処理
	//*********************************************************************************
	void Update()
	{
		if (m_bTriClick)
		{
			m_fRockTime += Time.deltaTime;

			// 反応許容時間を超えたらトリガーフラグを下す
			if (m_fRockTime > REACT_TIME)
			{
				m_bTriClick = false;
				m_fRockTime = 0.0f;
			}
		}

		if (CManager.Instance.GetInput().GetMouseTrigger(0) && ! m_bTriClick) 
		{
			m_bTriClick = true;
			m_fRockTime = 0.0f;
		}

		if (m_bButtonRock && CManager.Instance.GetInput().GetMouseRelease(0))
			OnRelease();


		if (m_bButtonRock && !CManager.Instance.GetInput().GetMouseStay(0))
			m_bButtonRock = false;
	}


	//**関数***************************************************************************
	//	概要	:	当たり判定
	//*********************************************************************************
	void OnTriggerEnter2D(Collider2D col)
	{
		if (m_nState == (int)eButtonState.UNUSE)
			return;

		if (col.tag == "InputCol" && m_bTriClick)
			OnTrigger();
	}


	//**関数***************************************************************************
	//	概要	:	当たり判定
	//*********************************************************************************
	void OnTriggerExit2D(Collider2D col)
	{
		// トリガーでボタンが押されていなければ処理なし
		if (!m_bButtonRock)
			return;

		// 対象がInputのカーソルならトリガーを下す
		if (col.tag == "InputCol")
		{
			m_bButtonRock = false;
		}
	}


	//**関数***************************************************************************
	//	概要	:	状態変更
	//*********************************************************************************
	public void SetState(eButtonState eButtonState)
	{
		m_nState = (int)eButtonState;

		switch (eButtonState)
		{
		case eButtonState.DEFAULT:
			gameObject.GetComponent<CObj2D>().SetColor(DEFAULT_COLOR);
			break;

 		case eButtonState.ACTIVE:
			gameObject.GetComponent<CObj2D>().SetColor(ACTIVE_COLOR);
			break;

		case eButtonState.UNUSE:
			gameObject.GetComponent<CObj2D>().SetColor(UNUSE_COLOR);
			break;

		default:
			Debug.Log("CButton Set State Error");
			break;
		}
	}
	

	//**関数***************************************************************************
	//	概要	:	トリガー時処理
	//*********************************************************************************
	protected void OnTrigger()
	{
		m_bButtonRock = true;
	}


	//**関数***************************************************************************
	//	概要	:	リリース時処理
	//*********************************************************************************
	protected void OnRelease()
	{ 
		switch(m_nState)
		{
		case (int)eButtonState.DEFAULT:
			if(m_bButtonRock)
				SetState(eButtonState.ACTIVE);
			break;

		case (int)eButtonState.ACTIVE:
			if(m_bButtonRock)
				SetState(eButtonState.DEFAULT);
			break;

		default:
			// エラー
			break;
		}

		m_bButtonRock = false;
	}
}
