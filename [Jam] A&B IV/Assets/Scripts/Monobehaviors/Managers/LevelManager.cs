using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static int currentWave = 0;

    public List<Waypoints> waypoints;

    public List<Wave> waves;

    private int lockers = 0;
    private bool VerifyLock()
        => lockers == 0;

    public void StartLevel()
    {
        StartCoroutine(_StartLevel());

        IEnumerator _StartLevel()
        {
            yield return null;
            foreach (var wave in waves) // I can have multiple waves
            {
                foreach (var packnPath in wave.packnPaths)
                {
                    foreach (var pack in packnPath.packs)
                        StartCoroutine(SpawnPack(pack, packnPath.pathNumber));
                }

                yield return new WaitForSeconds(.02f);
                while (!VerifyLock())
                {
                    yield return new WaitForSeconds(.2f);
                }
            }
        }

        IEnumerator SpawnPack(Pack pack, int pathIndex)
        {
            yield return null;

            lockers += 1;
            foreach (var group in pack.groups) // Each pack contain multiple groups
            {
                for (int i = 0; i < group.amount; i++)
                {
                    var position = waypoints[pathIndex].transform.position;

                    var enemy = Instantiate(group.enemy, position, Quaternion.identity).GetComponent<Enemy>();
                    enemy.StartFollow(waypoints[pathIndex]);

                    yield return new WaitForSeconds(group.timeBetweenSpawns);
                }

                yield return new WaitForSeconds(group.timeAfterFinish);
            }
            lockers -= 1;
        }
    }
}
