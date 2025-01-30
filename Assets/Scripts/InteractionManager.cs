using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{

    public static InteractionManager instance { get; set; }
    public Weapon hoverWeapon = null;
    public AmmoBox hoverAmmoBox  = null;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }

        else
        {
            instance = this;   
        }
    }

    private void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            
            GameObject objectHitByRayCast = hit.transform.gameObject;

            if (objectHitByRayCast.GetComponent<Weapon>() && objectHitByRayCast.GetComponent<Weapon>().isActiveWeapon == false)
            {
               hoverWeapon = objectHitByRayCast.gameObject.GetComponent<Weapon>();
                hoverWeapon.GetComponent<Outline>().enabled = true;
               
                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.instance.PickupWeapon(objectHitByRayCast.gameObject);
                }
                
            }
            else
            {
                if (hoverWeapon)
                {
                    hoverWeapon.GetComponent<Outline>().enabled = false;
                }
            }

            if (objectHitByRayCast.GetComponent<AmmoBox>())
            {
                hoverAmmoBox = objectHitByRayCast.gameObject.GetComponent<AmmoBox>();
                hoverAmmoBox.GetComponent<Outline>().enabled = true;
               
                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.instance.PickupAmmo(hoverAmmoBox);
                    Destroy(objectHitByRayCast.gameObject);
                }

            }
            else
            {
                if (hoverAmmoBox)
                {
                    hoverAmmoBox.GetComponent<Outline>().enabled = false;
                }
            }
        }

       
        
    }
}
