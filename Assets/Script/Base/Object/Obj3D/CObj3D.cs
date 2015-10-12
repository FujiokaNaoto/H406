using UnityEngine;
using System.Collections;

public class CObj3D : CObject 
{
	//**関数***************************************************************************
	//	概要	:	生成時処理
	//*********************************************************************************
	public override bool Create(GameObject parent)
	{
		if (Initialize())
			return base.Create(parent);

		return base.Create(parent);
	}
	public override bool Create(GameObject parent, Vector3 Localpos)
	{
		if (Initialize())
			return base.Create(parent, Localpos);

		return base.Create(parent, Localpos);
	}
	public override bool Create(GameObject parent, Vector3 Localpos, Vector3 Localrot, Vector3 Localscale)
	{
		if (Initialize())
			return base.Create(parent, Localpos, Localrot, Localscale);

		return base.Create(parent, Localpos, Localrot, Localscale);
	}


	//**関数***************************************************************************
	//	概要	:	初期化
	//*********************************************************************************
	public virtual bool Initialize()
	{
		
		return true;
	}

	void Update()
	{
		if (CCommon.GetPauseFlg ()) return;
	}
}
