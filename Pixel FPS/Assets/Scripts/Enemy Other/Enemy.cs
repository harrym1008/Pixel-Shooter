using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Target target;
    public EnemyMovement enemyMovement;

    [Header("Looking for player parameters")]
    public bool targetInSight = false;
    public bool currentlyInfighting = false;
    [SerializeField] float seeOffsetY;
    [SerializeField] float maxSeeDistance;
    [SerializeField] LayerMask seeingLayerMask;


    private void Start()
    {
        target = GetComponent<Target>();
        enemyMovement = GetComponent<EnemyMovement>();
    }


    public virtual void LookForPlayer(Transform player)
    {
        bool line = Physics.Linecast(transform.position + seeOffsetY * transform.up, player.position, seeingLayerMask);
        bool distance = Vector3.Distance(player.position, transform.position) <= maxSeeDistance;

        if (!line && distance)
        {
            ChangeTarget(player.GetComponent<Target>());
            StateToAttacking();
        }
    }


    public virtual void StateToWander() { }
    public virtual void StateToAttacking() { }
    public virtual void Die() { }
    public virtual void Hurt() { }
    public virtual void ChangeTarget(Target target) { }
}
