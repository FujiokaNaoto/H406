using UnityEngine;
using System.Collections;

public class CChanging : CScene 
{
	// �ύX�^�C�v���Ƃ̃I�u�W�F�N�g�v���n�u
	public GameObject BLACK_PREFAB;
	public GameObject WHITE_PREFAB;

    // �ύX�^�C�v
	public enum eChangeType
	{
		CHANGE_NONE,
		WHITE_FEAD,
		BLACK_FEAD,
	};

	// --�ϐ��錾--
	private bool m_bChange;			// �ύX���t���O

	CSceneManager.eSceneID m_Bedore;
	CSceneManager.eSceneID m_After;
	eChangeType m_eChangeType;

	GameObject m_ChangeObject;
	GameObject m_SceneManage;


	//**�֐�***************************************************************************
	//	�T�v	:	�ϐ�������
	//*********************************************************************************
	void Awake()
	{
		m_bChange = false;

		m_Bedore = CSceneManager.eSceneID.NONE;
		m_After = CSceneManager.eSceneID.NONE;
		m_eChangeType = eChangeType.CHANGE_NONE;

		m_ChangeObject = null;
		m_SceneManage = null;
	}


	//**�֐�***************************************************************************
	//	�T�v	:	����
	//*********************************************************************************
	public bool Create(eChangeType eType , CSceneManager.eSceneID Current , CSceneManager.eSceneID After , GameObject SceneManage)
	{
		if (m_bChange)
			return false;

		m_eChangeType = eType;
		m_Bedore = Current;
		m_After = After;
		m_SceneManage = SceneManage;
		m_bChange = true;

		transform.SetParent(SceneManage.transform);

		Initialize ();
		return true;
	}


	//**�֐�***************************************************************************
	//	�T�v	:	������
	//*********************************************************************************
	public override bool Initialize()
	{
		// �ύX�^�C�v�ɉ������I�u�W�F�N�g����
		switch (m_eChangeType)
		{ 
		case eChangeType.WHITE_FEAD:
			m_ChangeObject = (GameObject)GameObject.Instantiate(WHITE_PREFAB);
			m_ChangeObject.GetComponent<CChangeFead>().Create(gameObject , CFead.eFeadType.WHITEFEAD);
			break;

		case eChangeType.BLACK_FEAD:
			m_ChangeObject = (GameObject)GameObject.Instantiate(BLACK_PREFAB);
			m_ChangeObject.GetComponent<CChangeFead>().Create(gameObject, CFead.eFeadType.BLACKFEAD);
			break;

		case eChangeType.CHANGE_NONE:
			SceneChange();
			ChangeEnd();
			break;
		}

		return true;
	}


	//**�֐�***************************************************************************
	//	�T�v	:	�V�[���ύX
	//*********************************************************************************
	public virtual void SceneChange()
	{
		// �O�V�[����j�����ĐV�K�V�[���𐶐�
		//m_SceneManage.GetComponent<CSceneManager>().GetSceneObj(m_Bedore).SetActive(false);
		m_SceneManage.GetComponent<CSceneManager>().ClearOnce(m_Bedore);
		m_SceneManage.GetComponent<CSceneManager>().SetOnce(m_After);
		//m_SceneManage.GetComponent<CSceneManager>().GetSceneObj(m_After).SetActive(true);
	}


	//**�֐�***************************************************************************
	//	�T�v	:	�ύX�I��
	//*********************************************************************************
	public virtual void ChangeEnd()
	{
		m_bChange = false;
		if (m_ChangeObject)
		{
			Destroy(m_ChangeObject);
			m_ChangeObject = null;
		}
	}

	
	//**�֐�***************************************************************************
	//	�T�v	:	�X�V����
	//*********************************************************************************
	void Update () 
	{
	
	}


	//**�֐�***************************************************************************
	//	�T�v	:	�ύX���t���O�擾
	//*********************************************************************************
	public bool GetChangeFlg()
	{
		return m_bChange;
	}
}