using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    [SerializeField] MusicMechanics aleksanderMusicScript;
    [SerializeField] Viktor viktorScript;
    [SerializeField] EnemyWeak enemyWeakScript;

    public LayerMask viktorMask;
    public LayerMask pWallMask;
    public LayerMask enemyWeakMask;
    public LayerMask obstacleMask;

    EnemyWeak[] enemyWeakList;
    string currEnemyWeak;

    private void Start()
    {
        StartCoroutine(FindTargetsWithDelay(0.1f));
        enemyWeakList = GameObject.FindObjectsOfType<EnemyWeak>();
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        Collider2D viktor = Physics2D.OverlapCircle(transform.position, viewRadius, viktorMask);
        Collider2D pWall = Physics2D.OverlapCircle(transform.position, viewRadius, pWallMask);
        Collider2D enemyWeak = Physics2D.OverlapCircle(transform.position, viewRadius, enemyWeakMask);

        if (viktor != null)
        {
            Transform target = viktor.transform;
            Vector3 dirToTarget = (transform.position - target.position).normalized;
            if (Vector3.Angle(-transform.up, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    if (aleksanderMusicScript.hasBuff3)
                    {
                        viktorScript.Buff();
                        aleksanderMusicScript.hasBuff3 = false;
                    }
                }
            }
        }
        if (pWall != null)
        {
            Transform target = pWall.transform;
            Vector3 dirToTarget = (transform.position - target.position).normalized;
            if (Vector3.Angle(-transform.up, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    if (aleksanderMusicScript.hasBuff2)
                    {
                        target.gameObject.SetActive(false);
                        aleksanderMusicScript.hasBuff2 = false;
                    }
                }
            }
        }
        if (enemyWeak != null)
        {
            Transform target = enemyWeak.transform;
            Vector3 dirToTarget = (transform.position - target.position).normalized;
            if (Vector3.Angle(-transform.up, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    if (aleksanderMusicScript.hasBuff1)
                    {
                        currEnemyWeak = target.gameObject.name;
                        for (var i = 0; i < enemyWeakList.Length; i++)
                        {
                            if (enemyWeakList[i].gameObject.name == currEnemyWeak)
                            {
                                enemyWeakList[i].Debuff();
                            }
                        }
                        aleksanderMusicScript.hasBuff1 = false;
                    }
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees -= transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }
}
