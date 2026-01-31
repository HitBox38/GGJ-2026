using UnityEngine;
using System;

public class GooseMask : MaskObject 
{
    public class GooseFriend : MaskEffect { }
    public class RavenCaller : MaskEffect
    {
        // Find the closest raven to the player

        // save the raven's original goose's parameneters

        // mimic the goose the raven is currently protecting

        // onDestroy, reset the raven's parameters to the original goose
    }

    public override void ApplyEffects()
    {
        // add the goose friend buff component
        AddBuff<GooseFriend>();
        // add the raven caller debuff component
        AddDebuff<RavenCaller>();
    }
}
