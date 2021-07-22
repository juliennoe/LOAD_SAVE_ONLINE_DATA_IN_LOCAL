using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageDownloader : MonoBehaviour
{
    [SerializeField]
    private Text m_text;
    [SerializeField]
    private string m_url;
    [SerializeField]
    private string m_savePath;
   
    private void Start()
    {
        ReadUrlData();
    }

    // Defini le chemin d'acces de l'URL et le chemin de sauvegarde
    private void ReadUrlData()
    {
        Debug.Log(Application.persistentDataPath);
        m_url = "https://www.pristimantis.com/json/info.jpg";
        //Save Path
        m_savePath = Path.Combine(Application.persistentDataPath + "/data/Images/logo.png");
        m_text.text = m_savePath;
        DownloadImage(m_url, m_savePath);
    }

    // Lance la coroutine de lecture du lien URL
    public void DownloadImage(string url, string pathToSaveImage)
    {
        StartCoroutine(C_downloadImage(url, pathToSaveImage));
    }

    // Coroutine de téléchargement du lien URL
    private IEnumerator C_downloadImage(string url, string savePath)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.LogError(uwr.error);
            }
            else
            {
                Debug.Log("Success");
                Texture myTexture = DownloadHandlerTexture.GetContent(uwr);
                byte[] results = uwr.downloadHandler.data;
                SaveImage(savePath, results);

            }
        }
    }

    // Sauvegarde de l'image URL en local via Bytes
    private void SaveImage(string path, byte[] imageBytes)
    {
        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        try
        {
            File.WriteAllBytes(path, imageBytes);
            Debug.Log("Saved Data to: " + path.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Save Data to: " + path.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }
    }
    byte[] LoadImage(string path)
    {
        byte[] dataByte = null;

        //Exit if Directory or File does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Debug.LogWarning("Directory does not exist");
            return null;
        }

        if (!File.Exists(path))
        {
            Debug.Log("File does not exist");
            return null;
        }

        try
        {
            dataByte = File.ReadAllBytes(path);
            Debug.Log("Loaded Data from: " + path.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Load Data from: " + path.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }

        return dataByte;
    }
}