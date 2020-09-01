using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class CameraShake : MonoBehaviour
{
    public Camera cam;
    private RigidbodyFirstPersonController playerMouse;
    private WeaponSwitch weapons;

    private void Start()
    {
        playerMouse = GetComponent<RigidbodyFirstPersonController>();
        weapons = GetComponent<WeaponSwitch>();
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            //float x = Random.Range(-1f, 1f) * magnitude;
            float x = Random.Range(-0.2f, -0.8f) * magnitude;
            float y = weapons.selectedWeapon.spreadPattern[weapons.selectedWeapon.spread].x * 0.02f;

            playerMouse.mouseLook.shakeX = x;
            playerMouse.mouseLook.shakeY = y;

            elapsed += Time.deltaTime;

            yield return null;
        }
    }
}
