using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;

public class PowerUp : MonoBehaviour
{
    public TypeOfPowerUp typeOfPowerUp;
    public float duration = 3.0f;
    public float strength = 1.0f;
    private PowerUp[] currentPowerUps;
    public bool isAttached = false;

    private PlayerScript playerScript;
    private float TEMP_originalSpeed; // DELETE LATER

    // Multiplier
    private float[] originalStrengths;


    public void SetValues(PowerUp _powerUp, bool _isAttached)
    {
        typeOfPowerUp = _powerUp.typeOfPowerUp; 
        duration = _powerUp.duration; 
        strength = _powerUp.strength; 
        isAttached = _isAttached;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PowerUp newPowerUp = other.gameObject.GetComponentInParent<PlayerScript>().gameObject.AddComponent<PowerUp>();
            newPowerUp.SetValues(this, true);
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!isAttached)
            return;

        CatchSamePowerUp();

        if (typeOfPowerUp == TypeOfPowerUp.MULTIPLIER)
            MultiplierPowerUp();

        playerScript = this.GetComponent<PlayerScript>();
        TEMP_originalSpeed = playerScript.moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttached)
            return;

        if (duration > 0.0f)
        {
            duration -= Time.deltaTime;
            UsePowerUp();
        }
        else
        {
            UsePowerDown();
            Destroy(this);
        }
    }

    void UsePowerUp()
    {
        switch(typeOfPowerUp)
        {
            case (TypeOfPowerUp.SPEED):
                SpeedPowerUp();
                break;
            case (TypeOfPowerUp.SHIELD):
                ShieldPowerUp();
                break;
        }
    }

    void UsePowerDown()
    {
        switch(typeOfPowerUp)
        {
            case TypeOfPowerUp.SPEED:
                SpeedPowerDown();
                break;
            case TypeOfPowerUp.MULTIPLIER:
                MultiplierPowerDown();
                break;
        }
    }

    void SpeedPowerUp()
    {
        playerScript.moveSpeed = strength;
    }

    void SpeedPowerDown()
    {
        playerScript.moveSpeed = TEMP_originalSpeed;
        // playerScript.moveSpeed = playerScript.normalSpeed
    }

    void ShieldPowerUp()
    {
        playerScript.SetShieldActive(true);
        // playerScript.shield = strength;
    }

    void MultiplierPowerUp()
    {
        if (currentPowerUps.Length <= 0)
            return;

        originalStrengths = new float[currentPowerUps.Length];
        for (int i = 0; i < currentPowerUps.Length; i++)
        {
            originalStrengths[i] = currentPowerUps[i].strength;
            currentPowerUps[i].strength *= strength;
        }
    }

    void MultiplierPowerDown()
    {
        if (currentPowerUps.Length <= 0)
            return;

        for (int i = 0; i < currentPowerUps.Length; i++)
        {
            if (currentPowerUps[i] != null)
            {
                currentPowerUps[i].strength = originalStrengths[i];
            }
        }
    }

    void CatchSamePowerUp()
    {
        currentPowerUps = this.GetComponents<PowerUp>();

        if (currentPowerUps.Length <= 1)
            return;

        foreach (PowerUp other in currentPowerUps)
        {
            if (other == this)
                continue;

            if (other.typeOfPowerUp == this.typeOfPowerUp)
            {
                other.duration = this.duration;
                Destroy(this);
            }
        }
    }
}
