//**プログラムヘッダ***************************************************************
//	プログラム概要	:	時計台パネル
//*********************************************************************************
using UnityEngine;
using System.Collections;

public class ClockPanel : CFieldPanel 
{
	// --定数--
	public readonly int ADD_TIME = 5;

	// --プレハブ--
	public GameObject clockEffectPre;

	// --変数--
	protected CClock m_ClockEffect;
	protected CTimer m_TimerScr;



	//**関数***************************************************************************
	//	概要	:	色情報セット
	//*********************************************************************************
	public override int color
	{
		get { return base.color; }
		set
		{
			base.color = value;
			m_ClockEffect.SetClock();				// エフェクト
			m_TimerScr = CSceneManager.Instance.GetSceneObj(CSceneManager.eSceneID.MAIN).GetComponent<CMain>().GetTimer();
			m_TimerScr.AddTime(ADD_TIME);			// タイマ加算
		}
	}


	//**関数***************************************************************************
	//	概要	:	生成時処理
	//*********************************************************************************
	protected override void Awake()
	{
		GameObject buf = (GameObject)Instantiate(clockEffectPre);
		buf.transform.SetParent(transform);
		m_ClockEffect = buf.GetComponent<CClock>();

		m_TimerScr = null;

		base.Awake();
	}
}
