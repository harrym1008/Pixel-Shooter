// This monobehaviour is here to debug or test stuff.
// Remove all code when a new thing is needed to be debugged or tested.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentDebug : MonoBehaviour
{
    public Enemy enemy;
    public Enemy target;

    private IEnumerator Start()
    {
        yield return Wait.Frame;
        enemy.StateToAttacking(target.transform);
    }


}
