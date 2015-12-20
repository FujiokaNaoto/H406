using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class CClockEffect : MonoBehaviour
{
	const float CHANGE = 100.0f;
	float fChange = CHANGE / 40;
	Twirl a;
	Vector3 v;

	void Start()
	{
		v = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0));
		a = gameObject.GetComponent<Twirl>();
		a.angle = 0;
		a.enabled = false;
	}
	void Update()
	{
		/*
		if (CManager.Instance.GetDataStorage().GetData((int)CDataStorage.DATACODE.CLOCK) == 1)
		{
			
			a.center.x = (float)((CManager.Instance.GetDataStorage().GetData((int)CDataStorage.DATACODE.CLOCK_X)) + (v.x / 2)) / v.x;
			a.center.y = (float)((CManager.Instance.GetDataStorage().GetData((int)CDataStorage.DATACODE.CLOCK_Y)) + (v.y / 2)) / v.y;
			//CManager.Instance.GetCamera().gameObject.GetComponent<CClockEffect>();

			a.enabled = true;
			//CManager.Instance.GetDataStorage().SetData(0, (int)CDataStorage.DATACODE.CLOCK);
		}
		*/

		if (a.enabled)
		{
			a.angle += fChange;
			if (a.angle >= CHANGE)
				fChange *= -1;
			if (a.angle < 0.0f)
			{
				a.angle = 0.0f;
				fChange *= -1;
				a.enabled = false;
			}
		}
	}


	public void Play(Vector2 vec)
	{
		/*
		a.center.x = (float) (vec.x + (v.x / 2)) / v.x;
		a.center.y = (float) (vec.x + (v.y / 2)) / v.y;
		*/

		vec = vec * 0.5f;
		a.center.x = (float)(vec.x + (v.x / 2)) / v.x;
		a.center.y = (float)(vec.y + (v.y / 2)) / v.y;

		a.enabled = true;

		CAudio.Instance.PlaySE(CAudio.SECODE.CLOCK_EFFECT);
	}
}
