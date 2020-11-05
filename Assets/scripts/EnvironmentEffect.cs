using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentEffect : MonoBehaviour
{
    public static EnvironmentEffect Instance { get; private set; }
    //public AudioSource Fansource { get => fansource; set => fansource = value; }
    public AudioClip humN3;
    public AudioClip humN2;
    public AudioClip humN1;
    public AudioClip hum0;
    public AudioClip hum1;
    public AudioClip hum2;
    public AudioClip hum3;
    public AudioClip fanSound;

    private AudioSource source;
    public AudioSource fansource;
    //private GameObject fanparent;

    BasicMovement BasMov;

    float waterStartTime;
    
    GameObject[] vines;
    GameObject[] bushes;
    GameObject[] fans;
    GameObject[] fronds;
    GameObject[] mosses;
    BoxCollider2D[] waterColliders;
    Vector2[] waterColliderOffsetOriginals;
    Vector2[] waterColliderOffsetStarts;
    Vector2[] waterColliderSizeOriginals;
    Vector2[] waterColliderSizeStarts;
    GameObject[] waterLines;
    Vector3[] waterOriginalPoints;
    Vector3[] waterStartPoints;
    Vector3[] waterEndPoints;
    Vector2 vineSize;

    LayerMask playerLayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void Start()
    {
        source = GetComponent<AudioSource>();

        BasMov = GameObject.FindGameObjectWithTag("Player").GetComponent<BasicMovement>();
        playerLayer = LayerMask.GetMask("Player");
        vines = GameObject.FindGameObjectsWithTag("vine");
        bushes = GameObject.FindGameObjectsWithTag("bush");
        fans = GameObject.FindGameObjectsWithTag("fan");
        fronds = GameObject.FindGameObjectsWithTag("frond");
        mosses = GameObject.FindGameObjectsWithTag("moss");
        GameObject[] waterColliderHolders = GameObject.FindGameObjectsWithTag("water");
        waterLines = GameObject.FindGameObjectsWithTag("waterLine");
        if (waterLines.Length > 0)
        {
            Debug.Log("water lines length: "+waterLines.Length);
            waterColliders = new BoxCollider2D[waterLines.Length];
            waterColliderSizeOriginals = new Vector2[waterLines.Length];
            waterColliderSizeStarts = new Vector2[waterLines.Length];
            waterColliderOffsetOriginals = new Vector2[waterLines.Length];
            waterColliderOffsetStarts = new Vector2[waterLines.Length];
            waterEndPoints = new Vector3[waterLines.Length];
            waterStartPoints = new Vector3[waterLines.Length];
            waterOriginalPoints = new Vector3[waterLines.Length];
            for (int i = 0; i < waterLines.Length; i++) {
                BoxCollider2D wbc = waterColliderHolders[i].GetComponent<BoxCollider2D>();
                waterColliders[i] = wbc;
                Debug.Log("first original "+wbc.size);
                waterColliderSizeOriginals[i] = wbc.size;
                waterColliderSizeStarts[i] = wbc.size;
                waterColliderOffsetOriginals[i] = wbc.offset;
                waterColliderOffsetStarts[i] = wbc.offset;

                waterEndPoints[i]= waterLines[i].transform.position;
                waterStartPoints[i]= waterLines[i].transform.position;
                waterOriginalPoints[i] = waterLines[i].transform.position;
            }
            if (PersistentManager.Instance.humidityLevel != 0)
            {
                changeWaterLevel(PersistentManager.Instance.humidityLevel);
            }
        }

        RoomValues rv = this.GetComponent<RoomValues>();
        rv.setInitialValues();
        changeWindAnimation();
    }

    private void FixedUpdate()
    {
        for (int i= 0; i< waterLines.Length; i++)
        {
            GameObject wat= waterLines[i];
            if (wat.transform.position != waterEndPoints[i])
            {
                float journeyLength = Vector3.Distance(waterStartPoints[i], waterEndPoints[i]);
                float distCovered = (Time.time - waterStartTime) * PersistentManager.Instance.waterChangeSpeed;


            float parentScale = waterColliders[i].gameObject.transform.parent.transform.localScale.y;
            BoxCollider2D coll= waterColliders[i];
            float waterChange = (waterEndPoints[i].y - waterOriginalPoints[i].y) / parentScale;
        
            coll.offset = Vector2.Lerp(waterColliderOffsetStarts[i],
                new Vector2(coll.offset.x, waterChange / 4 + waterColliderOffsetOriginals[i].y), distCovered / journeyLength);
            coll.size = Vector2.Lerp(waterColliderSizeStarts[i],
                new Vector2(coll.size.x, waterChange / 2 + waterColliderSizeOriginals[i].y), distCovered / journeyLength);

            wat.transform.position = Vector3.Lerp(waterStartPoints[i], waterEndPoints[i], distCovered / journeyLength);
            }
        }
    }


    public bool setHumidity(int value, bool muted = false)
    {
        Debug.Log("trying to set humidity to "+value);
        if ((value>=PersistentManager.Instance.minHumidity) && (value<=PersistentManager.Instance.maxHumidity))
        {
            if (!muted)
            {
                playHumiditySound(value);
            }
            PersistentManager.Instance.humidityLevel = value;
            changeWaterLevel(value);
            changeFrondWidth();
            changeMossSize();
            if (PersistentManager.Instance.tempLevel == 2)
            {
                if (value == 2 || value == 3)
                {
                    extinguishBushes();
                }
                else
                {
                    burnBushes();
                }
            }
            PersistentManager.Instance.updateText(PersistentManager.Instance.Humidity, value);
            return true;
        }
        return false;
    }


    private void playHumiditySound(int humValue)
    {
        source.Stop();
        if (humValue == -3) {source.clip = humN3;}
        else if (humValue == -2) {source.clip = humN2;}
        else if (humValue == -1) {source.clip = humN1;}
        else if (humValue == 0) {source.clip = hum0;}
        else if (humValue == 1) {source.clip = hum1;}
        else if (humValue == 2) {source.clip = hum2;}
        else if (humValue == 3){source.clip = hum3;}
        else { return; }
        source.Play();
    }

    void changeWaterLevel(int value)
    {
        for (int i = 0; i < waterLines.Length; i++)
        {
            BoxCollider2D coll= waterColliders[i];
            waterColliderOffsetStarts[i] = coll.offset;
            waterColliderSizeStarts[i] = coll.size;

            waterStartPoints[i] = waterLines[i].transform.position;
            waterEndPoints[i].y = value * PersistentManager.Instance.waterChangeDistance + waterOriginalPoints[i].y;
        }
        waterStartTime= Time.time;
    }

    void changeFrondWidth()
    {
        foreach (GameObject frond in fronds)
        {
            frond.transform.localScale = new Vector3(PersistentManager.Instance.humidityLevel * 0.4f + 2.7f, frond.transform.localScale.y, 0);
        }
    }



    public bool setWind(int value)
    {
        Debug.Log("changing wind by "+value);
        if ((value >= PersistentManager.Instance.minWind) && (value <= PersistentManager.Instance.maxWind))
        {
            PersistentManager.Instance.windLevel = value;
            PersistentManager.Instance.updateText(PersistentManager.Instance.Wind, value);
            changeWindAnimation();
            return true;
        }
        return false;
    }

    public void changeWindAnimation()
    {
        foreach (GameObject fan in fans)
        {
            GameObject win = null;
            GameObject blade = null;
            for (int i = 0; i < fan.transform.childCount; i++)
            {
                GameObject child = fan.transform.GetChild(i).gameObject;
                if (child.tag == "wind")
                {
                    win = child;
                }
                if (child.tag == "fanBlade")
                {
                    blade = child;
                }
            }
            if (win!=null)
            {
                fansource = win.transform.parent.gameObject.GetComponent<AudioSource>();
                //Debug.Log("wind's Parent: " + win.transform.parent.name);
                //fansource = fanparent.GetComponent<AudioSource>();
                fansource.clip = fanSound;
                //fansource.pitch = UnityEngine.Random.Range(0.25F, 1.25F);
                if (PersistentManager.Instance.windLevel > 0)
                {
                    fansource.Play();
                }
                else
                {
                    fansource.Stop();
                }
                Animator winAm = win.GetComponent<Animator>();
                winAm.SetFloat("windSpeed", PersistentManager.Instance.windLevel);                
            }
            if (blade!=null)
            {
                Animator bladeAnim = blade.GetComponent<Animator>();
                bladeAnim.SetFloat("windSpeed", PersistentManager.Instance.windLevel);
            }
        }
    }




    public void setTemp(int value)
    {
        if(value > PersistentManager.Instance.tempLevel)
        {
            if(value <= PersistentManager.Instance.maxTemp)
            {
                PersistentManager.Instance.tempLevel = value;
                PersistentManager.Instance.updateText(PersistentManager.Instance.Temperature, PersistentManager.Instance.tempLevel);

                if(value == -1)
                {
                    StartCoroutine(thawBushes());
                    BasMov.changePlayerStats();
                    thawMoss();
                }
                else if(value == 0)
                {
                    StartCoroutine(thawBushes());
                    thawWater();
                    thawVines();
                    BasMov.changePlayerStats();
                }
                else if(value == 2)
                {
                    burnBushes();
                    burnMoss();
                }
            }
        }
        else
        {
            if(value >= PersistentManager.Instance.minTemp)
            {
                PersistentManager.Instance.tempLevel = value;
                PersistentManager.Instance.updateText(PersistentManager.Instance.Temperature, PersistentManager.Instance.tempLevel);

                if(PersistentManager.Instance.tempLevel == -2)
                {
                    freezeVines();
                    freezeMoss();
                    freezeWater();
                    freezeBushes();
                    BasMov.changePlayerStats();
                }
                else if(PersistentManager.Instance.tempLevel == -1)
                {
                    BasMov.changePlayerStats();
                }
                else if(PersistentManager.Instance.tempLevel == 1)
                {
                    extinguishBushes();
                    extinguishMoss();
                }
            }
        }
    }

    void burnBushes()
    {
        foreach (GameObject bush in bushes)
        {
            StartCoroutine(changeChildColor(bush.transform, new Color(1f, 0f, 0.2f, 0.8f), Time.time));
            bush.layer = LayerMask.NameToLayer("Deadly");
        } 
    }
    void extinguishBushes()
    {
        foreach (GameObject bush in bushes)
        {
            StartCoroutine(changeChildColor(bush.transform, Color.white, Time.time));
            bush.layer = LayerMask.NameToLayer("Grounding");
        }
    }
    void freezeBushes()
    {
        foreach (GameObject bush in bushes)
        {
            Debug.Log("freezing");
            StartCoroutine(changeChildColor(bush.transform, new Color(1f, 1f, 1f, 0.3f), Time.time));
            BoxCollider2D bc = bush.GetComponent<BoxCollider2D>();
            bc.isTrigger = true;
        }
    }
    IEnumerator thawBushes()
    {
        foreach (GameObject bush in bushes)
        {
            BoxCollider2D bc = bush.GetComponent<BoxCollider2D>();
            Debug.Log("thawing...");
            while (bc.IsTouchingLayers(playerLayer))
            {
                yield return new WaitForFixedUpdate();
                Debug.Log("..");
            }
            // yield return new WaitForSeconds(0.1);
            Debug.Log("done thawing");
            bc.isTrigger = false;
            StartCoroutine(changeChildColor(bush.transform, Color.white, Time.time));
        }
    }

    void burnMoss()
    {
        foreach (GameObject moss in mosses)
        {
            StartCoroutine(changeChildColor(moss.transform, new Color(1f, 0f, 0.3f, 0.8f), Time.time));
            changeChildLayer(moss.transform, "Deadly");
            changeChildColliderToTrigger(moss.transform, false);
        }
    }
    void extinguishMoss()
    {
        foreach (GameObject moss in mosses)
        {
            StartCoroutine(changeChildColor(moss.transform, Color.white, Time.time));
            changeChildLayer(moss.transform, "Grounding");
            changeChildColliderToTrigger(moss.transform, true);
        }
    }
    void freezeMoss()
    {
        foreach (GameObject moss in mosses)
        {
            changeChildColliderToTrigger(moss.transform, false);
        }
    }
    void thawMoss()
    {
        foreach (GameObject moss in mosses)
        {
            changeChildColliderToTrigger(moss.transform, true);
        }
    }
    void changeMossSize()
    {
        int hum = PersistentManager.Instance.humidityLevel;
        List<string> inactive = new List<string>();
        if(hum < 2) {
            inactive.Add("PlusTwo");
        }
        if(hum < 1) {
            inactive.Add("PlusOne");
        }
        if(hum < 0) {
            inactive.Add("Neutral");
        }
        if(hum < -1) {
            inactive.Add("MinusOne");
        }
        foreach (GameObject moss in mosses)
        {
            for (int j = 0; j < moss.transform.childCount; j++)
            {
                GameObject mossling = moss.transform.GetChild(j).gameObject;
                bool wantActive = !inactive.Contains(mossling.name);
                if (mossling.activeInHierarchy != wantActive)
                {
                    mossling.SetActive(wantActive);
                }
            }
        }
    }
    void freezeVines()
    {
        foreach (GameObject vine in vines)
        {
            changeChildLayer(vine.transform, "Grounding");
            changeChildColliderToTrigger(vine.transform, false);
        }
    }
    void thawVines()
    {
        foreach (GameObject vine in vines)
        {
            changeChildLayer(vine.transform, "Grippable");
            changeChildColliderToTrigger(vine.transform, true);
        }
    }
    void freezeWater()
    {
        foreach (BoxCollider2D box in waterColliders)
        {
            //stop lily pads somehow
            box.gameObject.layer = LayerMask.NameToLayer("Grounding");
            box.isTrigger = false;
        }
    }
    void thawWater()
    {
        foreach (BoxCollider2D box in waterColliders)
        {
            if (PersistentManager.Instance.toxicLevel >= 1)
            {
                box.gameObject.layer = LayerMask.NameToLayer("Deadly");
                box.isTrigger = false;
            }
            else
            {
                box.gameObject.layer = LayerMask.NameToLayer("Water");
                box.isTrigger = true;
            }
        }
    }

    void changeChildLayer(Transform trans, string change)
     {
        if (trans.childCount > 0 )
        {
            foreach(Transform child in trans)
            {            
                changeChildLayer(child, change);
            }
        }
        else {
            trans.gameObject.layer = LayerMask.NameToLayer(change);
        }
     }
     void changeChildColliderToTrigger(Transform trans, bool change)
     {
        if (trans.childCount > 0 )
        {
             foreach(Transform child in trans)
            {            
                changeChildColliderToTrigger(child, change);
            }
        }
        else {
            trans.gameObject.GetComponent<BoxCollider2D>().isTrigger = change;
        }
     }
     IEnumerator changeChildColor(Transform trans, Color clr, float tim)
     {
        if (trans.childCount > 0 )
        {
            foreach(Transform child in trans)
            {
                StartCoroutine(changeChildColor(child, clr, Time.time));
            }
        }
        else {
            while (true)
            {
                float percentageComplete = (Time.time - tim) / 1;

                trans.gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(
                    trans.gameObject.GetComponent<SpriteRenderer>().color, clr, percentageComplete);

                if (percentageComplete >= 1) break;

                yield return new WaitForFixedUpdate();
            }
        }
     }


    public void setToxicity(int toxicLevel)
    {
        PersistentManager.Instance.toxicLevel = toxicLevel;
        Debug.Log("toxic set to "+toxicLevel);
        if (PersistentManager.Instance.tempLevel > -2)
        {
            if (toxicLevel >= 1)
            {
                infectWater();
            }
            else
            {
                Debug.Log("cleaning");
                cleanWater();
            }
        }
    }

    void infectWater()
    {
        foreach (BoxCollider2D box in waterColliders)
        {
            box.gameObject.layer = LayerMask.NameToLayer("Deadly");
            box.isTrigger = false;
        }
    }
    void cleanWater()
    {
        foreach (BoxCollider2D box in waterColliders)
        {
            box.gameObject.layer = LayerMask.NameToLayer("Water");
            box.isTrigger = true;
        }
    }



    public T[] addElement<T>(T[] array, T element)
    {
        var stack = new Stack<T>(array);
        stack.Push(element);
        T[] arr = stack.ToArray();
        System.Array.Reverse(arr);
        return arr;
    }
    public T[] addFirstElement<T>(T[] array, T element)
    {
        System.Array.Reverse(array);
        var stack = new Stack<T>(array);
        stack.Push(element);
        T[] arr = stack.ToArray();
        //Debug.Log("added to bottom?");
        return arr;
    }

    public Vector2[] removeLastElement(Vector2[] array)
    {
        var stack = new Stack<Vector2>(array);
        stack.Pop();
        Vector2[] arr = stack.ToArray();
        System.Array.Reverse(arr);
        return arr;
    }
    public Vector2[] removeFirstElement(Vector2[] array)
    {
        System.Array.Reverse(array);
        var stack = new Stack<Vector2>(array);
        stack.Pop();
        Vector2[] arr = stack.ToArray();
        return arr;   
    }
}
