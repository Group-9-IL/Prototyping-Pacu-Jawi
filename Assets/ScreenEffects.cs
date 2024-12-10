using System.Collections;
using UnityEngine;

public class ScreenEffects : MonoBehaviour
{
    public enum EffectType { SpeedBoost, CleanRun, Ram }

    [SerializeField] private GameObject speedBoostFX;
    [SerializeField] private GameObject cleanRunFX;
    [SerializeField] private GameObject ramFX;

    private void Start()
    {
        // Ensure FX are initially inactive
        if (speedBoostFX) speedBoostFX.SetActive(false);
        if (cleanRunFX) cleanRunFX.SetActive(false);
        if (ramFX) ramFX.SetActive(false);
    }

    public void ActivateEffect(EffectType effectType, float duration)
    {
        Debug.Log("effect active");
        switch (effectType)
        {
            case EffectType.SpeedBoost:
                StartCoroutine(ActivateEffectCoroutine(speedBoostFX, duration));
                break;

            case EffectType.CleanRun:
                StartCoroutine(ActivateEffectCoroutine(cleanRunFX, duration));
                break;
                
            case EffectType.Ram:
                StartCoroutine(ActivateEffectCoroutine(ramFX, duration));
                break;
        }
    }

    private IEnumerator ActivateEffectCoroutine(GameObject effect, float duration)
    {
        if (effect)
        {
            effect.SetActive(true);
            yield return new WaitForSeconds(duration);
            effect.SetActive(false);
        }
    }
}
