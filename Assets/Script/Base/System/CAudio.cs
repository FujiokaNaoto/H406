using UnityEngine;
using System.Collections;

struct BGM
{
	public string Name;
	public AudioSource Source;
	public AudioClip Clip;
	public GameObject Clone;

	public CAudio.BGMSTATE State;

	public float time;
	public float fadeInTime;
	public float fadeOutTime;
	public float volume;
}

struct SE
{
	public string Name;
	public AudioSource Source;

	public AudioClip Clip;

	public GameObject Clone;

	public bool flg;
}


public class CAudio : CSingleton<CAudio>
{
	public GameObject AudioPre;

	string BGMHierarchy = "Audio/BGM/";
	string SEHierarchy = "Audio/SE/";

	public enum BGMCODE
	{
		TITLE,
		GAME,
		RESULT_GOLD,
		MAX
	};
	public enum SECODE
	{
		BUTTON_SE,
		PIECE_ADD,
		PIECE_CANCEL,
		PIECE_DECISION,
		PIECE_ROTATE,
		PIECE_SELECT,
		PIECE_SET,

		SET_FLOWER,	
		CLOCK_EFFECT,	
		WALK,
		LINEAR,

		MAX
	};

	public enum BGMSTATE
	{
		NONE,
		FEADIN,
		PLAY,
		FEADOUT,
		END,
	}

	BGM[] bgm;
	SE[] se;

	void Awake()
	{
		if (this != Instance)
		{
			Destroy(this);
			return;
		}
		DontDestroyOnLoad(this.gameObject);

		bgm = new BGM[(int)BGMCODE.MAX];

		// ここでBGMのディレクトリ設定
		bgm[(int)BGMCODE.TITLE].Name = BGMHierarchy + "TitleBGM";
		bgm[(int)BGMCODE.GAME].Name = BGMHierarchy + "MainBGM";
		bgm[(int)BGMCODE.RESULT_GOLD].Name = BGMHierarchy + "ResultBGM1";

		for (int i = 0; i < (int)BGMCODE.MAX; i++)
		{
			bgm[i].Clip = (AudioClip)Resources.Load(bgm[i].Name);
			bgm[i].Source = null;
			bgm[i].Clone = null;
			bgm[i].State = BGMSTATE.NONE;
			bgm[i].time = 0.0f;
			bgm[i].fadeInTime = 3.0f;
			bgm[i].fadeOutTime = 3.0f;
			bgm[i].volume = 0.0f;
		}

		se = new SE[(int)SECODE.MAX];

		// ここでSEのディレクトリ設定
		se[(int)SECODE.BUTTON_SE].Name = SEHierarchy + "ButtonSE";
		se[(int)SECODE.PIECE_ADD].Name = SEHierarchy + "PieceAdd";
		se[(int)SECODE.PIECE_CANCEL].Name = SEHierarchy + "PieceCancel";
		se[(int)SECODE.PIECE_DECISION].Name = SEHierarchy + "PieceDecision";
		se[(int)SECODE.PIECE_ROTATE].Name = SEHierarchy + "PieceRotate";
		se[(int)SECODE.PIECE_SELECT].Name = SEHierarchy + "PieceSelect";
		se[(int)SECODE.PIECE_SET].Name = SEHierarchy + "PieceSet";

		se[(int)SECODE.SET_FLOWER].Name = SEHierarchy + "SetFlower";
		se[(int)SECODE.CLOCK_EFFECT].Name = SEHierarchy + "ClockEffect";
		se[(int)SECODE.WALK].Name = SEHierarchy + "Walk";
		se[(int)SECODE.LINEAR].Name = SEHierarchy + "Linear";

		for (int i = 0; i < (int)SECODE.MAX; i++)
		{
			se[i].Clip = (AudioClip)Resources.Load(se[i].Name);
			se[i].Source = null;
			se[i].Clone = null;
			se[i].flg = false;
		}

	}

	// Use this for initialization
	void Start ()
	{
		/*
		bgm = new BGM[(int)BGMCODE.MAX];

		bgm[(int)BGMCODE.TITLE].Name = BGMHierarchy + "タイトルBGM/game_maoudamashii_5_castle01";
		bgm[(int)BGMCODE.GAME].Name = BGMHierarchy + "ゲームBGM/chess";
		bgm[(int)BGMCODE.RESULT].Name = BGMHierarchy + "リザルトBGM/akaifuusen";

		for (int i = 0; i < (int)BGMCODE.MAX; i++)
		{
			bgm[i].Clip = (AudioClip)Resources.Load(bgm[i].Name);
			bgm[i].Source = null;
			bgm[i].Clone = null;
			bgm[i].State = BGMSTATE.NONE;
			bgm[i].time = 0.0f;
			bgm[i].fadeInTime = 3.0f;
			bgm[i].fadeOutTime = 3.0f;
			bgm[i].volume = 0.0f;
		}

		se = new SE[(int)SECODE.MAX];

		for (int i = 0; i < (int)SECODE.MAX; i++)
		{
			se[(int)SECODE.PUSH_START].Name = SEHierarchy + "スタートボタン押す音/decision4";

			se[i].Clip = (AudioClip)Resources.Load(se[i].Name);
			se[i].Source = null;
			se[i].Clone = null;
			se[i].flg = false;
		}
		*/
	}

	// Update is called once per frame
	void Update()
	{
		BGMUpdate();
		SEUpdate();
	}

	// Update is called once per frame
	void BGMUpdate()
	{
		for (int i = 0; i < (int)BGMCODE.MAX; i++)
		{
			switch (bgm[i].State)
			{
				case BGMSTATE.NONE:
					break;
				case BGMSTATE.FEADIN:
					bgm[i].time += Time.deltaTime;
					bgm[i].Source.volume = bgm[i].time / bgm[i].fadeInTime;
					if (bgm[i].time >= bgm[i].fadeInTime)
					{
						bgm[i].time = 0.0f;
                        bgm[i].Source.volume = 1.0f;
						bgm[i].State = BGMSTATE.PLAY;
					}
					break;
				case BGMSTATE.PLAY:
					break;
				case BGMSTATE.FEADOUT:
					bgm[i].time += Time.deltaTime;
					bgm[i].Source.volume = bgm[i].volume * (1.0f - bgm[i].time / bgm[i].fadeOutTime);
					if (bgm[i].time >= bgm[i].fadeOutTime)
					{
						bgm[i].time = 0.0f;
						bgm[i].Source.volume = 0.0f;
						bgm[i].State = BGMSTATE.END;
					}
					break;
				case BGMSTATE.END:
					StopBGM((BGMCODE)i);
					bgm[i].State = BGMSTATE.NONE;
					break;
			}
        }
	}

	// Update is called once per frame
	void SEUpdate()
	{
		for (int i = 0; i < (int)SECODE.MAX; i++)
		{
			if (se[i].flg && !se[i].Source.isPlaying)
			{
				Destroy(se[i].Clone);
				se[i].Source = null;
				se[i].flg = false;
			}
		}
	}

	public void PlayBGM(BGMCODE eCode)
	{
		bgm[(int)eCode].Clone = GameObject.Instantiate(AudioPre);
        bgm[(int)eCode].Source = bgm[(int)eCode].Clone.GetComponent<AudioSource>();
		bgm[(int)eCode].Source.clip = bgm[(int)eCode].Clip;
		bgm[(int)eCode].Source.loop = true;
		bgm[(int)eCode].Source.Play();
		bgm[(int)eCode].Source.volume = 0.0f;
		bgm[(int)eCode].State = BGMSTATE.FEADIN;
    }

	public void PlaySE(SECODE eCode)
	{
		se[(int)eCode].Clone = GameObject.Instantiate(AudioPre);
		se[(int)eCode].Source = se[(int)eCode].Clone.GetComponent<AudioSource>();
		se[(int)eCode].Source.clip = se[(int)eCode].Clip;
		se[(int)eCode].Source.Play();
		se[(int)eCode].flg = true;
	}

	public void EndBGM(BGMCODE eCode)
	{
		bgm[(int)eCode].volume = bgm[(int)eCode].Source.volume;
		bgm[(int)eCode].State = BGMSTATE.FEADOUT;
	}

	public void StopBGM(BGMCODE eCode)
	{
		bgm[(int)eCode].Source.Stop();
		Destroy(bgm[(int)eCode].Clone);
		bgm[(int)eCode].Source = null;
	}
}
