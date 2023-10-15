#if UNITY_EDITOR
namespace EasyPlayfab.Editor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using System.IO;
    using PlayFab.PfEditor;
    using PlayFab.PfEditor.EditorModels;
    using PlayFab;
    using PlayFab.ClientModels;
    using UnityEngine.Networking;

    public class EasyPlayfabEditor : EditorWindow
    {
        [MenuItem("EasyPlayfab/Settings", false, 1)]
        private static void Init()
        {
            EasyPlayfabEditor s = CreateWindow<EasyPlayfabEditor>();
            s.Show();
            s.titleContent = new GUIContent()
            {
                text = "Settings",
                tooltip = "Settings for EasyPlayfab"
            };

            if (EditorPrefs.HasKey("appId"))
            {
                appId = EditorPrefs.GetString("appId");
                appSecret = EditorPrefs.GetString("appSecret");
            }
        }
        bool last;
        bool callable = true;
        static string appId = "app id";
        static string appSecret = "app secret";


        public void OnGUI()
        {
            maxSize = new Vector2(300, 200);
            minSize = new Vector2(300, 200);
            GUILayout.BeginVertical("box");
            GUILayout.Label("Will we automatically login to playfab?\nIf not, then we will change it to a callable method.\n<size=10%>(Optional)</size>");
            callable = GUILayout.Toggle(callable, "Auto-login");
            if (callable != last)
            {
                ModifyCheckForAutoLogin();
            }
            last = callable;
            GUILayout.EndVertical();
            GUILayout.Space(10);
            GUI.color = Color.cyan;
            GUILayout.BeginVertical("box");
            GUI.color = Color.white;
            GUILayout.BeginHorizontal();
            GUILayout.Label("OC|");
            appId = EditorGUILayout.TextField(appId);
            GUILayout.Label("|");
            appSecret = EditorGUILayout.TextField(appSecret);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUI.skin.label.richText = true;
            GUI.backgroundColor = Color.black;
            GUILayout.BeginHorizontal("box");
            GUILayout.Label("This will add the cloudscript\nneeded for your title.\n<size=10%>(Required)</size>", GUILayout.ExpandWidth(false), GUILayout.Width(160));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("box");
            GUILayout.Label("Rule Setup Tut.\n<size=10%>(Required)</size>", GUILayout.ExpandWidth(false), GUILayout.Width(104));
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.white;
            if (!int.TryParse(appId, out int app))
                GUI.enabled = false;
            else
            {
                EditorPrefs.SetString("appId", appId);
                EditorPrefs.SetString("appSecret", appSecret);
                GUI.enabled = true;
            }
            if (GUILayout.Button("Validate CloudScript", GUILayout.ExpandWidth(false), GUILayout.Width(170)))
            {
                PlayFabEditorApi.GetCloudScriptRevision(new GetCloudScriptRevisionRequest { }, s =>
                {
                    Debug.Log("Validating...");
                    bool contained = false;
                    foreach (CloudScriptFile file in s.Files)
                    {
                        if (file.FileContents.Contains("handlers.NewLogin"))
                        {
                            contained = true;
                        }
                    }
                    if (s.IsPublished && !contained)
                    {
                        Debug.Log("Didnt contain required cloudscript, adding now...");
                        List<CloudScriptFile> Files = s.Files;
                        Files.Add(new CloudScriptFile() { Filename = "easyplayfab", FileContents = FetchTextFromURL().Replace("appId", appId).Replace("appSecret", appSecret) });
                        PlayFabEditorApi.UpdateCloudScript(new UpdateCloudScriptRequest { Files = Files, Publish = true }, r =>
                        {
                            if (EditorUtility.DisplayDialog("IMPORTANT", "The cloudscript is in your title, but you still have one more step to complete. Go to EasyPlayfab (at the top) > Rule", "Sure", "Later"))
                            {
                                CreateWindow<EasyPlayfabRule>();
                            }
                        }, e => { Debug.Log("Error with request: " + e.ErrorMessage); });
                    }
                    else
                    {
                        Debug.Log("Cloudscripts already contained the needed cloudscript.");
                    }
                }, e => { Debug.Log("Error with request: " + e.ErrorMessage); });
            }
            if (GUILayout.Button("Rule Tutorial", GUILayout.ExpandWidth(false), GUILayout.Width(112)))
            {
                CreateWindow<EasyPlayfabRule>();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUI.color = Color.white;
        }

        private void ModifyCheckForAutoLogin()
        {
            string scriptPath = "Assets/EasyPlayfab/LoginHandle.cs";
            string scriptContent = File.ReadAllText(scriptPath);

            string privateMethodSignature = "private static void Login()";
            string publicMethodSignature = "public static void Login()";

            bool isPrivate = scriptContent.Contains(privateMethodSignature);

            if (!callable)
            {
                if (isPrivate)
                {
                    scriptContent = scriptContent.Replace(privateMethodSignature, publicMethodSignature);
                    scriptContent = scriptContent.Replace("[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]", "");
                }
            }
            else
            {
                if (!isPrivate)
                {
                    scriptContent = scriptContent.Replace(publicMethodSignature, privateMethodSignature);
                    scriptContent = scriptContent.Replace("private static void Login()", "[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]\n        private static void Login()");
                }
            }

            File.WriteAllText(scriptPath, scriptContent);
            AssetDatabase.Refresh();
        }

        private string FetchTextFromURL()
        {
            UnityWebRequest webRequest = UnityWebRequest.Get("https://raw.githubusercontent.com/JokerJosh0/EasyPlayfab/main/cloudscripts");
            webRequest.SendWebRequest();

            while (!webRequest.isDone)
            {
            }

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                return webRequest.downloadHandler.text;
            }
            return "";
        }

    }

    public class HelpEditor : EditorWindow
    {
        [MenuItem("EasyPlayfab/Help || Feedback")]
        private static void Init()
        {
            CreateWindow<HelpEditor>().titleContent = new GUIContent()
            {
                text = "Help/Feedback"
            }; ;
        }

        public void OnGUI()
        {
            GUILayout.BeginVertical("box");
            GUILayout.Label("You can get help here, or just if you want to suggest what to add next");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Discord Server"))
            {
                Application.OpenURL("https://discord.gg/eSPXMZGJgn");
            }
            GUI.skin.button.richText = true;
            if (GUILayout.Button("My Discord <size=10%>(Profile)</size>"))
            {
                Application.OpenURL("https://discord.com/users/791550177780563998");
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
    }
}
#endif
