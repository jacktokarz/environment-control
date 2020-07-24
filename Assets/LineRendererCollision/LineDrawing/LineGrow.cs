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
        bool sideways = this.transform.parent.rotation.eulerAngles.z==90;
        bool inverseSideways = this.transform.parent.rotation.eulerAngles.z == 270;
        bool upsideDown = this.transform.parent.rotation.eulerAngles.z == 180;
        bool flipped = this.transform.rotation.eulerAngles.y==180;
        int xSkew = sideways || (flipped&&!inverseSideways&&!upsideDown) || (!flipped&&upsideDown) ? -1 : 1;
        int ySkew = (sideways && flipped) || (inverseSideways && !flipped) || upsideDown ? -1 : 1;
        Vector2 worldPosition = new Vector2(
            (sideways||inverseSideways ? currentPosition.y : currentPosition.x) * xSkew,
            (sideways||inverseSideways ? currentPosition.x : currentPosition.y) * ySkew
        ) + new Vector2(this.transform.position.x, this.transform.position.y);
        bool blocked = false;
        Collider2D[] overlapper= Physics2D.OverlapBoxAll(
            worldPosition,
            new Vector2(1, 0.09f),
            this.transform.rotation.eulerAngles.z,
            PersistentManager.Instance.whatBlocksVines
        );
        // Debug.Log("world pos of "+this.name+" is "+worldPosition+" with x "+xSkew+" y "+ySkew);
        // Debug.Log(" overlapping "+overlapper.Length);
        if(overlapper.Length > 0)
        {
            foreach (Collider2D overlap in overlapper)
            {
                if (overlap.CompareTag("wind"))
                {
                    WindDirection wd = overlap.gameObject.GetComponent(typeof(WindDirection)) as WindDirection;
                    if(wd.direction != new Vector2(0,0))
                    {
                        Vector2 actualWDDir = sideways || inverseSideways ?
                            new Vector2(wd.direction.y, wd.direction.x) : wd.direction;
                        newPosition = newPosition +
                            (actualWDDir *
                                PersistentManager.Instance.windLevel *
                                PersistentManager.Instance.vineWindAffect *
                                (((!inverseSideways&&!upsideDown)&&flipped) || ((inverseSideways||upsideDown)&&!flipped) ? -1 : 1)
                            );
                    }
                }
                else {
                    if(!overlap.CompareTag("vine") && (!overlap.CompareTag("vinePiece"))) {
                        // Debug.Log("running into "+overlap.name+" at "+worldPosition);
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
