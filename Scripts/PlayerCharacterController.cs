using Godot;

public class PlayerCharacterController : KinematicBody
{
    private MoveLateralBasic lateralMoveController;    // ~~I will "definitely" have the time to turn this into an interface later
    private IJumper jumpController;
    //private IGravityApplier gravityController;

    private Vector3 velocity;

    public override void _Ready()
    {
        // ~~gotta love getting relative nodes from STRINGS
        // type mismatch with scene references leads to InvalidCastException. logged in a less obvious place than Unity, but fair.
        lateralMoveController = GetNode<MoveLateralBasic>(new NodePath("SCR_Move"));
        jumpController = GetNode<IJumper>(new NodePath("SCR_Jump"));
        //gravityController = GetNode<IGravityApplier>(new NodePath("SCR_Gravity"));
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        Vector3 newVelocity = velocity;
        // also handles sideways input...
        Vector3 velocityH = lateralMoveController.GetLateralMoveVector();
        newVelocity.x = velocityH.x;
        newVelocity.z = velocityH.z;

        if (IsOnFloor() && Input.IsActionPressed("jump"))
        {
            newVelocity.y = jumpController.GetJumpInitialVelocity();
        }

        newVelocity.y += jumpController.GetJumpBaseGravity() * delta;

        Utils.DebugPrintTimed(30, newVelocity);
        velocity = MoveAndSlide(newVelocity, Vector3.Up);
    }
}
