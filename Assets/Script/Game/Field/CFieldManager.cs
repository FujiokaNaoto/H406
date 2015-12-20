using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CFieldManager : CSingleton<CFieldManager> {

	public const int FIELD_SIZE = 9;
	protected int rootX = FIELD_SIZE / 2;
	protected int rootY = FIELD_SIZE / 2;
	
	/*
	public GameObject fieldPanelPrefab;
	public GameObject stationPanelPrefab;
	public GameObject fountPanelPrefab;
	*/
	
	[HideInInspector]
	public CFieldPanel root = null;  // 基点
	private CFieldPanel[,] fieldPanels = new CFieldPanel[FIELD_SIZE, FIELD_SIZE];
	
	public string path = "\0";
	
	public int [] objectIDs = new int[FIELD_SIZE * FIELD_SIZE];
	public Sprite [] objectSprites = new Sprite[(int)CFieldPanel.EColor.MAX];
	public int [] groundIDs = new int[FIELD_SIZE * FIELD_SIZE];
	public Sprite [] groundSprites = new Sprite[(int)CFieldPanel.EColor.MAX];
	public CPanel.EDirection[] groundDirections = new CPanel.EDirection[FIELD_SIZE * FIELD_SIZE];
	public int [] prefabIDs = new int[FIELD_SIZE * FIELD_SIZE];
	public GameObject[] panelPrefabs = new GameObject[1];
	
	protected bool isSet;
	
	
	// 初期化処理
	void Awake () {
		if (this != Instance) {
			Destroy(this);
			return;
		}

		Quaternion quaternion = new Quaternion();
		quaternion.eulerAngles = new Vector3(30.0f, 0.0f, 0.0f);
		transform.rotation *= quaternion;

		Quaternion [] quaternions = new Quaternion[(int)CPanel.EDirection.MAX];
		for (int i = 0; i < (int)CPanel.EDirection.MAX; ++i) {
			quaternions[i].eulerAngles = new Vector3(0.0f, 0.0f, 90.0f * i);
		}

		for (int x = 0; x < FIELD_SIZE; ++x) {
			for (int y = 0; y < FIELD_SIZE; ++y) {
				GameObject obj;
				obj = Instantiate(panelPrefabs[prefabIDs[x + y * FIELD_SIZE]]);
				
				fieldPanels[x, y] = obj.GetComponent<CFieldPanel>();

				obj.transform.localPosition = new Vector3((float)(x - (float)rootX / 2 - rootX / 2) * 8.0f / FIELD_SIZE, (float)(-y + (float)rootY / 2 + rootY / 2) * 8.0f / FIELD_SIZE);
				obj.transform.parent = transform;
				obj.transform.localScale = new Vector3(8.0f / FIELD_SIZE, 8.0f / FIELD_SIZE, 1.0f);
				obj.transform.FindChild("Ground").gameObject.transform.rotation *= quaternions[(int)groundDirections[x + y * FIELD_SIZE]];
				

				for (int i = 0; i < (int)CPanel.EColor.MAX; ++i)
				{
					fieldPanels[x, y].objectSprites[i] = objectSprites[objectIDs[x + y * FIELD_SIZE] * (int)CPanel.EColor.MAX + i];
					fieldPanels[x, y].groundSprites[i] = groundSprites[groundIDs[x + y * FIELD_SIZE] * (int)CPanel.EColor.MAX + i];
				}
			}
		}


		root = fieldPanels[rootX, rootY];
		transform.localRotation = new Quaternion(0.0f, 0.0f, 45.0f, 0.0f);
		transform.localPosition = new Vector3(-2.0f, 0.0f);

		for (int x = 0; x < FIELD_SIZE; ++x) {
			for (int y = 0; y < FIELD_SIZE; ++y) {
				if (x < FIELD_SIZE - 1) {
					fieldPanels[x, y].right = fieldPanels[x + 1, y];
				}
				if (y < FIELD_SIZE - 1) {
					fieldPanels[x, y].down = fieldPanels[x, y + 1];
				}
			}
		}

		isSet = false;
		//fieldPanels[FIELD_SIZE - 1, FIELD_SIZE - 1].color = (int)CPanel.EColor.ORANGE;
	}
	
	// 更新処理
	void Update () {
	
	}


	public bool QueryGameOver(CPiece[] pieces) {
		if (pieces == null) return false;

		bool isGameOver = true;

		for (int i = 0; i < (int)CPanel.EDirection.MAX; ++i) {

			for (int x = 0; x < FIELD_SIZE && isGameOver; ++x) {
				for (int y = 0; y < FIELD_SIZE && isGameOver; ++y) {
					if (fieldPanels[x, y].color != (int)CPanel.EColor.NONE) continue;

					for (int j = 0; j < pieces.Length && isGameOver; ++j) {
						if (fieldPanels[x, y].QuerySetColor(pieces[j].root)) {
							isGameOver = false;
						}
					}
				}
			}
			if (!isGameOver) i = (int)CPanel.EDirection.MAX - 1;

			for (int j = 0; j < pieces.Length; ++j) {
				foreach (CPanel child in pieces[j].GetComponentsInChildren<CPanel>()) {
					child.direction = i + 1;
				}
			}
		}
		return isGameOver;
	}


	public bool IsSet {
		set {
			isSet = value;
		}
		get {
			return isSet;
		}
	}
}
