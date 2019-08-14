using UnityEngine;


public class EnvironmentChange : MonoBehaviour
{
    public EnvironmentEffect envEffect;


    void FixedUpdate()
    {
        if (Input.GetKey("1"))
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
        if (Input.GetKey("2"))
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
