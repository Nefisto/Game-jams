using System.Collections;
using UnityEngine;

public partial class Enemy
{
    public void StartFollow(Waypoints followPath = null)
    {
        pathToFollow = followPath 
            ?? pathToFollow 
            ?? throw new System.Exception("Enemy does not have defined a path");

        StartCoroutine(FollowPath());

        IEnumerator FollowPath()
        {
            yield return null; // UNITY ISSUES .... 

            foreach(var currentTarget in pathToFollow.GetAllNodes())
            {
                var dir = (currentTarget.position - transform.position).normalized;

                while ((transform.position - currentTarget.position).sqrMagnitude > .05f)
                {
                    Movement(dir);

                    yield return null;
                }
            }

            Die(true);
        }
    }
}