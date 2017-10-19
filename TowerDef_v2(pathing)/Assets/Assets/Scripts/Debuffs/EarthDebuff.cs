using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthDebuff : Debuff {


    public EarthDebuff(Monster target, float duration) : base(target, duration)
    {
        if (target != null)
        {
            target.Speed = 0;   //stunned
        }

    }


    public override void Remove()
    {
        if (target != null)
        {
            target.Speed = target.MaxSpeed; //return to maxspeed
            base.Remove();
        }

    }

}
