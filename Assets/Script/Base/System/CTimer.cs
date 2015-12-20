using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CTimer : CObj2D
{
	readonly Vector3 TIMER_POS = new Vector3(-0.25f, 0.0f, 0.0f);
	
	public GameObject pTimer;
	public GameObject SetEffectPre;
	protected CSetEffect[] m_SetEffect;
	
	const float DEFAULT_TIME_DOWN = 120;
	const int MAX_TIME = 3;
	const float ADD_TIME = 0.05f;
	const float DIGIT_LENGE = 0.75f;			// 桁の間隔
	
	float time_down = DEFAULT_TIME_DOWN;
	bool flg_timer = true;
	bool flg_Add = false;
	bool flg_End = false;
	float fAdd = 0.0f;
	float oldTime = 0.0f;
	GameObject[] Timer;
	CObj2D[] objTimer;
	
	void Awake()
	{
		Timer = new GameObject[MAX_TIME];
		objTimer = new CObj2D[MAX_TIME];

		for (int i = 0;i < MAX_TIME; i++)
		{
			Timer[i] = null;
			objTimer[i] = null;
		}
		
		SetTimer();
		
		GameObject[] buf;
		buf = new GameObject[MAX_TIME];
		m_SetEffect = new CSetEffect[MAX_TIME];
        for (int i = 0;i < MAX_TIME; i++)
		{
			buf[i]	= (GameObject)GameObject.Instantiate(SetEffectPre);
			m_SetEffect[i] = buf[i].GetComponent<CSetEffect>();
			m_SetEffect[i].transform.SetParent(transform);
			Vector3 v = new Vector3(i * DIGIT_LENGE + TIMER_POS.x, 0.0f + TIMER_POS.y, TIMER_POS.z);
			m_SetEffect[i].transform.localPosition = v;
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		if (!flg_timer)
		{
			objTimer[0].SetAnimOneScene("NoFont", (int)time_down / 100);
			objTimer[1].SetAnimOneScene("NoFont", (int)time_down / 10 % 10);
			objTimer[2].SetAnimOneScene("NoFont", (int)time_down % 10);
			
			time_down -= Time.deltaTime;

			if (time_down <= 0)
			{
				flg_End = true;
				EndTimer();
			}
		}
		if (flg_Add)
		{
			if (fAdd > 0.0f)
			{
				oldTime = time_down;
				time_down += ADD_TIME;
				fAdd -= ADD_TIME;
				if ((int)time_down / 100 - (int)oldTime / 100 > 0)
					m_SetEffect[0].Play();
				if ((int)time_down / 10 % 10 - (int)oldTime / 10 % 10 > 0)
					m_SetEffect[1].Play();
				objTimer[0].SetAnimOneScene("NoFont", (int)time_down / 100);
				objTimer[1].SetAnimOneScene("NoFont", (int)time_down / 10 % 10);
				objTimer[2].SetAnimOneScene("NoFont", (int)time_down % 10);
			}
			else
			{
				flg_Add = false;
				flg_timer = false;
			}
		}
	}

	public void SetTimer()
	{
		for(int i = 0;i < MAX_TIME; i++)
		{
			Timer[i] = (GameObject)GameObject.Instantiate(pTimer);
			objTimer[i] = Timer[i].GetComponent<CObj2D>();
			objTimer[i].Create(gameObject);

			Vector3 v = new Vector3(i * DIGIT_LENGE + TIMER_POS.x, 0.0f + TIMER_POS.y, TIMER_POS.z);
			Timer[i].transform.localPosition = v;
		}
		flg_timer = false;
	}

	public void AddTime(float time)
	{
		fAdd = time;
		flg_Add = true;
		flg_timer = true;
		m_SetEffect[2].Play();
	}

	public void KillTimer()
	{
		time_down = DEFAULT_TIME_DOWN;

		flg_timer = true;

		for (int i = 0; i < MAX_TIME; i++)
		{
			Destroy(Timer[i]);
			Destroy(objTimer[i]);
			Timer[i] = null;
			objTimer[i] = null;
		}
	}

	public void EndTimer()
	{
		time_down = 0;

		flg_timer = true;

		objTimer[0].SetAnimOneScene("NoFont", (int)time_down / 100);
		objTimer[1].SetAnimOneScene("NoFont", (int)time_down / 10 % 10);
		objTimer[2].SetAnimOneScene("NoFont", (int)time_down % 10);
	}

	public float GetTimeDown() { return time_down; }
	public bool GetEndFlag() { return flg_End; }
	public void StartTimer() { flg_timer = false; }
	public void StopTimer() { flg_timer = true; }
}