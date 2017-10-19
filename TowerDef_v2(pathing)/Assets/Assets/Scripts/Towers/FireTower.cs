using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : Tower {

    [SerializeField]
    private float tickTime;

    [SerializeField]
    private float tickDamage;

    [SerializeField]
    private float debuffDuration1, procChance1, tickTime1;
    [SerializeField]
    private int price1, damage1, specialDamage1;
    [SerializeField]
    private float debuffDuration2, procChance2, tickTime2;
    [SerializeField]
    private int price2, damage2, specialDamage2;

    public float TickTime
    {
        get
        {
            return tickTime;
        }

    }

    public float TickDamage
    {
        get
        {
            return tickDamage;
        }
    }

    private void Start()
    {
        ElementType = Element.FIRE;

        Upgrades = new TowerUpgrade[]
        {
            //new TowerUpgrade(2,2,.5f,5,-0.1f,1),    //first upgrade
            //new TowerUpgrade(5,3,.5f,5,-0.1f,1),    //second upgrade
            new TowerUpgrade(price1,damage1,debuffDuration1,procChance1,tickTime1,specialDamage1),    //first upgrade
            new TowerUpgrade(price2,damage2,debuffDuration2,procChance2,tickTime2, specialDamage2),
        };
    }

    public override Debuff GetDebuff()
    {
        return new FireDebuff(tickDamage, tickTime, DebuffDuration, Target);
    }

    public override string GetStats()
    {
        if (NextUpgrade != null) //If the next is avaliable
        {
            return string.Format("<color=#ffa500ff>{0}</color>{1} \nTick time: {2} <color=#00ff00ff>{4}</color>\nTick damage: {3} <color=#00ff00ff>+{5}</color>", "<size=20><b>Fire</b></size> ", base.GetStats(), TickTime, TickDamage, NextUpgrade.TickTime, NextUpgrade.SpecialDamage);
        }

        //Returns the current upgrade
        return string.Format("<color=#ffa500ff>{0}</color>{1} \nTick time: {2}\nTick damage: {3}", "<size=20><b>Fire</b></size> ", base.GetStats(), TickTime, TickDamage);
    }

    public override void Upgrade()
    {
        this.tickTime -= NextUpgrade.TickTime;      //reduce time between ticks
        this.tickDamage += NextUpgrade.SpecialDamage;
        base.Upgrade();
    }
}
