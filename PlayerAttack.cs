using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //private int AAmmo = 10, PAmmo = 5;
    public static int TotalAssaultAmmo = 50, CurrentAssaultAmmo = 10, TotalShotgunAmmo = 20, TotalPistalAmmo = 20, CurrentPistalAmmo = 5;

  

    private WeaponManager weapon_Manager;

    public float fireRate = 10f;
    private float nextTimeToFire;
    public float damage = 10f;




    private Animator zoomCameraAnim;
    private bool zoomed;

    private Camera mainCam;

    private GameObject crosshair;

    private bool is_Aiming;

    [SerializeField]
    private GameObject arrow_Prefab, spear_Prefab;

    [SerializeField]
    private Transform arrow_Bow_StartPosition;

    void Awake()
    {

        weapon_Manager = GetComponent<WeaponManager>();

        zoomCameraAnim = transform.Find(Tags.LOOK_ROOT)
                                  .transform.Find(Tags.ZOOM_CAMERA).GetComponent<Animator>();

        crosshair = GameObject.FindWithTag(Tags.CROSSHAIR);

        mainCam = Camera.main;

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        WeaponShoot();
        ZoomInAndOut();
    }
    IEnumerator AReload()
    {
        yield return new WaitForSeconds(2f);
        if (CurrentAssaultAmmo == 0)
        {
            if (TotalAssaultAmmo >= 10)
            {
                CurrentAssaultAmmo = 10;
                TotalAssaultAmmo = TotalAssaultAmmo - 10;
            }
            else
            {
                CurrentAssaultAmmo = TotalAssaultAmmo;
                TotalAssaultAmmo = 0;
            }
        }
        else
        {
            if (CurrentAssaultAmmo + TotalAssaultAmmo < 10)
            {
                CurrentAssaultAmmo = CurrentAssaultAmmo + TotalAssaultAmmo;
                TotalAssaultAmmo = 0;
            }
            else
            {
                TotalAssaultAmmo -= (10 - CurrentAssaultAmmo);
                CurrentAssaultAmmo = 10;
            }
        }
    }
    IEnumerator PReload()
    {

        yield return new WaitForSeconds(2f);
        if(CurrentPistalAmmo==0)
        {
            if(TotalPistalAmmo>=5)
            {
                CurrentPistalAmmo = 5;
                TotalPistalAmmo = TotalPistalAmmo - 5;
            }
            else
            {
                CurrentPistalAmmo = TotalPistalAmmo;
                TotalPistalAmmo = 0;
            }
        }
        else
        {
            if(CurrentPistalAmmo+TotalPistalAmmo<5)
            {
                CurrentPistalAmmo = CurrentPistalAmmo + TotalPistalAmmo;
                TotalPistalAmmo = 0;
            }
            else
            {
                TotalPistalAmmo -= (5 - CurrentPistalAmmo);
                CurrentPistalAmmo = 5;
            }
        }
    }

    void WeaponShoot()
    {

        // if we have assault riffle

        if (weapon_Manager.GetCurrentSelectedWeapon().fireType == WeaponFireType.MULTIPLE)
        {

            if (CurrentAssaultAmmo > 0)
            {

                // if we press and hold left mouse click AND
                // if Time is greater than the nextTimeToFire
                if (Input.GetMouseButton(0) && Time.time > nextTimeToFire)
                {

                    nextTimeToFire = Time.time + 1f / fireRate;
                    
                    
                    weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();

                    BulletFired();
                    CurrentAssaultAmmo--;

                }
            }
            else if (TotalAssaultAmmo > 0)
            {

                StartCoroutine(AReload());


            }

            // if we have a regular weapon that shoots once
        }
        else
        {

            if (Input.GetMouseButtonDown(0))
            {

                // handle axe
                if (weapon_Manager.GetCurrentSelectedWeapon().tag == Tags.AXE_TAG)
                {
                    weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                }

                // handle shoot
                if (weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.BULLET)
                {
                    if (weapon_Manager.CurrentWeaponIndex() == 1)
                    {
                        if (CurrentPistalAmmo > 0)
                        {
                            weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();

                            BulletFired();
                            CurrentPistalAmmo--;
                        }
                        else if (TotalPistalAmmo >= 0)
                        {
                            StartCoroutine(PReload());
                        }
                    }
                    else if (TotalShotgunAmmo > 0)
                    {
                        Debug.Log("ShotGun");
                        TotalShotgunAmmo--;
                        weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();

                        BulletFired();
                    }


                }
                


            } // if input get mouse button 0

        } // else

    } // weapon shoot

    void ZoomInAndOut()
    {

        // we are going to aim with our camera on the weapon
        if (weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.AIM)
        {

            // if we press and hold right mouse button
            if (Input.GetMouseButtonDown(1))
            {

                zoomCameraAnim.Play(AnimationTags.ZOOM_IN_ANIM);

                crosshair.SetActive(false);
            }

            // when we release the right mouse button click
            if (Input.GetMouseButtonUp(1))
            {

                zoomCameraAnim.Play(AnimationTags.ZOOM_OUT_ANIM);

                crosshair.SetActive(true);
            }

        } // if we need to zoom the weapon

        

    } // zoom in and out

   

    void BulletFired()
    {

        RaycastHit hit;
        
            if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit))
        {

            if (hit.transform.tag == Tags.ENEMY_TAG)
            {
                hit.transform.GetComponent<HealthScript>().ApplyDamage(damage);
            }

        }

    } // bullet fired

} // class































