using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;

public class ObstaculeOfColor : MonoBehaviour
{
    [SerializeField] private ColorType colorType;
    [SerializeField] private float shrinkVelocity = 0.5f;
    private BoxCollider boxCollider;
    private Vector3 initialScale;
    bool isShrinking;

    bool shrinked;
    bool expanded;

    private PlayerScript playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.FindObjectOfType<PlayerScript>();
        boxCollider = this.GetComponent<BoxCollider>();
        initialScale = this.transform.localScale;

    }

    // Update is called once per frame
    void Update()
    {
        Shrink();
        Unshrink();
    }

    void Shrink()
    {
        if (shrinked || playerMovement.currentColor != colorType)
            return;

        expanded = false;

        float step = shrinkVelocity * Time.deltaTime;
        Vector3 newScale = new Vector3();

        if (Vector3.Distance(this.transform.localScale, Vector3.zero) > step)
            newScale = this.transform.localScale - initialScale.normalized * step;
        else
        {
            newScale = Vector3.zero;
            shrinked = true;
            boxCollider.enabled = false;
        }

        this.transform.localScale = newScale;
    }

    void Unshrink()
    {
        if (expanded || playerMovement.currentColor == colorType)
            return;

        shrinked = false;

        float step = shrinkVelocity * Time.deltaTime;
        Vector3 newScale = new Vector3();

        if (Vector3.Distance(this.transform.localScale, initialScale) > step)
            newScale = this.transform.localScale + initialScale.normalized * step;
        else
        {
            newScale = initialScale;
            expanded = true;
            boxCollider.enabled = true;
        }

        this.transform.localScale = newScale;
    }
}
