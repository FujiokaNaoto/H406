using UnityEngine;
using System.Collections;

public class CClockNeedle : CObj2D
{
    const float SET_SCALE = 1.5f / 60;
    // --変数宣言--
    Vector3 vr,v,vs;
    float a = 0.0f, m = 1f / 60, n = -1f / 60;
    bool b = false;
    float s = SET_SCALE, ds = SET_SCALE;

    //**関数***************************************************************************
    //	概要	:	変数初期化
    //*********************************************************************************
    void Awake()
    {
        vs.Set(s, s, 1);
        v.Set(0, 0, 0);
        vr.Set(0,0,6);
        transform.localScale = vs;
        transform.TransformVector(v);
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
            if (a >= 1.0f)
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
            transform.Rotate(vr);
        }
    }

    public void SetFlg(bool flg) { b = flg; }
    public bool GetFlg() { return b; }
}
