using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATest : Ability
{
    public override void perform()
    {
        Debug.Log("Ability performed");
    }

    public override void cancel()
    {
        Debug.Log("Ability cancelled");
    }
}
