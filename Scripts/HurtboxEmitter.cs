using Godot;

public class HurtboxEmitter : Area
{
    public override void _Ready()
    {
        Error err = Connect("body_entered", this, nameof(PropagateHurtboxHandling));
        //GD.Print(err);
    }

    private void PropagateHurtboxHandling(Node body)
    {
        if (body is IHurtable)
        {
            (body as IHurtable).TakeHurtboxCollisionEffect();
        }
    }
}
