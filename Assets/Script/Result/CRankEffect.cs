//**プログラムヘッダ***************************************************************
//	プログラム概要	:	ランクエフェクト
//*********************************************************************************
using UnityEngine;
using System.Collections;

public class CRankEffect : CObject 
{
	// --プレハブ--
	public GameObject Intensive1Pre;
	public GameObject Intensive2Pre;

	// --定数--
	public enum eClearRank
	{
		NONE,
		GOLD,
		SILVER,
		BRONZE,
	};
	readonly float INTENS_CHANGE_TIME = 0.048f;						// 集中線が切り替わる時間
	readonly float INTENS_LIMTIME = 1.0f;							// 集中線が表示される時間


	// --変数--
	protected GameObject[] m_Intensive = new GameObject[2];
	protected int m_nAnimCur;
	protected float m_fIntensTimer;
	protected eClearRank m_eClearRank;
	protected float m_fIntensLim;

	void Awake()
	{
		m_nAnimCur = 0;
		m_fIntensTimer = 0.0f;
		m_eClearRank = eClearRank.NONE;
		m_fIntensLim = 0.0f;

		// 集中線
		m_Intensive[0] = Instantiate(Intensive1Pre);
		m_Intensive[0].transform.SetParent(transform);
		m_Intensive[0].SetActive(false);
		m_Intensive[1] = Instantiate(Intensive2Pre);
		m_Intensive[1].transform.SetParent(transform);
		m_Intensive[1].SetActive(false);
	}

	//**関数***************************************************************************
	//	概要	:	クリアランクに応じたエフェクトの開始
	//*********************************************************************************
	public void Play(eClearRank eRank)
	{
		m_nAnimCur = 0;
		m_fIntensTimer = 0.0f;
		m_eClearRank = eRank;
		m_fIntensLim = 0.0f;

		GetComponent<ParticleSystem>().Stop();
		m_Intensive[0].SetActive(false);
		m_Intensive[1].SetActive(false);

		// 銅か金ランクの時は集中線エフェクトを出す
		if (m_eClearRank == eClearRank.BRONZE || m_eClearRank == eClearRank.GOLD)
			m_Intensive[m_nAnimCur].SetActive(true);

		// 銀か金ランクの時は光エフェクトを出す
		if (m_eClearRank == eClearRank.SILVER || m_eClearRank == eClearRank.GOLD)
			GetComponent<ParticleSystem>().Play();
	}

	void Update()
	{
		if (m_eClearRank == eClearRank.NONE || m_fIntensLim >= INTENS_LIMTIME) return;

		m_fIntensTimer += Time.deltaTime;
		m_fIntensLim += Time.deltaTime;

		// 集中線アニメーション
		if (m_fIntensTimer >= INTENS_CHANGE_TIME)
		{
			m_fIntensTimer = 0.0f;

			if (m_eClearRank == eClearRank.BRONZE || m_eClearRank == eClearRank.GOLD)
			{
				m_Intensive[m_nAnimCur].SetActive(false);
				m_nAnimCur ++;
				if (m_nAnimCur >= m_Intensive.Length)
					m_nAnimCur = 0;
				m_Intensive[m_nAnimCur].SetActive(true);
			}
		}

		// 集中線の表示切替
		if (m_fIntensLim <= INTENS_LIMTIME)
		{
			float fAlpha = 1.0f - (1.0f * (m_fIntensLim / INTENS_LIMTIME));
			m_Intensive[m_nAnimCur].GetComponent<CObj2D>().SetColor_Alpha(fAlpha);
		}
		else
		{
			for (int i = 0; i < m_Intensive.Length; i++)
				m_Intensive[i].SetActive(false);
		}
	}

}
