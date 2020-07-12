using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGrow : MonoBehaviour
{

    public float defaultGrowRate;
    private float growRate;
    public Vector3 growDirection = Vector3.up;

    public float turbulenceStrength = .5f;
    public float turbulenceWavelength = 1.5f;

    public float maxHeight;
    public float distancePerLine;
    public float currentHeight;

    private LineCollision lineCollision;
    private LineRenderer lineRenderer;


    // Start is called before the first frame update
    void Start()
    {
        lineCollision = GetComponent<LineCollision>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        growRate = PersistentManager.Instance.humidityLevel * defaultGrowRate;
        currentHeight = (lineRenderer.GetPosition(lineRenderer.positionCount - 1).y);

        if (growRate > 0)
        {
            if (currentHeight < maxHeight)
            {
                GrowLine();
            }
        }else if(growRate < 0)
        {
            if(currentHeight > 0.1)
            {
                ShrinkLine();
            }
        }
    }

    void GrowLine()
    {
        //get line position 
        Vector3 currentPosition = lineRenderer.GetPosition(lineRenderer.positionCount - 1);

        //sine wave movement
        Vector3 naturalFlow = (Vector3.right * Mathf.Clamp(Mathf.Sin(currentPosition.y * turbulenceWavelength), -turbulenceStrength, turbulenceStrength)) * Time.deltaTime * growRate;

        //grow direction influence
        Vector3 newPosition = currentPosition + (growRate * growDirection * Time.deltaTime) + naturalFlow;

        //set new line position 
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPosition);

        float distance = Vector3.Distance(lineRenderer.GetPosition(lineRenderer.positionCount - 2), lineRenderer.GetPosition(lineRenderer.positionCount - 1));

        //add a new point
        if(distance > distancePerLine)
        {
            Vector3[] currentPositions = new Vector3[lineRenderer.positionCount];

            Vector3[] newPositions = new Vector3[lineRenderer.positionCount + 1];

            lineRenderer.GetPositions(currentPositions);

            for(int i=0; i< currentPositions.Length; i++)
            {
                newPositions[i] = currentPositions[i];
            }

            newPositions[newPositions.Length -1] = currentPositions[currentPositions.Length -1];

            //Debug.Log(newPositions.Length);

            lineRenderer.SetVertexCount(newPositions.Length);

            lineRenderer.SetPositions(newPositions);

            //update colliders 
            lineCollision.SyncCollision();
        }
    }

    void ShrinkLine()
    {

        //only do this if we have more than one point
        if (lineRenderer.positionCount > 1)
        {
            //get line position 
            Vector3 currentPosition = lineRenderer.GetPosition(lineRenderer.positionCount - 1);

            Vector3 previousPosition = lineRenderer.GetPosition(lineRenderer.positionCount - 2);

            Vector3 reverseDirection = (currentPosition - previousPosition).normalized;

            //grow direction influence
            Vector3 newPosition = currentPosition + (growRate * reverseDirection * Time.deltaTime);

            //set new line position 
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPosition);


            float distance = Vector3.Distance(lineRenderer.GetPosition(lineRenderer.positionCount - 2), lineRenderer.GetPosition(lineRenderer.positionCount - 1));

            //remove point 
            if (distance < distancePerLine / 10)
            {
                Vector3[] currentPositions = new Vector3[lineRenderer.positionCount];

                Vector3[] newPositions = new Vector3[lineRenderer.positionCount - 1];

                lineRenderer.GetPositions(currentPositions);

                for (int i = 0; i < newPositions.Length; i++)
                {
                    newPositions[i] = currentPositions[i];
                }

                lineRenderer.SetVertexCount(newPositions.Length);

                lineRenderer.SetPositions(newPositions);

                //update colliders 
                lineCollision.SyncCollision();
            }

        }

    }


}
