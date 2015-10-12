using UnityEngine;
using System.Collections;

public class CScene : MonoBehaviour 
{
	public enum eScenePhase
	{
		INIT,
		PLAY,
		UNINIT,
	};

	protected int m_nSceneID;
	protected eScenePhase m_ePhase;


	void Awake()
	{
		m_nSceneID = 0;
	}

	// Use this for initialization
	void Start () 
	{
	}

	// 各種オブジェクト生成
	public bool Create(int nSceneID)
	{
		m_nSceneID = nSceneID;
		Initialize ();
		return true;
	}


	// Update is called once per frame
	void Update () {
		
	}

	public virtual bool Initialize()
	{
		return true;
	}


	public int GetSceneID()
	{
		return m_nSceneID;
	}
}