using UnityEngine;
using System.Collections;

public class CFieldPanel : CPanel 
{
	public GameObject SetEffectPre;
	protected CSetEffect m_SetEffect;

    // TODO 子に建物のオブジェクトを持つ

	protected Color[] COLOR_TYPE = new Color[(int)EColor.MAX]
	{
		Color.gray , new Color(1.0f, 0.5f, 0.0f) , Color.red , 
		Color.green , new Color(0.5f, 1.0f, 1.0f) , Color.yellow
	};

	public override int color
	{
		get { return base.color; }
		set {
			base.color = value; 
			SetColor(COLOR_TYPE[(int)value]);
			m_SetEffect.Play(COLOR_TYPE[(int)value]);
		}
	}



    protected virtual void Awake()
	{
		// エフェクト生成
		GameObject buf = (GameObject)GameObject.Instantiate(SetEffectPre);
		m_SetEffect = buf.GetComponent<CSetEffect>();
		m_SetEffect.transform.SetParent(transform);
	}


    // 初期化処理
    void Start () 
	{
		
	}
	
	// 更新処理
	void Update () {


	}
}
