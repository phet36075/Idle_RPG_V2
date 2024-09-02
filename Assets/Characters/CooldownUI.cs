using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class CooldownUI : MonoBehaviour
{
    public SkillManager skillManager;
    public Image[] skillIcons;
    public Image[] cooldownOverlays;
   public Text[] cooldownTexts;
   
    private void Update()
    {
        /*float cooldownTime = skillManager.GetSkillCooldownTime(0);
        float cooldownPercentage = skillManager.GetSkillCooldownPercentage(0);
        cooldownTexts[0].text = cooldownPercentage.ToString();
        
        cooldownOverlays[0].fillAmount = cooldownPercentage;*/
        
        for (int i = 0; i < skillIcons.Length; i++)
        {
            float cooldownPercentage = skillManager.GetSkillCooldownPercentage(i);
            float remainingCooldownTime = skillManager.GetRemainingCooldownTime(i);

            // อัพเดทเงา cooldown
            cooldownOverlays[i].fillAmount = cooldownPercentage;

            // อัพเดทเวลา cooldown
            if (cooldownPercentage > 0)
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(remainingCooldownTime);
                cooldownTexts[i].text = timeSpan.ToString(@"s\.f");
                cooldownTexts[i].enabled = true;
            }
            else
            {
                cooldownTexts[i].enabled = false;
            }
        }
    }
}
