using UnityEngine;
using System.Collections;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(CPiece))]
public class CPieceEditor : Editor {

    public override void OnInspectorGUI() {
        CPiece piece = target as CPiece;

        for (int i = 0; i < (int)CPanel.EColor.MAX - 1; ++i) {
            piece.panelPrefabs[i] = EditorGUILayout.ObjectField(((CPanel.EColor)(i + 1)).ToString(), piece.panelPrefabs[i], typeof(GameObject), true) as GameObject;
        }

        Texture2D[] tex2ds = new Texture2D[(int)CPanel.EColor.MAX] {
            Resources.Load("Texture/Editor/white") as Texture2D,
            Resources.Load("Texture/Editor/orange") as Texture2D,
            Resources.Load("Texture/Editor/red") as Texture2D,
            Resources.Load("Texture/Editor/green") as Texture2D,
            Resources.Load("Texture/Editor/blue") as Texture2D,
            Resources.Load("Texture/Editor/yellow") as Texture2D,
        };
        GUIStyle[] styles = new GUIStyle[(int)CPanel.EColor.MAX];
        for (int i = 0; i < styles.Length; ++i) {
            styles[i] = new GUIStyle(GUI.skin.button);
            styles[i].normal.background = tex2ds[i];
            styles[i].active.background = tex2ds[i];
            styles[i].fontSize = 10;
            styles[i].margin = new RectOffset(2, 2, 2, 2);
            styles[i].fixedWidth = 32.0f;
            styles[i].fixedHeight = 32.0f;
        }

        EditorGUILayout.BeginHorizontal();
        for (int x = 0; x < CPiece.PIECE_SIZE; ++x) {
            EditorGUILayout.BeginVertical();
            for (int y = 0; y < CPiece.PIECE_SIZE; ++y) {
                if (GUILayout.Button((piece.rootX == x && piece.rootY == y) ? "■" : " ", styles[(int)piece.colors[x + CPiece.PIECE_SIZE * y]])) {
                    Event evt = Event.current;
                    if (evt.button == 0) {
                        piece.colors[x + CPiece.PIECE_SIZE * y] = (CPanel.EColor)(((int)piece.colors[x + CPiece.PIECE_SIZE * y] + 1) % (int)CPanel.EColor.MAX);
                    }
                    else {
                        piece.rootX = x;
                        piece.rootY = y;
                    }
                }
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndVertical();
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorUtility.SetDirty(target);
    }
}
