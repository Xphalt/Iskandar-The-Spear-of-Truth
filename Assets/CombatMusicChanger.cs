using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class CombatMusicChanger : MonoBehaviour
{
    internal int currentlyPlayingPlaylist = 0;
    bool hasPlayedCombatMusic = false;

    PlayerSFXPlayer psp;

    internal float timeTillNextCheck = 0f;
    internal float changeMusicCooldown = 0f;

    public float musicRange;
    public List<EnemyBase> enemiesInRange = new List<EnemyBase>();

    private void Start()
    {
        psp = transform.parent.GetComponent<PlayerSFXPlayer>();
    }

    private void Update()
    {
        if (enemiesInRange.Count > 0)
        {
            if (0.5f <= timeTillNextCheck)
            {
                timeTillNextCheck = 0f;
                changeMusicCooldown = 0f;
                enemiesInRange = enemiesInRange.Where(item => item.gameObject.activeInHierarchy && item.curState != EnemyBase.EnemyStates.Patrolling).ToList();
            }
            timeTillNextCheck += Time.deltaTime;
        }
        else if (currentlyPlayingPlaylist == 1)
        {
            if (0.5f <= changeMusicCooldown)
            {
                changeMusicCooldown = 0f;
                timeTillNextCheck = 0f;
                if (enemiesInRange.Count == 0 && hasPlayedCombatMusic)
                {
                    psp.ChangeMusic(2); // Post Combat Music;
                }
            }
            changeMusicCooldown += Time.deltaTime;
        }

        foreach (Collider other in Physics.OverlapSphere(transform.position, musicRange))
        {
            if (other.TryGetComponent(out EnemyBase enemy))
            {
                if (!enemiesInRange.Contains(enemy) && enemy.curState != EnemyBase.EnemyStates.Patrolling)
                    enemiesInRange.Add(enemy);
            }
        }

        if (enemiesInRange.Count > 0 && currentlyPlayingPlaylist != 1)
        {
            psp.ChangeMusic(1); // Combat Music;
            hasPlayedCombatMusic = true;
        }
    }

}
