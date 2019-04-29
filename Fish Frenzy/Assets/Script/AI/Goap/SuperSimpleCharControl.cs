using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSimpleCharControl : MonoBehaviour
{
    public float speed;
    private Dictionary<KeyCode, Vector3> moveDirections = new Dictionary<KeyCode, Vector3>();

    void Start()
    {
        moveDirections.Add(KeyCode.W, Vector3.up);
        moveDirections.Add(KeyCode.A, Vector3.left);
        moveDirections.Add(KeyCode.S, Vector3.down);
        moveDirections.Add(KeyCode.D, Vector3.right);
        moveDirections.Add(KeyCode.E, Vector3.forward);
        moveDirections.Add(KeyCode.Q, Vector3.back);

    }

    void Update()
    {
        foreach(KeyCode inputKey in moveDirections.Keys)
        {
            if (Input.GetKey(inputKey))
            {
                transform.Translate(moveDirections[inputKey] * Time.deltaTime * speed);
            }

        }
        
    }
}
