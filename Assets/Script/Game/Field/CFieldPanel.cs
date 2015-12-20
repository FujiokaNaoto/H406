using UnityEngine;
using System.Collections;

public class CFieldPanel : CPanel 
{
	public GameObject SetEffectPre;
	protected CSetEffect m_SetEffect;

	// TODO 子に建物のオブジェクトを持つ
	SpriteRenderer objectRenderer;
	SpriteRenderer groundRenderer;
	public Sprite [] objectSprites = new Sprite[(int)CPanel.EColor.MAX];
	public Sprite [] groundSprites = new Sprite[(int)CPanel.EColor.MAX];

	protected Color[] COLOR_TYPE = new Color[(int)EColor.MAX]
	{
		Color.gray, new Color(1.0f, 0.5f, 0.0f), new Color(0.5f, 1.0f, 1.0f)
	};

	public override int color
	{
		get { return base.color; }
		set {
			base.color = value; 
			//SetColor(COLOR_TYPE[(int)value]);
			//m_SetEffect.Play();
		}
	}



	protected virtual void Awake()
	{
		// エフェクト生成
		GameObject buf = (GameObject)GameObject.Instantiate(SetEffectPre);
		m_SetEffect = buf.GetComponent<CSetEffect>();
		m_SetEffect.transform.SetParent(transform);
		
		objectRenderer = transform.FindChild("Object").gameObject.GetComponent<SpriteRenderer>();
		groundRenderer = transform.FindChild("Ground").gameObject.GetComponent<SpriteRenderer>();
		for (int i = 0; i < (int)CPanel.EColor.MAX; ++i) {
			objectSprites[i] = null;
			groundSprites[i] = null;
		}
	}


	// 初期化処理
	void Start () 
	{
		objectRenderer.sprite = objectSprites[color];
        groundRenderer.sprite = groundSprites[color];
	}
	
	// 更新処理
	protected virtual void Update () {
		

	}
	
	public void SpriteUpdate() {
        objectRenderer.sprite = objectSprites[color];
		groundRenderer.sprite = groundSprites[color];
        m_SetEffect.Play();
		CAudio.Instance.PlaySE(CAudio.SECODE.SET_FLOWER);
    }
}
