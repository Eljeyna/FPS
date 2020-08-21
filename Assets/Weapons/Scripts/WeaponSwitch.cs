using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public AmmoGUI ammoText;

    public Gun prevWeapon;
    public Gun selectedWeapon;

    public Gun[] weapons;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && weapons[0] != null)
        {
            SwitchWeapon(weapons[0]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && weapons[1] != null)
        {
            SwitchWeapon(weapons[1]);
        }
    }

    public void SwitchWeapon(Gun weapon)
    {
        selectedWeapon.gameObject.SetActive(false);
        selectedWeapon.nextAttack = Time.time + 0.5f;
        selectedWeapon.reloading = false;
        prevWeapon = selectedWeapon;
        selectedWeapon = weapon;
        selectedWeapon.nextAttack = Time.time + 0.5f;
        selectedWeapon.gameObject.SetActive(true);
        ammoText.ChangeText();
    }
}
