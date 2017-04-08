using UnityEngine;
using UnityEngine.SceneManagement;

using System;

public class Entry : MonoBehaviour
{
    void Awake()
    {
        CreateEternalGO("EventSystem");
        CreateEternalGO("Environment");
        CreateEternalGO("Astar");
        CreateEternalGO("Cameras");
        CreateEternalGO("LoadingUI");

        SceneManager.LoadScene("HomeTown");
    }

    void CreateEternalGO(string name)
    {
        GameObject prefab = Resources.Load<GameObject>("Main/" + name);
        GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity, null);
        go.name = name;
        DontDestroyOnLoad(go);
    }
    
}

