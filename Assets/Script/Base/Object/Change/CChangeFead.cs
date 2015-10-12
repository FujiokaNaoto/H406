using UnityEngine;
using System.Collections;

public class CChangeFead : CChangeObj 
{
	// --変数宣言--
	public GameObject FeadPrefab;
	protected GameObject m_Fead;

	//**関数***************************************************************************
	//	概要	:	変数初期化
	//*********************************************************************************
	void Awake()
	{
		m_Fead = null;
		m_bUseFlg = false;
	}

	//**関数***************************************************************************
	//	概要	:	生成時処理
	//*********************************************************************************
	public virtual bool Create(GameObject changeObj, CFead.eFeadType eType)
	{
		if(Initialize(changeObj, eType))
			return base.Create(changeObj);

		return base.Create(changeObj);
	}


	//**関数***************************************************************************
	//	概要	:	初期処理
	//*********************************************************************************
	public virtual bool Initialize(GameObject changeObj, CFead.eFeadType eType)
	{
		// フェードオブジェクトを生成、中間で停止するようにしてフェード開始
		if (!m_Fead) m_Fead = (GameObject)GameObject.Instantiate(FeadPrefab);
		m_Fead.GetComponent<CFead>().Create(gameObject);
		m_Fead.GetComponent<CFead>().SetMiddleStop(true);
		m_Fead.GetComponent<CFead>().FeadStart(eType);

		// 変更中シーンオブジェクト
		m_Change = changeObj;
		m_bUseFlg = true;

		return true;
	}


	//**関数***************************************************************************
	//	概要	:	更新
	//*********************************************************************************
	void Update()
	{
		if (!m_bUseFlg)
			return;

		switch (m_Fead.GetComponent<CFead>().GetState())
		{ 
			case (int)CFead.eFeadState.FEAD_MIDDLE_STOP:
				if (m_Change)
					m_Change.GetComponent<CChanging>().SceneChange();

					// 中間停止中のフェードをスタート
					m_Fead.GetComponent<CFead>().MiddleStart();
				break;

			case (int)CFead.eFeadState.STOP:
				ChangeEnd();
				break;
		}
	}

	//**関数***************************************************************************
	//	概要	:	シーン変更終了
	//*********************************************************************************
	public override void ChangeEnd()
	{
		if (m_Change) m_Change.GetComponent<CChanging>().ChangeEnd();
		m_nChangeCnt = 0;

		base.ChangeEnd();
	}
}
