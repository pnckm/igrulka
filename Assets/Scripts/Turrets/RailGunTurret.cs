using System;
using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class RailGunTurret : MonoBehaviour, ITurret
{
    [SerializeField] private GameObject RailGunProjectile;
    public static int Damage { get; private set; }
    public static float Speed { get; private set; }
    public static float CoolDown { get; private set; }
    private int level;
    public static GameObject ownShip;
    private PlayerShipBehaviour shipBehaviour;
    private float elapsed = 0.5f;

    private readonly Dictionary<int, string> DescriptionDict = new()
    {
        {0, "Выстреливает пробивным снарядом в ближайшего врага."},
        {1, "Скорострельность турели увеличивается на 30%."},
        {2, "Скорострельность турели увеличивается на 30%."},
        {3, "Урон от снаряда увеличивается на 50."},
        {4, "Скорость снаряда увеличивается на 40%."},
    };

    private readonly Dictionary<int, Action> LevelUpDict = new()
    {
        { 1, () => CoolDown *= 0.7f },
        { 2, () => CoolDown *= 0.7f },
        { 3, () => Damage += 50},
        { 4, () => Speed *= 1.4f },
    };

    void Start()
    {
        ownShip = GameObject.Find("OwnShip");
        shipBehaviour = ownShip.GetComponent<PlayerShipBehaviour>();
        CoolDown = 5f;
        Damage = 100;
        Speed = 80f;
    }
    
    void Update()
    {
        var enemies = GameObject.FindGameObjectsWithTag("enemy");
        if (enemies.Length == 0)
            return;
        Transform nearestEnemy = null;
        var maxDistance = Mathf.Infinity;
        foreach (var enemy in enemies)
        {
            var distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < maxDistance)
            {
                nearestEnemy = enemy.transform;
                maxDistance = distance;
            }
        }
        var directionToEnemy = nearestEnemy.position - transform.position;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, directionToEnemy);
        elapsed += Time.deltaTime;
        if (elapsed >= CoolDown)
        {
            Instantiate(RailGunProjectile, transform.position , transform.rotation);
            elapsed = 0;
        }
    }

    public Sprite GetSprite()
    {
        return gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    public string GetDescription()
    {
        return $"level {level}\n{DescriptionDict[level]}";
    }

    public int GetLevel()
    {
        return level;
    }

    public void LevelUp()
    {
        LevelUpDict[level].Invoke();
        level++;
    }

    public void Init()
    {
        Start();
        var turret = Instantiate(gameObject, ownShip.transform.position, ownShip.transform.rotation);
        turret.transform.SetParent(ownShip.transform);
        turret.transform.localPosition = shipBehaviour.positionsList[shipBehaviour.turretsCount];
        shipBehaviour.turretsCount++;
        level++;
    }
}
