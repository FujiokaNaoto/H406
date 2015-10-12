//**プログラムヘッダ***************************************************************
//	プログラム概要	:	入力管理
//*********************************************************************************
using UnityEngine;
using System.Collections;

public class CInput : MonoBehaviour 
{
	// --変数--
	// TODO カーソルとなるレクト情報を保持
	public GameObject InputColPrefab;
	private GameObject m_InputCol;

	// レクト情報を取得

	//**関数***************************************************************************
	//	概要	:	変数初期化
	//*********************************************************************************
	void Awake()
	{ 

	}


	//**関数***************************************************************************
	//	概要	:	更新
	//*********************************************************************************
	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			Vector3 vec = CManager.Instance.GetCamera().ScreenToWorldPoint(Input.mousePosition);
			vec.z = 0;
			m_InputCol.GetComponent<CObj2D>().SetLocalPos(vec);
		}

		if (Input.GetMouseButtonDown(0))
			m_InputCol.SetActive(true);

		else if (Input.GetMouseButtonUp(0))
			m_InputCol.SetActive(false);
		
	}

	//**関数***************************************************************************
	//	概要	:	開始
	//*********************************************************************************
	void Start()
	{
		m_InputCol = (GameObject)GameObject.Instantiate(InputColPrefab);
		m_InputCol.SetActive(false);
		m_InputCol.transform.SetParent(transform);
	}

	//**関数***************************************************************************
	//	概要	:	マウス情報取得
	//*********************************************************************************
	public bool GetMouseStay(int nButton)
	{
		if (Input.GetMouseButton(nButton))
			return true;

		return false;
	}
	public bool GetMouseTrigger(int nButton)
	{
		if (Input.GetMouseButtonDown(nButton))
			return true;

		return false;
	}
	public bool GetMouseRelease(int nButton)
	{
		if (Input.GetMouseButtonUp(nButton))
			return true;

		return false;
	}


	//**関数***************************************************************************
	//	概要	:	指定番目のタッチ情報取得
	//*********************************************************************************
	public bool GetTouchStay(int nTouchNo)
	{
		return false;
	}

	//**関数***************************************************************************
	//	概要	:	トリガーリリースのイベントが起こったときその番号を返す、なければ0
	//*********************************************************************************
	public int GetTouchTrigger()
	{
		return 0;
	}
	public int GetTouchRelease()
	{
		return 0;
	}


	//**関数***************************************************************************
	//	概要	:	キー入力情報取得（ただラップしただけ）
	//*********************************************************************************
	public bool GetKeyStay(KeyCode code)
	{
		if (Input.GetKey(code))
			return true;
		return false;
	}
	public bool GetKeyTrigger(KeyCode code)
	{
		if (Input.GetKeyDown(code))
			return true;
		return false;
	}
	public bool GetKeyRelease(KeyCode code)
	{
		if (Input.GetKeyUp(code))
			return true;
		return false;
	}


}
