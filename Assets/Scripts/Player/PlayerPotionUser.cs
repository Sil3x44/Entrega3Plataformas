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
        float timer = damagePotionDuration;

        while (timer > 0)
        {
            GameManager.Instance.ShowDamagePotionTimer(timer);

            timer -= Time.deltaTime;

            yield return null;
        }

        GameManager.Instance.DisableDamageBoost();
        GameManager.Instance.HideDamagePotionTimer();
    }
}
