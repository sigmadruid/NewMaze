using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;

using GameLogic;

public class Loading : MonoBehaviour
{
	public UISlider SliderProgress;
	private AsyncOperation operation;

	void Start()
	{
		string sceneName = Game.Instance.LoadingStageEnum.ToString();
		operation = SceneManager.LoadSceneAsync(sceneName.ToString());
		operation.allowSceneActivation = false;

		StartCoroutine(UpdateLoading());
	}

	IEnumerator UpdateLoading()
	{
		while(operation.progress < 0.9f) 
		{
			SliderProgress.value = operation.progress;
			yield return new WaitForEndOfFrame();
		}
		SliderProgress.value = 1f;
		
		operation.allowSceneActivation = true;  
//		yield return new WaitForEndOfFrame();
//		Game.Instance.SwitchStageComplete();
	}
}

