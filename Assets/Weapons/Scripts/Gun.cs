using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Gun : MonoBehaviour
{
    public float damage;
    public float headModifier = 3f;
    public float bodyModifier = 1f;
    public float legModifier = 1f;
    public float range;
    public float fireRatePrimary;
    public float fireRateSecondary;
    public float impactForce;

    public RigidbodyFirstPersonController player;

    public float nextAttack;
    public float reloadTime;
    public bool reloading = false;
    public bool fireWhenEmpty = false;

    public int clip;
    public int maxClip;
    public int ammo;
    public int maxAmmo;

    public Vector2 recoil;
    public Vector2[] recoilPattern;
    public int recoilNum;

    public void PrimaryAttack()
    {
        return;
    }

    public void SecondaryAttack()
    {
        return;
    }

    public void Deploy()
    {
        return;
    }

    public bool Reload()
    {
        if (ammo <= 0)
        {
            return false;
        }

        int cl = Mathf.Min(maxClip - clip, ammo);

        if (cl <= 0)
            return false;

        nextAttack = Time.time + reloadTime;
        reloading = true;

        return true;
    }
}
