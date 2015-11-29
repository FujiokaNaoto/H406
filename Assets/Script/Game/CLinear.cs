using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CLinear : MonoBehaviour {

    // Lerp制御
    protected Transform start = null;
    protected Transform end = null;
    [SerializeField]
    protected float time = 1.0f;
    protected float diff = 0.0f;
    [SerializeField]
    protected AnimationCurve curve;

    protected int direction = 0;
    protected int startDirection = 0;
    protected int endDirection = 0;
    protected Quaternion [] directionQuaternions = new Quaternion[2];

    public CPanel panel = null;
    public CPanel.EColor color = CPanel.EColor.NONE;

    protected bool isLerp = false;

    [SerializeField]
    protected List<CPanel.EDirection> directions;

    void Awake () {
        for (int i = 0; i < 2; ++i) {
            directionQuaternions[i] = new Quaternion();
            directionQuaternions[i].eulerAngles = new Vector3(0.0f, 180.0f * i, 0.0f);
        }
    }
	
	void Update () {
        if (!isLerp) {
            if (panel && directions.Count > 0) {
                OnNext();
            } else {
                OnFade();
            }
        }
        if (isLerp) {
            OnLerp();
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
            panel.color = (int)color;
            return;
        }

        float rate = curve.Evaluate(diff / time);
        transform.position = Vector3.Lerp(start.position, end.position, rate);
        transform.rotation = Quaternion.Lerp(start.rotation * directionQuaternions[startDirection], end.rotation * directionQuaternions[endDirection], rate);
        transform.localScale = Vector3.Lerp(start.localScale, end.localScale, rate);
    }

    protected void OnNext() {
        CPanel next = null;
        switch (directions[0]) {
            case CPanel.EDirection.UP:      next = panel.up;    break;
            case CPanel.EDirection.LEFT:    next = panel.left;  break;
            case CPanel.EDirection.DOWN:    next = panel.down;  break;
            case CPanel.EDirection.RIGHT:   next = panel.right; break;
        }
        if (next == null) {
            panel = null;
            return;
        }

        start = panel.transform;
        end = next.transform;
        startDirection = direction;
        switch (directions[0]) {
            case CPanel.EDirection.LEFT: direction = 0; break;
            case CPanel.EDirection.RIGHT: direction = 1; break;
        }
        endDirection = direction;
        isLerp = true;

        panel = next;
        directions.RemoveAt(0);
    }

    protected void OnFade() {
        Destroy(gameObject);
    }
}
