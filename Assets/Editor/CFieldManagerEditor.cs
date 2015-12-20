using UnityEngine;
using System.Collections;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(CFieldManager))]
public class CFieldManagerEditor : Editor {

    string editControlName;
    int    editControlValue;
    string srcTargetID;
    string fieldName;

    bool isFoldOut1 = false;
    bool isFoldOut2 = false;

    enum ESelect {
        OBJECT_SPRITE,
        GROUND_SPRITE,
        PREFAB
    };
    ESelect param = ESelect.OBJECT_SPRITE;

    public override void OnInspectorGUI() {
        CFieldManager srcTarget = target as CFieldManager;
        serializedObject.Update();
        srcTargetID = string.Format("{0}({1}-{2})", typeof(CFieldManager).Name, srcTarget.gameObject.GetInstanceID(), srcTarget.GetInstanceID());

        Texture2D defaultTexture = Resources.Load("Texture/Editor/white") as Texture2D;
        GUIStyle style = new GUIStyle(GUI.skin.button);
        style.fixedWidth = 40.0f;
        style.fixedHeight = 40.0f;
        style.margin = new RectOffset(2, 2, 2, 2);
        style.wordWrap = true;

        param = (ESelect)EditorGUILayout.EnumPopup(param);

        Texture2D texture;
        string text;
        Matrix4x4 matrix = GUI.matrix;
        Vector2 pivotPoint;
        EditorGUILayout.BeginHorizontal();
        for (int x = 0; x < CFieldManager.FIELD_SIZE; ++x) {
            EditorGUILayout.BeginVertical();
            for (int y = 0; y < CFieldManager.FIELD_SIZE; ++y) {
                switch (param) {
                    case ESelect.OBJECT_SPRITE:
                        if (srcTarget.objectSprites.Length <= 0) break;
                        srcTarget.objectIDs[x + y * CFieldManager.FIELD_SIZE] %= (srcTarget.objectSprites.Length / (int)CPanel.EColor.MAX);
                        texture = srcTarget.objectSprites[srcTarget.objectIDs[x + y * CFieldManager.FIELD_SIZE] * (int)CPanel.EColor.MAX] ? srcTarget.objectSprites[srcTarget.objectIDs[x + y * CFieldManager.FIELD_SIZE] * (int)CPanel.EColor.MAX].texture : null;
                        style.normal.background = texture ? texture : defaultTexture;
                        style.active.background = texture ? texture : defaultTexture;
                        style.fontSize = 20;
                        text = texture ? " " : srcTarget.objectIDs[x + y * CFieldManager.FIELD_SIZE].ToString();
                        if (GUILayout.Button(text, style)) {
                            Event evt = Event.current;
                            if (evt.button == 0) srcTarget.objectIDs[x + y * CFieldManager.FIELD_SIZE] = (srcTarget.objectIDs[x + y * CFieldManager.FIELD_SIZE] + 1) % (srcTarget.objectSprites.Length / (int)CPanel.EColor.MAX);
                            else srcTarget.objectIDs[x + y * CFieldManager.FIELD_SIZE] = (srcTarget.objectIDs[x + y * CFieldManager.FIELD_SIZE] + (srcTarget.objectSprites.Length / (int)CPanel.EColor.MAX) - 1) % (srcTarget.objectSprites.Length / (int)CPanel.EColor.MAX);
                        }
                        break;
                    case ESelect.GROUND_SPRITE:
                        if (srcTarget.groundSprites.Length <= 0) break;
                        srcTarget.groundIDs[x + y * CFieldManager.FIELD_SIZE] %= (srcTarget.groundSprites.Length / (int)CFieldPanel.EColor.MAX);
                        texture = srcTarget.groundSprites[srcTarget.groundIDs[x + y * CFieldManager.FIELD_SIZE] * (int)CFieldPanel.EColor.MAX] ? srcTarget.groundSprites[srcTarget.groundIDs[x + y * CFieldManager.FIELD_SIZE] * (int)CFieldPanel.EColor.MAX].texture : null;
                        style.normal.background = texture ? texture : defaultTexture;
                        style.active.background = texture ? texture : defaultTexture;
                        text = texture ? " " : srcTarget.groundIDs[x + y * CFieldManager.FIELD_SIZE].ToString();
                        style.fontSize = 20;
                        pivotPoint = GUILayoutUtility.GetRect(0.0f, 0.0f, style).center;
                        pivotPoint.x += 20.0f;
                        pivotPoint.y += 22.5f;
                        GUIUtility.RotateAroundPivot((int)srcTarget.groundDirections[x + y * CFieldManager.FIELD_SIZE] * -90.0f, pivotPoint);
                        if (GUILayout.Button(text, style)) {
                            Event evt = Event.current;
                            if (evt.button == 0) srcTarget.groundIDs[x + y * CFieldManager.FIELD_SIZE] = (srcTarget.groundIDs[x + y * CFieldManager.FIELD_SIZE] + 1) % (srcTarget.groundSprites.Length / (int)CFieldPanel.EColor.MAX);
                            else if(evt.button == 1) srcTarget.groundIDs[x + y * CFieldManager.FIELD_SIZE] = (srcTarget.groundIDs[x + y * CFieldManager.FIELD_SIZE] + (srcTarget.groundSprites.Length / (int)CFieldPanel.EColor.MAX) - 1) % (srcTarget.groundSprites.Length / (int)CFieldPanel.EColor.MAX);
                            else srcTarget.groundDirections[x + y * CFieldManager.FIELD_SIZE] = (CPanel.EDirection)(((int)srcTarget.groundDirections[x + y * CFieldManager.FIELD_SIZE] + 1) % (int)CPanel.EDirection.MAX);
                        }
                        GUI.matrix = matrix;

                        break;
                    case ESelect.PREFAB:
                        if (srcTarget.panelPrefabs.Length <= 0) break;
                        srcTarget.prefabIDs[x + y * CFieldManager.FIELD_SIZE] %= srcTarget.panelPrefabs.Length;
                        style.normal.background = defaultTexture;
                        style.active.background = defaultTexture;
                        text = srcTarget.panelPrefabs[srcTarget.prefabIDs[x + y * CFieldManager.FIELD_SIZE]] ? srcTarget.panelPrefabs[srcTarget.prefabIDs[x + y * CFieldManager.FIELD_SIZE]].name : " ";
                        style.fontSize = 8;
                        if (GUILayout.Button(text, style)) {
                            Event evt = Event.current;
                            if (evt.button == 0) srcTarget.prefabIDs[x + y * CFieldManager.FIELD_SIZE] = (srcTarget.prefabIDs[x + y * CFieldManager.FIELD_SIZE] + 1) % srcTarget.panelPrefabs.Length;
                            else srcTarget.prefabIDs[x + y * CFieldManager.FIELD_SIZE] = (srcTarget.prefabIDs[x + y * CFieldManager.FIELD_SIZE] + srcTarget.panelPrefabs.Length - 1) % srcTarget.panelPrefabs.Length;
                        }
                        break;
                }
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndVertical();
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        GUI.matrix = matrix;

        int getNum;
        switch (param) {
            case ESelect.OBJECT_SPRITE:
                isFoldOut1 = EditorGUILayout.Foldout(isFoldOut1, "Textures");
                if (isFoldOut1 == false) break;
                fieldName = srcTargetID + "." + param.ToString() + ".Size";
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(" ", GUILayout.Width(32));
                getNum = IntFieldEx("Size", srcTarget.objectSprites.Length / (int)CPanel.EColor.MAX) * (int)CPanel.EColor.MAX;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();
                for (int y = -1; y < srcTarget.objectSprites.Length / (int)CPanel.EColor.MAX; ++y) {
                    EditorGUILayout.LabelField((y < 0) ? " " : y.ToString(), GUILayout.Width(32));
                }
                EditorGUILayout.EndVertical();
                for (int x = 0; x < (int)CPanel.EColor.MAX; ++x) {
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.LabelField(((CPanel.EColor)x).ToString(), GUILayout.Width(64));
                    for (int y = 0; y < srcTarget.objectSprites.Length / (int)CPanel.EColor.MAX; ++y) {
                        srcTarget.objectSprites[x + y * (int)CPanel.EColor.MAX] = (Sprite)EditorGUILayout.ObjectField(srcTarget.objectSprites[x + y * (int)CPanel.EColor.MAX], typeof(Sprite), false);
                    }
                    EditorGUILayout.EndVertical();
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                if (getNum > 0 && getNum != srcTarget.objectSprites.Length) {
                    System.Array.Resize(ref srcTarget.objectSprites, getNum);
                }
                break;
            case ESelect.GROUND_SPRITE:
                isFoldOut2 = EditorGUILayout.Foldout(isFoldOut2, "Textures");
                if (isFoldOut2 == false) break;
                fieldName = srcTargetID + "." + param.ToString() + ".Size";
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(" ", GUILayout.Width(32));
                getNum = IntFieldEx("Size", srcTarget.groundSprites.Length / (int)CPanel.EColor.MAX) * (int)CPanel.EColor.MAX;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();
                for (int y = -1; y < srcTarget.groundSprites.Length / (int)CPanel.EColor.MAX; ++y){
                    EditorGUILayout.LabelField((y < 0) ? " " : y.ToString(), GUILayout.Width(32));
                }
                EditorGUILayout.EndVertical();
                for (int x = 0; x < (int)CPanel.EColor.MAX; ++x) {
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.LabelField(((CPanel.EColor)x).ToString(), GUILayout.Width(64));
                    for (int y = 0; y < srcTarget.groundSprites.Length / (int)CPanel.EColor.MAX; ++y) {
                        srcTarget.groundSprites[x + y * (int)CPanel.EColor.MAX] = (Sprite)EditorGUILayout.ObjectField(srcTarget.groundSprites[x + y * (int)CPanel.EColor.MAX], typeof(Sprite), false);
                    }
                    EditorGUILayout.EndVertical();
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                if (getNum > 0 && getNum != srcTarget.groundSprites.Length) {
                    System.Array.Resize(ref srcTarget.groundSprites, getNum);
                }
                break;
            case ESelect.PREFAB:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("panelPrefabs"), new GUIContent("Prefabs"), true);
                break;
        }

        EditorUtility.SetDirty(target);
        serializedObject.ApplyModifiedProperties();
    }

    int IntFieldEx(string label, int value) {
        bool isEdit = editControlName == fieldName;
        int displayValue = (isEdit) ? editControlValue : value;
        GUI.SetNextControlName(fieldName);
        int getNum = EditorGUILayout.IntField(label, displayValue);
        bool isFocused = GUI.GetNameOfFocusedControl() == fieldName;
        bool isChanged = getNum != value;
        bool isEnter = (Event.current.isKey && (Event.current.keyCode == KeyCode.Return));
        bool isFocusInputEnter = isChanged && isFocused && isEnter;
        bool isLostFocusEnter = isChanged && (isFocused == false) && isEdit;
        if (isFocused && (displayValue != getNum)) {
            editControlName = fieldName;
            editControlValue = getNum;
        }
        if (isFocusInputEnter || isLostFocusEnter) {
            editControlName = null;
            editControlValue = 0;
            return getNum;
        }
        return value;
    }
}
