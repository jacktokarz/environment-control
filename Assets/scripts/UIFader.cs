using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFader : MonoBehaviour {

    public static UIFader Instance { get; private set; }
    public CanvasGroup uiElement;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void fadeIn(CanvasGroup selectedElement)
    {
        selectedElement.alpha = 0;
        StartCoroutine(FadeCanvasGroup(selectedElement, selectedElement.alpha, 1, PersistentManager.Instance.fadeSpeed));
    }

    public void fadeOut(CanvasGroup uiElement)
    {
        StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 0, PersistentManager.Instance.fadeSpeed));
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 1)
	{
		float _timeStartedLerping = Time.time;
		float timeSinceStarted = Time.time - _timeStartedLerping;
		float percentageComplete = timeSinceStarted / lerpTime;

        if (start == 0) {
            cg.gameObject.SetActive(true);
        }

		while (true)
		{
			timeSinceStarted = Time.time - _timeStartedLerping;
			percentageComplete = timeSinceStarted / lerpTime;

			float currentValue = Mathf.Lerp(start, end, percentageComplete);

            cg.alpha = currentValue;

            if (percentageComplete >= 1) break;

			yield return new WaitForFixedUpdate();
		}
	}
}
