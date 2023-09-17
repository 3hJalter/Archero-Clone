using System.Collections.Generic;

public class MoreDamageSkill : Skill
{
    public Dictionary<SkillLevel, float> DamageDic { get; } = new()
    {
        {SkillLevel.One, 1.2f},
        {SkillLevel.Two, 1.5f},
        {SkillLevel.Three, 2f},
    };
}