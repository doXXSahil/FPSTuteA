using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public bool isActiveWeapon;
    
    //Shooting
    public bool isShooting, isReadyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;
    //Burst
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;
    //spread
    public float spreadIntensity;
    //Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity=30f;
    public float bulletPrefabLifeTime = 3f;

    //Loading
    public float reloadTime;
    public int magzineSize, bulletsLeft;
    public bool isReloading;

    //muzzleEffect
    public GameObject muzzleEffect;

    //Animation
    internal Animator animator;

    //WeaponSpawn
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    public enum WeaponModel
    {
        pistolM1911,
        M107
    }

    public WeaponModel thisWeaponModel;

    
    public enum shootingMode
    {
        Single,
        Burst,
        Auto
    }

    public shootingMode currentShootingMode;

    private void Awake()
    {
        isReadyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        

        bulletsLeft = magzineSize;

        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (isActiveWeapon)
        {
            if (bulletsLeft == 0 && isShooting)
            {
                SoundManager.instance.emptyMagSoundM1911.Play();
            }
            //Lest mose click for fire 
            if (currentShootingMode == shootingMode.Auto)
            {
                //only when holding the left mouse
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == shootingMode.Single || currentShootingMode == shootingMode.Burst)
            {
                //One click one shot
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magzineSize && isReloading == false && WeaponManager.instance.CheckAmmoLeftFor(thisWeaponModel) > 0)
            {
                Reload();
            }
            if (isReadyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletsLeft = bulletsPerBurst;
                FireWeapon();
            }
            //AutomaticReload
            if (isReadyToShoot && isShooting == false && isReloading == false && bulletsLeft <= 0)
            {
                //Reload();
            }           
        }
       
    }

    
    private void FireWeapon()
    {
        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("RECOIL");
        //SoundManager.instance.shootingSoundM1911.Play();  

        SoundManager.instance.PlayShootingSound(thisWeaponModel);

        bulletsLeft--;
        isReadyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;
        //instanciate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        //pointing bullet at shooting direction
        bullet.transform.forward = shootingDirection;  

        //Fire the bullet
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        //Destroy the bullet after sometime
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        

        //BurstMode

        if(currentShootingMode == shootingMode.Burst && burstBulletsLeft >1) //We already shoot once before checking this point
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }

        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }      
    }

    private void Reload()
    {
        //SoundManager.instance.reloadingSoundM1911.Play();
        SoundManager.instance.PlayReloadSound(thisWeaponModel);
        animator.SetTrigger("RELOAD"); 
        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);

    }

    private void ReloadCompleted()
    {

        if (WeaponManager.instance.CheckAmmoLeftFor(thisWeaponModel) > magzineSize)
        {
            bulletsLeft = magzineSize;
            WeaponManager.instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }
        else
        {
            bulletsLeft = WeaponManager.instance.CheckAmmoLeftFor(thisWeaponModel);
            WeaponManager.instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }
        isReloading = false;
    }

    private void ResetShot()
    {
        isReadyToShoot = true;
        allowReset = true;
    }


    public Vector3 CalculateDirectionAndSpread()
    {
        //shooting ray from middle of the screen and when we are pointing at target
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;

        if(Physics.Raycast(ray, out hit))
        {
            //hitting something
            targetPoint = hit.point;
        }
        else
        {
            // shooting in air
            targetPoint = ray.GetPoint(100);
        }
        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        //returning the shooting direction and spread
        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
        
    } 
}
