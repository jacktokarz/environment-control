using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGrow : MonoBehaviour
{

    public float defaultGrowRate;
    private float growRate;
    public Vector2 growDirection = Vector2.up;

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
        Vector2 currentPosition = lineRenderer.GetPosition(lineRenderer.positionCount - 1);

        //sine wave movement
        Vector2 naturalFlow = (Vector2.right *
            Mathf.Clamp(
                Mathf.Sin(currentPosition.y * turbulenceWavelength),
                -turbulenceStrength, turbulenceStrength
            )) * Time.deltaTime * growRate;

        //grow direction influence
        Vector2 newPosition = currentPosition + (growRate * growDirection * Time.deltaTime) + naturalFlow;

        //colliders: wind and more
        Vector2 windFlow = new Vector2(0,0);
        Vector2 worldPosition = new Vector2(currentPosition.x * (this.transform.rotation.y==0 ? 1 : -1), currentPosition.y)
            + new Vector2(this.transform.position.x, this.transform.position.y);
        bool blocked = false;
        Collider2D[] overlapper= Physics2D.OverlapBoxAll(
            worldPosition,
            new Vector2(1, 0.09f),
            this.transform.rotation.eulerAngles.z,
            PersistentManager.Instance.whatBlocksVines
        );
        Debug.Log("world pos of "+this.name+" is "+worldPosition);
        if(overlapper.Length > 0)
        {
            foreach (Collider2D overlap in overlapper)
            {
                if (overlap.CompareTag("wind"))
                {
                    WindDirection wd = overlap.gameObject.GetComponent(typeof(WindDirection)) as WindDirection;
                    if(wd.direction != new Vector2(0,0))
                    {
                        newPosition = newPosition +
                            (PersistentManager.Instance.windLevel *
                                PersistentManager.Instance.vineWindAffect *
                                wd.direction *
                                (this.transform.rotation.y==0 ? 1 : -1)
                            );
                    }
                }
                else {
                    if(!overlap.CompareTag("vine") && (!overlap.CompareTag("vinePiece"))) {
                        Debug.Log("running into "+overlap.name+" at "+worldPosition);
                        blocked = true;
                        break;
                    }
                }
            }
        }

        //set new line position
        if (!blocked) {
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPosition);
        }

        float distance = Vector2.Distance(
            lineRenderer.GetPosition(lineRenderer.positionCount - 2),
            lineRenderer.GetPosition(lineRenderer.positionCount - 1)
        );

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


            float distance = Vector3.Distance(
                lineRenderer.GetPosition(lineRenderer.positionCount - 2),
                lineRenderer.GetPosition(lineRenderer.positionCount - 1)
            );

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
        else {
            currentHeight= 0;
        }

    }


}
