using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineLock : MonoBehaviour, ILock
{
    private enum MachineSkilltype
    {
        GravitySkill,
        EartquakeSkill,
        StopAllTurretsSkill,
    }
    [Tooltip("The skill this locked machine will unlock for the level")]
    [SerializeField]MachineSkilltype MachineSkill;

    public void GetUnlocked()
    {
        GrantMachineSkill();
    }
    void GrantMachineSkill()
    {
        //grant the relevant skill
        switch (MachineSkill)
        {
            case MachineSkilltype.GravitySkill: break;
            case MachineSkilltype.EartquakeSkill: break;
            case MachineSkilltype.StopAllTurretsSkill: break;
            default: break;
        }
    }
}
