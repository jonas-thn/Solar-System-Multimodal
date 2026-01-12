using UnityEngine;
using System.IO;

public class ModelLoader : MonoBehaviour
{
    void Awake()
    {
        string sourcePath = Application.streamingAssetsPath;
        string destinationPath = Application.persistentDataPath;

        if (!Directory.Exists(sourcePath)) return;

        foreach (var filepath in Directory.GetFiles(sourcePath, "*"))
        {
            if (filepath.EndsWith(".meta"))
            {
                continue;
            }

            string fileName = Path.GetFileName(filepath);
            string destFile = Path.Combine(destinationPath, fileName);

            try
            {
                File.Copy(filepath, destFile, true);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Fehler beim Kopieren von {fileName}: {e.Message}");
            }
        }

        Destroy(gameObject);
    }
}
