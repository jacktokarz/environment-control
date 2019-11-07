using UnityEngine;
using System.Collections.Generic;


public class EnvironmentChange : MonoBehaviour
{
    public AudioClip humN3;
    public AudioClip humN2;
    public AudioClip humN1;
    public AudioClip hum0;
    public AudioClip hum1;
    public AudioClip hum2;
    public AudioClip hum3;

    private AudioSource source;
    private int lastHumVal; //*Justin* rememberlast humidity value
    private void Start()
    {
        source = GetComponent<AudioSource>();
        lastHumVal = PersistentManager.Instance.humidityLevel;
    }

    private void Update()
    {
        List<string> tl = PersistentManager.Instance.TreasureList;

        if (tl.Contains("humidity") && Input.GetKey("1"))
        {
            if (Input.GetKeyDown("up"))
            {
                EnvironmentEffect.Instance.setHumidity(PersistentManager.Instance.humidityLevel + 1);
                if (lastHumVal != 3) { playHumiditySound(); }
            }
            else if (Input.GetKeyDown("down"))
            {
                EnvironmentEffect.Instance.setHumidity(PersistentManager.Instance.humidityLevel - 1);
                if (lastHumVal != -3) { playHumiditySound(); } //So that the lowest sound doesn't play when you're already on the lowest setting
                
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

    private void playHumiditySound()
    {
        //Play Correct Humidity Sound Clip
        if (PersistentManager.Instance.humidityLevel == 0)
        {
            source.Stop();
            source.clip = hum0;
            source.Play();
            lastHumVal = PersistentManager.Instance.humidityLevel;
        }
        else if (PersistentManager.Instance.humidityLevel == -1)
        {
            source.Stop();
            source.clip = humN1;
            source.Play();
            lastHumVal = PersistentManager.Instance.humidityLevel; 
            //source.clip = null;
        }
        else if (PersistentManager.Instance.humidityLevel == -2)
        {
            source.Stop();
            source.clip = humN2;
            source.Play();
            lastHumVal = PersistentManager.Instance.humidityLevel;
            //source.clip = null;
        }
        else if (PersistentManager.Instance.humidityLevel == -3)
        {
            source.Stop();
            source.clip = humN3;
            source.Play();
            lastHumVal = PersistentManager.Instance.humidityLevel;
            //source.clip = null;
        }
        else if (PersistentManager.Instance.humidityLevel == 1)
        {
            source.Stop();
            source.clip = hum1;
            source.Play();
            lastHumVal = PersistentManager.Instance.humidityLevel;
            //source.clip = null;
        }
        else if (PersistentManager.Instance.humidityLevel == 2)
        {
            source.Stop();
            source.clip = hum2;
            source.Play();
            lastHumVal = PersistentManager.Instance.humidityLevel;
            //source.clip = null;
        }
        else if (PersistentManager.Instance.humidityLevel == 3)
        {
            source.Stop();
            source.clip = hum3;
            source.Play();
            lastHumVal = PersistentManager.Instance.humidityLevel;
            //source.clip = null;
        }
    }
}

