using Robust.Shared.GameStates;

namespace Content.Shared._Funkystation.Storage;

[RegisterComponent, NetworkedComponent]
public sealed partial class LockedStorageComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool ContainerLocked { get; set; }
}
