using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomValues : MonoBehaviour
{
	public int startingHumidity = 0;
	public int startingWind = 0;
	public int startingTemperature = 0;
    void Start()
    {
		PersistentManager.Instance.setHumidity(startingHumidity);
        PersistentManager.Instance.setWind(startingWind);
        EnvironmentEffect.Instance.setTemp(startingTemperature);
    }
}
