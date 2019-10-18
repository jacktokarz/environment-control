using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentEffect : MonoBehaviour
{
    public static EnvironmentEffect Instance { get; private set; }

    BasicMovement BasMov;
    float vineGrowWait;
    float vineCounter= -1f;

    float waterStartTime;
    
    GameObject[] vines;
    GameObject[] waterLines;
    GameObject[] bushes;
    GameObject[] winds;
    GameObject[] fronds;
    GameObject[] mosses;
    Vector3[] waterEndPoints;
    Vector3[] waterStartPoints;
    Vector3[] waterOriginalPoints;
    Vector2 vineSize;

    public LayerMask playerLayer;

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
        BasMov = GameObject.FindGameObjectWithTag("Player").GetComponent<BasicMovement>();
        playerLayer = LayerMask.GetMask("Player");
        vines = GameObject.FindGameObjectsWithTag("vine");
        vineSize = new Vector2(PersistentManager.Instance.vinePieceWidth, PersistentManager.Instance.vinePieceHeight);
        bushes = GameObject.FindGameObjectsWithTag("bush");
        winds = GameObject.FindGameObjectsWithTag("wind");
        fronds = GameObject.FindGameObjectsWithTag("frond");
        mosses = GameObject.FindGameObjectsWithTag("moss");
        waterLines = GameObject.FindGameObjectsWithTag("waterLine");
        if (waterLines.Length > 0)
        {
            Debug.Log("water lines length: "+waterLines.Length);
            waterEndPoints = new Vector3[waterLines.Length];
            waterStartPoints = new Vector3[waterLines.Length];
            waterOriginalPoints = new Vector3[waterLines.Length];
            for (int i = 0; i < waterLines.Length; i++) {
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
        updateVineGrowWait();
        changeWindAnimation();
    }

    private void FixedUpdate()
    {
        vineCounter++;
        if (vineCounter >= vineGrowWait)
        {
            checkVines();
            vineCounter = 0f;
        }
        for (int i= 0; i< waterLines.Length; i++)
        {
            GameObject wat= waterLines[i];
            if (wat.transform.position != waterEndPoints[i])
            {
                float journeyLength = Vector3.Distance(waterStartPoints[i], waterEndPoints[i]);
                float distCovered = (Time.time - waterStartTime) * PersistentManager.Instance.waterChangeSpeed;
                wat.transform.position = Vector3.Lerp(waterStartPoints[i], waterEndPoints[i], distCovered / journeyLength);
            }
        }
    }


    public bool setHumidity(int value)
    {
        Debug.Log("trying to set humidity to "+value);
        if ((value>=PersistentManager.Instance.minHumidity) && (value<=PersistentManager.Instance.maxHumidity))
        {
            PersistentManager.Instance.humidityLevel = value;
            updateVineGrowWait();
            changeWaterLevel(value);
            changeFrondWidth();
            PersistentManager.Instance.updateText(PersistentManager.Instance.Humidity, value);
            return true;
        }
        return false;
    }


    void checkVines() {
        foreach (GameObject vin in vines)
        {
            int childCount = vin.transform.childCount;
            Transform topVinePiece = vin.transform.GetChild(childCount - 1);
            if (PersistentManager.Instance.humidityLevel > 0)
            {
                if (childCount < PersistentManager.Instance.vineMaxHeight && PersistentManager.Instance.tempLevel < 2)
                {
                    growVine(vin, topVinePiece);
                }
            }
            if (PersistentManager.Instance.humidityLevel < 0 || PersistentManager.Instance.tempLevel == 2)
            {
                if (childCount > PersistentManager.Instance.vineMinHeight)
                {
                    shrinkVine(topVinePiece);
                }
            }
        }
    }

    void growVine(GameObject parentVine, Transform topVinePiece) {
        Vector2 newPos = new Vector2();
        float zRot = parentVine.transform.rotation.eulerAngles.z;
        if (zRot == 90) {
            newPos= new Vector2(topVinePiece.position.x - PersistentManager.Instance.vinePieceHeight, topVinePiece.position.y);
        }
        else if (zRot == 270) {
            newPos= new Vector2(topVinePiece.position.x + PersistentManager.Instance.vinePieceHeight, topVinePiece.position.y);
        }
        else if (zRot == 180) {
            newPos= new Vector2(topVinePiece.position.x, topVinePiece.position.y - PersistentManager.Instance.vinePieceHeight);
        }
        else {
            newPos= new Vector2(topVinePiece.position.x, topVinePiece.position.y + PersistentManager.Instance.vinePieceHeight);
        }
        Collider2D[] overlapper= Physics2D.OverlapBoxAll(newPos, vineSize, parentVine.transform.rotation.eulerAngles.z, PersistentManager.Instance.whatBlocksVines);
        if(overlapper.Length > 0)
        {
        	foreach (Collider2D overlap in overlapper)
        	{
	            if (overlap.CompareTag("wind"))
	            {
	                WindDirection wd = overlap.gameObject.GetComponent(typeof(WindDirection)) as WindDirection;
	                if(wd.direction != new Vector2(0,0))
	                {
	                    newPos = newPos + (PersistentManager.Instance.windLevel * PersistentManager.Instance.vineWindAffect * wd.direction);
	                }
	            }
        	}
        }
        Transform newPiece = Instantiate(topVinePiece, newPos, parentVine.transform.rotation, parentVine.transform);
    }
    void shrinkVine(Transform topVinePiece) {
        Destroy(topVinePiece.gameObject);
    }

    void updateVineGrowWait()
    {
        int hum = PersistentManager.Instance.humidityLevel;
        int dividend = ((hum == 0 || (PersistentManager.Instance.tempLevel == 2 && hum > -2)) ? 1 : Mathf.Abs(hum));
        vineGrowWait= PersistentManager.Instance.vineGrowDefaultSpeed / dividend;
    }

    void changeWaterLevel(int value)
    {
        for (int i = 0; i < waterLines.Length; i++)
        {
            waterStartPoints[i] = waterLines[i].transform.position;
            waterEndPoints[i].y = value * PersistentManager.Instance.waterChangeDistance + waterOriginalPoints[i].y;
            //Debug.Log("NEW end points: " + waterEndPoints[i].ToString());
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
        foreach (GameObject win in winds)
        {
            Animator winAm = win.GetComponent<Animator>();
            winAm.SetFloat("windSpeed", PersistentManager.Instance.windLevel);
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

                if(value >= 0)
                {
                    thawBushes();
                    BasMov.changePlayerStats();
                }
                else if(value == -1)
                {
                    //thawBushes();
                    BasMov.changePlayerStats();
                }

                if(value == 2)
                {
                    updateVineGrowWait();
                    //burnBushes();
                }
                if (value > -2)
                {
                    thawWater();
                    thawVines();
                    thawMoss();
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
                    BasMov.changePlayerStats();
                }
                else if(PersistentManager.Instance.tempLevel == -1)
                {
                    BasMov.changePlayerStats();
                }
                else if(PersistentManager.Instance.tempLevel == 1)
                {
                    updateVineGrowWait();
                    //extinguishBushes();
                }

                if(PersistentManager.Instance.tempLevel < 0)
                {
                    freezeBushes();
                }
            }
        }
    }

    void burnBushes()
    {
        foreach (GameObject bush in bushes)
        {
            //SpriteRenderer sr = bush.GetComponent<SpriteRenderer>();
            //sr.color = new Color(100f, 1f, 1f, 1f);
            bush.layer = LayerMask.NameToLayer("Deadly");
        } 
    }
    void freezeBushes()
    {
        foreach (GameObject bush in bushes)
        {
            SpriteRenderer sr = bush.GetComponent<SpriteRenderer>();
            sr.color = new Color(1f, 1f, 1f, 0.4f);
            BoxCollider2D bc = bush.GetComponent<BoxCollider2D>();
            bc.isTrigger = true;
        }
    }
    void thawBushes()
    {
        foreach (GameObject bush in bushes)
        {
            BoxCollider2D bc = bush.GetComponent<BoxCollider2D>();
            if(!bc.IsTouchingLayers(playerLayer))
            {
                bc.isTrigger = false;
                SpriteRenderer sr = bush.GetComponent<SpriteRenderer>();
                sr.color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }
    void extinguishBushes()
    {
        foreach (GameObject bush in bushes)
        {
            bush.layer = LayerMask.NameToLayer("Grounding");
        }
    }

    void freezeMoss()
    {
        foreach (GameObject moss in mosses)
        {
            changeChildCollider(moss.transform, false);
        }
    }
    void thawMoss()
    {
        foreach (GameObject moss in mosses)
        {
            changeChildCollider(moss.transform, true);
        }
    }
    void freezeVines()
    {
        foreach (GameObject vine in vines)
        {
            changeChildLayer(vine.transform, "Grounding");
            changeChildCollider(vine.transform, false);
        }
    }
    void thawVines()
    {
        foreach (GameObject vine in vines)
        {
            changeChildLayer(vine.transform, "Grippable");
            changeChildCollider(vine.transform, true);
        }
    }
    void freezeWater()
    {
        foreach (GameObject line in waterLines)
        {
            //stop lily pads somehow
            changeChildLayer(line.transform, "Grounding");
        }
    }
    void thawWater()
    {
        foreach (GameObject line in waterLines)
        {
            if (PersistentManager.Instance.toxicLevel >= 2)
            {
                changeChildLayer(line.transform, "Deadly");
            }
            else
            {
                changeChildLayer(line.transform, "Ignore-Player");
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
            Debug.Log("changing "+change);
             trans.gameObject.layer = LayerMask.NameToLayer(change);
         }
     }
     void changeChildCollider(Transform trans, bool change)
     {
        if (trans.childCount > 0 )
         {
             foreach(Transform child in trans)
             {            
                 changeChildCollider(child, change);
             }
         }
         else {
            Debug.Log("changing "+change);
             trans.gameObject.GetComponent<BoxCollider2D>().isTrigger = change;
         }
     }


    public void setToxicity(int toxicLevel)
    {
        PersistentManager.Instance.toxicLevel = toxicLevel;
        Debug.Log("toxic set to "+toxicLevel);
        if (PersistentManager.Instance.tempLevel > -2)
        {
            if (toxicLevel >= 2)
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
        foreach (GameObject line in waterLines)
        {
            changeChildLayer(line.transform, "Deadly");
        }
    }
    void cleanWater()
    {
        foreach (GameObject line in waterLines)
        {
            changeChildLayer(line.transform, "Ignore-Player");
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
