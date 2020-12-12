using UnityEngine;
using System.Collections;

public class StickyLauncher : Gun
{
    [SerializeField]
    private float BasePower = 100f;
    [SerializeField]
    private float MaxShootPower = 1.3f; //130%
    [SerializeField]
    private float PowerIncreaseSpeed = .15f;
    [SerializeField]
    private Transform ShootPosition;

    [Header("Sticky Bomb Parameters")]
    [SerializeField]
    private GameObject StickyBombPrefab;
    [SerializeField]
    private float ActivationTime = .7f;
    [SerializeField]
    private int MaxBombsAtTime = 8;

    private StickyBomb[] StickyBombs;
    private int StickiesCount = 0;

    //Current Shoot Power, the longer player presses LMB the bigger power is applied
    private float ShootPower = 1f;

    private void Start()
    {
        StickyBombs = new StickyBomb[MaxBombsAtTime];
    }

    protected override void Tick()
    {
        if (Input.GetMouseButton((int)MouseButton.Left))
        {
            ShootPower = Mathf.MoveTowards(ShootPower, MaxShootPower, Time.deltaTime * PowerIncreaseSpeed);
        }

        base.Tick();

        if (Input.GetMouseButton((int) MouseButton.Right))
        {
            Detonate();
        }
    }

    private void Detonate()
    {
        for (int i = 0; i < MaxBombsAtTime; i++)
        {
            if(StickyBombs[i] == null) continue;

            StickyBombs[i].Explode();
        }
    }

    protected override void Shoot()
    {
        base.Shoot();

        var bomb = GameObject.Instantiate(StickyBombPrefab, ShootPosition.position, Quaternion.identity).GetComponent<StickyBomb>();
        bomb.GetComponent<Rigidbody>().AddForceAtPosition(transform.forward * BasePower * ShootPower, ShootPosition.position, ForceMode.Impulse);
        bomb.Throw(ActivationTime);
        //If we exceeded more than MaxBombsAtTime Destroy first Stickie without explosion
        if(StickyBombs[StickiesCount] != null) StickyBombs[StickiesCount].Destroy();
        StickyBombs[StickiesCount] = bomb;
        StickiesCount = (++StickiesCount) % MaxBombsAtTime;
        ShootPower = 1;
    }

    private void OnGUI()
    {
        GUILayout.BeginArea( new Rect(Screen.width - 100, Screen.height - 150, 100, 150));
        GUILayout.Label($"POWER: {(ShootPower * 100):F}");
        GUILayout.EndArea();
    }
}
