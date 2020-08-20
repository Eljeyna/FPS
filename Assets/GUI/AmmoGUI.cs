using TMPro;
using UnityEngine;

public class AmmoGUI : MonoBehaviour
{
    public WeaponSwitch switchScript;
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        ChangeText();
    }

    public void ChangeText()
    {
        text.text = switchScript.selectedWeapon.clip + " / " + switchScript.selectedWeapon.ammo;
    }
}
