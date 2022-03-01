using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : AbstractSpell
{

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void init(GameObject elemRef, Vector3 target)
    {
        base.init(elemRef, target);

    }


    public override void Terminate()
    {
        throw new System.NotImplementedException();
    }

}
