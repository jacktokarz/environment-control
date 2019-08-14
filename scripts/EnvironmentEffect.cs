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
                    shrinkVine(pc, topVinePiece);
                }
            }
        }
    }

    void growVine(GameObject parentVine, PolygonCollider2D pc, Transform topVinePiece) {
        Vector2[] vecArray = pc.GetPath(0);
        Vector2 newPos= new Vector2();
        if (parentVine.transform.rotation.eulerAngles.z == 90) {
            newPos= new Vector2(topVinePiece.position.x - PersistentManager.Instance.vinePieceHeight, topVinePiece.position.y);
        }
        else {
            newPos= new Vector2(topVinePiece.position.x, topVinePiece.position.y + PersistentManager.Instance.vinePieceHeight);
        }
        Collider2D overlapper= Physics2D.OverlapBox(newPos, vineSize, parentVine.transform.rotation.eulerAngles.z, PersistentManager.Instance.whatBlocksVines);
        if(overlapper)
        {
            Debug.Log("overlapping with "+overlapper);
            return;
        }
        Instantiate(topVinePiece, newPos, parentVine.transform.rotation, parentVine.transform);
        vecArray= addElement(vecArray, new Vector2(PersistentManager.Instance.vinePieceWidth / 2, topVinePiece.localPosition.y + PersistentManager.Instance.vinePieceHeight * 3/2));
        vecArray= addElement(vecArray, new Vector2(-PersistentManager.Instance.vinePieceWidth / 2, topVinePiece.localPosition.y + PersistentManager.Instance.vinePieceHeight * 3/2));
        pc.SetPath(0, vecArray);
    }
    void shrinkVine(PolygonCollider2D pc, Transform topVinePiece) {
        Destroy(topVinePiece.gameObject);
        Vector2[] vecArray = pc.GetPath(0);
        Vector2[] vecArrayToo = removeLastElement(vecArray);
        Vector2[] vecArrayFinal = removeLastElement(vecArrayToo);
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
            Debug.Log("NEW end points: " + waterEndPoints[i].ToString());
        }
        waterStartTime= Time.time;
    }



    public T[] addElement<T>(T[] array, T element)
    {
        var stack = new Stack<T>(array);
        stack.Push(element);
        T[] arr = stack.ToArray();
        System.Array.Reverse(arr);
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
}
