using UnityEngine;

public class Pistol : Gun
{
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public AmmoGUI ammoText;

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
            Reload();
        }
        else if (clip <= 0 && nextAttack <= Time.time)
        {
            fireWhenEmpty = false;
            if (!Reload())
            {
                return; // Switch weapon
            }
        }
    }

    new void PrimaryAttack()
    {
        if (clip <= 0 && fireWhenEmpty)
        {
            nextAttack = Time.time + 0.1f;
            return;
        }

        clip--;
        muzzleFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(player.cam.transform.position, player.cam.transform.forward, out hit, range))
        {
            BaseEntity entity = hit.transform.GetComponent<BaseEntity>();
            if (entity != null)
            {
                entity.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 0.25f);
        }

        nextAttack = Time.time + fireRatePrimary;
        player.mouseLook.recoil = new Vector2(recoil.x, Random.Range(-recoil.y, recoil.y));
    }
}
