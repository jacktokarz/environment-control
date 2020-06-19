using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class LineCollision : MonoBehaviour
{
    private EdgeCollider2D edgeCollider;
    private LineRenderer lineRenderer;

    public bool Generate;



    // Start is called before the first frame update
    void Start()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();

    }


    private void OnDrawGizmosSelected()
    {

        if (Generate == true)
        {
            if (edgeCollider == null)
            {
                edgeCollider = GetComponent<EdgeCollider2D>();
                lineRenderer = GetComponent<LineRenderer>();
            }


            SyncCollision();

           // Generate = false;
        }

    }


    public void SyncCollision()
    {
        Vector3[] lineRenderPoints = new Vector3[lineRenderer.positionCount];
        Vector2[] edgePoints = new Vector2[lineRenderer.positionCount];

        lineRenderer.GetPositions(lineRenderPoints);
        int i = 0;


        foreach (Vector3 position in lineRenderPoints)
        {
            edgePoints[i] = new Vector2(position.x, position.y);
           i+= 1;
        }

       // edgeCollider.points = new Vector2[lineRenderer.positionCount];
        edgeCollider.points = edgePoints;
    }


}
