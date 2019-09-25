using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMove : MonoBehaviour
{
    private PlayerMovement m_playerMovement;
    public PlayerMovement PlayerMovement
    {
        get
        {
            if(m_playerMovement == null)
            {
                m_playerMovement = GetComponent<PlayerMovement>();
            }
            return m_playerMovement;
        }
    }


    private void Update()
    {
        Move();
    }

    public void Move()
    {
        float axisRawX = Input.GetAxisRaw("KeyHori1");
        float axisRawY = Input.GetAxisRaw("KeyVerti1");

        PlayerMovement.Move(new Vector3(axisRawX, 0.0f, axisRawY));
        PlayerMovement.ChangeDirection(axisRawX, axisRawY);
    }
}
