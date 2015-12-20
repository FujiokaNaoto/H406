using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CClock : MonoBehaviour
{
	public GameObject pClockBoard;
	public GameObject pClockNeedle;

	GameObject clockBoard = null;
	CClockBoard objClockBoard = null;
	GameObject clockNeedle = null;
	CClockNeedle objClockNeedle = null;

	bool b = false;

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {
		if (b && !objClockNeedle.GetFlg())
			KillClock();
	}

	public void SetClock()
	{
		clockBoard = (GameObject)GameObject.Instantiate(pClockBoard);
		objClockBoard = clockBoard.GetComponent<CClockBoard>();
		objClockBoard.Create(gameObject);
		objClockBoard.SetFlg(true);
		objClockBoard.transform.SetParent(transform);
		objClockBoard.transform.localPosition = transform.localPosition;

		clockNeedle = (GameObject)GameObject.Instantiate(pClockNeedle);
		objClockNeedle = clockNeedle.GetComponent<CClockNeedle>();
		objClockNeedle.Create(gameObject);
		objClockNeedle.SetFlg(true);
		objClockNeedle.transform.SetParent(transform);
		objClockNeedle.transform.localPosition = transform.localPosition;

		b = true;

		/*
		CManager.Instance.GetDataStorage().SetData(1, (int)CDataStorage.DATACODE.CLOCK);
		CManager.Instance.GetDataStorage().SetData((int)transform.localPosition.x, (int)CDataStorage.DATACODE.CLOCK_X);
		CManager.Instance.GetDataStorage().SetData((int)transform.localPosition.y, (int)CDataStorage.DATACODE.CLOCK_Y);
		*/
		CManager.Instance.GetCamera().gameObject.GetComponent<CClockEffect>().Play((Vector2)transform.position);
	}

	public void KillClock()
	{
		Destroy(clockBoard);
		Destroy(objClockBoard);
		Destroy(clockNeedle);
		Destroy(objClockNeedle);
		clockBoard = null;
		objClockBoard = null;
		clockNeedle = null;
		objClockNeedle = null;
		b = false;
	}

	public bool GetFlg() { return b; }
}
