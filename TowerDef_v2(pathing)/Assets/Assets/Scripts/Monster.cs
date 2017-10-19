using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    [SerializeField]
    private int hp = 15;

    [SerializeField]
    private int extraHealth = 5;

    [SerializeField]
    private int goldOnDeath = 2;

    [SerializeField]
    private float speed;

    private Stack<Node> path;

    private List<Debuff> debuffs = new List<Debuff>();

    private List<Debuff> debuffsToRemove = new List<Debuff>();

    private List<Debuff> newDebuffs = new List<Debuff>();

    [SerializeField]
    private Element elementType;

    private SpriteRenderer spriteRenderer;

    private int invulnerability = 2;

    private Animator myAnimator;

    [SerializeField]
    private Stat health;

    public bool Alive
    {
        get { return health.CurrentVal > 0; }
    }

    public Point GridPosition { get; set; }

    private Vector3 destination;

    public bool IsActive {
        get;

        set; }

    public float MaxSpeed { get; set; }

    public Element ElementType
    {
        get
        {
            return elementType;
        }

    }

    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }

    private void Update()
    {
        HandleDebuffs();
        Move();
    }

    public void Spawn(bool healthIncrease)
    {
        if(healthIncrease)
        {
            this.hp += extraHealth;
        }
        transform.position = LevelManager.Instance.SpawnPortal.transform.position;  //get spawn portals positions

        myAnimator = GetComponent<Animator>();  //gets attached animator.. Could this section be put into another "awake" function
        spriteRenderer = GetComponent<SpriteRenderer>();
        MaxSpeed = speed;
        this.health.Initialize();

        this.health.Bar.Reset();
        this.health.MaxVal = hp;
        this.health.CurrentVal = this.health.MaxVal;

        StartCoroutine(Scale(new Vector3(1f, 1f), new Vector3(2.2f, 2.2f),false));

        SetPath(LevelManager.Instance.Path);
    }

    public IEnumerator Scale(Vector3 from, Vector3 to, bool remove)  //Monster changing size on spawn/despawn
    {
        //IsActive = false;

        float progress = 0;

        while (progress <= 1)
        {
            transform.localScale = Vector3.Lerp(from, to, progress);

            progress += Time.deltaTime;

            yield return null;
        }

        transform.localScale = to;

        IsActive = true;
        if (remove)
        {
            Release();
        }
    }

    private void Move()
    {
        if (IsActive)   //allows time to grow animation
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, Speed * Time.deltaTime);    //move from current pos -> destination at speed*deltaTime

            if (transform.position == destination)
            {
                if (path != null && path.Count > 0) //still tiles to travel
                {
                    Animate(GridPosition, path.Peek().GridPosition);
                    GridPosition = path.Peek().GridPosition;
                    destination = path.Pop().WorldPosition;
                }
            }
        }

    }

    //sets intial path
    private void SetPath(Stack<Node> newPath)
    {
        if (newPath != null)
        {
            this.path = newPath;

            Animate(GridPosition, path.Peek().GridPosition);

            GridPosition = path.Peek().GridPosition;

            destination = path.Pop().WorldPosition;
                
        }
    }

    private void Animate(Point currentPos, Point newPos)
    {
        if (currentPos.Y > newPos.Y)    //if moving down
        {
            myAnimator.SetInteger("Horinzontal", 0);
            myAnimator.SetInteger("Vertical", 1);
        }
        else if (currentPos.Y < newPos.Y) //moving up
        {
            myAnimator.SetInteger("Horinzontal", 0);
            myAnimator.SetInteger("Vertical", -1);
        }
        else if (currentPos.Y == newPos.Y)   //moving across
        {
            if (currentPos.X > newPos.X)    //left
            {
                myAnimator.SetInteger("Horinzontal", -1);
                myAnimator.SetInteger("Vertical", 0);
            }
            else if (currentPos.X < newPos.X)   //right
            {
                myAnimator.SetInteger("Horinzontal", 1);
                myAnimator.SetInteger("Vertical", 0);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coin")
        {
            StartCoroutine(Scale(new Vector3(2.2f, 2.2f), new Vector3(1f, 1f), true));

            GameManager.Instance.Lives--;   //monster got to end so -lives
        }

        if (other.tag == "Tile")
        {
            spriteRenderer.sortingOrder = other.GetComponent<TileScript>().GridPosition.Y;  //so monster appears in front of objects behind it
        }
    }

    //sent to be reused, much efficeincy many wow
    public void Release()
    {
        debuffs.Clear();    //removes debuffs from previous life
        IsActive = false;   //stop moving until its done scale
        GridPosition = LevelManager.Instance.PortalSpawn;
        GameManager.Instance.Pool.ReleaseObject(gameObject);
        GameManager.Instance.RemoveMonster(this);   //remove monster from active monster list 
    }

    public void TakeDamage(float damage, Element dmgSource)
    {
        if (IsActive)
        {
            if (dmgSource == ElementType)
            {
                damage = damage / invulnerability;
                invulnerability++;
            }
            health.CurrentVal -= damage;

            if (health.CurrentVal <= 0)
            {
                SoundManager.Instance.PlaySFX("explosion");

                GameManager.Instance.Currency += goldOnDeath;

                myAnimator.SetTrigger("Die");   //trigger death animator

                IsActive = false;

                GetComponent<SpriteRenderer>().sortingOrder--;  //put the dead moster back 1 in the sorting order so other monsters can step on it
            }
        }

    }

    public void AddDebuff(Debuff debuff)
    {
        if (!debuffs.Exists(x => x.GetType() == debuff.GetType()))      //check if debuff is new or not only apply if it is
        {
            newDebuffs.Add(debuff);
        }

    }

    public void RemoveDebuff(Debuff debuff)
    {
        debuffsToRemove.Add(debuff);
    }

    private void HandleDebuffs()
    {
        if (newDebuffs.Count > 0)
        {
            debuffs.AddRange(newDebuffs);   //adds everything from newDebuffs to debuffs

            newDebuffs.Clear();
        }

        foreach (Debuff debuff in debuffsToRemove)
        {
            debuffs.Remove(debuff); 
        }

        debuffsToRemove.Clear();

        foreach (Debuff debuff in debuffs)
        {
            debuff.Update();
        }
    }
}
