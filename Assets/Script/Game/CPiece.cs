//**プログラムヘッダ***************************************************************
//	プログラム概要	:	ピースクラス
//*********************************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CPiece : CObject {
	public const int PIECE_SIZE = 5;
	public int rootX = PIECE_SIZE / 2;
	public int rootY = PIECE_SIZE / 2;
	public CPanel.EColor[] colors = new CPanel.EColor[PIECE_SIZE * PIECE_SIZE];
	public GameObject [] panelPrefabs = new GameObject[(int)CPanel.EColor.MAX - 1];
	public CPanel root { get; private set; }
	
	// 初期化処理
	void Awake() {
		GameObject[,] objs = new GameObject[PIECE_SIZE, PIECE_SIZE];
		CPanel[,] panels = new CPanel[PIECE_SIZE, PIECE_SIZE];
		for (int x = 0; x < PIECE_SIZE; ++x) {
			for (int y = 0; y < PIECE_SIZE; ++y) {
				if (colors[x + PIECE_SIZE * y] == CPanel.EColor.NONE || panelPrefabs[(int)colors[x + PIECE_SIZE * y] - 1] == null)
					continue;
				objs[x, y] = Instantiate(panelPrefabs[(int)colors[x + PIECE_SIZE * y] - 1]);
				objs[x, y].transform.parent = transform;
				objs[x, y].transform.localPosition = new Vector3((float)x - rootX, (float)-y + rootY);
				panels[x, y] = objs[x, y].GetComponent<CPanel>();
			}
		}
		for (int x = 0; x < PIECE_SIZE; ++x) {
			for (int y = 0; y < PIECE_SIZE; ++y) {
				if (objs[x, y] == null) {
					continue;
				}
				if (x < PIECE_SIZE - 1) {
					panels[x, y].right = panels[x + 1, y];
				}
				if (y < PIECE_SIZE - 1) {
					panels[x, y].down = panels[x, y + 1];
				}
			}
		}
		root = panels[rootX, rootY];
	}

	/*
	// --定数--
	public enum eType
	{
		BLUE,ORANGE,MIX,
	};
	protected float PANEL_LENGE = 0.55f;
	
	// --公開変数--
	public int PIECE_NO;
	public GameObject	BLUE_PRE;
	public GameObject ORANGE_PRE;

	public eType TYPE_NAME;
	
	// --変数--
	protected List<CPanel> m_Children;
	protected CPanel m_Key;

	// Leap制御用
	private Vector3 m_oldPosition;
	private Vector3 m_newPosition;
	private float m_moveTime;
	private float m_moveSpeed;
	private float m_oldAngle;
	private float m_newAngle;
	private float m_rotTime;
	private float m_rotSpeed;


	//**関数***************************************************************************
	//	概要	:	初期化
	//*********************************************************************************
	void Start () 
	{
		m_Children = new List<CPanel>();

		m_oldPosition = m_newPosition = transform.position;
		m_moveTime = 1.0f;
		m_moveSpeed = 0.5f;

		if (PIECE_NO < CPieceLib.BLUE.Length && TYPE_NAME == eType.BLUE) 
			CreateBlue();			// 青ピース生成
		else if (PIECE_NO < CPieceLib.ORANGE.Length && TYPE_NAME == eType.ORANGE)
			CreateOrange();			// 黄色ピース生成
		else if (PIECE_NO < CPieceLib.MIX.Length && TYPE_NAME == eType.MIX)
			CreateMix();			// 多色ピース生成
	}


	//**関数***************************************************************************
	//	概要	:	更新
	//*********************************************************************************
	void Update () 
	{
		// リープ処理
		if (m_oldPosition != m_newPosition && m_moveTime < 1.0f)
		{
			Vector3 pos = transform.position;
			pos.x = Mathf.Lerp(m_oldPosition.x, m_newPosition.x, m_moveTime);
			pos.y = Mathf.Lerp(m_oldPosition.y, m_newPosition.y, m_moveTime);
			transform.position = pos;
			m_moveTime += Time.deltaTime / m_moveSpeed;
		}
		else if (m_oldPosition != m_newPosition)
		{
			Vector3 pos = transform.position;
			pos.x = m_newPosition.x;
			pos.y = m_newPosition.y;
			transform.position = pos;
			m_oldPosition = m_newPosition;		// 再リープ防止
		}

		// リープ処理
		if (m_oldAngle != m_newAngle && m_rotTime < 1.0f)
		{
			Vector3 angle = transform.eulerAngles;
			angle.z = Mathf.LerpAngle(m_oldAngle, m_newAngle, m_rotTime);
			transform.eulerAngles = angle;
			m_rotTime += Time.deltaTime / m_rotSpeed;
		}
		else if (m_oldAngle != m_newAngle)
		{
			Vector3 angle = transform.eulerAngles;
			angle.z = m_newAngle;
			transform.eulerAngles = angle;
			m_oldAngle = m_newAngle;		// 再リープ防止
		}
	}


	
	//**関数***************************************************************************
	//	概要	:	青ピース生成
	//*********************************************************************************
	protected bool CreateBlue()
	{
		CPanel[][] bufSrc = new CPanel[CPieceLib.HEIGHT][];
		GameObject buf;
		Vector3 posBuf = Vector3.zero;
		int nXCnt = 0, nYCnt = 0;

		// オブジェクト生成
		nYCnt = - CPieceLib.HEIGHT / 2;
		for (int i = 0; i < CPieceLib.HEIGHT; i++ , nYCnt ++)
		{
			bufSrc[i] = new CPanel[CPieceLib.WIDTH];
			posBuf.y = nYCnt * PANEL_LENGE;
			nXCnt = - CPieceLib.WIDTH / 2;

			for (int j = 0; j < CPieceLib.WIDTH; j++ , nXCnt ++)
			{
				if (PIECE_NO < CPieceLib.BLUE.Length &&
					CPieceLib.BLUE[PIECE_NO][i][j] <= 0) bufSrc[i][j] = null;
				else{
					buf = GameObject.Instantiate(BLUE_PRE);
					bufSrc[i][j] = buf.GetComponent<CPanel>();
					posBuf.x = nXCnt * PANEL_LENGE;
					bufSrc[i][j].transform.SetParent(transform);
					bufSrc[i][j].SetLocalPos(posBuf);
					m_Children.Add(bufSrc[i][j]);

					// 10以上のところは基準点とする
					if (CPieceLib.BLUE[PIECE_NO][i][j] >= 10) m_Key = bufSrc[i][j];
				}
			}
		}

		// 相互接続
		Connect(bufSrc);
		return true;
	}


	//**関数***************************************************************************
	//	概要	:	黄色ピース生成
	//*********************************************************************************
	protected bool CreateOrange()
	{
		CPanel[][] bufSrc = new CPanel[CPieceLib.HEIGHT][];
		GameObject buf;
		Vector3 posBuf = Vector3.zero;
		int nXCnt = 0, nYCnt = 0;

		// オブジェクト生成
		nYCnt = -CPieceLib.HEIGHT / 2;
		for (int i = 0; i < CPieceLib.HEIGHT; i++ , nYCnt ++)
		{
			bufSrc[i] = new CPanel[CPieceLib.WIDTH];
			posBuf.y = nYCnt * PANEL_LENGE;
			nXCnt = -CPieceLib.WIDTH / 2;

			for (int j = 0; j < CPieceLib.WIDTH; j++, nXCnt++)
			{
				if (PIECE_NO < CPieceLib.ORANGE.Length &&
					CPieceLib.ORANGE[PIECE_NO][i][j] <= 0) bufSrc[i][j] = null;
				else
				{
					buf = GameObject.Instantiate(ORANGE_PRE);
					bufSrc[i][j] = buf.GetComponent<CPanel>();
					posBuf.x = nXCnt * PANEL_LENGE;
					bufSrc[i][j].transform.SetParent(transform);
					bufSrc[i][j].SetLocalPos(posBuf);
					m_Children.Add(bufSrc[i][j]);

					// 10以上のところは基準点とする
					if (CPieceLib.ORANGE[PIECE_NO][i][j] >= 10) m_Key = bufSrc[i][j];
				}
			}
		}

		// 相互接続
		Connect(bufSrc);

		return true;
	}


	//**関数***************************************************************************
	//	概要	:	多色ピース生成
	//*********************************************************************************
	protected bool CreateMix()
	{
		CPanel[][] bufSrc = new CPanel[CPieceLib.HEIGHT][];
		GameObject buf;
		Vector3 posBuf = Vector3.zero;
		int nXCnt = 0, nYCnt = 0;

		// オブジェクト生成
		nYCnt = -CPieceLib.HEIGHT / 2;
		for (int i = 0; i < CPieceLib.HEIGHT; i++ , nYCnt ++)
		{
			bufSrc[i] = new CPanel[CPieceLib.WIDTH];
			posBuf.y = nYCnt * PANEL_LENGE;
			nXCnt = -CPieceLib.WIDTH / 2;

			for (int j = 0; j < CPieceLib.WIDTH; j++, nXCnt++)
			{
				if (PIECE_NO < CPieceLib.MIX.Length &&
					CPieceLib.MIX[PIECE_NO][i][j] <= 0) 
				{
					bufSrc[i][j] = null;
				}
				else if (CPieceLib.MIX[PIECE_NO][i][j] % 10 == 1)
				{
					buf = GameObject.Instantiate(BLUE_PRE);
					bufSrc[i][j] = buf.GetComponent<CPanel>();
					posBuf.x = nXCnt * PANEL_LENGE;
					bufSrc[i][j].transform.SetParent(transform);
					bufSrc[i][j].SetLocalPos(posBuf);
					m_Children.Add(bufSrc[i][j]);

					// 10以上のところは基準点とする
					if (CPieceLib.MIX[PIECE_NO][i][j] >= 10) m_Key = bufSrc[i][j];
				}
				else if (CPieceLib.MIX[PIECE_NO][i][j] % 10 == 2)
				{
					buf = GameObject.Instantiate(ORANGE_PRE);
					bufSrc[i][j] = buf.GetComponent<CPanel>();
					posBuf.x = nXCnt * PANEL_LENGE;
					bufSrc[i][j].transform.SetParent(transform);
					bufSrc[i][j].SetLocalPos(posBuf);
					m_Children.Add(bufSrc[i][j]);

					// 10以上のところは基準点とする
					if (CPieceLib.MIX[PIECE_NO][i][j] >= 10) m_Key = bufSrc[i][j];
				}
				else
					bufSrc[i][j] = null;
			}
		}

		// 相互接続
		Connect(bufSrc);

		return true;
	}


	//**関数***************************************************************************
	//	概要	:	パネル相互接続
	//*********************************************************************************
	protected void Connect(CPanel[][] array)
	{
		for (int i = 0; i < CPieceLib.HEIGHT; i++)
		{
			for (int j = 0; j < CPieceLib.WIDTH; j++)
			{
				if (array[i][j] == null) continue;

				if (array[i][j].up == null && i > 0 &&
					array[i - 1][j] != null)
				{
					array[i][j].up = array[i - 1][j];
				}

				if (array[i][j].right == null && j < array[i].Length - 1 &&
					array[i][j + 1] != null)
				{
					array[i][j].right = array[i][j + 1];
				}

				if (array[i][j].down == null && i < array.Length - 1 &&
					array[i + 1][j] != null)
				{
					array[i][j].down = array[i + 1][j];
				}

				if (array[i][j].left == null && j > 0 &&
					array[i][j - 1] != null)
				{
					array[i][j].left = array[i][j - 1];
				}
			}
		}
	}


	//**関数***************************************************************************
	//	概要	:	リープスタート
	//*********************************************************************************
	public void LeapStart(Vector3 newPos, float newRot , float spd)
	{
		m_oldPosition = transform.position;
		m_newPosition = newPos;
		m_moveTime = 0.0f;
		m_moveSpeed = spd;

		m_oldAngle = transform.eulerAngles.z;
		m_newAngle = newRot;
		m_rotTime = 0.0f;
		m_rotSpeed = spd;
	}


	//**関数***************************************************************************
	//	概要	:	基準パネル取得
	//*********************************************************************************
	public CPanel _dest {
		get {
			return m_Key;
		}
	}

	//**関数***************************************************************************
	//	概要	:	保持パネル一覧取得
	//*********************************************************************************
	public List<CPanel> _children{
		get {
			return m_Children;
		}
	}
	*/
}
