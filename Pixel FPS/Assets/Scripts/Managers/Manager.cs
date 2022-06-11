using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager managersManager;

    public static EnemyManager enemy;
    public static BloodManager blood;
    public static PlayerRecoil playerRecoil;
    public static InputMaster controls;

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
        AssignMine();
    }

    private void Start()
    {
        AssignRest();
    }


    void AssignMine()
    {
        enemy = GetComponent<EnemyManager>();
        blood = GetComponent<BloodManager>();
    }

    void AssignRest()
    {
        playerRecoil = PlayerRecoil.playerRecoil;
        controls = Controls.controls;
    }
}