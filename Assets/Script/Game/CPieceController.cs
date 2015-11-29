using UnityEngine;
using System.Collections;

public class CPieceController : CObject {
    protected const float JOY_RANGE = 0.7f;

    // Lerp制御
    protected Transform start = null;
    protected Transform end = null;
    [SerializeField]
    protected float time = 1.0f;
    protected float diff = 0.0f;
    [SerializeField]
    protected AnimationCurve curve;

    protected int direction      = 0;
    protected int startDirection = 0;
    protected int endDirection   = 0;
    protected Quaternion [] directionQuaternions = new Quaternion[(int)CPanel.EDirection.MAX];

    protected CPanel panel = null;
    protected CSelectBox selectBox = null;
    protected int cursor = 0;

    protected bool isSelect = true;
    protected bool isLerp = false;

	// 初期化処理
	void Awake () {
	}

    public bool Create(GameObject _parent, GameObject _selectBox) {
        

        selectBox = _selectBox.GetComponent<CSelectBox>();
        transform.position = selectBox.refPoints[cursor].transform.position;
        transform.rotation = selectBox.refPoints[cursor].transform.rotation;
        transform.localScale = selectBox.refPoints[cursor].transform.localScale;
        selectBox.pieces[cursor].transform.parent = transform;

        for (int i = 0; i < (int)CPanel.EDirection.MAX; ++i) {
            directionQuaternions[i] = new Quaternion();
            directionQuaternions[i].eulerAngles = new Vector3(0.0f, 0.0f, 90.0f * i);
        }
        return base.Create(_parent);
    }

    // 更新処理
    void Update () {
        if (isLerp) {
            OnLerp();
            return;
        }

        if (isSelect) {
            OnSelect();
        }
        else {
            OnField();
        }
	}

    protected void OnSelect() {

        // ピース選択
        if ((CManager.Instance.GetInput().GetKeyTrigger(KeyCode.DownArrow) || 
            (CInput.Instance.GetJoyLStick().y < -JOY_RANGE && CInput.Instance.GetOldLStick.y > - JOY_RANGE)) &&
            cursor < (int)CSelectBox.SELECT_NUM - 1) {
            selectBox.pieces[cursor].transform.parent = selectBox.transform;
            start = selectBox.refPoints[cursor].transform;
            cursor ++;
            end = selectBox.refPoints[cursor].transform;
            startDirection = direction;
            endDirection = direction;
            isLerp = true;
        }
        else if ((CManager.Instance.GetInput().GetKeyTrigger(KeyCode.UpArrow) ||
            (CInput.Instance.GetJoyLStick().y > JOY_RANGE && CInput.Instance.GetOldLStick.y < JOY_RANGE )) &&
            cursor > 0) {
            selectBox.pieces[cursor].transform.parent = selectBox.transform;
            start = selectBox.refPoints[cursor].transform;
            cursor --;
            end = selectBox.refPoints[cursor].transform;
            startDirection = direction;
            endDirection = direction;
            isLerp = true;
        }


        // ピース確定
        if (CManager.Instance.GetInput().GetKeyTrigger(KeyCode.Space) || CInput.Instance.GetJoyRelease(4)) {
            isSelect = false;

            selectBox.pieces[cursor].transform.parent = transform;
            start = selectBox.refPoints[cursor].transform;
            panel = CFieldManager.Instance.root;
            end = panel.transform;
            startDirection = direction;
            endDirection = direction;
            isLerp = true;
        }
    }

    protected void OnField() {

        // TODO ピース設置
        if (CManager.Instance.GetInput().GetKeyTrigger(KeyCode.Space) || CInput.Instance.GetJoyRelease(4)) {
            // 再起問い合わせ、成功で配置
            if (panel.SetColors(selectBox.pieces[cursor].root)) {
                Destroy(selectBox.pieces[cursor].gameObject);
                start = panel.transform;
                end = selectBox.refPoints[cursor].transform;
                startDirection = direction;
                direction = 0;
                endDirection = direction;
                isSelect = true;
                isLerp = true;
            }
        }


        // ピース選択モードへ戻る
        else if (CManager.Instance.GetInput().GetKeyTrigger(KeyCode.N) || CInput.Instance.GetJoyRelease(3)) {
            // セレクトボードにオブジェクト返却
            start = panel.transform;
            end = selectBox.refPoints[cursor].transform;
            startDirection = direction;
            direction = 0;
            endDirection = direction;
            foreach (CPanel child in selectBox.pieces[cursor].GetComponentsInChildren<CPanel>()) {
                child.direction = 0;
            }
            isSelect = true;
            isLerp = true;
        }

        int newDirection = selectBox.pieces[cursor].GetComponentInChildren<CPanel>().direction;

        if (CManager.Instance.GetInput().GetKeyTrigger(KeyCode.Z) || CInput.Instance.GetJoyRelease(5)) newDirection = (newDirection + 1) % (int)CPanel.EDirection.MAX;
        if (CManager.Instance.GetInput().GetKeyTrigger(KeyCode.X) || CInput.Instance.GetJoyRelease(6)) newDirection = (newDirection + (int)CPanel.EDirection.MAX - 1) % (int)CPanel.EDirection.MAX;

        if (newDirection != selectBox.pieces[cursor].GetComponentInChildren<CPanel>().direction) {
            start = panel.transform;
            end = panel.transform;
            startDirection = direction;
            direction = newDirection;
            endDirection = direction;
            foreach (CPanel child in selectBox.pieces[cursor].GetComponentsInChildren<CPanel>()) {
                child.direction = direction;
            }
            isLerp = true;
        }

        CPanel newPanel = panel;
        if ((Input.GetKeyDown(KeyCode.UpArrow) || (CInput.Instance.GetJoyLStick().y > JOY_RANGE && CInput.Instance.GetOldLStick.y < JOY_RANGE) )&& newPanel.up) newPanel = newPanel.up;
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || (CInput.Instance.GetJoyLStick().x < - JOY_RANGE && CInput.Instance.GetOldLStick.x > - JOY_RANGE)) && newPanel.left) newPanel = newPanel.left;
        if ((Input.GetKeyDown(KeyCode.DownArrow) || (CInput.Instance.GetJoyLStick().y < - JOY_RANGE && CInput.Instance.GetOldLStick.y > - JOY_RANGE)) && newPanel.down) newPanel = newPanel.down;
        if ((Input.GetKeyDown(KeyCode.RightArrow) || (CInput.Instance.GetJoyLStick().x > JOY_RANGE && CInput.Instance.GetOldLStick.x < JOY_RANGE)) && newPanel.right) newPanel = newPanel.right;
        if (newPanel != panel) {
            start = panel.transform;
            panel = newPanel;
            end = panel.transform;
            startDirection = direction;
            endDirection = direction;
            isLerp = true;
        }
    }

    protected void OnLerp() {
        diff += Time.deltaTime / time;
        if (diff >= time) {
            transform.position = end.position;
            transform.rotation = end.rotation * directionQuaternions[endDirection];
            transform.localScale = end.localScale;
            diff = 0.0f;
            isLerp = false;
            return;
        }

        float rate = curve.Evaluate(diff / time);
        transform.position = Vector3.Lerp(start.position, end.position, rate);
        transform.rotation = Quaternion.Lerp(start.rotation * directionQuaternions[startDirection],end.rotation * directionQuaternions[endDirection], rate);
        transform.localScale = Vector3.Lerp(start.localScale, end.localScale, rate);
    }
}
