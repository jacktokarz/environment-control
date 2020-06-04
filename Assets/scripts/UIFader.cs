using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFader : MonoBehaviour {

    public CanvasGroup uiElement;

    public void fadeIn(CanvasGroup selectedElement)
    {
        Debug.Log("starting to fade in");
        selectedElement.alpha = 0;
        StartCoroutine(FadeCanvasGroup(selectedElement, selectedElement.alpha, 1, PersistentManager.Instance.fadeSpeed));
    }

    public void fadeOut(CanvasGroup uiElement)
    {
        Debug.Log("starting to fade out");
        StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 0, PersistentManager.Instance.fadeSpeed));
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 1)
	{
        Debug.Log("fading");
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
        Debug.Log("done fading");

        if (end == 0) {
            cg.gameObject.SetActive(false);
        }
	}
}
