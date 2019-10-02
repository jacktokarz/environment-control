using UnityEngine;
using System.Collections.Generic;


public class EnvironmentChange : MonoBehaviour
{
    
    private void Start()
    {
        PersistentManager.Instance.changeWindAnimation();
    }

    private void Update()
    {
        List<string> tl = PersistentManager.Instance.TreasureList;
        if (tl.Contains("humidity") && Input.GetKey("1"))
        {
            if (Input.GetKeyDown("up"))
            {
                if (PersistentManager.Instance.changeHumidity(1))
                {
                    EnvironmentEffect.Instance.humidityChange(1);
                }
            }
            else if (Input.GetKeyDown("down"))
            {
                if (PersistentManager.Instance.changeHumidity(-1))
                {
                    EnvironmentEffect.Instance.humidityChange(-1);
                }
            }
        }
        if (tl.Contains("wind") && Input.GetKey("2"))
        {
            if (Input.GetKeyDown("up"))
            {
                PersistentManager.Instance.changeWind(1);
            }
            else if (Input.GetKeyDown("down"))
            {
                PersistentManager.Instance.changeWind(-1);
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
