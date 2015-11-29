using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CManager : CSingleton<CManager>
{
	// --プレハブ--
	public GameObject pInput;
	public GameObject pSceneManager;
	public GameObject pDataStorage;
	// TODO カメラ、ライト


	// --変数--
	private GameObject m_Input = null;
	private GameObject m_SceneManager = null;
	private GameObject m_Camera = null;
	private GameObject m_DataStorage = null;
	
	//**関数***************************************************************************
	//	概要	:	オブジェクト多重生成防止
	//*********************************************************************************
	void Awake()
	{
		if(this != Instance)
		{
			Destroy(this);
			return;
		}
		DontDestroyOnLoad(this.gameObject);
	}


	//**関数***************************************************************************
	//	概要	:	初期オブジェクト生成
	//*********************************************************************************
	void Start()
	{
		if (!m_Input)
		{
			m_Input = (GameObject)GameObject.Instantiate(pInput);
			m_Input.transform.SetParent(transform);
		}

		if (!m_SceneManager)
		{
			m_SceneManager = (GameObject)GameObject.Instantiate(pSceneManager);
			m_SceneManager.transform.SetParent(transform);
		}

		if (!m_Camera)
		{
			m_Camera = transform.FindChild("Main Camera").gameObject;
		}
		if (!m_DataStorage)
		{
			m_DataStorage = (GameObject)GameObject.Instantiate(pDataStorage);
			m_DataStorage.transform.SetParent(transform);
		}
	}


	//**関数***************************************************************************
	//	概要	:	各オブジェクト取得
	//*********************************************************************************
	// シーン管理
	public CSceneManager GetSceneManager()
	{
		return m_SceneManager.GetComponent<CSceneManager>();
	}

	// 入力管理
	public CInput GetInput()
	{
		return m_Input.GetComponent<CInput>();
	}
	
	public CDataStorage GetDataStorage()
	{
		return m_DataStorage.GetComponent<CDataStorage>();
	}

	//**関数***************************************************************************
	//	概要	:	カメラ取得
	//*********************************************************************************
	public Camera GetCamera()
	{
		return m_Camera.GetComponent<Camera>();
	}
}