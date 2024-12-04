using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;


public class ObjectGroupper : EditorWindow
{
    private GameObject baseObject;
    private string namingForm;

    [MenuItem("Tools/ObjectTool/Object Grouping")]
    private static void OpenWindow()
    {
        ObjectGroupper window = GetWindow<ObjectGroupper>();
        window.titleContent = new GUIContent("Object Groupper");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("부모 오브젝트를 할당하고 자식으로 넣을 오브젝트를 선택해주세요");
        GUILayout.Label("다중 선택도 가능합니다");

        baseObject = EditorGUILayout.ObjectField("부모 오브젝트", baseObject, typeof(GameObject), true) as GameObject;
        GUILayout.Label("");

        GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
        labelStyle.wordWrap = true;
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button) { fontSize = 30 };

        buttonStyle.normal.textColor = Color.green;
        GUILayout.Label("[직접 추가]는 선택한 오브젝트를 추가하는 기능입니다.", labelStyle);
        if (GUILayout.Button("직접 추가", buttonStyle))
        {
            Execute_AdditionDirectly();
        }
        GUILayout.Label("");

        buttonStyle.normal.textColor = Color.green * 0.75f;
        GUILayout.Label("[간접 추가]는 선택한 오브젝트의 부모 오브젝트를 추가하는 기능입니다.", labelStyle);
        if (GUILayout.Button("간접 추가", buttonStyle))
        {
            Execute_AdditionIndirectly();
        }
        GUILayout.Label("");

        buttonStyle.normal.textColor = Color.yellow;
        GUILayout.Label("[직접 묶음]은 선택한 오브젝트를 빈 오브젝트에 넣어 묶는 기능입니다.", labelStyle);
        if (GUILayout.Button("직접 묶음", buttonStyle))
        {
            Execute_BindDirectly();
        }
        GUILayout.Label("");

        buttonStyle.normal.textColor = Color.yellow * 0.75f;
        GUILayout.Label("[간접 묶음]은 선택한 오브젝트의 부모 오브젝트를 빈 오브젝트에 넣어 묶는 기능입니다.", labelStyle);
        if (GUILayout.Button("간접 묶음", buttonStyle))
        {
            Execute_BindIndirectly();
        }
        GUILayout.Label("--------------------------------------------");
        GUILayout.Label("네이밍");
        GUILayout.Label("네이밍 양식을 입력하고 적용할 오브젝트를 선택해주세요");
        GUILayout.Label("네이밍 적용 시 기존 이름은 지워지고 입력한 양식대로 변경됩니다.");
        GUILayout.Label("다중 오브젝트 선택 시 가장 마지막에 있는 번호가 1씩 증가합니다. 번호는 '_'로 구분되어 있어야 합니다.", labelStyle);
        GUILayout.Label("예시)\n네이밍 양식 : Pivot_Bridge_0\n적용 전 : Tape1013, File5067\n적용 후: Pivot_Bridge_0, Pivot_Bridge_1", labelStyle);
        namingForm = EditorGUILayout.TextField("네이밍 양식", namingForm);
        GUILayout.Label("");
        if (GUILayout.Button("네이밍 적용"))
        {
            Execute_Naming();
        }
    }

    private void Execute_AdditionDirectly()
    {
        if (baseObject == null)
        {
            Debug.LogError("부모 오브젝트가 비어있습니다");
            return;
        }

        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogError("선택한 오브젝트가 없습니다");
            return;
        }

        Undo.RegisterCompleteObjectUndo(selectedObjects, "Execute");

        foreach (GameObject obj in selectedObjects)
        {
            obj.transform.parent = baseObject.transform;
        }

        Undo.RecordObjects(selectedObjects, "Grouping Objects");
    }
    private void Execute_AdditionIndirectly()
    {
        if (baseObject == null)
        {
            Debug.LogError("부모 오브젝트가 비어있습니다");
            return;
        }

        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogError("선택한 오브젝트가 없습니다");
            return;
        }

        Undo.RegisterCompleteObjectUndo(selectedObjects, "Execute");

        foreach (GameObject obj in selectedObjects)
        {
            obj.transform.parent.parent = baseObject.transform;
        }

        Undo.RecordObjects(selectedObjects, "Grouping Objects");
    }

    private void Execute_BindDirectly()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogError("선택한 오브젝트가 없습니다");
            return;
        }

        Undo.RegisterCompleteObjectUndo(selectedObjects, "Execute");

        GameObject emptyObj = new GameObject("Bundle");
        emptyObj.transform.parent = selectedObjects[0].transform.parent;
        emptyObj.transform.position = selectedObjects[0].transform.position;
        emptyObj.transform.rotation = selectedObjects[0].transform.parent.rotation;

        foreach (GameObject obj in selectedObjects)
        {
            obj.transform.parent = emptyObj.transform;
        }
        baseObject = emptyObj;
        Undo.RecordObjects(selectedObjects, "Grouping Objects");
    }

    private void Execute_BindIndirectly()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogError("선택한 오브젝트가 없습니다");
            return;
        }

        Undo.RegisterCompleteObjectUndo(selectedObjects, "Execute");

        GameObject emptyObj = new GameObject("Bundle");
        emptyObj.transform.parent = selectedObjects[0].transform.parent.parent;
        emptyObj.transform.position = selectedObjects[0].transform.parent.position;
        emptyObj.transform.rotation = selectedObjects[0].transform.parent.parent.rotation;

        foreach (GameObject obj in selectedObjects)
        {
            obj.transform.parent.parent = emptyObj.transform;
        }
        baseObject = emptyObj;
        Undo.RecordObjects(selectedObjects, "Grouping Objects");
    }

    private void Execute_Naming()
    {
        if (namingForm == string.Empty)
        {
            Debug.LogError("네이밍 양식을 입력해주세요");
            return;
        }

        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogError("선택한 오브젝트가 없습니다");
            return;
        }

        string[] forms = namingForm.Split('_');
        int num = 0;
        for (int i = forms.Length -1; i >= 0; i--) 
        {
            if (int.TryParse(forms[i], out num))
            {
                forms[i] = "num";
                break;
            }
        }

        Undo.RegisterCompleteObjectUndo(selectedObjects, "Execute");

        foreach (GameObject obj in selectedObjects)
        {
            StringBuilder newNameBuilder = new(forms.Length);
            for (int i = 0; i < forms.Length; i++)
            {
                if (forms[i].Equals("num")) { newNameBuilder.Append($"{num++}"); }
                else { newNameBuilder.Append(forms[i]); }

                if (i < forms.Length - 1) { newNameBuilder.Append('_'); }
            }

            obj.name = newNameBuilder.ToString();
        }

        Undo.RecordObjects(selectedObjects, "Grouping Objects");
    }
}
