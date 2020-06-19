using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class LineDraw : MonoBehaviour {


	public LineRenderer lineRenderer;

	private int curveCount = 1;	
	private int layerOrder = 0;
	private int SEGMENT_COUNT = 50;


	public Vector3 startPosition {get; set;}
	public Vector3 midPosition {get; set;}
	public Vector3 endPosition {get; set;}

	private bool LineDrawn { get; set;}

	// Use this for initialization
	void Start () {
		if (!lineRenderer)
		{
			lineRenderer = GetComponent<LineRenderer>();
		}
	}

	public void ToggleLine(bool isOn)
	{
		lineRenderer.enabled = isOn;

		LineDrawn = isOn;

	}

	public void SetDrawValues(Vector3 start, Vector3 mid, Vector3 end)
	{
		startPosition = start;
		midPosition = mid;
		endPosition = end;
	}


	IEnumerator AnimateLine()
	{
		for (int j = 0; j <curveCount; j++)
		{
			for (int i = 1; i < SEGMENT_COUNT; i++)
			{
				float t = i / (float)SEGMENT_COUNT;
				int nodeIndex = j * 3;

				Vector3 pixel = CalculateQuadBezierPoint(t, startPosition, midPosition, endPosition);
				lineRenderer.SetVertexCount(((j * SEGMENT_COUNT) + i));
				lineRenderer.SetPosition((j * SEGMENT_COUNT) + (i - 1), pixel);

				yield return null;
			}
		}

		LineDrawn = true;
	}

		
	public void DrawCurve()
	{
		if (LineDrawn == true) {

			for (int j = 0; j < curveCount; j++) {
				for (int i = 1; i < SEGMENT_COUNT; i++) {
					float t = i / (float)SEGMENT_COUNT;
					int nodeIndex = j * 3;

					Vector3 pixel = CalculateQuadBezierPoint (t, startPosition, midPosition, endPosition);
					lineRenderer.SetVertexCount (((j * SEGMENT_COUNT) + i));
					lineRenderer.SetPosition ((j * SEGMENT_COUNT) + (i - 1), pixel);
				}
			}
		}
	}


	public Vector3 CalculateQuadBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
	{
		float u = 1 - t;
		float tt = t * t;
		float uu = u * u;

		Vector3 p = uu * p0; 
		p += 3 * u * t * p1; 
		p += tt * p2; 

		return p;
	}
}
