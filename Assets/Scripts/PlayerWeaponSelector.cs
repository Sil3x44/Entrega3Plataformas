using UnityEngine;

public class PlayerWeaponSelector : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameManager.Instance.ChangeWeapon(WeaponType.Sword);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameManager.Instance.ChangeWeapon(WeaponType.Spear);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameManager.Instance.ChangeWeapon(WeaponType.Bow);
        }
    }
}
