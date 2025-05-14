using Content.Shared.Containers.ItemSlots;
using Content.Shared.Whitelist;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server._Funkystation.Powersuits;

[RegisterComponent]
public sealed partial class PowerSuitComponent : Component
{
    // Status effects to refresh while suit is worn.
    [DataField]
    public Dictionary<ProtoId<PowerSuitEffectPrototype>, string> Refresh = default!;

    // Inventory component that contains all the modules installed into this suit
    [DataField]
    public EntityUid InstalledModsInventory = default!;

    // Maximum TOTAL complexity value of installed modules
    [DataField]
    public int MaximumComplexity = 6;

    public int NumSlots = 15;

    [DataField("pack", customTypeSerializer:typeof(PrototypeIdSerializer<PowerSuitInventoryPrototype>))]
    [ViewVariables(VVAccess.ReadWrite)]
    public string? PackPrototypeId = default!;


    [DataField]
    [Access(typeof(ItemSlotsSystem), Other = AccessPermissions.ReadWriteExecute)]
    public EntityWhitelist? StorageWhitelist;

    /// <summary>
    /// List of storage slots that were created at MapInit.
    /// </summary>
    [DataField]
    public List<string> ModuleSlotIds = new List<string>();

    [DataField]
    public List<ItemSlot> ModuleSlots = new List<ItemSlot>();

    [DataField]
    public ItemSlot MaterialSlot = new();

    [DataField]
    public EntProtoId InitialMaterial = default!;

    public static string BaseStorageSlotId = "SuitModule-storageSlot";
}

