//**プログラムヘッダ***************************************************************
//	プログラム概要	:	噴水エフェクト
//*********************************************************************************
using UnityEngine;
using System.Collections;

public class CFountPanel : CFieldPanel
{
	public GameObject FountEffectPre;
	protected ParticleSystem m_FountEffect;

	protected bool m_bOnce;


	//**関数***************************************************************************
	//	概要	:	色情報セット
	//*********************************************************************************
	public override int color
	{
		get { return base.color; }
		set
		{
			base.color = value;
			m_FountEffect.Play();
			m_bOnce = true;
		}
	}

	//**関数***************************************************************************
	//	概要	:	生成
	//*********************************************************************************
	protected override void Awake() 
	{
		GameObject buf = (GameObject)GameObject.Instantiate(FountEffectPre);
		m_FountEffect = buf.GetComponent<ParticleSystem>();
		m_FountEffect.transform.SetParent(transform);
		m_bOnce = false;

		base.Awake();
	}


	//**関数***************************************************************************
	//	概要	:	噴水回転
	//*********************************************************************************
	protected override void Update()
	{
		base.Update();

		// 何かしらの色が噴水についていたら周辺を回転
		if (base.color == (int)EColor.NONE || !CFieldManager.Instance.IsSet)
			return;

		if (m_bOnce)
		{
			CFieldManager.Instance.IsSet = false;
			m_bOnce = false;
			return;
		}

		CFieldManager.Instance.IsSet = false;

		// 影響下にあるパネルの色情報を回転
		int nBufA = (int)EColor.NONE;
		int nBufB = (int)EColor.NONE;

		// 右から右下
		nBufA = right.down.color;
		right.down.color = right.color;
		((CFieldPanel)right.down).SpriteUpdate();

		// 右下から下
		nBufB = down.color;
		down.color = nBufA;
		((CFieldPanel)down).SpriteUpdate();

		// 下から左下
		nBufA = down.left.color;
		down.left.color = nBufB;
		((CFieldPanel)down.left).SpriteUpdate();

		// 左下から左
		nBufB = left.color;
		left.color = nBufA;
		((CFieldPanel)left).SpriteUpdate();

		// 左から左上
		nBufA = left.up.color;
		left.up.color = nBufB;
		((CFieldPanel)left.up).SpriteUpdate();

		// 左上から上
		nBufB = up.color;
		up.color = nBufA;
		((CFieldPanel)up).SpriteUpdate();

		// 上から右上
		nBufA = up.right.color;
		up.right.color = nBufB;
		((CFieldPanel)up.right).SpriteUpdate();

		// 右上から右
		right.color = nBufA;
		((CFieldPanel)right).SpriteUpdate();
	}
	
}
