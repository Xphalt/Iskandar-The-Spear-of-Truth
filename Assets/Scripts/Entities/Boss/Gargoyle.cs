using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Gargoyle : BossStats
{
    public override bool LightAttack()
    {
        //this attack needs to be tied to an active raidius around the Gargoyle 
        //and if the player gets inside it, the Gargoyle will jab at the
        //player, inflicting damage and a knockback effect



        return true;
    }

    public override bool HeavyAttack()
    {
        //this charge is different to the Boar's, as it goes past the Player's position
        //if attack hits return true, if not return false
        //dont forget to only return when charge ends (be it missing or hitting the player)
        //For the Gargoyle this is the only attack that we care to return true or false back to the FSM,
        //since there'll be different outcomes
        return true;
    }

    public override bool FinalAttack()
    {
        return true;
    }
}
