using UnityEngine;

public class PlayerWeaponSelector : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SelectNextWeapon();
        }
    }

    private void SelectNextWeapon()
    {
        WeaponType currentWeapon = GameManager.Instance.GetCurrentWeapon();

        if (currentWeapon == WeaponType.Sword)
        {
            TrySelectInOrder(WeaponType.Spear, WeaponType.Bow, WeaponType.Sword);
        }
        else if (currentWeapon == WeaponType.Spear)
        {
            TrySelectInOrder(WeaponType.Bow, WeaponType.Sword, WeaponType.Spear);
        }
        else if (currentWeapon == WeaponType.Bow)
        {
            TrySelectInOrder(WeaponType.Sword, WeaponType.Spear, WeaponType.Bow);
        }
    }

    private void TrySelectInOrder(WeaponType first, WeaponType second, WeaponType fallback)
    {
        if (GameManager.Instance.HasWeapon(first))
        {
            GameManager.Instance.ChangeWeapon(first);
        }
        else if (GameManager.Instance.HasWeapon(second))
        {
            GameManager.Instance.ChangeWeapon(second);
        }
        else
        {
            GameManager.Instance.ChangeWeapon(fallback);
        }
    }
}
