using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineActivity : MonoBehaviour
{

	public Stack<int> turningPoints;
    void Start()
    {
        turningPoints = new Stack<int>();
    }
}
