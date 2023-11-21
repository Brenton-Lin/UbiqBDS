using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zinnia.Action;

public class Flashlight : MonoBehaviour
{
    public BooleanAction buttonPress;
    public GameObject light;

    [SerializeField]
    public bool lit = false;

    [SerializeField]
    private bool click = false;
    bool last;
    private int clicked = 0;

    // Start is called before the first frame update
    void Start()
    {
        last = buttonPress.Value;
    }

    // Update is called once per frame
    void Update()
    {


/*        Debug.Log(last);
        if (buttonPress.Value != last)
        {
            Debug.Log("clicked fuck off");
            ClickClick();
        }*/

        

    }

    // trigger pressed then released, track by bool = true, bool = false
    public void ClickClick()
    {
        
        clicked++;
        if (clicked >= 2 && !lit)
        {
            LightUp();
            clicked = 0;
        }
        else if (clicked >= 2 && lit)
        {
            LightOff();
            clicked = 0;
        }
        last = buttonPress.Value;
    }

    public void LightUp()
    {
        lit = true;
        light.SetActive(true);
    }

    public void LightOff()
    {
        lit = false;
        light.SetActive(false);
    }


    // back button press

    // slideup/slidedown axis

    // press and hold Y button to switch grip

    // thoughts on haptic feedback/rumble?
}
