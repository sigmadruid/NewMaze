using UnityEngine;
using UnityEngine.UI;

using System.Collections;

using Base;

public class MazeEditorPanel : MonoBehaviour 
{
    public Image imgRegenerate;

    public MazeMain mazeMain;

    void Awake()
    {
        EventTriggerListener.Get(imgRegenerate.gameObject).onClick = OnRegenerateMaze;
    }

    private void OnRegenerateMaze(GameObject go)
    {
        int seed = UnityEngine.Random.Range(0, 20140413);
        mazeMain.GenerateMaze(seed);
    }
}
