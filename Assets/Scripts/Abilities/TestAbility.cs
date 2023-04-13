using UnityEngine;

public class TestAbility : Ability
{
	protected override void Perform()
	{
		Debug.Log("Ability performed on " +  transform.name);
	}

	protected override void Cancel()
	{
		Debug.Log("Ability canceled on " +  transform.name);
	}
}
