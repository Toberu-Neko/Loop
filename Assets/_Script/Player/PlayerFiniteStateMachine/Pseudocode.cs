using System;
using System.Collections;
using UnityEngine;

public class Pseudocode : MonoBehaviour
{
    bool isGrounded;
    bool input;
    bool noInput;

    bool skillInput;
    bool isInSkill;
    bool canUseSkill;

    private void Update()
    {
        if (isGrounded && noInput && !isInSkill)
        {
            // Idle update
        }
        else if (isGrounded && input && !isInSkill)
        {
            // Move update
        }
        else if (!isGrounded && !isInSkill)
        {
            // In air update
        }
        else if (isGrounded && skillInput && canUseSkill)
        {
            isInSkill = true;
            // Skill update
        }

        if (!isInSkill)
        {
            // Skill cooldown update
            canUseSkill = true;
        }
    }
}