using UnityEngine;

public class Pistol : Gun
{
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public AmmoGUI ammoText;

    private Animator animations;
    private BasePlayer thisPlayer;

    [HideInInspector]
    public CameraShake shaker;

    private void Awake()
    {
        animations = GetComponent<Animator>();
        thisPlayer = gameObject.transform.parent.parent.GetComponent<BasePlayer>();
        shaker = player.GetComponent<CameraShake>();
    }

    void Update()
    {
        if (reloading && nextAttack <= Time.time)
        {
            int cl = Mathf.Min(maxClip - clip, ammo);
            clip += cl;
            ammo -= cl;
            fireWhenEmpty = false;
            reloading = false;
            ammoText.ChangeText();
        }

        if (Input.GetButton("Fire2") && nextAttack <= Time.time)
        {
            SecondaryAttack();
        }
        else if (Input.GetButton("Fire1") && nextAttack <= Time.time)
        {
            if (clip <= 0)
            {
                fireWhenEmpty = true;
            }

            PrimaryAttack();
            ammoText.ChangeText();
        }
        else if (Input.GetButton("Reload") && !reloading)
        {
            spread = 0;
            Reload();
        }
        else if (clip != -1 && clip == 0 && nextAttack <= Time.time)
        {
            fireWhenEmpty = false;
            spread = 0;
            if (autoreload && !Reload())
            {
                weaponSwitch.ShouldSwitch();
            }
        }
        else if (weaponIdle <= Time.time)
        {
            spread = 0;
            animations.SetInteger("Animation", 0);
            weaponIdle = Time.time + 1f;
        }
    }

    public override void PrimaryAttack()
    {
        if (clip <= 0 && fireWhenEmpty)
        {
            spread = 0;
            nextAttack = Time.time + 0.1f;
            weaponIdle = Time.time + 0.1f;
            return;
        }

        clip--;
        muzzleFlash.Play();
        animations.Play("Fire");

        RaycastHit hit;

        if (Physics.Raycast(player.cam.transform.position, Spread(), out hit, range))
        //if (Physics.Raycast(player.cam.transform.position, player.cam.transform.forward, out hit, range))
        {
            BaseEntity entity = hit.transform.GetComponent<BaseEntity>();
            if (entity != null)
            {
                entity.TakeDamage(damage, thisPlayer);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, fireRatePrimary / 1.5f);
        }

        nextAttack = Time.time + fireRatePrimary;
        weaponIdle = Time.time + fireRatePrimary;
    }

    public override void SecondaryAttack()
    {
        return;
    }

    public override Vector3 Spread()
    {
        Vector3 forwardVector = Vector3.forward;
        float deviation = Random.Range(spreadPattern[0].x, spreadPattern[1].x);
        float angle = Random.Range(spreadPattern[0].y, spreadPattern[1].y);
        forwardVector = Quaternion.AngleAxis(deviation, Vector3.up) * forwardVector;
        forwardVector = Quaternion.AngleAxis(angle, Vector3.forward) * forwardVector;
        forwardVector = player.cam.transform.rotation * forwardVector;

        //StartCoroutine(shaker.Shake(fireRatePrimary / 2, 0.25f));

        return forwardVector;
    }

    public override bool Reload()
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

    public override void Deploy()
    {
        animations.SetInteger("Animation", 1);
        weaponIdle = Time.time + deployTime;
    }

    public override void Holster()
    {
        animations.SetInteger("Animation", 2);
        weaponIdle = Time.time + holsterTime;
    }
}
