using Godot;
using System;

public class TouchPopupDisplayText : Node
{
    [Export] private string actualText;
    [Export] private NodePath markerLabelPath;
    private Label3D markerLabel;
    [Export] private NodePath actualTextLabelPath;
    private Label3D actualTextLabel;

    public override void _Ready()
    {
        markerLabel = GetNode<Label3D>(markerLabelPath);
        actualTextLabel = GetNode<Label3D>(actualTextLabelPath);
        actualTextLabel.Text = actualText;
    }

    public void OnPlayerEnteredShowTutorialText(Node body)
    {
        if (body.IsInGroup("Player"))
        {
            markerLabel.Visible = false;
            actualTextLabel.Visible = true;
        }
    }

    public void OnPlayerExitedHideTutorialText(Node body)
    {
        if (body.IsInGroup("Player"))
        {
            markerLabel.Visible = true;
            actualTextLabel.Visible = false;
        }
    }
}
