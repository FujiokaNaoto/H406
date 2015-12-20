using UnityEngine;
using System.Collections;

public class CCamera : MonoBehaviour {

    bool isLerpPosition = false;
    bool isLerpSize = false;

    Vector3 startPosition;
    Vector3 endPosition;
    float lerpPositionDiff = 0.0f;
    float lerpPositionTime = 0.0f;
    [SerializeField]
    protected AnimationCurve lerpPositionCurve;

    float startSize;
    float endSize;
    float lerpSizeDiff = 0.0f;
    float lerpSizeTime = 0.0f;
    [SerializeField]
    protected AnimationCurve lerpSizeCurve;



    // 初期化処理
    void Awake () {

	}
	
	// 更新処理
	void Update () {
        if (isLerpPosition) {
            lerpPositionDiff += Time.deltaTime / lerpPositionTime;
            if (lerpPositionDiff >= lerpPositionTime) {
                transform.position = endPosition;
                lerpPositionDiff = 0.0f;
                isLerpPosition = false;
            } else {
                float rate = lerpPositionCurve.Evaluate(lerpPositionDiff / lerpPositionTime);
                transform.position = Vector3.Lerp(startPosition, endPosition, rate);
            }
        }
        if (isLerpSize) {
            lerpSizeDiff += Time.deltaTime / lerpSizeTime;
            if (lerpSizeDiff >= lerpSizeTime) {
                Camera.main.orthographicSize = endSize;
                lerpSizeDiff = 0.0f;
                isLerpSize = false;
            }
            else {
                float rate = lerpSizeCurve.Evaluate(lerpSizeDiff / lerpSizeTime);
                Camera.main.orthographicSize = startSize * (1.0f - rate) + endSize * rate;
            }
        }
	}

    public void LerpPosition(Vector3 position, float time = 1.0f) {
        position.z = transform.position.z;
        if (time <= 0.0f) {
            transform.position = position;
            lerpPositionDiff = 0.0f;
            isLerpPosition = false;
            return;
        }
        startPosition = transform.position;
        endPosition = position;
        lerpPositionDiff = 0.0f;
        lerpPositionTime = time;
        isLerpPosition = true;
    }

    public void LerpSize(float size, float time = 1.0f) {
        if (time <= 0.0f) {
            Camera.main.orthographicSize = size;
            lerpSizeDiff = 0.0f;
            isLerpSize = false;
            return;
        }
        startSize = Camera.main.orthographicSize;
        endSize = size;
        lerpSizeDiff = 0.0f;
        lerpSizeTime = time;
        isLerpSize = true;
    }
}
