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

    public Transform aimTarget;

    public int leanLimit = 20;
    public int leanSpeed = 40;
    public float lookSpeed = 340f;

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
        pos.x = Mathf.Clamp(pos.x, 1 - clampBoundariesMax , clampBoundariesMax);
        pos.y = Mathf.Clamp(pos.y, 1 - clampBoundariesMax, clampBoundariesMax);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
        
        //Aim Position
        aimTarget.parent.position = Vector3.zero;
        aimTarget.localPosition = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 1);
        transform.GetChild(0).rotation = Quaternion.RotateTowards(transform.GetChild(0).rotation, Quaternion.LookRotation(aimTarget.position), Mathf.Deg2Rad * lookSpeed * Time.deltaTime * 10);

        // Lean Horizontal
        Vector3 targetEulerAngels = transform.GetChild(0).localEulerAngles;
        transform.GetChild(0).localEulerAngles = new Vector3(targetEulerAngels.x, targetEulerAngels.y, Mathf.LerpAngle(targetEulerAngels.z, -Input.GetAxis("Horizontal") * leanLimit , leanSpeed));

    }

    // Update is called once per frame
    void Update()
    {
        MovementInput();
        ColorInput();
    }
}
