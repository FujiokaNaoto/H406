using UnityEngine;
using System.Collections;

public class CObjJudge : CObj2D {
	// 定数
	const float MOVETIME = 0.5f;
	const float SPRINK_TIME = 1.0f;

	const float TIMER = 0;

	public enum eDirection
	{
		RIGHTUP,
		RIGHTDOWN,
		LEFTUP,
		LEFTDOWN,
	};

	public enum eState
	{
		GOING,
		STAY,
		RETURN,
		DELETE,
	};

	private string[] animeName = new string[4]
		{"Left","Back","Front","Right"};

	protected Vector2 startpos;
	protected Vector2 endpos;
	protected float movetime;
	protected float timer;

	protected Vector2 m_vTargetPos;
	protected Vector2 m_vStartPos;

	private eDirection m_eDir;
	
	protected CFieldPanel fieldPanel;
	
	// Use this for initialization
	void Awake () {
	}
	
	// Update is called once per frame
	void Update () {

		switch (m_nState) {
			case (int)eState.GOING :
				AddLocalPos(CCommon.TimeUpdate(movetime) * (m_vTargetPos - m_vStartPos));
				timer += Time.deltaTime;// 
				if (timer > movetime)
				{
					m_nState = (int)eState.STAY;
					SetLocalPos(m_vTargetPos);
					timer = 0;
					movetime = SPRINK_TIME;
					// todo 次ステートアニメーションの開始
					SetStayAnimetion();
				}
				break;
			case (int)eState.STAY:
				timer += Time.deltaTime;
				if (timer > movetime)
				{
					m_nState = (int)eState.RETURN;
					timer = 0;
					movetime = MOVETIME;
					fieldPanel.SpriteUpdate();
					CFieldManager.Instance.IsSet = true;		// ピースセットを通知

					// todo 次ステートアニメーションの開始
					SetAnimetorBool();

					CAudio.Instance.PlaySE(CAudio.SECODE.WALK);
				}
				break;
			case (int)eState.RETURN:
				AddLocalPos(CCommon.TimeUpdate(movetime) * (m_vStartPos - m_vTargetPos));
				timer += Time.deltaTime;
				if (timer > movetime)
				{
					m_nState = (int)eState.DELETE;
					timer = TIMER;
					movetime = MOVETIME;
					// todo 次ステートアニメーションの開始
					Destroy(gameObject);
				}
				break;
			case (int)eState.DELETE:
				break;
		}
	}

	public virtual void Create(GameObject parent, Vector2 vStartPos, Vector2 vTargetPos , eDirection eDir, CFieldPanel panel)
	{
		base.Create(parent);
		fieldPanel = panel;
		SetLocalPos(vStartPos);// エラーが出たらbaseの上に
		m_vStartPos = vStartPos;
		m_vTargetPos = vTargetPos;
		timer = TIMER;
		movetime = MOVETIME;
		m_nState = (int)eState.GOING;
		m_eDir = eDir;
		Vector2 Rot = gameObject.transform.localScale;

		CAudio.Instance.PlaySE(CAudio.SECODE.WALK);

		// 以下アニメーションの設定
		if (eDir == eDirection.LEFTDOWN)
		{
			gameObject.GetComponent<Animator>().SetBool("Right", true);
			Rot.x *= -1;
		}
		else if(eDir == eDirection.LEFTUP)
		{
			gameObject.GetComponent<Animator>().SetBool("Front", true);
			Rot.x *= -1;
		}
		else if (eDir == eDirection.RIGHTDOWN)
		{
			gameObject.GetComponent<Animator>().SetBool("Back", true);
		}
		else if (eDir == eDirection.RIGHTUP)
		{
			gameObject.GetComponent<Animator>().SetBool("Left", true);
		}
		gameObject.transform.localScale = Rot;
	}

	private void SetAnimetorBool()
	{
		Vector2 Rot = gameObject.transform.localScale;
		gameObject.GetComponent<Animator>().SetBool("Stay", false);
		switch (m_eDir)
		{
			case eDirection.RIGHTUP:
				gameObject.GetComponent<Animator>().SetBool("Right", true);
				Rot.x *= -1;
				break;
			case eDirection.RIGHTDOWN:
				gameObject.GetComponent<Animator>().SetBool("Front", true);
				Rot.x *= -1;
				break;
			case eDirection.LEFTUP:
				gameObject.GetComponent<Animator>().SetBool("Back", true);
				break;
			case eDirection.LEFTDOWN:
				gameObject.GetComponent<Animator>().SetBool("Left", true);
				Rot.x *= -1;
				break;
		}
		gameObject.transform.localScale = Rot;
	}

	private void SetStayAnimetion()
	{
		gameObject.GetComponent<Animator>().SetBool(animeName[(int)m_eDir], false);
		gameObject.GetComponent<Animator>().SetBool("Stay", true);
	}
}
