using Godot;
using System;

public class Player : KinematicBody
{
    private Spatial _cameraPivot;
    private Camera _cameraTps;

    private Spatial _head;
    private Camera _cameraFps;

    private ViewportContainer _fpsView;

    private bool _isTps = true;

    public override void _Ready()
    {
        _cameraPivot = GetNode<Spatial>("CameraPivot");
        _cameraTps = GetNode<Camera>("CameraPivot/SpringArm/Camera");

        _head = GetNode<Spatial>("Head");
        _cameraFps = GetNode<Camera>("Head/Camera");

        _fpsView = GetNode<ViewportContainer>("ViewportContainer");

        Input.SetMouseMode(Input.MouseMode.Captured);
        ToggleCameraMode();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion _motion)
        {
            // Tps
            var rotation = RotationDegrees;
            rotation.y -= _motion.Relative.x * 0.3f;
            RotationDegrees = rotation;

            var rotation2 = _cameraPivot.RotationDegrees;
            rotation2.x -= _motion.Relative.y * 0.3f;
            rotation2.x = Mathf.Clamp(rotation2.x, -50, 50);

            _cameraPivot.RotationDegrees = rotation2;

            // Fps
            // _head.RotateY(Mathf.Deg2Rad(-_motion.Relative.x * 0.3f));
            var xDelta = _motion.Relative.y * 0.3f;
            if (_cameraFps.RotationDegrees.x - xDelta > -50 && _cameraFps.RotationDegrees.x - xDelta < 50)
            {
                _cameraFps.RotateX(Mathf.Deg2Rad(-xDelta));
            }
        }
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("toggle_camera_mode"))
        {
            ToggleCameraMode();
        }

        if (Input.IsActionJustPressed("ui_cancel"))
        {
            Input.SetMouseMode(Input.MouseMode.Visible);
        }
    }

    private void ToggleCameraMode()
    {
        if (_isTps)
        {
            _isTps = false;
            _cameraFps.Current = true;
            _fpsView.Show();
        }
        else
        {
            _isTps = true;
            _cameraTps.Current = true;
            _fpsView.Hide();
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        var basis = Transform.basis;
        var dir = new Vector3();
        if (Input.IsActionPressed("move_forward"))
        {
            dir -= basis.z;
        }
        if (Input.IsActionPressed("move_backward"))
        {
            dir += basis.z;
        }
        if (Input.IsActionPressed("strafe_left"))
        {
            dir -= basis.x;
        }
        if (Input.IsActionPressed("strafe_right"))
        {
            dir += basis.x;
        }

        dir = dir.Normalized();

        dir.y = -9f;

        MoveAndSlide(dir * 10, Vector3.Down);
    }
}
