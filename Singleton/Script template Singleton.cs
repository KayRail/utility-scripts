using _BINDINGS_NAMESPACE_;
using System;

public partial class _CLASS_ : _BASE_
{
    public override void _Ready()
    {
        SingletonHub.AddSingleton(this);
    }

    public override void _Process(double delta)
    {
    }

    public override void _ExitTree()
    {
        SingletonHub.RemoveSingleton<_CLASS_>();

        base._ExitTree();
    }
}
