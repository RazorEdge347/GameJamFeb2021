using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;
using System.Linq;
using System;

public class PlayerScript : MonoBehaviour
{
    public ColorType currentColor;

    // Start is called before the first frame update
    public float moveSpeed;
    
    [Range(0, 1)]
    public float clampBoundariesMax;

    public float maxHP;
    public float currentHP;

    public float currentShield;


    void Start()
    {

    }

    void ColorInput()
    {
        if (Input.GetButtonDown("Color"))
        {
            int index = (int)currentColor + 1;
            if (index >= Enum.GetNames(typeof(ColorType)).Length)
                index = 0;
            ChangeColor((ColorType)index);
        }

        if (Input.GetButtonDown("Blue"))
        {
            ChangeColor(ColorType.BLUE);
        }

        if (Input.GetButtonDown("Magenta"))
        {
            ChangeColor(ColorType.MAGENTA);
        }

        if (Input.GetButtonDown("Yellow"))
        {
            ChangeColor(ColorType.YELLOW);
        }
    }

    void ChangeColor(ColorType newColor)
    {
        if (newColor == currentColor)
            return;

        currentColor = newColor;
        Debug.Log(currentColor);
    }

    void MovementInput()
    {
        // We take of the clamp later
        transform.Translate(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime,
        Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0);
        // Boundaries
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x, 1f - clampBoundariesMax, clampBoundariesMax);
        pos.y = Mathf.Clamp(pos.y, 1f - clampBoundariesMax, clampBoundariesMax);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    // Update is called once per frame
    void Update()
    {
        MovementInput();
        ColorInput();
    }
}
