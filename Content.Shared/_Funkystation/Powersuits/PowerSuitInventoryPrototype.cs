using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;

namespace Content.Server._Funkystation.Powersuits;

[Serializable, NetSerializable, Prototype("powerSuitInventory")]
public sealed class PowerSuitInventoryPrototype : IPrototype
{
    [DataField("inventory", customTypeSerializer: typeof(PrototypeIdListSerializer<EntityPrototype>))]
    public List<string> Inventory = new();

    [ViewVariables, IdDataField]
    public string ID { get; private set; } = default!;
}
