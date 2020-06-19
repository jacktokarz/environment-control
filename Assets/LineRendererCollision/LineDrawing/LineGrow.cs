using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGrow : MonoBehaviour
{

    public float growRate = 1;
    public Vector3 growDirection = Vector3.up;

    public float turbulenceStrength = .5f;
    public float turbulenceWavelength = 1.5f;


    public float MaxHeight;

    public float distancePerLine;

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
        if(growRate > 0)
        {

            float currentHeight = (lineRenderer.GetPosition(lineRenderer.positionCount - 1).y);

            if (currentHeight < MaxHeight)
            {
                GrowLine();
            }
        }

    }


    void GrowLine()
    {

        Vector3 currentPosition = lineRenderer.GetPosition(lineRenderer.positionCount - 1);

        Vector3 naturalFlow = (Vector3.right * Mathf.Clamp(Mathf.Sin(currentPosition.y * turbulenceWavelength), -turbulenceStrength, turbulenceStrength)) * Time.deltaTime * growRate;

        Vector3 newPosition = currentPosition + (growRate * growDirection * Time.deltaTime) + naturalFlow;

        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPosition);

        float distance = Vector3.Distance(lineRenderer.GetPosition(lineRenderer.positionCount - 2), lineRenderer.GetPosition(lineRenderer.positionCount - 1));


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
}
