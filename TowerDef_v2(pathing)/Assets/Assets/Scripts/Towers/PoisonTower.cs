using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTower : Tower {
    [SerializeField]
    private float tickTime;

    [SerializeField]
    private PoisonSplash splashPrefab;

    [SerializeField]
    private int splashDamage;

    [SerializeField]
    private float debuffDuration1, procChance1, tickTime1;
    [SerializeField]
    private int price1, damage1, specialDamage1;
    [SerializeField]
    private float debuffDuration2, procChance2, tickTime2;
    [SerializeField]
    private int price2, damage2, specialDamage2;

    public int SplashDamage
    {
        get
        {
            return splashDamage;
        }
    }

    public float TickTime
    {
        get
        {
            return tickTime;
        }
    }

    private void Start()
    {
        ElementType = Element.POISON;

        Upgrades = new TowerUpgrade[]
        {
            //new TowerUpgrade(2,1,0.5f,5,-0.1f,1),   // first upgrade
            //new TowerUpgrade(5,1,0.5f,5,-0.1f,1),   //second upgrade
            new TowerUpgrade(price1,damage1,debuffDuration1,procChance1,tickTime1, specialDamage1),
            new TowerUpgrade(price2,damage2,debuffDuration2,procChance2,tickTime2, specialDamage2),
        };
    }

    public override Debuff GetDebuff()
    {
        return new PoisonDebuff(splashDamage, tickTime, splashPrefab, DebuffDuration, Target);
    }
    public override string GetStats()
    {
        if (NextUpgrade != null)
        {
            return string.Format("<color=#00ff00ff>{0}</color>{1} \nTick time: {2} <color=#00ff00ff>{4}</color>\nSplash damage: {3} <color=#00ff00ff>+{5}</color>", "<size=20><b>Poison</b></size>", base.GetStats(), TickTime, SplashDamage, NextUpgrade.TickTime, NextUpgrade.SpecialDamage);
        }

        return string.Format("<color=#00ff00ff>{0}</color>{1} \nTick time: {2}\nSplash damage: {3}", "<size=20><b>Poison</b></size>", base.GetStats(), TickTime, SplashDamage);

    }
    
    public override void Upgrade()
    {
        this.splashDamage += NextUpgrade.SpecialDamage;
        this.tickTime -= NextUpgrade.TickTime;
        base.Upgrade();
    }
}
