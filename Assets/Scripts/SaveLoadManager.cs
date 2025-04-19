using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class SaveLoadManager : MonoBehaviour
{
    private string savePath;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "savegame.json");
    }

    // Metoda pro ulo�en� stavu sc�ny
    public void SaveScene()
    {
        List<ObjectData> objectDataList = new List<ObjectData>();

        // Projdi v�echny objekty ve sc�n�, kter� maj� komponentu "SaveableObject"
        foreach (SaveableObject saveableObject in FindObjectsOfType<SaveableObject>())
        {
            ObjectData data = new ObjectData
            {
                objectName = saveableObject.gameObject.name,
                position = saveableObject.transform.position,
                rotation = saveableObject.transform.rotation,
                scale = saveableObject.transform.localScale,
                // M��e� p�idat dal�� data, kter� chce� ulo�it
            };

            objectDataList.Add(data);
        }

        // P�evedeme data na JSON
        string json = JsonUtility.ToJson(new SceneData { objects = objectDataList });

        // Ulo��me JSON do souboru
        File.WriteAllText(savePath, json);

        Debug.Log("Scene saved to " + savePath);
    }

    // Metoda pro na�ten� stavu sc�ny
    public void LoadScene()
    {
        if (File.Exists(savePath))
        {
            // Na�teme JSON ze souboru
            string json = File.ReadAllText(savePath);

            // P�evedeme JSON zp�t na objekt
            SceneData sceneData = JsonUtility.FromJson<SceneData>(json);

            // Obnov�me stav v�ech objekt�
            foreach (ObjectData data in sceneData.objects)
            {
                GameObject obj = GameObject.Find(data.objectName);
                if (obj != null)
                {
                    obj.transform.position = data.position;
                    obj.transform.rotation = data.rotation;
                    obj.transform.localScale = data.scale;
                }
            }

            Debug.Log("Scene loaded from " + savePath);
        }
        else
        {
            Debug.Log("No save file found.");
        }
    }

    // T��da pro ukl�d�n� dat sc�ny
    [System.Serializable]
    private class SceneData
    {
        public List<ObjectData> objects;
    }

    // T��da pro ukl�d�n� dat jednoho objektu
    [System.Serializable]
    private class ObjectData
    {
        public string objectName;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
    }
}