using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisHelper
{
    public float Value
    {
        get;
        private set;
    }

    public bool IsDown
    {
        get;
        private set;
    }

    private string axisName = null;
    private bool downLastUpdate = false;
    public AxisHelper(string axisName)
    {
        this.axisName = axisName;
    }

    // Update is called once per frame
    public void UpdateAxis()
    {
        if (axisName == null)
            return;

        Value = Input.GetAxis(axisName);
        var val = Input.GetAxisRaw(axisName);
        var down = false;
        if(val != 0)
            down = true;
        else
            down = false;

        IsDown = !downLastUpdate && down;
        downLastUpdate = down;     
    }   
}
