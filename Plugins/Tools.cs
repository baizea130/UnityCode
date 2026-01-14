using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PlayerPrefs管理器 : EditorWindow
{
    private string[] options = { "int", "float", "string" };
    private string enter_key;
    private int num_int;
    private float num_float;
    private string num_string;
    private int select_option = 0;
    [MenuItem("Tools/PlayerPrefs管理器", false, 0)]//可获取playerprefs的值
    public static void show_window()
    {
        GetWindow<PlayerPrefs管理器>();
    }
    private void OnEnable()
    {
        minSize = new Vector2(600, 300);
        maxSize = new Vector2(600, 300);
    }
    private void OnGUI()
    {
        GUILayout.Label("输入键值以获取数据", EditorStyles.label);
        enter_key = EditorGUILayout.TextField("想设置的playerprefs标签：", enter_key);
        if (!PlayerPrefs.HasKey(enter_key))
        {
            GUI.contentColor = Color.yellow;
            GUILayout.Label("不存在此键值,可在下方输入数据以创建");
            GUI.contentColor = Color.white;
        }
        else
        {
            GUILayout.Label("int类型下值为: " + PlayerPrefs.GetInt(enter_key).ToString(), EditorStyles.boldLabel);
            GUILayout.Label("float类型下值为: " + PlayerPrefs.GetFloat(enter_key).ToString(), EditorStyles.boldLabel);
            GUILayout.Label("string类型下值为: " + PlayerPrefs.GetString(enter_key).ToString(), EditorStyles.boldLabel);
            GUI.contentColor = Color.yellow;
            GUILayout.Label("请自行辨别正确的类型!");
            GUI.contentColor = Color.white;
        }
        if (PlayerPrefs.HasKey(enter_key))
        {
            GUI.contentColor = Color.yellow;
            GUILayout.Label("该键标签已存在！可以修改此标签对应值类型和数据！");
            GUI.contentColor = Color.white;
        }
        {
            select_option = EditorGUILayout.Popup("标签类型：", select_option, options);
            if (select_option == 0)
            {
                num_int = EditorGUILayout.IntField("输入键值：", num_int);
                if (GUILayout.Button("确定设置int类型的值"))
                {
                    PlayerPrefs.SetInt(enter_key, num_int);
                    PlayerPrefs.Save();
                    Debug.Log("已经成功设置名称“" + enter_key + "”标签的键值为“" + num_int.ToString() + "”");
                    Repaint();
                }
            }
            else if (select_option == 1)
            {
                num_float = EditorGUILayout.FloatField("输入键值：", num_float);
                if (GUILayout.Button("确定设置float类型的值"))
                {
                    PlayerPrefs.SetFloat(enter_key, num_float);
                    PlayerPrefs.Save();
                    Debug.Log("已经成功设置名称“" + enter_key + "”标签的键值为“" + num_float.ToString() + "”");
                    Repaint();
                }
            }
            else if (select_option == 2)
            {
                num_string = EditorGUILayout.TextField("输入键文本：", num_string);
                if (GUILayout.Button("确定设置string类型的值"))
                {
                    PlayerPrefs.SetString(enter_key, num_string);
                    PlayerPrefs.Save();
                    Debug.Log("已经成功设置名称“" + enter_key + "”标签的键值为“" + num_string.ToString() + "”");
                    Repaint();
                }
            }
            if (GUILayout.Button("刷新"))
            {
                Repaint();
            }
        }
    }
}
public class 场景管理器 : EditorWindow
{
    private static List<string> sceneNames = new List<string>();
    private static List<string> scenePath = new List<string>();
    private bool AutoSave = true;

    [MenuItem("Tools/场景管理器", false, 1)]
    public static void t()
    {
        GetWindow<场景管理器>();
    }
    private void OnEnable()
    {
        minSize = new Vector2(350, 400);
        UpdateName();
    }

    private void OnGUI()
    {
        if (EditorApplication.isPlaying == false)
        {
            int num = sceneNames.Count;
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.alignment = TextAnchor.MiddleLeft; // 设置文本左对齐
            buttonStyle.fontSize = 14; // 设置字体大小
            GUILayout.Label("点击对应按钮可以保存当前场景并进入相应场景", EditorStyles.label);
            GUI.contentColor = Color.yellow;
            GUILayout.Label("只有在生成设置内包含的场景才会显示", EditorStyles.label);
            GUI.contentColor = Color.white;

            for (int i = 0; i < num; i++)
            {
                if (GUILayout.Button(i + "." + sceneNames[i], buttonStyle))
                {
                    if (AutoSave == true)
                    {
                        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                    }
                    EditorSceneManager.OpenScene(scenePath[i]);
                }
            }
            GUI.contentColor = Color.yellow;
            if (GUILayout.Button("刷新"))
            {
                UpdateName();
            }
            GUI.contentColor = Color.white;
            AutoSave = EditorGUILayout.Toggle("是否自动保存跳转前的场景", AutoSave);
        }
        else
        {
            GUI.contentColor = Color.yellow;
            GUILayout.Label("[运行模式下无法使用场景跳转功能]", EditorStyles.label);
            GUI.contentColor = Color.white;
        }
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("运行", GUILayout.Width(40), GUILayout.Height(40)))
        {
            EditorApplication.EnterPlaymode();
        }
        if (GUILayout.Button("停止", GUILayout.Width(40), GUILayout.Height(40)))
        {
            EditorApplication.ExitPlaymode();
        }
        GUILayout.EndHorizontal();
    }

    private static void UpdateName()
    {
        sceneNames.Clear();
        scenePath.Clear();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            string sceneName = Path.GetFileNameWithoutExtension(scene.path);
            sceneNames.Add(sceneName);
            scenePath.Add(scene.path);
        }
    }
}

[CustomEditor(typeof(MonoBehaviour), true)]
public class ExternScript : Editor
{
    private string[] options = { "自身", "父物体" };
    private enum SearchType
    {
        Self, Parent
    }
    private SearchType Type;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("自动检测并赋值组件"))
        {
            MonoBehaviour targetScript = target as MonoBehaviour;
            DetectAndAssignComponents(targetScript, Type);
        }
        Type = (SearchType)EditorGUILayout.Popup("检测范围：", (int)Type, options, GUILayout.MinWidth(200));
        GUILayout.EndHorizontal();
        GUIStyle style = new GUIStyle(GUI.skin.button);
        style.normal.textColor = Color.yellow;
        if (GUILayout.Button("打开调试函数面板", style))
        {
            ScriptsMethods.ShowPanel();
            ScriptsMethods.type = target.GetType();
            ScriptsMethods.target = (MonoBehaviour)this.target;
        }
    }

    private void DetectAndAssignComponents(MonoBehaviour targetScript, SearchType type)
    {
        GameObject target = targetScript.gameObject;
        switch (type)
        {
            case SearchType.Self:
                target = targetScript.gameObject;
                break;
            case SearchType.Parent:
                target = targetScript.gameObject.transform.parent.gameObject;
                break;
        }

        var fields = targetScript.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var field in fields)
        {
            if (typeof(Component).IsAssignableFrom(field.FieldType))
            {
                Component currentValue = field.GetValue(targetScript) as Component;
                if (currentValue == null)
                {
                    Component component = target.GetComponent(field.FieldType);
                    if (component != null)
                    {
                        field.SetValue(targetScript, component);
                        Debug.Log($"为脚本 {targetScript.GetType().Name} 中的字段 {field.Name} 赋值了组件 {component.name}");
                    }
                }
            }
        }
    }
}
public class ScriptsMethods : EditorWindow
{
    public static MonoBehaviour target;
    public static Type type;
    private static MethodInfo[] Methods;
    private static int MethodNum;
    private static string[] CurrentParams;
    private static object[] Realparameters;
    public static void ShowPanel()
    {
        GetWindow<ScriptsMethods>("Methods");
    }
    void OnGUI()
    {
        try
        { Methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance); }
        catch { }
        if (Methods != null)
        {
            MethodNum = Methods.Length;
            for (int i = 0; i < MethodNum; i++)
            {
                if (Methods[i].DeclaringType == type)
                {
                    GUILayout.BeginHorizontal();
                    ParameterInfo[] parameters = Methods[i].GetParameters();
                    if (parameters.Length > 0)
                    {
                        if (CurrentParams == null || CurrentParams.Length != parameters.Length)
                        {
                            CurrentParams = new string[parameters.Length];
                        }
                        for (int a = 0; a < CurrentParams.Length; a++)
                        {
                            GUILayout.Label($"{parameters[a].Name}({parameters[a].ParameterType})");
                            CurrentParams[a] = EditorGUILayout.TextField(CurrentParams[a]);
                        }
                        try
                        {
                            if (Realparameters == null || Realparameters.Length != parameters.Length)
                            {
                                Realparameters = new object[parameters.Length];
                            }
                            for (int k = 0; k < parameters.Length; k++)
                            {
                                if (parameters[k] != null)
                                {
                                    Realparameters[k] = Convert.ChangeType(CurrentParams[k], parameters[k].ParameterType);
                                }
                            }
                        }
                        catch
                        {
                            //Debug.LogError($"调用方法 {Methods[i].Name} 时出错: {e.Message}");
                        }
                        GUILayout.EndHorizontal();
                        if (GUILayout.Button(Methods[i].Name))
                        {
                            if (Realparameters != null)
                            {
                                Methods[i]?.Invoke(target, Realparameters);
                            }
                        }
                        GUILayout.Label("", GUI.skin.horizontalSlider);
                        EditorGUILayout.Space();
                    }
                    else
                    {
                        GUILayout.EndHorizontal();
                        if (GUILayout.Button(Methods[i].Name))
                        {
                            Methods[i]?.Invoke(target, null);
                        }
                        GUILayout.Label("", GUI.skin.horizontalSlider);
                        EditorGUILayout.Space();
                    }
                }
            }
        }
    }
}
public class TextOptimization : EditorWindow
{
    private TextAsset selectedTextAsset;
    private bool ifCouldBeRevoke = false;
    StringBuilder tempContenet;
    StringBuilder lastContenet = null;
    [MenuItem("Tools/TextOptimization", false, 2)]
    public static void ShowWindow()
    {
        GetWindow<TextOptimization>("TextOptimization");
    }
    private void OnGUI()
    {
        minSize = new Vector2(500, 200);
        maxSize = new Vector2(600, 200);
        selectedTextAsset = (TextAsset)EditorGUILayout.ObjectField("Text Asset", selectedTextAsset, typeof(TextAsset), true);
        GUILayout.Label("用于优化用作字体资产的文本文档，将重复字符和空格删除以减小文件大小", EditorStyles.label);
        if (selectedTextAsset != null)
        {
            if (GUILayout.Button("TextOptimize"))
            {
                ifCouldBeRevoke = true;
                lastContenet = new StringBuilder(selectedTextAsset.text);
                tempContenet = new StringBuilder(selectedTextAsset.text);
                char tempChar;
                for (int i = 0; i < tempContenet.Length; i++)
                {
                    tempChar = tempContenet[i];
                    for (int j = i + 1; j < tempContenet.Length; j++)
                    {
                        if (tempChar == tempContenet[j])
                        {
                            tempContenet.Remove(j, 1);
                            j--;
                        }
                    }
                }
                string path = AssetDatabase.GetAssetPath(selectedTextAsset);
                File.WriteAllText(path, tempContenet.ToString());
                AssetDatabase.Refresh();
                Debug.Log($"文本优化完成！共删除 {lastContenet.Length - tempContenet.Length} 个字符");
            }
            if (ifCouldBeRevoke)
            {
                if (GUILayout.Button("RevokeOptimize"))
                {
                    string path = AssetDatabase.GetAssetPath(selectedTextAsset);
                    File.WriteAllText(path, lastContenet.ToString());
                    AssetDatabase.Refresh();
                    ifCouldBeRevoke = false;
                    Debug.Log("已撤销优化！");
                }
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.yellow;
                GUILayout.Label("关闭此界面则无法撤回", style);
            }
        }
    }

}
