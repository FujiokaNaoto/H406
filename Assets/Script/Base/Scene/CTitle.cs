//**プログラムヘッダ***************************************************************
//	プログラム概要	:	タイトルシーンクラス
//*********************************************************************************
using UnityEngine;
using System.Collections;

public class CTitle : CScene 
{
	// --定数宣言--
	readonly private Vector3 MIN_BUTTONSCALE = new Vector3(2.8f, 2.8f, 1.0f);
	readonly private Vector3 MAX_BUTTONSCALE = new Vector3(3.2f, 3.2f, 1.0f);
	readonly private float SCALE_SPD = 1.0f;

	// --変数宣言--
	public GameObject[] BGPrefab;						// 背景のプレハブ
	public GameObject LogoPrefab;						// ロゴプレハブ
	public GameObject ButtonPrefab;						// ボタンプレハブ

	private GameObject[] m_BG;							// 背景
	private GameObject m_Logo;							// ロゴ
	private GameObject m_Button;						// ボタンオブジェ
	private int m_nCuurentBG;


	//**関数***************************************************************************
	//	概要	:	変数初期化	:	外部とはアクセスしないこと
	//*********************************************************************************
	void Awake()
	{
		m_nCuurentBG = 0;
	}


	//**関数***************************************************************************
	//	概要	:	初期化処理
	//*********************************************************************************
	public override bool Initialize()
	{
		// 使用するオブジェクト生成
		m_BG = new GameObject[BGPrefab.Length];

		for (int i = 0; i < BGPrefab.Length; i++)
		{
			m_BG[i] = (GameObject)GameObject.Instantiate(BGPrefab[i]);
			m_BG[i].GetComponent<CObj2D>().Create(gameObject);

			if (i != 0)	m_BG[i].SetActive(false);
		}
		m_BG[0].GetComponent<TitleBG>().MoveStart(TitleBG.MOVE_RIGHT);

		m_Button = (GameObject)GameObject.Instantiate(ButtonPrefab);
		m_Button.GetComponent<CObj2D>().Create(gameObject);
		m_Button.GetComponent<TimeScaler>().SetTargetScale(MAX_BUTTONSCALE, SCALE_SPD, true);

		m_Logo = (GameObject)GameObject.Instantiate(LogoPrefab);
		m_Logo.GetComponent<CObj2D>().Create(gameObject);

		return base.Initialize();
	}


	//**関数***************************************************************************
	//	概要	:	更新処理
	//*********************************************************************************
	void Update()
	{
		// 背景の切り替わり
		if (m_BG[m_nCuurentBG].GetComponent<TitleBG>().GetState() == (int)TitleBG.eState.FEADOUT)
		{
			int nNextBG = m_nCuurentBG + 1;
			if (nNextBG >= m_BG.Length) nNextBG = 0;

			// 背景が動ききったら次に切替
			m_BG[nNextBG].SetActive(true);

			if (m_BG[m_nCuurentBG].GetComponent<TitleBG>().GetMoveDir() == TitleBG.MOVE_RIGHT)
				m_BG[nNextBG].GetComponent<TitleBG>().MoveStart(TitleBG.MOVE_LEFT);
			else if (m_BG[m_nCuurentBG].GetComponent<TitleBG>().GetMoveDir() == TitleBG.MOVE_LEFT)
				m_BG[nNextBG].GetComponent<TitleBG>().MoveStart(TitleBG.MOVE_RIGHT);
			
			//m_BG[m_nCuurentBG].SetActive(false);
			m_nCuurentBG = nNextBG;
		}

		// ボタンの拡大縮小、
		if (!m_Button.GetComponent<TimeScaler>().GetScaleFlg() && m_Button.transform.localScale.x <= MIN_BUTTONSCALE.x)
			m_Button.GetComponent<TimeScaler>().SetTargetScale(MAX_BUTTONSCALE, SCALE_SPD , true);
		else if (!m_Button.GetComponent<TimeScaler>().GetScaleFlg() && m_Button.transform.localScale.x >= MAX_BUTTONSCALE.x)
			m_Button.GetComponent<TimeScaler>().SetTargetScale(MIN_BUTTONSCALE, SCALE_SPD, true);

		// シーン遷移
		if (CInput.Instance.GetKeyTrigger(KeyCode.Return) || 
			CInput.Instance.GetJoyTrigger(4) || CInput.Instance.GetJoyTrigger(10))
			CManager.Instance.GetSceneManager().SetNextScene(CSceneManager.eSceneID.MAIN, CChanging.eChangeType.WHITE_FEAD);
	}

}
