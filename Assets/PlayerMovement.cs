using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed;
    [Range(0, 1)]
    public float clampBoundariesMax;


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x, 1 - clampBoundariesMax, clampBoundariesMax);
        pos.y = Mathf.Clamp(pos.y, 1 - clampBoundariesMax, clampBoundariesMax);
        transform.position = Camera.main.ViewportToWorldPoint(pos);

        transform.Translate(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime,
                Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0);
    }
}