using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections;

using GameLogic;

public enum LoadingState
{
    EndStage,
    LoadScene,
    StartStage,
}

public class Loading : MonoBehaviour
{
    public Slider SliderProgress;
	private AsyncOperation operation;
    private string sceneName;
    private LoadingState currentState;

    public static Loading Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }
    void OnDestroy()
    {
        Instance = null;
    }

    void Update()
    {
        SetProgress(LoadingState.LoadScene, (int)(operation.progress * 33));
    }

    public void Begin(string sceneName)
    {
        this.sceneName = sceneName;
        gameObject.SetActive(true);
    }

    public void SetProgress(LoadingState state, int progress)
    {
        int totalProgress = 0;
        if(state == LoadingState.EndStage)
        {
            totalProgress = Mathf.Min(0 + progress, 33);
        }
        else if(state == LoadingState.LoadScene)
        {
            if(currentState == LoadingState.EndStage)
            {
                SceneManager.LoadScene("Loading");
                operation = SceneManager.LoadSceneAsync(sceneName);
                operation.allowSceneActivation = false;
            }
            totalProgress = Mathf.Min(33 + progress, 66);
        }
        else if(state == LoadingState.StartStage)
        {
            if(currentState == LoadingState.LoadScene)
            {
                operation.allowSceneActivation = true; 
            }
            totalProgress = Mathf.Min(66 + progress, 100);
            if(totalProgress >= 100)
                gameObject.SetActive(false);
        }
        currentState = state;

        SliderProgress.value = totalProgress * 1f / 100f;
    }

}

