using UnityEngine;

public class Hazard_Zack : MonoBehaviour
{//to be attached to any environmental hazard
    public enum HazardType
    {
        RootTrap,
        VenomSpit,
        FrozenFloor,
        HeavySnow,
        Antlion,
        Pitfall
    }

    //some are initially set to public to allow for balancing
    private float CurrentTimer;
    private float Interval;

    public int Movement_Debuff;
    public float FrostTicks = 6;
    private float CurrentFrostTicks;
    public bool Frosted;

    private PlayerMovement_Jerzy2 playerMovement;

    public float AreaInterval = 0.5f;
    public int DOT_DEBUFF;

    [SerializeField]
    HazardType hazard;
    public HazardType GetHazardType() { return hazard; }

    void Start()
    {
        
    }

    public void Interact()
    {
        switch (hazard)
        {
            case HazardType.RootTrap:
                break;
            case HazardType.VenomSpit:
                break;
            case HazardType.FrozenFloor:

                break;
            case HazardType.HeavySnow:
                //this is to be streamlined
                if (!Frosted)
                {
                    CurrentTimer += Time.deltaTime;
                    if (CurrentTimer > AreaInterval)
                    {
                        DOT_DEBUFF += 1;
                        CurrentTimer = 0;
                    }
                }
                if (DOT_DEBUFF == 10)
                {
                    Frosted = true;
                }

                if (Frosted)
                {
                    Debug.Log("Affected by frost blight");
                    CurrentTimer += Time.deltaTime;

                    if (CurrentTimer > AreaInterval)
                    {
                        CurrentFrostTicks--;
                        playerMovement.m_Speed -= Movement_Debuff;
                        playerMovement.dashCooldown += 0.6f;
                        CurrentTimer = 0;
                        Frosted = !(CurrentFrostTicks == 0);
                    }

                    if (CurrentFrostTicks == 0)
                    {
                        DOT_DEBUFF = 0;
                        Frosted = false;
                    }
                }
                break;
            case HazardType.Antlion:
                break;
            case HazardType.Pitfall:
                break;            
            default:
                print("Error: Hazard not recognised");
                break;
        }

    }

}
