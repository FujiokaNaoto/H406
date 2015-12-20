using UnityEngine;
using System.Collections;

public class CJudgeManager : CSingleton<CJudgeManager>
{
	readonly Vector2 RIGHT_UP = new Vector2(4,6);
	readonly Vector2 RIGHT_DOWN = new Vector2(7, -6);
	readonly Vector2 LEFT_UP = new Vector2(-10, 6);
	readonly Vector2 LEFT_DOWN = new Vector2(-10, -6);
	
	public int StateNum;				// 現在の状態

	public GameObject jijiPrefab;
	public GameObject babaPrefab;
	protected Vector2 m_vFieldPos;


	// Use this for initialization
	void Awake () {
		if (this != Instance)
		{
			Destroy(this);
			return;
		}
	}

	void Start()
	{
		m_vFieldPos = CFieldManager.Instance.root.transform.position;
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void SetTargetPosition(CPanel panel)
	{
		Vector2 vPos = panel.transform.position;
		GameObject obj;
		
		int nRand = Random.Range(0, 2);

		if (nRand == 0)
		{
			obj = (GameObject)GameObject.Instantiate(jijiPrefab);
		}
		else
		{
			obj = (GameObject)GameObject.Instantiate(babaPrefab);
		}

		// 右に配置
		if(m_vFieldPos.x < vPos.x)
		{
			// 上に配置(右上)
			if(m_vFieldPos.y < vPos.y)
			{
				obj.GetComponent<CObjJudge>().Create(gameObject, RIGHT_UP, vPos, CObjJudge.eDirection.RIGHTUP, (CFieldPanel)panel);
			}
			// 下に配置(右下)
			else
			{
				obj.GetComponent<CObjJudge>().Create(gameObject, RIGHT_DOWN, vPos, CObjJudge.eDirection.RIGHTDOWN, (CFieldPanel)panel);

			}
		}
		else
		{
			// 上に配置(左上)
			if (m_vFieldPos.y < vPos.y)
			{
				obj.GetComponent<CObjJudge>().Create(gameObject, LEFT_UP, vPos, CObjJudge.eDirection.LEFTUP, (CFieldPanel)panel);

			}
			// 下に配置(左下)
			else
			{
				obj.GetComponent<CObjJudge>().Create(gameObject, LEFT_DOWN, vPos, CObjJudge.eDirection.LEFTDOWN, (CFieldPanel)panel);
			}
		}
	}
}
