//**プログラムヘッダ***************************************************************
//	プログラム概要	:	メインシーンクラス
//*********************************************************************************
using UnityEngine;
using System.Collections;

public class CMain : CScene
{
	// --定数--
	

	// --プレハブ宣言--
	public GameObject playerPrefab;
	public GameObject selectBoxPrefab;
	public GameObject fieldManagerPrefab;
	public GameObject readyPrefab;
	public GameObject timerPrefab;
	public GameObject FieldEffectPre;
	public GameObject judgeManagerPrefab;
	public GameObject BGPrefab;

	// --変数宣言--
	private CPieceController player;
	private CSelectBox selectBox;
	private GameObject fieldManager;
	private CReady ready;
	private CTimer timer;
	private CResFieldEffect fieldEffect;
	private GameObject judgeManager;
	private GameObject bgObj;
	
	enum EMainPhase {
		READY_INIT,
		READY,
		GAME_INIT,
		GAME,
		GAME_OVER,
		RESULT_INIT,
		RESULT,
		PHASE_MAX
	};
	private EMainPhase phase;


	//**関数***************************************************************************
	//	概要	:	変数初期化	:	外部とはアクセスしないこと
	//*********************************************************************************
	void Awake()
	{
		phase = EMainPhase.READY_INIT;
	}


	//**関数***************************************************************************
	//	概要	:	初期化処理
	//*********************************************************************************
	public override bool Initialize()
	{
		// 使用するオブジェクト生成
		
		/*
		// 制限時間表示
		GameObject buf = (GameObject)GameObject.Instantiate(timerPrefab);
		timer = buf.GetComponent<CTimer>();
		timer.transform.SetParent(transform);
		*/
		
		// ピース選択枠
		GameObject buf = (GameObject)GameObject.Instantiate(selectBoxPrefab);
		selectBox = buf.GetComponent<CSelectBox>();
		selectBox.Create(gameObject);
		
		/*
		// ピースを保持するプレイヤー
		player = (GameObject)GameObject.Instantiate(playerPrefab);
		player.GetComponent<CPieceController>().Create(gameObject , selectBox);
		*/
		
		// フィールド生成
		fieldManager = (GameObject)GameObject.Instantiate(fieldManagerPrefab);
		fieldManager.transform.parent = transform;
		
		/*
		// レディゴー表示用
		buf = (GameObject)GameObject.Instantiate(readyPrefab);
		ready = buf.GetComponent<CReady>();
		ready.transform.SetParent(transform);
		*/
		
		// フィールドエフェクト
		buf = (GameObject)GameObject.Instantiate(FieldEffectPre);
		fieldEffect = buf.GetComponent<CResFieldEffect>();
		fieldEffect.Create(gameObject);
		
		// 背景
		buf = (GameObject)Instantiate(BGPrefab);
		buf.transform.SetParent(transform);

		Vector3 vecBuf = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
		buf = (GameObject)Instantiate(BGPrefab);
		buf.GetComponent<CObj2D>().AddLocalPos(vecBuf.x * 2.0f , 0.0f , 0.0f);
		buf.transform.SetParent(transform);
		buf = (GameObject)Instantiate(BGPrefab);
		buf.GetComponent<CObj2D>().AddLocalPos(vecBuf.x * - 2.0f, 0.0f, 0.0f);
		buf.transform.SetParent(transform);

		// 演出
		judgeManager = (GameObject)GameObject.Instantiate(judgeManagerPrefab);
		judgeManager.transform.SetParent(transform);

		CAudio.Instance.PlayBGM(CAudio.BGMCODE.GAME);

		return base.Initialize();
	}


	//**関数***************************************************************************
	//	概要	:	更新処理
	//*********************************************************************************
	void Update()
	{
		GameObject buf;
		switch (phase) {
			case EMainPhase.READY_INIT:
				buf = (GameObject)GameObject.Instantiate(readyPrefab);
				ready = buf.GetComponent<CReady>();
				ready.transform.SetParent(transform);
				phase = EMainPhase.READY;
				break;
			case EMainPhase.READY:
				if (ready && ready.GetTweenEnd())
					phase = EMainPhase.GAME_INIT;
				break;
			case EMainPhase.GAME_INIT:
				buf = (GameObject)GameObject.Instantiate(playerPrefab);
				player = buf.GetComponent<CPieceController>();
				player.Create(gameObject, selectBox.gameObject);

				buf = (GameObject)GameObject.Instantiate(timerPrefab);
				timer = buf.GetComponent<CTimer>();
				timer.transform.SetParent(transform);

				phase = EMainPhase.GAME;
				break;
			case EMainPhase.GAME:
				if (selectBox.isGameOver || timer.GetEndFlag()) {
					phase = EMainPhase.GAME_OVER;
				}
				break;
			case EMainPhase.GAME_OVER:
				player.isDestroy = true;
				//timer.SetEndFlag(true);
				timer.EndTimer();
				GetReady().SetReady((int)CReady.eReadyState.FINISH, 1.0f);
				Camera.main.GetComponent<CCamera>().LerpSize(10.0f);
				Camera.main.GetComponent<CCamera>().LerpPosition(fieldManager.transform.position);
				phase = EMainPhase.RESULT_INIT;
				break;
			case EMainPhase.RESULT_INIT:
				if (player == null)
					phase = EMainPhase.RESULT;
				break;
			case EMainPhase.RESULT:
				break;
		}
		
		
		// デバッグコード
		if (CManager.Instance.GetInput().GetKeyTrigger(KeyCode.Return))
			CManager.Instance.GetSceneManager().SetNextScene(CSceneManager.eSceneID.TITLE, CChanging.eChangeType.BLACK_FEAD);
			
		if (CInput.Instance.GetKeyTrigger(KeyCode.E))
			GetReady().SetReady((int)CReady.eReadyState.FINISH, 1.0f);
		
	}


	//**関数***************************************************************************
	//	概要	:	レディ取得
	//*********************************************************************************
	public CReady GetReady() { return ready;}


	//**関数***************************************************************************
	//	概要	:	タイマー取得
	//*********************************************************************************
	public CTimer GetTimer() { return timer; }

}
