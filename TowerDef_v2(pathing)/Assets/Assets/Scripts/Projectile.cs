using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    private Monster target; //target of projectile

    private Tower parent;

    private Element elementType;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        MoveToTarget();
	}

    public void Initialize(Tower parent)
    {
        this.target = parent.Target;
        this.parent = parent;
        this.elementType = parent.ElementType;
    }

    private void MoveToTarget()
    {
        if (target != null && target.IsActive) //if we have a target && is alive/spawned 
        {
            //move the projectile to monster 
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * parent.ProjectileSpeed);

            Vector2 dir = target.transform.position - transform.position;   //direction of travel

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;    //get the angle of projectile

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); 
        }
        else if (!target.IsActive)  //if monster dies or goes into portal remove projectile
        {
            GameManager.Instance.Pool.ReleaseObject(gameObject);    //remove the projectile from the game ready to be reused
        }
    }

    private void ApplyDebuff()
    {
        if (target.ElementType != elementType)
        {
            float roll = Random.Range(0, 100);  //chance for debuff to be applied

            if (roll <= parent.Proc)
            {
                target.AddDebuff(parent.GetDebuff());
            }
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Monster")
        {
            if (target.gameObject == other.gameObject)
            {
                target.TakeDamage(parent.Damage, elementType);
                Monster hitInfo = other.GetComponent<Monster>();
                GameManager.Instance.Pool.ReleaseObject(gameObject);

                ApplyDebuff();
            }
        }
    }


}
