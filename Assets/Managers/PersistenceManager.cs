using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class RunData
{
    public int bestTime;
    public int deathCount;
}

public class PersistenceManager : MonoBehaviour
{
    public static PersistenceManager Instance;
    private string filePath;
    private RunData currentBest = new RunData();

    void Awake()
    {
        // Singleton para acceso global
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            filePath = Application.persistentDataPath + "/bestRun.json";
            LoadData();
        } else {
            Destroy(gameObject);
        }
    }

    public void EvaluateRun(int newTime, int newDeaths)
    {
        if (File.Exists(filePath) == false) //Si no existe el archivo, se crea uno nuevo con los datos actuales
        {
            currentBest.bestTime = newTime;
            currentBest.deathCount = newDeaths;
            SaveData();
            return;
        }

        //Si ya existe el archivo, se evalua:
        if (newDeaths > currentBest.deathCount) return; //Si la nueva run tiene mas muertes que la anterior, se descarta
        if (newDeaths == currentBest.deathCount && newTime >= currentBest.bestTime) return; //Si tiene las mismas muertes pero peor o igual tiempo, se descarta

        currentBest.bestTime = newTime;
        currentBest.deathCount = newDeaths;
        SaveData();
    }

    private void SaveData()
    {
        string json = JsonUtility.ToJson(currentBest, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Datos guardados en: " + filePath);
    }

    private void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            currentBest = JsonUtility.FromJson<RunData>(json);
        }
    }

    public String GetBestRunString()
    {
        String runText;
        
        if (File.Exists(filePath) == false)
        {
            runText = "Time: -- Deaths: --";
        }
        else
        {
            runText ="Time: " + currentBest.bestTime + " - Deaths: " + currentBest.deathCount;
        }

        return runText;
    }
}