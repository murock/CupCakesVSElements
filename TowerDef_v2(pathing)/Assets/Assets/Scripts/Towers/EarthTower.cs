using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthTower : Tower {

    [SerializeField]
    private int price1, damage1; 
    [SerializeField]
    private float debuffDuration1, procChance1;
    [SerializeField]
    private int price2, damage2;
    [SerializeField]
    private float debuffDuration2, procChance2;

    private void Start()
    {
        ElementType = Element.EARTH;

        Upgrades = new TowerUpgrade[]
        {
            //new TowerUpgrade(2,2,1,2),   // first upgrade
            //new TowerUpgrade(5,3,1,2),   //second upgrade
            new TowerUpgrade(price1,damage1,debuffDuration1,procChance1),
            new TowerUpgrade(price2,damage2,debuffDuration2,procChance2),
        };
    }

    public override Debuff GetDebuff()
    {
        return new EarthDebuff(Target, DebuffDuration);
    }

    public override string GetStats()
    {
        return string.Format("<color=#8b4513ff>{0}</color>{1}", "<size=20><b>Earth</b></size>", base.GetStats());
    }
}
