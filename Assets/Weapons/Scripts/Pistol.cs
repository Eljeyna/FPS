﻿using UnityEngine;

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
            player.mouseLook.smooth = false;
            recoilNum = 0;
            Reload();
        }
        else if (clip <= 0 && nextAttack <= Time.time)
        {
            fireWhenEmpty = false;
            player.mouseLook.smooth = false;
            recoilNum = 0;
            if (!Reload())
            {
                return; // Switch weapon
            }
        }
        else if (nextAttack <= Time.time)
        {
            player.mouseLook.smooth = false;
            recoilNum = 0;
        }
    }

    new void PrimaryAttack()
    {
        if (clip <= 0 && fireWhenEmpty)
        {
            player.mouseLook.smooth = false;
            nextAttack = Time.time + 0.1f;
            return;
        }

        clip--;
        muzzleFlash.Play();

        RaycastHit hit;
        Vector3 recoiling = new Vector3(-recoilPattern[recoilNum].x, recoilPattern[recoilNum].y, 0f);
        if (Physics.Raycast(recoiling + player.cam.transform.position, recoiling - player.cam.transform.forward, out hit, range))
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
        //player.mouseLook.recoil = new Vector2(recoil.x, Random.Range(-recoil.y, recoil.y));
        player.mouseLook.recoil = recoilPattern[recoilNum];
        player.mouseLook.smooth = true;
        recoilNum++;
        if (recoilNum > recoilPattern.Length - 1)
            recoilNum = 0;
    }
}
