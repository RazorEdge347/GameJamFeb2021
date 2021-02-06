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
    public float clampBoundariesMin = 0.1f;

    public Transform aimTarget;

    public int leanLimit = 20;
    public int leanSpeed = 40;
    public float lookSpeed = 340f;

    [HideInInspector] public float maxHP;
    public float hp = 100.0f;

    public bool hasShield = false;
    public bool isDead = false;

    [SerializeField] private float collInvunerability = 5.0f;
    private float currentInvunerability = 0.0f;

    [SerializeField] private float collTwinkle = 0.1f;
    private float currentcolTwinkle = 0.0f;

    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private GameObject shieldVFXobj;

    void Start()
    {
        maxHP = hp;
        SetShieldActive(false);
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
        pos.x = Mathf.Clamp(pos.x, clampBoundariesMin , 1 - clampBoundariesMin);
        pos.y = Mathf.Clamp(pos.y, clampBoundariesMin, 1 - clampBoundariesMin);
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
        CollisionInvunerability();
    }

    public void ChangeHP(float amount)
    {
        if (amount < 0)
        {
            if (hasShield)
            {
                SetShieldActive(false);
                return;
            }
            else
            {
                currentInvunerability = collInvunerability;
                currentcolTwinkle = collTwinkle;
            }
        }

        hp += amount;
        hp = Mathf.Clamp(hp, 0, maxHP);

        if (hp <= 0)
        {
            isDead = true;
        }
    }

    public void SetShieldActive(bool active)
    {
        hasShield = active;
        shieldVFXobj.SetActive(active);
    }

    void CollisionInvunerability()
    {
        if (currentInvunerability > 0)
        {
            currentInvunerability -= Time.deltaTime;
            boxCollider.enabled = false;

            if (currentcolTwinkle > 0)
            {
                currentcolTwinkle -= Time.deltaTime;
            }
            else
            {
                meshRenderer.enabled = !meshRenderer.enabled;
                currentcolTwinkle = collTwinkle;
            }

        }
        else
        {
            if (!boxCollider.enabled)
            {
                boxCollider.enabled = true;
                meshRenderer.enabled = true;
            }
        }
    }
}