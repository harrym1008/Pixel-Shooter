using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager managersManager;

    public static EnemyManager enemy;
    public static BloodManager blood;

    private void Awake()
    {
        if (managersManager != null)
        {
            Destroy(gameObject);
            return;
        }

        managersManager = this;

        gameObject.name = "*** MANAGER ***";
        transform.parent = null;

        DontDestroyOnLoad(gameObject);
        Assign();

    }


    void Assign()
    {
        enemy = GetComponent<EnemyManager>();
        blood = GetComponent<BloodManager>();
    }
}