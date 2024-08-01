using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Transform savePoint;
    public Image damageImage;
    public float cooldownTime = 2.0f;
    public float fadeSpeed = 1.0f;
    public int maxHits = 4;
    public bool isHiding = false;
    public bool isInteracting = false;
    public int currentHits = 0; 
    public float cooldownTimer = 0.0f;
    public bool hasDoorKey = false;
    
    private bool isCoolingDown = false;

    private void Update()
    {
        if (isCoolingDown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                cooldownTimer = 0;
                isCoolingDown = false;
            }
        }
        else
        {
            if (damageImage.color.a > 0)
            {
                Color color = damageImage.color;
                float newAlpha = Mathf.Lerp(color.a, 0, fadeSpeed * Time.deltaTime);
                color.a = newAlpha;
                damageImage.color = color;
                ResetDamage();
            }
        }
    }

    public void TakeDamage()
    {
        currentHits = Mathf.Clamp(currentHits + 1, 0, maxHits);
        UpdateDamageEffect();
        cooldownTimer = cooldownTime;
        isCoolingDown = true;
    }

    void UpdateDamageEffect()
    {
        float alpha = (float)currentHits / maxHits;
        Color color = damageImage.color;
        color.a = alpha;
        damageImage.color = color;
    }

    public void ResetDamage()
    {
        currentHits = 0;
    }
}
