using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovingObjects
{
    public class Foreground : MovingObject 
    {   
        protected float foregroundSpeed = 10f;

        protected override float Speed
        {
            get { return foregroundSpeed * baseSpeed; }
        }
    }
}

