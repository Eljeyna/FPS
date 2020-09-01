using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public abstract class Gun : MonoBehaviour
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
    public WeaponSwitch weaponSwitch;

    [HideInInspector]
    public float nextAttack;
    public float reloadTime;
    public float holsterTime;
    public float deployTime;
    [HideInInspector]
    public float weaponIdle;
    public bool reloading = false;
    public bool fireWhenEmpty = false;

    public bool autoreload = true;

    public int clip;
    public int maxClip;
    public int ammo;
    public int maxAmmo;

    public Vector2[] spreadPattern;
    [HideInInspector]
    public int spread;

    public abstract void PrimaryAttack();
    public abstract void SecondaryAttack();
    public abstract Vector3 Spread();
    public abstract bool Reload();
    public abstract void Deploy();
    public abstract void Holster();
}
