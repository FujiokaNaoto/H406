using UnityEngine;
using System.Collections;

public class CStationPanel : CFieldPanel {
    public override int color{
        get { return base.color; }
        set {
            base.color = value;
            CreateLinear();
        }
    }

    [SerializeField]
    protected GameObject linearPrefab;

    protected override void Awake () {
        base.Awake();
	}
	
	void Update () {
	
	}

    protected void CreateLinear() {
        GameObject obj = (GameObject)Instantiate(linearPrefab, transform.position, transform.rotation);
        CLinear linear = obj.GetComponent<CLinear>();
        linear.transform.parent = transform;
        linear.panel = this;
        linear.color = _color;
    }
}
