//**プログラムヘッダ***************************************************************
//	プログラム概要	:	シーン管理マネージャクラス
//*********************************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSceneManager : CSingleton<CSceneManager> 
{
	public GameObject	ChangingPrefab;
	public GameObject	TitlePrefab;
	public GameObject	MainPrefab;
	public GameObject	ClearPrefab;
	public GameObject	OverPrefab;
	
	// 保持するシーン一覧
	public enum eSceneID
	{
		TITLE,
		MAIN,
		CLEAR,
		OVER,
		MAX,

		NONE,
	};
	
	private eSceneID		m_eCurrentSceneID = eSceneID.TITLE;
	private eSceneID	 	m_eNextSceneID = eSceneID.TITLE;

	private GameObject[]	m_ScenePrefab = new GameObject[(int)eSceneID.MAX];
	private GameObject[]	m_SceneObject = new GameObject[(int)eSceneID.MAX];
	private GameObject		m_Changing;
	private bool			m_bChangeFlg;


	//**関数***************************************************************************
	//	概要	:	変数初期化
	//*********************************************************************************
	void Awake () 
	{
		m_Changing = null;
		m_ScenePrefab [(int)eSceneID.TITLE] = TitlePrefab;
		m_ScenePrefab [(int)eSceneID.MAIN] = MainPrefab;
		m_ScenePrefab [(int)eSceneID.CLEAR] = ClearPrefab;
		m_ScenePrefab [(int)eSceneID.OVER] = OverPrefab;

		for (int i = 0; i < (int)eSceneID.MAX; i ++) 
		{
			// 開始シーンのみ生成する
			if (i != (int)m_eCurrentSceneID)
				continue;

			m_SceneObject[i] = (GameObject)GameObject.Instantiate(m_ScenePrefab[i]);
			m_SceneObject[i].GetComponent<CScene>().Create(i);
			m_SceneObject[i].transform.SetParent(transform);
		}

		if (this != Instance)
		{
			Destroy(this);
			return;
		}
		DontDestroyOnLoad(this.gameObject);
	}


	//===========================================================
	// 更新処理
	//===========================================================
	void Update()
	{
		if (m_bChangeFlg)
			Changing ();
	}


	//===========================================================
	// シーン変更中処理
	//===========================================================
	public void Changing()
	{
		if (! m_Changing.GetComponent <CChanging> ().GetChangeFlg ()) 
		{
			m_bChangeFlg = false;
			Destroy(m_Changing);
			m_Changing = null;
			m_eCurrentSceneID = m_eNextSceneID;
		}
	}


	//===========================================================
	// 全シーン破棄
	//===========================================================
	public void ListClear ()
	{
		for (int i = 0; i < (int)eSceneID.MAX; i ++) 
		{
			if(m_SceneObject[i])
			{
				Destroy(m_SceneObject[i]);
				m_SceneObject[i] = null;
			}
		}
	}


	//===========================================================
	// 指定シーンを破棄
	//===========================================================
	public bool ClearOnce(eSceneID scene)
	{
		if (m_SceneObject [(int)scene])
		{
			Destroy (m_SceneObject [(int)scene]);
			m_SceneObject[(int)scene] = null;
			return true;
		}
		return false;
	}


	//===========================================================
	// 指定シーンを生成し、セット
	//===========================================================
	public bool SetOnce(eSceneID scene)
	{
		if (! m_SceneObject[(int)scene])
		{
			m_SceneObject[(int)scene] = (GameObject)GameObject.Instantiate(m_ScenePrefab[(int)scene]);
			m_SceneObject[(int)scene].GetComponent<CScene>().Create((int)scene);
			m_SceneObject[(int)scene].SetActive(true);
			m_SceneObject[(int)scene].transform.SetParent(transform);
			return true;
		}
		return false;
	}


	//===========================================================
	// シーン変更指定
	//===========================================================
	public bool SetNextScene (eSceneID NextSceneID , CChanging.eChangeType eType)
	{
		// 変更中は受け付けない
		if (m_bChangeFlg)
			return false;

		m_eNextSceneID = NextSceneID;
		m_Changing = (GameObject)GameObject.Instantiate (ChangingPrefab);
		m_Changing.GetComponent<CChanging> ().Create (eType , m_eCurrentSceneID , m_eNextSceneID , gameObject);

		m_bChangeFlg = true;

		return true;
	}


	//===========================================================
	// 変更中フラグ取得
	//===========================================================
	public bool GetChangeFlg()
	{
		return m_bChangeFlg;
	}


	//===========================================================
	// 指定のシーンオブジェクト取得
	//===========================================================
	public GameObject GetSceneObj(eSceneID SceneID)
	{
		return m_SceneObject [(int)SceneID];
	}

}