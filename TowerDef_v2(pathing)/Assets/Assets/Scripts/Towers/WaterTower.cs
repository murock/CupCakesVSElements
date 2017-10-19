using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTower : Tower {

    [SerializeField]
    private float slowingFactor;
    [SerializeField]
    private int price1, damage1;
    [SerializeField]
    private float debuffDuration1, procChance1, slowingFactor1;
    [SerializeField]
    private int price2, damage2;
    [SerializeField]
    private float debuffDuration2, procChance2, slowingFactor2;

    public float SlowingFactor
    {
        get
        {
            return slowingFactor;
        }
    }

    private void Start()
    {
        ElementType = Element.WATER;


        Upgrades = new TowerUpgrade[]
        {
           // new TowerUpgrade(2,1,1,2,10),   // first upgrade
            //new TowerUpgrade(2,1,1,2,20),   //second upgrade
            new TowerUpgrade(price1,damage1,debuffDuration1,procChance1,slowingFactor1),
            new TowerUpgrade(price2,damage2,debuffDuration2,procChance2,slowingFactor2),
        };
    }

    public override Debuff GetDebuff()
    {
        return new WaterDebuff(SlowingFactor, DebuffDuration,Target);
    }

    public override string GetStats()
    {
        if (NextUpgrade != null)  //If the next is avaliable
        {
            return String.Format("<color=#00ffffff>{0}</color>{1} \nSlowing factor: {2}% <color=#00ff00ff>+{3}</color>", "<size=20><b>Water</b></size>", base.GetStats(), SlowingFactor, NextUpgrade.SlowingFactor);
        }

        //Returns the current upgrade
        return String.Format("<color=#00ffffff>{0}</color>{1} \nSlowing factor: {2}%", "<size=20><b>Water</b></size>", base.GetStats(), SlowingFactor);
    }

    public override void Upgrade()
    {
        this.slowingFactor += NextUpgrade.SlowingFactor;
        base.Upgrade();
    }
}
