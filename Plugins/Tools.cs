using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerPrefs管理器 : EditorWindow
{
    private string key;
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
        GUILayout.Label("查看键值：\n输入键值以获取数据", EditorStyles.label);
        key = EditorGUILayout.TextField("输入playerprefs标签：", key);
        if (!PlayerPrefs.HasKey(key))
        {
            GUI.contentColor = Color.yellow;
            GUILayout.Label("不存在此键值");
            GUI.contentColor = Color.white;
        }
        else
        {
            GUILayout.Label("int类型下值为: " + PlayerPrefs.GetInt(key).ToString(), EditorStyles.boldLabel);
            GUILayout.Label("float类型下值为: " + PlayerPrefs.GetFloat(key).ToString(), EditorStyles.boldLabel);
            GUILayout.Label("string类型下值为: " + PlayerPrefs.GetString(key).ToString(), EditorStyles.boldLabel);
            GUI.contentColor = Color.yellow;
            GUILayout.Label("请自行辨别正确的类型!");
            GUI.contentColor = Color.white;
        }
        GUILayout.Label("\n\n创建或修改键值：", EditorStyles.label);
        enter_key = EditorGUILayout.TextField("想设置的playerprefs标签：", enter_key);
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



