using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections;

using GameLogic;

public enum LoadingState
{
    EndStage,
    LoadScene,
    LoadAssets,
    StartStage,
    StartOver,
}

public class Loading : MonoBehaviour
{
    public Slider SliderProgress;
	private AsyncOperation operation;
    private string sceneName;
    private LoadingState currentState;
    private int totalProgress = 0;

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
        if (operation != null)
            SetProgress(LoadingState.LoadScene, (int)(operation.progress * 33));
    }

    public void Begin(string sceneName)
    {
        this.sceneName = sceneName;
        gameObject.SetActive(true);
    }

    public IEnumerator SetProgress(LoadingState state, int progress)
    {
        if(state == LoadingState.EndStage)
        {
            totalProgress = Mathf.Min(0 + progress, 10);
        }
        else if(state == LoadingState.LoadScene)
        {
            if(currentState == LoadingState.EndStage)
            {
                SceneManager.LoadScene("Loading");
                operation = SceneManager.LoadSceneAsync(sceneName);
                operation.allowSceneActivation = false;
                totalProgress = 10;
            }
            totalProgress = Mathf.Min(totalProgress + progress, 30);
        }
        else if(state == LoadingState.LoadAssets)
        {
            if(currentState == LoadingState.LoadScene)
            {
                totalProgress = 20;
            }
            totalProgress = Mathf.Min(totalProgress + progress, 80);
        }
        else if(state == LoadingState.StartStage)
        {
            if(currentState == LoadingState.LoadScene)
            {
                operation.allowSceneActivation = true; 
                totalProgress = 80;
            }
            totalProgress = Mathf.Min(totalProgress + progress, 90);
        }
        else if(state == LoadingState.StartOver)
        {
            totalProgress = 100;
            gameObject.SetActive(false);
        }
        currentState = state;
        Debug.LogFormat("{0} progress {1}", currentState, totalProgress);

        SliderProgress.value = totalProgress * 1f / 100f;
        yield return null;
    }

}

