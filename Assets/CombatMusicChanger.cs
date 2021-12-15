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

    private void Start()
    {
        psp = transform.parent.GetComponent<PlayerSFXPlayer>();
    }

    private void Update()
    {
        if (enemiesInRange.Count > 0)
        {
            if (5 <= timeTillNextCheck)
            {
                timeTillNextCheck = 0f;
                changeMusicCooldown = 0f;
                enemiesInRange = enemiesInRange.Where(item => item.gameObject != isActiveAndEnabled).ToList();
            }
            timeTillNextCheck += Time.deltaTime;
        }
        else if (currentlyPlayingPlaylist == 1)
        {
            if (5 <= changeMusicCooldown)
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
    }

    public List<EnemyBase> enemiesInRange = new List<EnemyBase>();

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<EnemyBase>())
        {
            if (!enemiesInRange.Contains(other.GetComponent<EnemyBase>()))
            {
                enemiesInRange.Add(other.GetComponent<EnemyBase>());
            }

            if (other.GetComponent<EnemyBase>().curState == EnemyBase.EnemyStates.Aggro && currentlyPlayingPlaylist != 1)
            {
                psp.ChangeMusic(1); // Combat Music;
                hasPlayedCombatMusic = true;
            }
        }
    }
}
