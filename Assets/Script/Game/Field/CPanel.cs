using UnityEngine;
using System.Collections;

public class CPanel : CObj2D {

	public virtual CPanel up {	  // 上方向参照
		get { return _up; }
		set { if (_up) _up._down = null;	_up = value;	if (_up) _up._down = this; }
	}
	protected CPanel _up {
		get { return __panels[((int)EDirection.UP - direction + (int)EDirection.MAX) % (int)EDirection.MAX]; }
		set { __panels[((int)EDirection.UP - direction + (int)EDirection.MAX) % (int)EDirection.MAX] = value; }
	}
	public virtual CPanel left {	// 左方向参照
		get { return _left; }
		set { if (_left) _left._right = null;	_left = value;	if (_left) _left._right = this; }
	}
	protected CPanel _left {
		get { return __panels[((int)EDirection.LEFT - direction + (int)EDirection.MAX) % (int)EDirection.MAX]; }
		set { __panels[((int)EDirection.LEFT - direction + (int)EDirection.MAX) % (int)EDirection.MAX] = value; }
	}
	public virtual CPanel down {	// 下方向参照
		get { return _down; }
		set { if (_down) _down._up = null;	_down = value;	if (_down) _down._up = this; }
	}
	protected CPanel _down {
		get { return __panels[((int)EDirection.DOWN - direction + (int)EDirection.MAX) % (int)EDirection.MAX]; }
		set { __panels[((int)EDirection.DOWN - direction + (int)EDirection.MAX) % (int)EDirection.MAX] = value; }
	}
	public virtual CPanel right {   // 右方向参照
		get { return _right; }
		set { if (_right) _right._left = null;	_right = value;	if (_right) _right._left = this; }
	}
	protected CPanel _right {
		get { return __panels[((int)EDirection.RIGHT - direction + (int)EDirection.MAX) % (int)EDirection.MAX]; }
		set { __panels[((int)EDirection.RIGHT - direction + (int)EDirection.MAX) % (int)EDirection.MAX] = value; }
	}
	[SerializeField]
	private CPanel[] __panels = new CPanel[(int)EDirection.MAX];

	public enum EDirection {		// 方向情報
		UP, LEFT, DOWN, RIGHT, MAX
	};
	public virtual int direction {
		get { return (int)_direction; }
		set { _direction = (EDirection)((value % (int)EDirection.MAX + (int)EDirection.MAX) % (int)EDirection.MAX); }
	}
	protected EDirection _direction = EDirection.UP;

	public enum EColor {			// カラー情報
		NONE, ORANGE, BLUE, MAX
	};
	public virtual int color {
		get { return (int)_color; }
		set { _color = (EColor)((value % (int)EColor.MAX + (int)EColor.MAX) % (int)EColor.MAX); }
	}
	[SerializeField]
	protected EColor _color = EColor.NONE;
	
	private bool isSentinel = false;
	
	// 初期化処理
	void Start () {

	}
	
	// 更新処理
	void Update () {
	   
	}

	// カラー情報セッター（再帰）
	public bool SetColors(CPanel src) {
		if (!QuerySetColor(src)) return false;
		color = src.color;

		CJudgeManager.Instance.SetTargetPosition(this);

		isSentinel = true;
		if (up) up._SetColors(src.up);
		if (left) left._SetColors(src.left);
		if (down) down._SetColors(src.down);
		if (right) right._SetColors(src.right);
		isSentinel = false;
		return true;
	}
	protected void _SetColors(CPanel src) {
		if (src == null || isSentinel) return;
		color = src.color;

		CJudgeManager.Instance.SetTargetPosition(this);

		isSentinel = true;
		if (up) up._SetColors(src.up);
		if (left) left._SetColors(src.left);
		if (down) down._SetColors(src.down);
		if (right) right._SetColors(src.right);
		isSentinel = false;
	}
	public bool QuerySetColor(CPanel src) {
		if (src == null) return false;
		if (!QueryColorEquals(src)) return false;
		if (!(QueryColorAdjacent(src) || QueryCornerAdjacent(src))) return false;
		return true;
	}

	// カラー情報問い合わせ（再帰）（src範囲の色が全てcolと等しいならtrue）
	public bool QueryColorEquals(CPanel src, EColor col = EColor.NONE) {
		if (src == null) return false;
		if (_color != col) return false;
		isSentinel = true;
		if (up && isSentinel && !up._QueryColorEquals(src.up, col)) isSentinel = false;
		if (left && isSentinel && !left._QueryColorEquals(src.left, col)) isSentinel = false;
		if (down && isSentinel && !down._QueryColorEquals(src.down, col)) isSentinel = false;
		if (right && isSentinel && !right._QueryColorEquals(src.right, col)) isSentinel = false;
		if (!isSentinel) return false;
		isSentinel = false;
		return true;
	}
	protected bool _QueryColorEquals(CPanel src, EColor col) {
		if (src == null || isSentinel) return true;
		if (_color != col) return false;
		isSentinel = true;
		if (up && isSentinel && !up._QueryColorEquals(src.up, col)) isSentinel = false;
		if (left && isSentinel && !left._QueryColorEquals(src.left, col)) isSentinel = false;
		if (down && isSentinel && !down._QueryColorEquals(src.down, col)) isSentinel = false;
		if (right && isSentinel && !right._QueryColorEquals(src.right, col)) isSentinel = false;
		if (!isSentinel) return false;
		isSentinel = false;
		return true;
	}

	// カラー隣接情報問い合わせ（再帰）（src範囲と隣接する色に同じ色が含まれていればtrue）
	public bool QueryColorAdjacent(CPanel src) {
		if (src == null) return false;
		isSentinel = true;
		if (up && isSentinel && up._QueryColorAdjacent(src.up, src._color)) isSentinel = false;
		if (left && isSentinel && left._QueryColorAdjacent(src.left, src._color)) isSentinel = false;
		if (down && isSentinel && down._QueryColorAdjacent(src.down, src._color)) isSentinel = false;
		if (right && isSentinel && right._QueryColorAdjacent(src.right, src._color)) isSentinel = false;
		if (!isSentinel) return true;
		isSentinel = false;
		return false;
	}
	protected bool _QueryColorAdjacent(CPanel src, EColor col) {
		if (isSentinel) return false;
		if (src == null) return (_color == col);
		isSentinel = true;
		if (up && isSentinel && up._QueryColorAdjacent(src.up, src._color)) isSentinel = false;
		if (left && isSentinel && left._QueryColorAdjacent(src.left, src._color)) isSentinel = false;
		if (down && isSentinel && down._QueryColorAdjacent(src.down, src._color)) isSentinel = false;
		if (right && isSentinel && right._QueryColorAdjacent(src.right, src._color)) isSentinel = false;
		if (!isSentinel) return true;
		isSentinel = false;
		return false;
	}

	// 角隣接情報問い合わせ（再帰）（src範囲と角が隣接していればtrue）
	public bool QueryCornerAdjacent(CPanel src)
	{
		if (src == null) return false;
		if (!(up || left) || !(left || down) || !(down || right) || !(right || up)) return true;
		isSentinel = true;
		if (up && isSentinel && up._QueryCornerAdjacent(src.up)) isSentinel = false;
		if (left && isSentinel && left._QueryCornerAdjacent(src.left)) isSentinel = false;
		if (down && isSentinel && down._QueryCornerAdjacent(src.down)) isSentinel = false;
		if (right && isSentinel && right._QueryCornerAdjacent(src.right)) isSentinel = false;
		if (!isSentinel) return true;
		isSentinel = false;
		return false;
	}
	protected bool _QueryCornerAdjacent(CPanel src)
	{
		if (isSentinel || src == null) return false;
		if (!(up || left) || !(left || down) || !(down || right) || !(right || up)) return true;
		isSentinel = true;
		if (up && isSentinel && up._QueryCornerAdjacent(src.up)) isSentinel = false;
		if (left && isSentinel && left._QueryCornerAdjacent(src.left)) isSentinel = false;
		if (down && isSentinel && down._QueryCornerAdjacent(src.down)) isSentinel = false;
		if (right && isSentinel && right._QueryCornerAdjacent(src.right)) isSentinel = false;
		if (!isSentinel) return true;
		isSentinel = false;
		return false;
	}
}
