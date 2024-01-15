using System.Collections;
using System.Collections.Generic;
using Tilia.Locomotors.AxisMove;
using UnityEngine;
using Zinnia.Action;

public class SprintLogic : MonoBehaviour
{
    public FloatAction verticalAxis;
    public AxisMoveFacade locomotor;
    public float acceleration;
    public float sprintValue;
    public float walkValue;

    private void Update()
    {
        Sprint();
    }

    public bool checkSprintConditions()
    {
        if(verticalAxis.Value > 0)
        {
            return true;
        }

        return false;
    }

    public void Sprint()
    {
        if (checkSprintConditions())
        {
            locomotor.HorizontalAxisMultiplier = sprintValue;
            locomotor.VerticalAxisMultiplier = sprintValue;
        }
        else
        {
            locomotor.HorizontalAxisMultiplier = walkValue;
            locomotor.VerticalAxisMultiplier = walkValue;
        }
    }

    private void OnDisable()
    {
        locomotor.HorizontalAxisMultiplier = walkValue;
        locomotor.VerticalAxisMultiplier = walkValue;
    }
}
