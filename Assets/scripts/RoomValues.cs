using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomValues : MonoBehaviour
{
	public int startingHumidity = 0;
	public int startingWind = 0;
	public int startingTemperature = 0;
	public int startingToxicity;
    
    public void setInitialValues()
    {
		EnvironmentEffect.Instance.setHumidity(startingHumidity);
        EnvironmentEffect.Instance.setWind(startingWind);
        EnvironmentEffect.Instance.setTemp(startingTemperature);
        EnvironmentEffect.Instance.setToxicity(startingToxicity);
    }
}
