using Content.Shared.Storage;
using Content.Shared.Storage.EntitySystems;

namespace Content.Shared._Funkystation.Storage;

public sealed class LockedStorageSystem : EntitySystem
{
    [Dependency] private readonly SharedStorageSystem _storage = default!;


    public override void Initialize()
    {
        SubscribeLocalEvent<LockedStorageComponent, StorageInteractAttemptEvent>(OnLockedStorageTransfer);
    }

    public void OnLockedStorageTransfer(Entity<LockedStorageComponent> component, ref StorageInteractAttemptEvent args)
    {
        if (component.Comp.ContainerLocked)
        {
            args.Cancelled = true;
        }
    }

}
