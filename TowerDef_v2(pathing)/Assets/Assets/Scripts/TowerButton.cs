using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour {

    [SerializeField]
    private GameObject towerPrefab;

    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private int price;

    [SerializeField]
    private Text priceTxt;

    public GameObject TowerPrefab
    {
        get
        {
            return towerPrefab;
        }
    }

    public Sprite Sprite
    {
        get
        {
            return sprite;
        }

    }

    public int Price
    {
        get
        {
            return price;
        }

        set
        {
            price = value;
        }
    }

    private void Start()
    {
        priceTxt.text = Price + "<color=yellow> G </color>";

        GameManager.Instance.Changed += new CurrencyChanged(PriceCheck);
    }

    private void PriceCheck()
    {
        if (price <= GameManager.Instance.Currency) //if player can afford tower
        {
            GetComponent<Image>().color = Color.white;
            priceTxt.color = Color.white;
        }
        else
        {
            GetComponent<Image>().color = Color.grey;
            priceTxt.color = Color.grey;
        }
    }

    //gets the tower type
    public void ShowInfo(string type)
    {
        string tooltip = string.Empty;
        switch(type)
        {
            case "Fire":
                FireTower fire = towerPrefab.GetComponentInChildren<FireTower>();
                tooltip = string.Format("<color=#ffa500ff><size=20><b>Fire</b></size></color>\n" +
                                        "Damage: {0} \n" +
                                        "Proc: {1}%\n" +
                                        "Debuff duration: {2}sec \n" +
                                        "Tick time: {3} sec \n" +
                                        "Tick damage: {4}\n" +
                                        "Can apply a DOT to the target",
                                        fire.Damage, fire.Proc, fire.DebuffDuration, fire.TickTime, fire.TickDamage);
                break;
            case "Water":
                WaterTower water = towerPrefab.GetComponentInChildren<WaterTower>();
                tooltip = string.Format("<color=#00ffffff><size=20><b>Water</b></size></color>\n" +
                                        "Damage: {0}\n" + 
                                        "Proc: {1}% \n" +
                                        "Debuff duration: {2}sec \n" +
                                        "Slowing factor: {3} \n" + 
                                        "Has a chance to slow down the target", 
                                        water.Damage, water.Proc,water.DebuffDuration, water.SlowingFactor);

                break;
            case "Poison":
                PoisonTower poison = towerPrefab.GetComponentInChildren<PoisonTower>();
                tooltip = string.Format("<color=#00ff00ff><size=20><b>Poison</b></size></color>\n" +
                                        "Damage: {0} \n" +
                                        "Proc: {1}%\n" +
                                        "Debuff duration: {2}sec \n" +
                                        "Tick time: {3} sec \n" +
                                        "Splash damage: {4}\n" +
                                        "Can apply dripping poison",
                                        poison.Damage, poison.Proc, poison.DebuffDuration, poison.TickTime, poison.SplashDamage);
                break;
            case "Earth":
                EarthTower earth = towerPrefab.GetComponentInChildren<EarthTower>();
                tooltip = string.Format("<color=#8b4513ff><size=20><b>Earth</b></size></color>\n" +
                                        "Damage: {0} \n" +
                                        "Proc: {1}%\n" +
                                        "Debuff duration: {2}sec\n" +
                                        "Has a chance to stun the target",
                                        earth.Damage, earth.Proc, earth.DebuffDuration);
                break;

        }
        
        GameManager.Instance.SetToolTipText(tooltip);
        GameManager.Instance.ShowStats();
    }
}
