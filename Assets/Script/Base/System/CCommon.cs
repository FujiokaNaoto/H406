using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CCommon : MonoBehaviour 
{
	private static bool m_bPause;

	void Awake()
	{
		m_bPause = false;
		DOTween.Init();
	}


	//**プログラムヘッダ***************************************************************
	//	プログラム概要	:	時間に応じた更新
	//					:	引数に変化にかけたい総時間を送ると、前回から今回のフレームにかかった時間に応じた割合を返す
	//*********************************************************************************
	public static float TimeUpdate(float fTotalTime)
	{
		return Time.deltaTime / fTotalTime;
	}


	//**プログラムヘッダ***************************************************************
	//	プログラム概要	:	ポーズ状態管理
	//*********************************************************************************
	public static void SetPause(bool bFlg)
	{
		m_bPause = bFlg;
	}
	public static bool GetPauseFlg(){return m_bPause;}


	//**関数***************************************************************************
	//	概要	:	スプライト取得
	//*********************************************************************************
	public static Sprite GetSprite(string FilePass , string spriteName)
	{
		Sprite[] sprites = Resources.LoadAll<Sprite>(FilePass);
		return System.Array.Find(sprites, (sprite) => sprite.name.Equals(spriteName));
	}
}