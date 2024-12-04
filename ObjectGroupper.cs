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
        GUILayout.Label("�θ� ������Ʈ�� �Ҵ��ϰ� �ڽ����� ���� ������Ʈ�� �������ּ���");
        GUILayout.Label("���� ���õ� �����մϴ�");

        baseObject = EditorGUILayout.ObjectField("�θ� ������Ʈ", baseObject, typeof(GameObject), true) as GameObject;
        GUILayout.Label("");

        GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
        labelStyle.wordWrap = true;
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button) { fontSize = 30 };

        buttonStyle.normal.textColor = Color.green;
        GUILayout.Label("[���� �߰�]�� ������ ������Ʈ�� �߰��ϴ� ����Դϴ�.", labelStyle);
        if (GUILayout.Button("���� �߰�", buttonStyle))
        {
            Execute_AdditionDirectly();
        }
        GUILayout.Label("");

        buttonStyle.normal.textColor = Color.green * 0.75f;
        GUILayout.Label("[���� �߰�]�� ������ ������Ʈ�� �θ� ������Ʈ�� �߰��ϴ� ����Դϴ�.", labelStyle);
        if (GUILayout.Button("���� �߰�", buttonStyle))
        {
            Execute_AdditionIndirectly();
        }
        GUILayout.Label("");

        buttonStyle.normal.textColor = Color.yellow;
        GUILayout.Label("[���� ����]�� ������ ������Ʈ�� �� ������Ʈ�� �־� ���� ����Դϴ�.", labelStyle);
        if (GUILayout.Button("���� ����", buttonStyle))
        {
            Execute_BindDirectly();
        }
        GUILayout.Label("");

        buttonStyle.normal.textColor = Color.yellow * 0.75f;
        GUILayout.Label("[���� ����]�� ������ ������Ʈ�� �θ� ������Ʈ�� �� ������Ʈ�� �־� ���� ����Դϴ�.", labelStyle);
        if (GUILayout.Button("���� ����", buttonStyle))
        {
            Execute_BindIndirectly();
        }
        GUILayout.Label("--------------------------------------------");
        GUILayout.Label("���̹�");
        GUILayout.Label("���̹� ����� �Է��ϰ� ������ ������Ʈ�� �������ּ���");
        GUILayout.Label("���̹� ���� �� ���� �̸��� �������� �Է��� ��Ĵ�� ����˴ϴ�.");
        GUILayout.Label("���� ������Ʈ ���� �� ���� �������� �ִ� ��ȣ�� 1�� �����մϴ�. ��ȣ�� '_'�� ���еǾ� �־�� �մϴ�.", labelStyle);
        GUILayout.Label("����)\n���̹� ��� : Pivot_Bridge_0\n���� �� : Tape1013, File5067\n���� ��: Pivot_Bridge_0, Pivot_Bridge_1", labelStyle);
        namingForm = EditorGUILayout.TextField("���̹� ���", namingForm);
        GUILayout.Label("");
        if (GUILayout.Button("���̹� ����"))
        {
            Execute_Naming();
        }
    }

    private void Execute_AdditionDirectly()
    {
        if (baseObject == null)
        {
            Debug.LogError("�θ� ������Ʈ�� ����ֽ��ϴ�");
            return;
        }

        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogError("������ ������Ʈ�� �����ϴ�");
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
            Debug.LogError("�θ� ������Ʈ�� ����ֽ��ϴ�");
            return;
        }

        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogError("������ ������Ʈ�� �����ϴ�");
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
            Debug.LogError("������ ������Ʈ�� �����ϴ�");
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
            Debug.LogError("������ ������Ʈ�� �����ϴ�");
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
            Debug.LogError("���̹� ����� �Է����ּ���");
            return;
        }

        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogError("������ ������Ʈ�� �����ϴ�");
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
