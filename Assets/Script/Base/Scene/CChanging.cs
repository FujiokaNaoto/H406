using UnityEngine;
using System.Collections;

public class CChanging : CScene 
{
	// 変更タイプごとのオブジェクトプレハブ
	public GameObject BLACK_PREFAB;
	public GameObject WHITE_PREFAB;

    // 変更タイプ
	public enum eChangeType
	{
		CHANGE_NONE,
		WHITE_FEAD,
		BLACK_FEAD,
	};

	// --変数宣言--
	private bool m_bChange;			// 変更中フラグ

	CSceneManager.eSceneID m_Bedore;
	CSceneManager.eSceneID m_After;
	eChangeType m_eChangeType;

	GameObject m_ChangeObject;
	GameObject m_SceneManage;


	//**関数***************************************************************************
	//	概要	:	変数初期化
	//*********************************************************************************
	void Awake()
	{
		m_bChange = false;

		m_Bedore = CSceneManager.eSceneID.NONE;
		m_After = CSceneManager.eSceneID.NONE;
		m_eChangeType = eChangeType.CHANGE_NONE;

		m_ChangeObject = null;
		m_SceneManage = null;
	}


	//**関数***************************************************************************
	//	概要	:	生成
	//*********************************************************************************
	public bool Create(eChangeType eType , CSceneManager.eSceneID Current , CSceneManager.eSceneID After , GameObject SceneManage)
	{
		if (m_bChange)
			return false;

		m_eChangeType = eType;
		m_Bedore = Current;
		m_After = After;
		m_SceneManage = SceneManage;
		m_bChange = true;

		transform.SetParent(SceneManage.transform);

		Initialize ();
		return true;
	}


	//**関数***************************************************************************
	//	概要	:	初期化
	//*********************************************************************************
	public override bool Initialize()
	{
		// 変更タイプに応じたオブジェクト生成
		switch (m_eChangeType)
		{ 
		case eChangeType.WHITE_FEAD:
			m_ChangeObject = (GameObject)GameObject.Instantiate(WHITE_PREFAB);
			m_ChangeObject.GetComponent<CChangeFead>().Create(gameObject , CFead.eFeadType.WHITEFEAD);
			break;

		case eChangeType.BLACK_FEAD:
			m_ChangeObject = (GameObject)GameObject.Instantiate(BLACK_PREFAB);
			m_ChangeObject.GetComponent<CChangeFead>().Create(gameObject, CFead.eFeadType.BLACKFEAD);
			break;

		case eChangeType.CHANGE_NONE:
			SceneChange();
			ChangeEnd();
			break;
		}

		return true;
	}


	//**関数***************************************************************************
	//	概要	:	シーン変更
	//*********************************************************************************
	public virtual void SceneChange()
	{
		// 前シーンを破棄して新規シーンを生成
		//m_SceneManage.GetComponent<CSceneManager>().GetSceneObj(m_Bedore).SetActive(false);
		m_SceneManage.GetComponent<CSceneManager>().ClearOnce(m_Bedore);
		m_SceneManage.GetComponent<CSceneManager>().SetOnce(m_After);
		//m_SceneManage.GetComponent<CSceneManager>().GetSceneObj(m_After).SetActive(true);
	}


	//**関数***************************************************************************
	//	概要	:	変更終了
	//*********************************************************************************
	public virtual void ChangeEnd()
	{
		m_bChange = false;
		if (m_ChangeObject)
		{
			Destroy(m_ChangeObject);
			m_ChangeObject = null;
		}
	}

	
	//**関数***************************************************************************
	//	概要	:	更新処理
	//*********************************************************************************
	void Update () 
	{
	
	}


	//**関数***************************************************************************
	//	概要	:	変更中フラグ取得
	//*********************************************************************************
	public bool GetChangeFlg()
	{
		return m_bChange;
	}
}