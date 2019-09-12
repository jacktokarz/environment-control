using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentEffect : MonoBehaviour
{
    float vineGrowWait;
    float vineCounter= -1f;

    float waterStartTime;
    
    GameObject[] vines;
    GameObject[] waterLines;
    Vector3[] waterEndPoints;
    Vector3[] waterStartPoints;
    Vector2 vineSize;

    bool firstWind = false;
    bool windy = false;

    void Start()
    {
        vineSize = new Vector2(PersistentManager.Instance.vinePieceWidth, PersistentManager.Instance.vinePieceHeight);

        vines = GameObject.FindGameObjectsWithTag("vine");
        waterLines = GameObject.FindGameObjectsWithTag("waterLine");
        if (waterLines.Length > 0)
        {
            waterEndPoints = new Vector3[waterLines.Length];
            waterStartPoints = new Vector3[waterLines.Length];
            for (int i = 0; i < waterLines.Length; i++) {
                waterEndPoints[i]= waterLines[i].transform.position;
                waterStartPoints[i]= waterLines[i].transform.position;
            }
            if (PersistentManager.Instance.humidityLevel != 0)
            {
                changeWaterLevel(PersistentManager.Instance.humidityLevel);
            }
        }
        updateVineGrowWait();
    }

    void FixedUpdate()
    {
        if (PersistentManager.Instance.humidityLevel != 0)
        {
            vineCounter++;
            if (vineCounter >= vineGrowWait)
            {
                checkVines();
                vineCounter = 0f;
            }
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

    void checkVines() {
        foreach (GameObject vin in vines)
        {
            int childCount = vin.transform.childCount;
            Transform topVinePiece = vin.transform.GetChild(childCount - 1);
            PolygonCollider2D pc = vin.GetComponent(typeof(PolygonCollider2D)) as PolygonCollider2D;
            if (PersistentManager.Instance.humidityLevel > 0)
            {
                if (childCount < PersistentManager.Instance.vineMaxHeight)
                {
                    growVine(vin, pc, topVinePiece);
                }
            }
            else
            {
                if (childCount > PersistentManager.Instance.vineMinHeight)
                {
                    shrinkVine(vin, pc, topVinePiece);
                }
            }
        }
    }

    void growVine(GameObject parentVine, PolygonCollider2D pc, Transform topVinePiece) {
        Vector2[] vecArray = pc.GetPath(0);
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
        Collider2D overlapper= Physics2D.OverlapBox(newPos, vineSize, parentVine.transform.rotation.eulerAngles.z, PersistentManager.Instance.whatBlocksVines);
        if(overlapper)
        {
            if (overlapper.CompareTag("wind"))
            {
                WindDirection wd = overlapper.gameObject.GetComponent(typeof(WindDirection)) as WindDirection;
                if(wd.direction != new Vector2(0,0))
                {
                    newPos = newPos + (PersistentManager.Instance.windLevel * PersistentManager.Instance.vineWindAffect * wd.direction);
                }
                if (firstWind == true)
                {
                    firstWind = false;
                }
                else
                {
                    vecArray = addFirstElement(vecArray, vecArray[vecArray.Length - 1]);
                    VineActivity va = parentVine.GetComponent<VineActivity>();
                    va.turningPoints.Push(vecArray.Length);
                    firstWind = true;
                }
                windy = true;
            }
            else
            {
                vecArray = windOff(vecArray);
                return;
            }
        }
        else
        {
            vecArray = windOff(vecArray);
        }
        Transform newPiece = Instantiate(topVinePiece, newPos, parentVine.transform.rotation, parentVine.transform);
        vecArray= addElement(vecArray, new Vector2(newPiece.localPosition.x + (PersistentManager.Instance.vinePieceWidth / 2), newPiece.localPosition.y + ( PersistentManager.Instance.vinePieceHeight/2)));
        vecArray= addElement(vecArray, new Vector2(newPiece.localPosition.x - (PersistentManager.Instance.vinePieceWidth / 2), newPiece.localPosition.y + (PersistentManager.Instance.vinePieceHeight/2)));
        pc.SetPath(0, vecArray);
    }
    void shrinkVine(GameObject parentVine, PolygonCollider2D pc, Transform topVinePiece) {
        Destroy(topVinePiece.gameObject);

        VineActivity va = parentVine.GetComponent<VineActivity>();
        Vector2[] delArray = pc.GetPath(0);
        Vector2[] vecArrayToo = removeLastElement(delArray);
        Vector2[] vecArrayFinal = removeLastElement(vecArrayToo);
        if((va.turningPoints.Count > 0) && (vecArrayFinal.Length <= va.turningPoints.Peek()))
        {
            Vector2[] actualFinal = removeFirstElement(vecArrayFinal);
            va.turningPoints.Pop();
            pc.SetPath(0, actualFinal);
            return;
        }
        pc.SetPath(0, vecArrayFinal);
    }

    public void humidityChange(int value)
    {
        updateVineGrowWait();
        changeWaterLevel(value);
    }

    void updateVineGrowWait()
    {
        vineGrowWait= PersistentManager.Instance.humidityLevel == 0 ? 0f : Mathf.Abs(PersistentManager.Instance.vineGrowDefaultSpeed / (PersistentManager.Instance.humidityLevel));
    }

    void changeWaterLevel(int value)
    {
        for (int i = 0; i < waterLines.Length; i++)
        {
            waterStartPoints[i] = waterLines[i].transform.position;
            waterEndPoints[i].y += value * PersistentManager.Instance.waterChangeDistance;
            //Debug.Log("NEW end points: " + waterEndPoints[i].ToString());
        }
        waterStartTime= Time.time;
    }

    Vector2[] windOff(Vector2[] vecArray)
    {
        if (windy == true)
        {
            vecArray = addFirstElement(vecArray, vecArray[vecArray.Length - 1]);
            windy = false;
        }
        return vecArray;
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
