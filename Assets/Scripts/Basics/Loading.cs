using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections;

using GameLogic;

public class Loading : MonoBehaviour
{
    public Slider SliderProgress;
	private AsyncOperation operation;

	void Start()
	{
		string sceneName = Game.Instance.LoadingStageEnum.ToString();
		operation = SceneManager.LoadSceneAsync(sceneName);
		operation.allowSceneActivation = false;
	}

    void Update()
    {
        if(operation.progress < 0.9f)
        {
            SliderProgress.value = operation.progress;
        }
        else
        {
            SliderProgress.value = 1f;
            operation.allowSceneActivation = true; 
        }
    }

}

