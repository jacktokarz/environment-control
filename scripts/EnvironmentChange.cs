using UnityEngine;
using System.Collections.Generic;


public class EnvironmentChange : MonoBehaviour
{
    public EnvironmentEffect envEffect;


    void FixedUpdate()
    {
        List<string> tl = PersistentManager.Instance.TreasureList;
        if (tl.Contains("humidity") && Input.GetKey("1"))
        {
            if (Input.GetKeyDown("up"))
            {
                if (PersistentManager.Instance.changeHumidity(1))
                {
                    envEffect.humidityChange(1);
                }
            }
            else if (Input.GetKeyDown("down"))
            {
                if (PersistentManager.Instance.changeHumidity(-1))
                {
                    envEffect.humidityChange(-1);
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
    }


}
