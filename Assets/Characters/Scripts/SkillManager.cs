using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public List<ISkill> skills = new List<ISkill>();
    
    // Start is called before the first frame update
    void Start()
    {
        skills.Add(GetComponent<Skill1>());
        skills.Add(GetComponent<Skill2>());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseSkill(0); // ใช้สกิลแรก
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
           UseSkill(1); // ใช้สกิลที่สอง
        }
    }

    private void UseSkill(int index)
    {
        if (index >= 0 && index < skills.Count)
        {
            skills[index].UseSkill();
        }
    }
}
