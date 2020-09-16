using UnityEngine;
using System.Collections.Generic;


public class EnvironmentChange : MonoBehaviour
{
    private bool needsReset;

    private void Update()
    {
        if (!PersistentManager.Instance.zooming && Input.GetKey(PersistentManager.Instance.ZoomKey))
        {
            if (PersistentManager.Instance.vcam == null)
            {
                PersistentManager.Instance.GetCameraAndZoom();
            }
            Debug.Log("vcam "+PersistentManager.Instance.vcam);
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
        if (Input.GetAxis("Vertical") == 0)
        {
            needsReset = false;
        }
        if (tl.Contains("humidity") && Input.GetKey(PersistentManager.Instance.HumidityKey))
        {
            if (Input.GetKeyDown("up") || (Input.GetAxis("Vertical") > 0.5f && !needsReset))
            {
                EnvironmentEffect.Instance.setHumidity(PersistentManager.Instance.humidityLevel + 1);
                needsReset = true;
            }
            else if (Input.GetKeyDown("down") || (Input.GetAxis("Vertical") < -0.5f && !needsReset))
            {
                EnvironmentEffect.Instance.setHumidity(PersistentManager.Instance.humidityLevel - 1);
                needsReset = true;
            }
        }
        if (tl.Contains("wind") && Input.GetKey("2"))
        {
            if (Input.GetKeyDown("up") || (Input.GetAxis("Vertical") > 0.5f && !needsReset))
            {
                EnvironmentEffect.Instance.setWind(PersistentManager.Instance.windLevel + 1);
                needsReset = true;
            }
            else if (Input.GetKeyDown("down") || (Input.GetAxis("Vertical") > 0.5f && !needsReset))
            {
                EnvironmentEffect.Instance.setWind(PersistentManager.Instance.windLevel - 1);
                needsReset = true;
            }
        }
        if (tl.Contains("temperature") && Input.GetKey("3"))
        {
            if (Input.GetKeyDown("up") || (Input.GetAxis("Vertical") > 0.5f && !needsReset))
            {
                EnvironmentEffect.Instance.setTemp(PersistentManager.Instance.tempLevel + 1);
                needsReset = true;
            }
            else if (Input.GetKeyDown("down") || (Input.GetAxis("Vertical") > 0.5f && !needsReset))
            {
                EnvironmentEffect.Instance.setTemp(PersistentManager.Instance.tempLevel - 1);
                needsReset = true;
            }
        }
    }
}

