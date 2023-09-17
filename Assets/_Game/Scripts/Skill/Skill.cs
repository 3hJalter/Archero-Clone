using UnityEngine;

public class Skill : HMonoBehaviour
{
    private SkillType skillType;

    public SkillLevel SkillLevel { get; set; }

    public SkillType SkillType => skillType;

    public void OnInit()
    {
        Debug.Log("Init Skill");
        SkillLevel = SkillLevel.One;
    }
    
    public void UpgradeSkill()
    {
        SkillLevel = SkillLevel switch
        {
            SkillLevel.One => SkillLevel.Two,
            SkillLevel.Two => SkillLevel.Three,
            _ => SkillLevel.Three
        };
    }
}