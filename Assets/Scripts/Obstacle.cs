using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private PlayerScript playerScript;
    [SerializeField] private float changeAmount = -10.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindObjectOfType<PlayerScript>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            playerScript.ChangeHP(changeAmount);
        }
    }
}
