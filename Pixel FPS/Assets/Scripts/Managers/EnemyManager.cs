using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static Difficulty difficulty = Difficulty.Normal;


    Enemy[] enemiesInLevel;
    Transform player;

    public int lookTicksPerSecond = 10;
    float waitBetweenLookTicks;
    float timeToNextLookTick;


    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        if (lookTicksPerSecond != 0)
            waitBetweenLookTicks = 1f / lookTicksPerSecond;
        else
            waitBetweenLookTicks = Mathf.Infinity;

        enemiesInLevel = FindObjectsOfType<Enemy>();
    }

    private void Update()
    {
        timeToNextLookTick -= Time.deltaTime;

        if (timeToNextLookTick <= 0)
        {
            LookTick();
            timeToNextLookTick = waitBetweenLookTicks;
        }
    }

    public void LookTick()
    {
        foreach (Enemy enemy in enemiesInLevel)
        {
            //print(enemy.myTarget);

            if (!enemy.myTarget.isDead && !enemy.targetInSight)
            {
                //print("Running look tick");
                enemy.LookForPlayer(player);
            }
        }
    }



    public enum Difficulty
    {
        Low,
        Normal,
        High
    }
}
