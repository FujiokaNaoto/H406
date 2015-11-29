using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CObject : MonoBehaviour 
{
	//static List<> m_ObjList;
	private static uint m_nUniqueID;
	protected uint 	m_nObjID = 0;
	protected int m_nClassID;
	protected int m_nState;
	protected bool m_bUseFlg;

	void Awake()
	{
		m_nUniqueID++;
		m_nObjID = m_nUniqueID;
		m_nState = 0;
		m_bUseFlg = true;
	}


	//**関数***************************************************************************
	//	概要	:	生成
	//*********************************************************************************
	public virtual bool Create(GameObject parent)
	{
		if (parent != null)
			transform.SetParent(parent.transform);
		return true;
	}
	public virtual bool Create(GameObject parent , Vector3 Localpos)
	{
		if (parent != null)
			transform.SetParent(parent.transform);
		SetLocalPos(Localpos);
		return true;
	}
	public virtual bool Create(GameObject parent, Vector3 Localpos, Vector3 Localrot, Vector3 Localscale)
	{
		if(parent != null)
			transform.SetParent(parent.transform);
		SetLocalPos(Localpos);
		SetLocalRot(Localrot);
		SetLocalScale(Localscale);
		return true;
	}
	
	void Update () 
	{
		if (CCommon.GetPauseFlg ()) return;
	}

	public int GetClassID()
	{
		return m_nClassID;
	}

	public uint GetObjID()
	{
		return m_nObjID;
	}

	//**関数***************************************************************************
	//	概要	:	位置代入
	//*********************************************************************************
	public Vector3 SetLocalPos(Vector3 vec)
	{
		Vector3 buf = gameObject.transform.localPosition;
		buf = vec;
		gameObject.transform.localPosition = buf;
		return buf;
	}
	public Vector3 SetLocalPos(float x , float y , float z)
	{
		Vector3 buf = gameObject.transform.localPosition;
		buf.x = x;
		buf.y = y;
		buf.z = z;
		gameObject.transform.localPosition = buf;
		return buf;
	}


	//**関数***************************************************************************
	//	概要	:	位置加算
	//*********************************************************************************
	public Vector3 AddLocalPos(Vector3 vec)
	{
		Vector3 buf = gameObject.transform.localPosition;
		buf += vec;
		gameObject.transform.localPosition = buf;
		return buf;
	}
	public Vector3 AddLocalPos(float x , float y , float z)
	{
		Vector3 buf = gameObject.transform.localPosition;
		buf.x += x;
		buf.y += y;
		buf.z += z;
		gameObject.transform.localPosition = buf;
		return buf;
	}


	//**関数***************************************************************************
	//	概要	:	位置代入
	//*********************************************************************************
	public Vector3 SetPos(Vector3 vec)
	{
		Vector3 buf = gameObject.transform.position;
		buf = vec;
		gameObject.transform.position = buf;
		return buf;
	}
	public Vector3 SetPos(float x , float y , float z)
	{
		Vector3 buf = gameObject.transform.position;
		buf.x = x;
		buf.y = y;
		buf.z = z;
		gameObject.transform.position = buf;
		return buf;
	}


	//**関数***************************************************************************
	//	概要	:	ローカル回転角代入
	//*********************************************************************************
	public Vector3 SetLocalRot(Vector3 vec)
	{
		Vector3 buf = gameObject.transform.eulerAngles;
		buf = vec;
		gameObject.transform.eulerAngles= buf;
		return buf;
	}
	public Vector3 SetLocalRot(float x , float y , float z)
	{
		Vector3 buf = gameObject.transform.eulerAngles;
		buf.x = x;
		buf.y = y;
		buf.z = z;
		gameObject.transform.eulerAngles = buf;
		return buf;
	}


	//**関数***************************************************************************
	//	概要	:	代入
	//*********************************************************************************
	public Vector3 SetLocalScale(Vector3 vec)
	{
		Vector3 buf = gameObject.transform.localScale;
		buf = vec;
		gameObject.transform.localScale = buf;
		return buf;
	}
	public Vector3 SetLocalScale(float x , float y , float z)
	{
		Vector3 buf = gameObject.transform.localScale;
		buf.x = x;
		buf.y = y;
		buf.z = z;
		gameObject.transform.localScale = buf;
		return buf;
	}


	//**関数***************************************************************************
	//	概要	:	ステータスゲットセット
	//*********************************************************************************
	public virtual int GetState(){ return m_nState; }
	public virtual void SetState(int nState){ m_nState = nState;}

	//**関数***************************************************************************
	//	概要	:	使用フラグ取得
	//*********************************************************************************
	public virtual bool GetUseFlg() { return m_bUseFlg; }
	public virtual void SetUseFlg(bool bFlg) { m_bUseFlg = bFlg; }
}
