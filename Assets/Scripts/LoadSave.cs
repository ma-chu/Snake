using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LoadSave : MonoBehaviour
{
    public static event Action Save;
    public static event Action Load;
    
    public static SaveData SaveSnapshot = new SaveData()/*ScriptableObject.CreateInstance<SaveData>()*/;
    
    // Для не нулевых значений сохраняемых полей нажимать при запущенной в редакторе игре
    [MenuItem("EF/Save config")]    // атрибут только для статических методов
    private static void SaveConfig()
    {
        Save?.Invoke();

        var json = JsonUtility.ToJson(SaveSnapshot);
        
        // сюда д.б. разрешение на запись везде: в андроиде, винде, ...
        var filePath = Path.Combine(Application.persistentDataPath/*Application.dataPath*/, "config.json");
        System.IO.File.WriteAllText(filePath, json);
        Debug.Log(filePath + " saved");
    }
    
    public static  void LoadConfig()
    {
        var filePath = Path.Combine(Application.persistentDataPath, "config.json");
        if (System.IO.File.Exists(filePath))
        {
            var str = System.IO.File.ReadAllText(filePath);

            try
            {
                SaveSnapshot = JsonUtility.FromJson<SaveData>(str);
            }
            catch
            {
                Debug.Log("Bad config.json file "+ filePath);
                return;
            }

            // Проверяем поля-примитивы на не 0 с помощью отражения
            Type type = SaveSnapshot.GetType();
            foreach (var field in type.GetFields())
            {
                var value = field.GetValue(SaveSnapshot);
                var valueType = value.GetType();
                if (valueType.IsPrimitive && value.Equals(0))
                {
                    Debug.Log("There are zero parameters in config.json file");
                    return;
                }
            }
            
            Load?.Invoke();
            Debug.Log("config.json loaded");
        }
        else
        {
            Debug.Log("config.json doesn't exist");
        }
    }
}
