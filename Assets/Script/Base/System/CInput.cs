//**プログラムヘッダ***************************************************************
//	プログラム概要	:	入力管理
//*********************************************************************************
using UnityEngine;
using System.Collections;

public class CInput : CSingleton<CInput> 
{
	// --変数--
	// TODO カーソルとなるレクト情報を保持
	public GameObject InputColPrefab;
	private GameObject m_InputCol;


	// ゲームパッドアナログスティック
	private Vector2 m_OldLJoySti;
	private Vector2 m_CurLJoySti;
	private Vector2 m_OldRJoySti;
	private Vector2 m_CurRJoySti;



	//**関数***************************************************************************
	//	概要	:	変数初期化
	//*********************************************************************************
	void Awake()
	{
		if (this != Instance)
		{
			Destroy(this);
			return;
		}
		DontDestroyOnLoad(this.gameObject);

		m_OldLJoySti = m_OldRJoySti = m_CurLJoySti = m_CurRJoySti = Vector2.zero;
	}


	//**関数***************************************************************************
	//	概要	:	更新
	//*********************************************************************************
	void Update()
	{
		m_OldRJoySti = m_CurRJoySti;

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
	//	概要	:	ゲームパッド情報取得
	//*********************************************************************************
	public bool GetJoyStay(int nButton)
	{
		if (Input.GetButton("Joy" + nButton.ToString()))
			return true;
		
		return false;
		
	}
	public bool GetJoyTrigger(int nButton)
	{
		if (Input.GetButtonDown("Joy" + nButton.ToString()))
			return true;
	
		return false;
	}
	public bool GetJoyRelease(int nButton)
	{
		if (Input.GetButtonUp("Joy" + nButton.ToString()))
			return true;
	
		return false;
	}

	public Vector2 GetJoyRStick()
	{
		//m_OldRJoySti = m_CurRJoySti;
		m_CurRJoySti.x = Input.GetAxis("JoyRHorizontal");
		m_CurRJoySti.y = Input.GetAxis("JoyRVertical");
		return m_CurRJoySti;
	}

	public Vector2 GetJoyLStick()
	{
		//m_OldLJoySti = m_CurLJoySti;
		m_CurLJoySti.x = Input.GetAxis("JoyLHorizontal");
		m_CurLJoySti.y = Input.GetAxis("JoyLVertical");
		return m_CurLJoySti;
	}

	public Vector2 GetOldLStick
	{
		get {
			return m_OldLJoySti;
		}
	}
	public Vector2 GetOldRStick
	{
		get
		{
			return m_OldRJoySti;
		}
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
