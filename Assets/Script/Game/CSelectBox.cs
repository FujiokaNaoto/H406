//**プログラムヘッダ***************************************************************
//	プログラム概要	:	選択肢保持ブロック
//*********************************************************************************
using UnityEngine;
using System.Collections;

public class CSelectBox : CObj2D
{
    public const int SELECT_NUM = 3;
    [SerializeField]
    public GameObject [] piecePrefabs;
    [HideInInspector]
    public Vector3[] refPositions = new Vector3[SELECT_NUM] {
        new Vector3(0.0f, 3.0f), new Vector3(0.0f, 0.0f), new Vector3(0.0f, -3.0f),
    };
    [HideInInspector]
    public GameObject [] refPoints = new GameObject[SELECT_NUM];
    [HideInInspector]
    public CPiece [] pieces = new CPiece[SELECT_NUM];

    // 初期化処理
    void Awake() {
        for (int i = 0; i < SELECT_NUM; ++i) {
            refPoints[i] = new GameObject("refPoint");
            refPoints[i].transform.parent = transform;
            refPoints[i].transform.localPosition = refPositions[i];
            refPoints[i].transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
        }
        for (int i = 0; i < SELECT_NUM; ++i) {
            GameObject obj = Instantiate(piecePrefabs[Random.Range(0, piecePrefabs.Length)]);
            pieces[i] = obj.GetComponent<CPiece>();
            pieces[i].transform.parent = transform;
            pieces[i].transform.position = refPoints[i].transform.position;
            pieces[i].transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
        }
    }

    // 更新処理
    void Update() {
        bool isInstantiate = false;

        // ピースの追加
        for (int i = 0; i < SELECT_NUM; ++i) {
            if (pieces[i] != null) continue;

            GameObject obj = Instantiate(piecePrefabs[Random.Range(0, piecePrefabs.Length)]);
            pieces[i] = obj.GetComponent<CPiece>();
            pieces[i].transform.parent = transform;
            pieces[i].transform.position = refPoints[i].transform.position;
            pieces[i].transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
            isInstantiate = true;
        }

        // 終了判定
        if (isInstantiate) {
            if (CFieldManager.Instance.QueryGameOver(pieces))
            {
                // ゲームオーバー時の処理
                Debug.Log("終了");
            }
            else
                Debug.Log("まだ終わってない");
        }
    }

    /*
	// --プレハブ--
	// ピースのプレハブ、色毎に配列で持つ
	public GameObject[] PieceOrangePre;
	public GameObject[] PieceBluePre;
	public GameObject[] PieceMixPre;

	// --定数宣言--
	public enum eSelect
	{
		ORANGE,
		BLUE,
		MIX,
		MAX,
	};

	protected const float MOVE_SPD = 0.25f;
	
	// 選択肢の基準位置
	public static readonly Vector3[] KEY_POS = new Vector3[(int)eSelect.MAX] {
		new Vector3(6.35f,3.25f,0.0f), new Vector3(6.35f, 0.0f, 0.0f), new Vector3(6.35f ,-3.25f ,0.0f),
	};

	// --変数宣言--
	private GameObject[][] m_PreArray = new GameObject[(int)eSelect.MAX][];				// 選択肢プレハブの配列
	private GameObject[] m_Select = new GameObject[(int)eSelect.MAX];



	//**関数***************************************************************************
	//	概要	:	初期化
	//*********************************************************************************
	void Awake() 
	{
		m_PreArray[(int)eSelect.ORANGE] = PieceOrangePre;
		m_PreArray[(int)eSelect.BLUE] = PieceBluePre;
		m_PreArray[(int)eSelect.MIX] = PieceMixPre;

		// 初期選択肢表示
		for (int i = 0; i < m_Select.Length; i++)
		{
			m_Select[i] = (GameObject)GameObject.Instantiate(m_PreArray[i][Random.Range(0, m_PreArray[i].Length)]);
			
			// TODO ピーススクリプト生成後、親追加処理を修正
			m_Select[i].transform.localPosition = KEY_POS[i];
			m_Select[i].transform.SetParent(transform);
		}
	}


	//**関数***************************************************************************
	//	概要	:	更新
	//*********************************************************************************
	void Update () 
	{
		
	}


	//**関数***************************************************************************
	//	概要	:	選択肢からオブジェクト取得
	//*********************************************************************************
	public CPiece GetSelectPiece(int nNo)
	{
		// 配列要素外を参照した場合は値を返さない
		if (nNo >= m_Select.Length || nNo < 0) return null;

		// 選ばれているオブジェクトを渡す
		GameObject obj = m_Select[nNo];
		m_Select[nNo] = null;

		return obj.GetComponent<CPiece>();
	}


	//**関数***************************************************************************
	//	概要	:	選択肢のオブジェクト参照
	//*********************************************************************************
	public CPiece RefSelectPiece(int nNo)
	{
		// 配列要素外を参照した場合は値を返さない
		if (nNo >= m_Select.Length || nNo < 0) return null;
		return m_Select[nNo].GetComponent<CPiece>();
	}


	//**関数***************************************************************************
	//	概要	:	選択肢へオブジェクトを戻す
	//*********************************************************************************
	public bool SetSelectPiece(CPiece piece, int nNo)
	{
		if (nNo >= m_Select.Length || nNo < 0) return false;

		// TODO 親オブジェクトに登録し直し
		m_Select[nNo] = piece.gameObject;
		//m_Select[nNo].transform.localPosition = KEY_POS[nNo];
		piece.LeapStart(KEY_POS[nNo] , 0.0f, MOVE_SPD);
		m_Select[nNo].transform.SetParent(transform);

		return true;
	}


	//**関数***************************************************************************
	//	概要	:	選択肢更新、プレイヤーがパネル配置後選択肢を新しく増やす
	//*********************************************************************************
	public bool RenewSelect(int nNo)
	{
		if (nNo >= m_Select.Length || nNo < 0) return false;

		m_Select[nNo] = (GameObject)GameObject.Instantiate(m_PreArray[nNo][Random.Range(0, m_PreArray[nNo].Length)]);

		return true;
	}
    */
}
