using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element { EARTH, FIRE, WATER, POISON, NONE}

public abstract class Tower : MonoBehaviour {   //abstract means it cannot be standalone other classes must inherit from it

    [SerializeField]
    private string projectileType;

    [SerializeField]
    private float projectileSpeed;

    [SerializeField]
    private int damage;

    [SerializeField]
    private float debuffDuration;

    [SerializeField]
    private float proc;     //chance to proc debuff

    [SerializeField]
    private GameObject range;

    [SerializeField]
    private float rangeIncrease = 1;

    public Element ElementType { get; protected set; }

    public int Price { get; set; }

    public float ProjectileSpeed
    {
        get { return projectileSpeed; }
    }


    private SpriteRenderer mySpriteRenderer;

    private Monster target;

    //towers cuurent upgrade level

    public int Level { get;protected set; }

    public Monster Target
    {
        get { return target; }
    }

    public int Damage
    {
        get
        {
            return damage;
        }
    }

    public float DebuffDuration
    {
        get
        {
            return debuffDuration;
        }

        set
        {
            debuffDuration = value;
        }
    }

    public float Proc
    {
        get
        {
            return proc;
        }

        set
        {
            proc = value;
        }
    }

    private Queue<Monster> monsters = new Queue<Monster>();

    private bool canAttack = true;

    private float attackTimer;

    [SerializeField]
    private float attackCooldown;

    public TowerUpgrade[] Upgrades { get;protected set; }

    public TowerUpgrade NextUpgrade
    {
        get
        {
            if (Upgrades.Length > Level -1)
            {
                return Upgrades[Level - 1];
            }
            return null;
        }
    }
    // Use this for initialization
    void Awake () {    
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        Level = 1;
	}
	
	// Update is called once per frame
	void Update () {
        Attack();
	}

    public void Select()
    {
        mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
        GameManager.Instance.UpdateUpgradeTip();   //makes sure correct tooltip is displayed
    }

    private void Attack()
    {

        if (!canAttack)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackCooldown)
            {
                canAttack = true;
                attackTimer = 0;
            }
        }
        //priority system attacks first one to enter range
        if (target == null && monsters.Count > 0 && monsters.Peek().IsActive)  //if we have no target, but there are more monsters in the Q
        {
            target = monsters.Dequeue();    //make target next in Q
        }

        if (target != null)
        {
            if (target != null && target.IsActive)  //dont attack dead or despawned monster
            {
                if (canAttack)
                {
                    Shoot();
                    canAttack = false;
                }
            }
            if (target != null && !target.Alive || !target.IsActive)
            {
                target = null;  //if target is dead then de target
            }
        }

    }

    public virtual string GetStats()
    {
        if (NextUpgrade != null)    //if upgrade avaliable
        {
            return string.Format("\nLevel: {0} \nDamage: {1}  <color=#00ff00ff> +{4}</color>\nProc: {2}% <color=#00ff00ff>+{5}%</color>\nDebuff: {3}sec <color=#00ff00ff>+{6}</color>",
                Level, damage, proc, DebuffDuration, NextUpgrade.Damage, NextUpgrade.ProcChance, NextUpgrade.DebuffDuration);
        }
        return string.Format("\nLevel: {0} \nDamage{1} \nProc: {2}% \nDebuff: {3}secs", Level, damage, proc, DebuffDuration);
    }

    private void Shoot()
    {
        Projectile projectile = GameManager.Instance.Pool.GetObject(projectileType).GetComponent<Projectile>();

        projectile.transform.position = transform.position;

        projectile.Initialize(this);
    }

    public virtual void Upgrade()
    {
        GameManager.Instance.Currency -= NextUpgrade.Price;     //take gold from player
        Price += NextUpgrade.Price;                             //increase price of tower by upgrade cost
        this.damage += NextUpgrade.Damage;                      //increase damage   
        this.proc += NextUpgrade.ProcChance;                    //increase proc chance
        this.DebuffDuration += NextUpgrade.DebuffDuration;      //increase debuff duration
        this.range.transform.localScale += new Vector3(rangeIncrease, rangeIncrease, 0);
        Level++;                                                //increase upgrade level
        GameManager.Instance.UpdateUpgradeTip();                //update tooltip to match
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Monster")
        {
            monsters.Enqueue(other.GetComponent<Monster>());    //add a monster to the Q
        }     
        
    }

    public abstract Debuff GetDebuff(); //return a debuff od a specfic type MUST be inherited as abstract

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Monster")
        {
            target = null;
        }
    }
}
