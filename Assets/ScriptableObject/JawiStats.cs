using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Jawi Stats", menuName = "Jawi Stats")]
public class JawiStats : ScriptableObject
{
    public float acceleration;
    public float maxSpeed;
    public float maxBoostSpeed;
    public float minTurnSpeed;
    public float maxTurnSpeed;
    public float maxStamina;
    public float boostSpeed;
    public float staminaDeplation;
    public float staminaRefill;
    public float driftTurnSpeed;
    public float bonusDriftSpeed;
}
