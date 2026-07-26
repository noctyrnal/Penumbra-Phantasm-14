using System.Numerics;
using Content.Shared._RMC14.Sprite;
using Robust.Shared.Network;

namespace Content.Shared._Penumbra.Light;

public sealed class DirectionalOffsetSystem : EntitySystem
{
    [Dependency] private readonly INetManager _net = default!;
    [Dependency] private readonly SharedRMCSpriteSystem _sprite = default!;

    private readonly HashSet<EntityUid> ToUpdate = new();
    public override void Initialize()
    {
        SubscribeLocalEvent<DirectionalOffsetComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<DirectionalOffsetComponent, MapInitEvent>(OnUpdate);
        SubscribeLocalEvent<DirectionalOffsetComponent, EntParentChangedMessage>(OnUpdate);
    }

    private void OnStartup(Entity<DirectionalOffsetComponent> ent, ref ComponentStartup args)
    {
        if (_net.IsClient)
            OffsetEntity(ent);
    }

    private void OnUpdate<T>(Entity<DirectionalOffsetComponent> ent, ref T args)
    {
        if (!TryComp(ent, out MetaDataComponent? metaData) ||
            metaData.EntityLifeStage < EntityLifeStage.MapInitialized)
        {
            return;
        }

        ToUpdate.Add(ent);

        if (_net.IsClient)
            return;

        if (TerminatingOrDeleted(ent))
            return;

        OffsetEntity(ent);
    }

    private void OffsetEntity(Entity<DirectionalOffsetComponent> ent)
    {
        var sprite = EnsureComp<SpriteSetRenderOrderComponent>(ent);
        var offset = EnsureComp<DirectionalOffsetComponent>(ent);

        switch (Transform(ent).LocalRotation.GetDir())
        {
            case Direction.South:
                _sprite.SetOffset(ent, offset.SouthOffset);
                break;
            case Direction.East:
                _sprite.SetOffset(ent, offset.EastOffset);
                break;
            case Direction.North:
                _sprite.SetOffset(ent, offset.NorthOffset);
                break;
            case Direction.West:
                _sprite.SetOffset(ent, offset.WestOffset);
                break;
        }

        Dirty(ent, sprite);
    }

}
