using System;

/// <summary>
/// Summary description for ToggleTakeActionVisibility
/// </summary>
public class ToggleTakeActionVisibilityEventArgs : EventArgs
{
    public bool IsTakeActionVisible { get; set; }

    public ToggleTakeActionVisibilityEventArgs(bool isTakeActionVisible)
        : base()
    {
        this.IsTakeActionVisible = isTakeActionVisible;
    }
}
