using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public AmmoGUI ammoText;
    public float nextSwitch;

    public Gun prevWeapon;
    public Gun selectedWeapon;

    public Gun[] weapons;

    private void Start()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != selectedWeapon)
            {
                weapons[i].gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (nextSwitch > 0 && nextSwitch <= Time.time)
        {
            nextSwitch = 0;
            prevWeapon.gameObject.SetActive(false);
            selectedWeapon.nextAttack = Time.time + selectedWeapon.deployTime;
            selectedWeapon.gameObject.SetActive(true);
            selectedWeapon.Deploy();
            ammoText.ChangeText();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && weapons[0] != null)
        {
            if (weapons[0] != selectedWeapon)
                SwitchWeapon(weapons[0]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && weapons[1] != null)
        {
            if (weapons[1] != selectedWeapon)
                SwitchWeapon(weapons[1]);
        }
    }

    public void SwitchWeapon(Gun weapon)
    {
        if (weapon.clip == 0 && weapon.ammo == 0)
            return;

        selectedWeapon.Holster();
        selectedWeapon.nextAttack = Time.time + selectedWeapon.holsterTime;
        selectedWeapon.reloading = false;
        prevWeapon = selectedWeapon;
        selectedWeapon = weapon;
        nextSwitch = Time.time + prevWeapon.holsterTime;
    }

    public bool ShouldSwitch()
    {
        for (int i = 0; i < weapons.Length - 1; i++)
        {
            if (weapons[i] != selectedWeapon && (weapons[i].clip == -1) || (weapons[i].clip > 0 && weapons[i].ammo > 0))
            {
                SwitchWeapon(weapons[i]);
            }
        }
        return true;
    }
}
