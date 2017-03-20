using UnityEngine;
using System;
using System.ComponentModel;

public class Switch : Interactive {

    [Tooltip("Object to switch between enable/disable state")]
    //public GameObject ActionObject;
    public GameObject ActionObject;

    //[ReadOnly(true)]
    //[Browsable(false)]
    [Obsolete("This property has been deprecated and should no longer be used.", false)]
    public new bool useGravity
    {
        get
        {
            return useGravity;
        }

        set
        {
            useGravity = value;
        }
    }

    // Updates the behaviors of the element's rigidbody and colliders
    protected override void UpdateBehaviors () {

		// Ensure physics control the rigidbody
		rigidbody.isKinematic = true;
    }
}
