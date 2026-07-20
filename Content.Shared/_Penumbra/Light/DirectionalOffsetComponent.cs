using Robust.Shared.GameStates;
using Robust.Shared.Utility;
using System.Numerics;

namespace Content.Shared._Penumbra.Light;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(DirectionalOffsetSystem))]
public sealed partial class DirectionalOffsetComponent : Component
{
    [DataField, AutoNetworkedField]
    public Vector2 NorthOffset = Vector2.Zero;

    [DataField, AutoNetworkedField]
    public Vector2 EastOffset = Vector2.Zero;

    [DataField, AutoNetworkedField]
    public Vector2 SouthOffset = Vector2.Zero;

    [DataField, AutoNetworkedField]
    public Vector2 WestOffset = Vector2.Zero;
}
