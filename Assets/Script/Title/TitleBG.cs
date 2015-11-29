//**プログラムヘッダ***************************************************************
//	プログラム概要	:	タイトル背景
//*********************************************************************************
using UnityEngine;
using System.Collections;

public class TitleBG : CObj2D 
{
	// --公開変数--
	public Vector3 LEFT_POS;
	public Vector3 RIGHT_POS;

	// --定数宣言--
	public enum eState
	{
		FEADIN,
		MOVE,
		FEADOUT,
		HIDE,
	};
	readonly float[] StateTimeArray = new float[3]{0.7f , 4.0f , 1.3f};
	static readonly public int MOVE_RIGHT = 0;
	static readonly public int MOVE_LEFT = 1;

	// --変数宣言--
	private float m_fTotalTime;
	private float m_fTimer;
	private int m_nMoveDir;

	//**関数***************************************************************************
	//	概要	:	初期化
	//*********************************************************************************
	void Awake()
	{
		m_fTimer = 0.0f;
		m_fTotalTime = 0.0f;
		m_nMoveDir = MOVE_RIGHT;

		for(int i = 0; i < StateTimeArray.Length ; i ++)
			m_fTotalTime += StateTimeArray[i];
	}


	//**関数***************************************************************************
	//	概要	:	更新
	//*********************************************************************************
	void Update()
	{
		// 隠れ状態時にアクティブになっていたら処理なし
		if (m_nState == (int)eState.HIDE) gameObject.SetActive(false);

		// 移動
		if (m_nMoveDir == MOVE_LEFT)  AddLocalPos(CCommon.TimeUpdate(m_fTotalTime) * (LEFT_POS - RIGHT_POS));
		else if (m_nMoveDir == MOVE_RIGHT)  AddLocalPos(CCommon.TimeUpdate(m_fTotalTime) * (RIGHT_POS - LEFT_POS));

		// ステート管理
		m_fTimer += Time.deltaTime;

		// 画像のフェードインアウト処理
		if (m_nState == (int)eState.FEADIN)
			SetColor_Alpha(1.0f * (m_fTimer / StateTimeArray[m_nState]));
		else if(m_nState == (int)eState.FEADOUT)
			SetColor_Alpha(1.0f - (1.0f * (m_fTimer / StateTimeArray[m_nState])));

		// ステート切替
		if (m_nState < StateTimeArray.Length && m_fTimer >= StateTimeArray[m_nState])
		{
			m_fTimer = 0.0f;
			m_nState ++;
		}

	}


	//**関数***************************************************************************
	//	概要	:	移動開始
	//*********************************************************************************
	public bool MoveStart(int nMoveDir)
	{
		m_nMoveDir = nMoveDir;

		if (m_nMoveDir == MOVE_RIGHT) SetLocalPos(LEFT_POS);
		else if(m_nMoveDir == MOVE_LEFT) SetLocalPos(RIGHT_POS);

		m_fTimer = 0.0f;
		m_nState = 0;
		return true;
	}


	//**関数***************************************************************************
	//	概要	:	移動方向取得
	//*********************************************************************************
	public int GetMoveDir() { return m_nMoveDir; }
}
