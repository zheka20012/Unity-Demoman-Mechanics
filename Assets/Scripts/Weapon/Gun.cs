using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base gun class, parent of all guns
public class Gun : MonoBehaviour
{
    [SerializeField]
    protected int ClipSize;
    [SerializeField]
    protected int AmmoSize;
    [SerializeField]
    protected float FireRate;
    [SerializeField]
    protected AudioSource GunSounds;
    [SerializeField]
    protected AudioClip FireSound;

    protected int RealClipSize, RealAmmoSize;

    private Animator controller;
    private float nextFireTime = 0f;


    private void Awake()
    {
        controller = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Tick();
    }

    protected virtual void Tick()
    {
        if (Input.GetMouseButtonUp((int)MouseButton.Left))
        {
            PrimaryAttack();
        }
    }

    protected virtual void PrimaryAttack()
    {

        // If there is more than one bullet between the last and this frame
        // Reset the nextFireTime
        if (Time.time - FireRate > nextFireTime)
            nextFireTime = Time.time - Time.deltaTime;

        // Keep firing until we used up the fire time
        while (nextFireTime < Time.time)
        {
            Shoot();
            nextFireTime += FireRate;
        }
    }

    protected virtual void Shoot()
    {
        controller.SetTrigger("Fire");
        GunSounds.PlayOneShot(FireSound);
        RealClipSize--;
    }

    public virtual void RefillAmmo(int value)
    {
        RealAmmoSize = Mathf.Min(value, AmmoSize);
    }
}
