﻿//**プログラムヘッダ***************************************************************
//	プログラム概要	:	シングルトンの元クラス
//*********************************************************************************
using UnityEngine;
using System.Collections;

public class CSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T instance;

	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = (T)FindObjectOfType(typeof(T));

				if (instance == null)
					Debug.LogError(typeof(T) + "is nothing");
			}

			return instance;
		}
	}
}
