﻿using UnityEngine;
using System.Collections;

public class CChangeObj : CObject 
{
	// --変数宣言--
	protected GameObject m_Change;			// チェンジシーンオブジェクト
	protected int m_nChangeCnt;

	// 変更の状態
	public enum eChangeObj
	{
		CHANGING,
		CHANGE_SCENE,
		CHANGE_END,	
	};

	//**関数***************************************************************************
	//	概要	:	初期化
	//*********************************************************************************
	void Awake()
	{
		m_Change = null;
		m_nChangeCnt = 0;
	}

	
	//**関数***************************************************************************
	//	概要	:	更新
	//*********************************************************************************
	void Update ()
	{
		
	}


	//**関数***************************************************************************
	//	概要	:	変更開始
	//*********************************************************************************
	public virtual void ChangeStart()
	{ 
	}


	//**関数***************************************************************************
	//	概要	:	変更終了
	//*********************************************************************************
	public virtual void ChangeEnd()
	{ 
	}
}
