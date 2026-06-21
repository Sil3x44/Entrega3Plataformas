using System;
using UnityEngine;

public class PlayerWeaponAnimations : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private RuntimeAnimatorController baseController;
    [SerializeField] private RuntimeAnimatorController swordController;
    [SerializeField] private RuntimeAnimatorController spearController;
    [SerializeField] private RuntimeAnimatorController bowController;

    private WeaponType currentWeapon;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentWeapon = GameManager.Instance.GetCurrentWeapon();
        UpdateAnimationSet();
    }

    private void Update()
    {
        if (currentWeapon != GameManager.Instance.GetCurrentWeapon())
        {
            currentWeapon = GameManager.Instance.GetCurrentWeapon();
            UpdateAnimationSet();
        }
    }

    public void UpdateAnimationSet()
    {
        switch (GameManager.Instance.GetCurrentWeapon())
        {
            case WeaponType.None:
                animator.runtimeAnimatorController = baseController;
                break;
            
            case WeaponType.Sword:
                animator.runtimeAnimatorController = swordController;
                break;

            case WeaponType.Spear:
                animator.runtimeAnimatorController = spearController;
                break;

            case WeaponType.Bow:
                animator.runtimeAnimatorController = bowController;
                break;
        }
    }
}
