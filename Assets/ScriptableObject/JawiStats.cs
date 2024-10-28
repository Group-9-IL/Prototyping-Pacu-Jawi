using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Jawi Stats", menuName = "Jawi Stats")]
public class JawiStats : ScriptableObject
{
    public float maxSpeed;
    public float accelarationRate;
    public float normalSteeringAngle;
    public float driftSteeringAngle;
    public float highSteeringAngle;
    public float downForce;
    public float initialAccelaration;
    public float maxStamina;
    public float staminaDepletion;
    public float staminaRefill;
    public float maxBoostSpeed;
    public float boostAccelarationRate;
}
