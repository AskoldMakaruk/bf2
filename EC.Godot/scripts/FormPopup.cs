using Godot;

public partial class FormPopup : Popup
{
    public override void _Ready()
    {
        base._Ready();
    }

    private void OnPopupHide()
    {
        QueueFree();
    }
}

public partial class FormField : Node2D
{
}