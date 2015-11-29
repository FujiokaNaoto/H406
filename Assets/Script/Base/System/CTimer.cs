using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CTimer : MonoBehaviour
{
	public GameObject pTimer;
	/*
	const float DEFAULT_TIME_DOWN = 30, DEFAULT_MINUTE_DOWN = 1,
		DEFAULT_TIME_UP = 0, DEFAULT_MINUTE_UP = 0;
	*/
	const float DEFAULT_TIME_DOWN = 120;
	const int MAX_TIME = 3;
	/*
	float time_up = DEFAULT_TIME_UP, time_down = DEFAULT_TIME_DOWN;
	float minute_up = DEFAULT_MINUTE_UP, minute_down = DEFAULT_MINUTE_DOWN;
	*/
	float time_down = DEFAULT_TIME_DOWN;
	bool flg_timer = true;
	GameObject[] Timer;
	CObj2D[] objTimer;
	//GameObject textTimer = null;

	// Use this for initialization
	void Awake()
	{
		Timer = new GameObject[MAX_TIME];
		objTimer = new CObj2D[MAX_TIME];

		for (int i = 0;i < MAX_TIME; i++)
		{
			Timer[i] = null;
			objTimer[i] = null;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (!flg_timer)
		{
			/*
			objTimer[0].SetAnimOneScene("FontNo", (int)minute_down % 10);
			objTimer[1].SetAnimOneScene("FontNo", (int)time_down / 10);
			*/
			objTimer[0].SetAnimOneScene("NoFont", (int)time_down / 100);
			objTimer[1].SetAnimOneScene("NoFont", (int)time_down / 10 % 10);
			objTimer[2].SetAnimOneScene("NoFont", (int)time_down % 10);
			//textTimer.GetComponent<Text>().text = (((int)minute_down).ToString() + "分" + ((int)time_down).ToString() + "秒");
			//textTimer.GetComponent<Text>().text = (((int)time_down).ToString() + "秒");

			time_down -= Time.deltaTime;
			/*
			time_up += Time.deltaTime;
			if (time_up >= 60)
			{
				minute_up++;
				time_up = 0;
			}

			if (time_down <= 0)
			{
				if (minute_down - 1 >= 0)
				{
					minute_down--;
					time_down = 60;
				}
			}
			*/
			//if (minute_down <= 0 && time_down <= 0)
			if (time_down <= 0)
			{
				flg_timer = true;
			}
		}
	}

	public void SetTimer()
	{
		for(int i = 0;i < MAX_TIME; i++)
		{
			Timer[i] = (GameObject)GameObject.Instantiate(pTimer);
			/*
			if (i == 0)
				Timer[i].transform.Translate(-1.0f,0.0f,0.0f);
			*/

			objTimer[i] = Timer[i].GetComponent<CObj2D>();
			objTimer[i].Create(gameObject);
			//Timer[i].transform.Translate(-1.0f + i, 0.0f, 0.0f);
			Timer[i].GetComponent<CObj2D>().AddLocalPos(-1.0f + i, 0.0f, 0.0f);
		}
		//textTimer = GameObject.Find("Text");
		flg_timer = false;
	}

	public float GetTimeDown()
	{
		return time_down;
	}
	/*
	public float GetTimeUp()
	{
		return time_up;
	}
	*/
	public bool GetEndFlag()
	{
		return flg_timer;
	}

	public void SetEndFlag(bool b)
	{
		flg_timer = b;
	}

	public void AddTime(float time)
	{
		time_down += time;
	}

	public void KillTimer()
	{
		time_down = DEFAULT_TIME_DOWN;
		/*
		time_up = DEFAULT_TIME_UP;
		minute_up = DEFAULT_MINUTE_UP;
		minute_down = DEFAULT_MINUTE_DOWN;
		*/
		flg_timer = true;

		for (int i = 0; i < MAX_TIME; i++)
		{
			Destroy(Timer[i]);
			Destroy(objTimer[i]);
			Timer[i] = null;
			objTimer[i] = null;
		}
		//textTimer = null;
	}
}