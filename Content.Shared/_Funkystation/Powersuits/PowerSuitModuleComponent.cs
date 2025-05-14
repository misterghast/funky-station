using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Server._Funkystation.Powersuits;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class PowerSuitModuleComponent : Component
{
    /// <summary>
    ///     When attached, the module will ensure these components on the entity, and delete them on removal.
    /// </summary>
    [DataField]
    public ComponentRegistry? OnAdd;

    /// <summary>
    ///     When removed, the module will ensure these components on the entity, and delete them on insertion.
    /// </summary>
    [DataField]
    public ComponentRegistry? OnRemove;

    /// <summary>
    ///     Is this module working or not?
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool Enabled = true;

    /// <summary>
    ///     Can this module be enabled or disabled? Far more used for modules than organs.
    /// </summary>
    [DataField]
    public bool CanEnable = false;
}
