﻿using UnityEngine;
using TeamUtility.IO;

namespace Assets.Scripts.Arrows
{
    public class GhostArrow : ArrowProperty
    {
        public override void Init()
        {
            base.Init();
            GetComponent<Collider>().isTrigger = true;
        }

        public override void Effect(PlayerID hitPlayer) { }
    } 
}
