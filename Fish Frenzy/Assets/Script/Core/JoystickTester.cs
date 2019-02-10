using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickTester : MonoBehaviour
{
    public int playerId;
    public Color defaultColor = Color.white;
    public Color testColor = Color.black;
    private bool testing = false;
    private SpriteRenderer spriterenderer;

    void Start()
    {
        spriterenderer = GetComponent<SpriteRenderer>();
        spriterenderer.color = defaultColor;
    }

    void Update()
    {
        if (JoystickManager.Instance.GetButtonDown("Jump", playerId))
        {
            testing = !testing;
            if (testing)
            {
                spriterenderer.color = testColor;
            }
            else
            {
                spriterenderer.color = defaultColor;
            }
        }
    }

}
