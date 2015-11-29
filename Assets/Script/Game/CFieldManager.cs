using UnityEngine;
using System.Collections;

public class CFieldManager : CSingleton<CFieldManager> {

    public GameObject fieldPanelPrefab;
    public GameObject stationPanelPrefab;

    public const int FIELD_SIZE = 8;
    protected int rootX = FIELD_SIZE / 2;
    protected int rootY = FIELD_SIZE / 2;

    [HideInInspector]
    public CFieldPanel root = null;  // 基点
    private CFieldPanel[,] fieldPanels = new CFieldPanel[FIELD_SIZE, FIELD_SIZE];
    
    // 初期化処理
    void Awake () {
        if (this != Instance) {
            Destroy(this);
            return;
        }

        Quaternion quaternion = new Quaternion();
        quaternion.eulerAngles = new Vector3(30.0f, 0.0f, 0.0f);
        transform.rotation *= quaternion;

        for (int x = 0; x < FIELD_SIZE; ++x) {
            for (int y = 0; y < FIELD_SIZE; ++y) {
                GameObject obj;
                if (x == 6 && y == 6)
                    obj = Instantiate(stationPanelPrefab);
                else
                    obj = Instantiate(fieldPanelPrefab);
                

                fieldPanels[x, y] = obj.GetComponent<CFieldPanel>();

                obj.transform.localPosition = new Vector3((float)x - (float)rootX / 2 - rootX / 2, (float)-y + (float)rootY / 2 + rootY / 2);
                obj.transform.parent = transform;
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
}
