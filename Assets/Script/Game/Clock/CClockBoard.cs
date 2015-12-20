using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class CClockBoard : CObj2D
{
	const float SET_SCALE = 1.5f / 60;
	const float SET_ALPHA = 0.5f;
	// --変数宣言--
	Vector3 vs;
    float a = 0.0f,m = SET_ALPHA / 40, n = -SET_ALPHA / 40;
    bool b = false;
    float s = SET_SCALE, ds = SET_SCALE;

    //**関数***************************************************************************
    //	概要	:	変数初期化
    //*********************************************************************************
    void Awake()
    {
        vs.Set(s,s,1);
        transform.localScale = vs;
        SetColor_Alpha(a);
        Initialize();
    }


    //**関数***************************************************************************
    //	概要	:	初期処理
    //*********************************************************************************
    public override bool Initialize()
    {
        return true;
    }


    //**関数***************************************************************************
    //	概要	:	更新
    //*********************************************************************************
    public void Update()
    {
        if (b)
        {
            a += m;
            if (a >= SET_ALPHA)
            {
                m = n;
                ds *= -1;
            }
            if (a < 0.0f)
                b = false;

            s += ds;

            vs.Set(s, s, 1);
            transform.localScale = vs;

            SetColor_Alpha(a);
        }
    }
    public void SetFlg(bool flg) { b = flg; }
    public bool GetFlg() { return b; }
}
