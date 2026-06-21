using System;
using System.Collections;
using UnityEngine;

public class PlayerPotionUser : MonoBehaviour
{
    [SerializeField] private float damagePotionDuration = 30f;
    
    private PlayerHealth playerHealth;
    private Coroutine damagePotionCoroutine;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameManager.Instance.TryUseHealthPotion(playerHealth);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            bool usedPotion = GameManager.Instance.TryUseDamagePotion();

            if (usedPotion)
            {
                if (damagePotionCoroutine != null)
                {
                    StopCoroutine(damagePotionCoroutine);
                }

                damagePotionCoroutine = StartCoroutine(DamagePotionCoroutine());
            }
        }
    }

    private IEnumerator DamagePotionCoroutine()
    {
        yield return new WaitForSeconds(damagePotionDuration);

        GameManager.Instance.DisableDamageBoost();
    }
}
