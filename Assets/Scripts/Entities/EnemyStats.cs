using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EnemyStats : StatsInterface
{
    public static int EnemiesKilled = 0;

    public bool vulnerable = true;
    public bool reviveOnLoad = false;
    [HideInInspector] public float deathTimer = 0.0f;

    public float despawnTime = 4;
    public bool isDead = false;
    public bool IsDead() { return isDead; }


    internal AudioSource audioSource;
    public List<AudioClip> damageClips = new List<AudioClip>();

    internal AudioMixer mixer;

    /*______________________________Damage_Flash_Variables_______________________________*/
    private SkinnedMeshRenderer SkinMesh;
    private MeshRenderer Mesh;
    private Color Origin;
    public float FlashTime;
    /*___________________________________________________________________________________*/

    EntityDrop drops;

    const float takeDamageCooldown = 0.5f;
    float timeSinceDamageTaken;

    private void Start()
    {
        health = MAX_HEALTH;
        drops = GetComponent<EntityDrop>();
        SkinMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        if (SkinMesh) Origin = SkinMesh.material.color;
        else
        {
            Mesh = GetComponentInChildren<MeshRenderer>();
            Origin = Mesh.material.color;
        }
        FlashTime = 0.5f;

        mixer = Resources.Load("Master") as AudioMixer;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
    }

    private void Update()
    {
        timeSinceDamageTaken += Time.deltaTime;
        if(health <= 0)
        {
            isDead = true;
            //timer til gameobj disable
            deathTimer += Time.deltaTime;
            if (deathTimer >= despawnTime)
            {
                ++EnemiesKilled;
                if (drops) drops.SpawnLoot();
                gameObject.SetActive(false);

                FinalBurst explosion = transform.GetComponentInChildren<FinalBurst>(true);
                if (explosion) explosion.Burst();
            }

            if (GetComponent<VenomSpitter>() != null)  GetComponentInChildren<Animator>().Play("VenomSpitterDie");
        }
    }

    /*______________________________Damage_Flash_________________________________________*/
    private IEnumerator EDamageFlash()
    {
        if (SkinMesh) SkinMesh.material.color = Color.HSVToRGB(0.08f,1,1);
        else Mesh.material.color = Color.HSVToRGB(0.08f, 1, 1);
        yield return new WaitForSeconds(FlashTime);
        if (SkinMesh) SkinMesh.material.color = Origin;
        if (Mesh) Mesh.material.color = Origin;
    }
    /*___________________________________________________________________________________*/

    public override void TakeDamage(float amount, bool scriptedKill = false)
    {
        if(timeSinceDamageTaken >= takeDamageCooldown)
        {
            if (!vulnerable) return;
            health -= amount;
            PlayAudio();
            if (SkinMesh || Mesh) StartCoroutine(EDamageFlash());

            timeSinceDamageTaken = 0;
        }

        // anything that happens when taking damage happens 
        if (health <= 0)
        {
            isDead = true;
        }
    }

    public override void DealDamage(StatsInterface target, float amount, bool scriptedKill = false)
    {
        if (target)
        {
            target.TakeDamage(amount, scriptedKill);
        }
    }

    public void PlayAudio()
    {
        audioSource.clip = damageClips[Random.Range(0, damageClips.Count)];
        audioSource.Play();
    }

}
