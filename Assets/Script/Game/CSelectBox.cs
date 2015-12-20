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

	public bool isGameOver { get; private set; }

	// 初期化処理
	void Awake() {
		isGameOver = false;
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
		for (int i = 0; i < SELECT_NUM; ++i) 
		{
			if (pieces[i] != null) continue;

			GameObject obj = Instantiate(piecePrefabs[Random.Range(0, piecePrefabs.Length)]);
			pieces[i] = obj.GetComponent<CPiece>();
			pieces[i].transform.parent = transform;
			pieces[i].transform.position = refPoints[i].transform.position;
			pieces[i].transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
			isInstantiate = true;

			CAudio.Instance.PlaySE(CAudio.SECODE.PIECE_ADD);
		}

		// 終了判定
		if (isInstantiate) {
			if (CFieldManager.Instance.QueryGameOver(pieces))
				isGameOver = true;
		}
	}
}
