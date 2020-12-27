using UnityEngine;

public class Pistol : Gun
{
    public ParticleSystem muzzleFlash;
    public LineRenderer bulletTrail;
    public Color lineColor;
    public float lineSpeed = 10f;
    public Transform shootPosition;
    //public GameObject impactEffect;
    public AmmoGUI ammoText;

    private Animator animations;
    private BasePlayer thisPlayer;
    private InputManager inputManager;

    public int animationID = 262342271;

    private void Awake()
    {
        animations = GetComponent<Animator>();
        thisPlayer = gameObject.transform.parent.GetComponent<BasePlayer>();
    }

    private void Start()
    {
        inputManager = InputManager.Instance;
    }

    void Update()
    {
        if (lineColor.a > 0f)
        {
            LineFade();
        }
        //LineFade();

        if (reloading && nextAttack <= Time.time)
        {
            int cl = Mathf.Min(maxClip - clip, ammo);
            clip += cl;
            ammo -= cl;
            fireWhenEmpty = false;
            reloading = false;
            ammoText.ChangeText();
        }
        else if (inputManager.IsPlayerReloading() && !reloading)
        {
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
        else if (inputManager.IsPlayerAttackSecondary() && nextAttack <= Time.time)
        {
            SecondaryAttack();
        }
        else if (inputManager.IsPlayerAttack() && nextAttack <= Time.time)
        {
            if (clip <= 0)
            {
                fireWhenEmpty = true;
            }

            PrimaryAttack();
            ammoText.ChangeText();
        }
        else if (weaponIdle <= Time.time)
        {
            spread = 0;
            animations.SetInteger(animationID, 0);
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
            animations.SetInteger(animationID, 0);
            return;
        }

        clip--;
        muzzleFlash.Play();
        animations.SetInteger(animationID, 10);

        RaycastHit hit;

        if (Physics.Raycast(player.cam.position, Spread(), out hit, range))
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

            /*GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, fireRatePrimary / 1.5f);*/
            UpdateBulletTrail(hit.point);
        }

        nextAttack = Time.time + fireRatePrimary;
        weaponIdle = Time.time + fireRatePrimary;
    }

    private void LineFade()
    {
        lineColor.a = Mathf.Lerp(lineColor.a, 0, Time.deltaTime * lineSpeed);
        bulletTrail.startColor = lineColor;
        bulletTrail.endColor = lineColor;
    }

    private void UpdateBulletTrail(Vector3 hitPoint)
    {
        lineColor.a = 1f;
        bulletTrail.SetPosition(0, shootPosition.position);
        bulletTrail.SetPosition(1, hitPoint);
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
        forwardVector = player.cam.rotation * forwardVector;

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

        spread = 0;
        nextAttack = Time.time + reloadTime;
        weaponIdle = Time.time + reloadTime;
        reloading = true;

        if (clip <= 0)
            animations.SetInteger(animationID, 4);
        else
            animations.SetInteger(animationID, 3);

        return true;
    }

    public override void Deploy()
    {
        animations.SetInteger(animationID, 1);
        weaponIdle = Time.time + deployTime;
    }

    public override void Holster()
    {
        animations.SetInteger(animationID, 2);
        weaponIdle = Time.time + holsterTime;
    }
}
