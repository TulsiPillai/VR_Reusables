using UnityEngine;
using System.Collections;

public class VirtualSwitch : MonoBehaviour
{

    // Enumerate states of virtual hand interactions
    public enum VirtualSwitchState
    {
        Open,
        Switching,
        Switched
    };

    // Inspector parameters
    //[Tooltip("The tracking device used for tracking the real hand.")]
    //public Tracker tracker;

    [Tooltip("The interactive used to represent the virtual hand.")]
    public Affect hand;

    [Tooltip("The button required to be pressed to grab objects.")]
    public Button button;

    // Private interaction variables
    VirtualSwitchState state;

    // Object to switch state
    private object switchobject;

    // Called at the end of the program initialization
    void Start()
    {

        // Set initial state to open
        state = VirtualSwitchState.Open;

        // Ensure hand interactive is properly configured
        hand.type = AffectType.Virtual;
    }

    // FixedUpdate is not called every graphical frame but rather every physics frame
    void FixedUpdate()
    {

        // If state is open
        if (state == VirtualSwitchState.Open)
        {

            // If the hand is touching something
            if (hand.triggerOngoing)
            {
                // Change state to touching
                state = VirtualSwitchState.Switching;
            }

            // Process current open state
            else
            {

                // Nothing to do for open
            }
        }

        // If state is switching
        else if (state == VirtualSwitchState.Switching)
        {

            // If the hand is not touching something
            if (!hand.triggerOngoing)
            {
                // Change state to open
                state = VirtualSwitchState.Open;
            }

            // If the hand is touching something and the button is pressed
            else if (hand.triggerOngoing && button.GetPress())
            {
                var target = hand.ongoingTriggers[0];
                var switchObject = target.gameObject.GetComponent("Switch") as Switch;
                switchObject.ActionObject.SetActive(!switchObject.ActionObject.activeSelf);
                state = VirtualSwitchState.Switched;
            }
        }

        // if the state is switched
        else if (state == VirtualSwitchState.Switched)
        {
            // If Button get release the switch should change
            if (hand.triggerOngoing && !button.GetPress())
            {
                state = VirtualSwitchState.Open;
            }
        }
    }
}
