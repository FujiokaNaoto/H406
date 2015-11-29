//**プログラムヘッダ***************************************************************
//	プログラム概要	:	メインシーンクラス
//*********************************************************************************
using UnityEngine;
using System.Collections;

public class CMain : CScene
{
	// --定数--
	readonly Vector3 TIMER_POS = new Vector3 (-7.25f , 4.0f , 0.0f);

	// --プレハブ宣言--
	public GameObject playerPrefab;
	public GameObject selectBoxPrefab;
	public GameObject fieldManagerPrefab;
	public GameObject readyPrefab;
	public GameObject timerPrefab;
	public GameObject ClockPrefab;
	public GameObject FieldEffectPre;

	// --変数宣言--
	private GameObject player;
	private GameObject selectBox;
	private GameObject fieldManager;
	private CReady ready;
	private CTimer timer;
	private CClock clock;
	private CResFieldEffect fieldEffect;


	//**関数***************************************************************************
	//	概要	:	変数初期化	:	外部とはアクセスしないこと
	//*********************************************************************************
	void Awake()
	{
	}


	//**関数***************************************************************************
	//	概要	:	初期化処理
	//*********************************************************************************
	public override bool Initialize()
	{
		// 使用するオブジェクト生成
		selectBox = (GameObject)GameObject.Instantiate(selectBoxPrefab);
		selectBox.GetComponent<CSelectBox>().Create(gameObject);

		player = (GameObject)GameObject.Instantiate(playerPrefab);
		player.GetComponent<CPieceController>().Create(gameObject , selectBox);

		fieldManager = (GameObject)GameObject.Instantiate(fieldManagerPrefab);
		fieldManager.transform.parent = transform;

		GameObject buf = (GameObject)GameObject.Instantiate(readyPrefab);
		ready = buf.GetComponent<CReady>();
		ready.transform.SetParent(transform);

		buf = (GameObject)GameObject.Instantiate(timerPrefab);
		timer = buf.GetComponent<CTimer>();
		timer.SetTimer();
		timer.transform.SetParent(transform);
		timer.transform.localPosition = TIMER_POS;

		buf = (GameObject)GameObject.Instantiate(ClockPrefab);
		clock = buf.GetComponent<CClock>();
		clock.transform.SetParent(transform);
		Vector3 vecBuf = clock.transform.localPosition;
		vecBuf.x += 2.0f;
		clock.transform.localPosition = vecBuf;


		// フィールドエフェクト
		buf = (GameObject)GameObject.Instantiate(FieldEffectPre);
		fieldEffect = buf.GetComponent<CResFieldEffect>();
		fieldEffect.Create(gameObject);

		return base.Initialize();
	}


	//**関数***************************************************************************
	//	概要	:	更新処理
	//*********************************************************************************
	void Update()
	{
		if (CManager.Instance.GetInput().GetKeyTrigger(KeyCode.Return))
			CManager.Instance.GetSceneManager().SetNextScene(CSceneManager.eSceneID.TITLE, CChanging.eChangeType.BLACK_FEAD);
			
		if (CInput.Instance.GetKeyTrigger(KeyCode.E))
			GetReady().SetReady((int)CReady.eReadyState.FINISH, 1.0f);


		if (CInput.Instance.GetKeyTrigger(KeyCode.C))
			clock.SetClock();

		if (CInput.Instance.GetKeyTrigger(KeyCode.T))
			timer.AddTime(3.0f);

		
	}


	//**関数***************************************************************************
	//	概要	:	レディ取得
	//*********************************************************************************
	public CReady GetReady() { return ready;}
}
