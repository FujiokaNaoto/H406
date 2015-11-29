//**プログラムヘッダ***************************************************************
//	プログラム概要	:	リザルト用、フィールドエフェクトまとめ
//*********************************************************************************
using UnityEngine;
using System.Collections;

public class CResFieldEffect : CObject
{
	// --プレハブ--
	public GameObject patel01Pre;
	public GameObject leaf03Pre;
	public GameObject patel03Pre;

	void Awake()
	{
		GameObject buf;
		// 花びら01生成
		buf = (GameObject)GameObject.Instantiate(patel01Pre);
		buf.transform.SetParent(transform);

		// 花びら03生成
		buf = (GameObject)GameObject.Instantiate(patel03Pre);
		buf.transform.SetParent(transform);

		// 葉03生成
		buf = (GameObject)GameObject.Instantiate(leaf03Pre);
		buf.transform.SetParent(transform);
	}
}
