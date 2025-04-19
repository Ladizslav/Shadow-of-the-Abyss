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

    // Metoda pro uložení stavu scény
    public void SaveScene()
    {
        List<ObjectData> objectDataList = new List<ObjectData>();

        // Projdi všechny objekty ve scénì, které mají komponentu "SaveableObject"
        foreach (SaveableObject saveableObject in FindObjectsOfType<SaveableObject>())
        {
            ObjectData data = new ObjectData
            {
                objectName = saveableObject.gameObject.name,
                position = saveableObject.transform.position,
                rotation = saveableObject.transform.rotation,
                scale = saveableObject.transform.localScale,
                // Mùžeš pøidat další data, která chceš uložit
            };

            objectDataList.Add(data);
        }

        // Pøevedeme data na JSON
        string json = JsonUtility.ToJson(new SceneData { objects = objectDataList });

        // Uložíme JSON do souboru
        File.WriteAllText(savePath, json);

        Debug.Log("Scene saved to " + savePath);
    }

    // Metoda pro naètení stavu scény
    public void LoadScene()
    {
        if (File.Exists(savePath))
        {
            // Naèteme JSON ze souboru
            string json = File.ReadAllText(savePath);

            // Pøevedeme JSON zpìt na objekt
            SceneData sceneData = JsonUtility.FromJson<SceneData>(json);

            // Obnovíme stav všech objektù
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

    // Tøída pro ukládání dat scény
    [System.Serializable]
    private class SceneData
    {
        public List<ObjectData> objects;
    }

    // Tøída pro ukládání dat jednoho objektu
    [System.Serializable]
    private class ObjectData
    {
        public string objectName;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
    }
}