using UnityEngine;
using System.Collections.Generic;


public class EnvironmentChange : MonoBehaviour
{

    private void Update()
    {
        List<string> tl = PersistentManager.Instance.TreasureList;

        if (tl.Contains("humidity") && Input.GetKey("1"))
        {
            if (Input.GetKeyDown("up"))
            {
                EnvironmentEffect.Instance.setHumidity(PersistentManager.Instance.humidityLevel + 1);
            }
            else if (Input.GetKeyDown("down"))
            {
                EnvironmentEffect.Instance.setHumidity(PersistentManager.Instance.humidityLevel - 1);
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
}

