using UnityEngine;
using System.Collections.Generic;


public class EnvironmentChange : MonoBehaviour
{

    private void Update()
    {
        if (!PersistentManager.Instance.zooming && Input.GetKey(PersistentManager.Instance.ZoomKey))
        {
            List<float> zoomO = new List<float>(PersistentManager.Instance.zoomOptions);
            int currentIndex = zoomO.IndexOf(PersistentManager.Instance.currentZoom);
            if(currentIndex==zoomO.Count-1) {
                StartCoroutine(PersistentManager.Instance.ChangeZoom(zoomO[0], PersistentManager.Instance.fadeSpeed));
            }
            else
            {
                StartCoroutine(PersistentManager.Instance.ChangeZoom(zoomO[currentIndex+1], PersistentManager.Instance.fadeSpeed));
            }
        }

        List<string> tl = PersistentManager.Instance.TreasureList;

        if (tl.Contains("humidity") && Input.GetKey(PersistentManager.Instance.HumidityKey))
        {
            if (Input.GetKeyDown("up"))
            {
                EnvironmentEffect.Instance.setHumidity(PersistentManager.Instance.humidityLevel + 1);
            }
            else if (Input.GetKeyDown("down"))
            {
                EnvironmentEffect.Instance.setHumidity(PersistentManager.Instance.humidityLevel - 1);
            }
        }   

        if (tl.Contains("wind") && Input.GetKey("2"))
        {
            if (Input.GetKeyDown("up"))
            {
                EnvironmentEffect.Instance.setWind(PersistentManager.Instance.windLevel + 1);
            }
            else if (Input.GetKeyDown("down"))
            {
                EnvironmentEffect.Instance.setWind(PersistentManager.Instance.windLevel - 1);
            }
        }

        if (tl.Contains("temperature") && Input.GetKey("3"))
        {
            if (Input.GetKeyDown("up"))
            {
                EnvironmentEffect.Instance.setTemp(PersistentManager.Instance.tempLevel + 1);
            }
            else if (Input.GetKeyDown("down"))
            {
                EnvironmentEffect.Instance.setTemp(PersistentManager.Instance.tempLevel - 1);
            }
        }
    }
}

