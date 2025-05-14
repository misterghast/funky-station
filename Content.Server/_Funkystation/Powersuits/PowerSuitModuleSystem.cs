using Content.Server.Armor;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Nutrition.EntitySystems;
using Robust.Server.GameObjects;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Timing;

namespace Content.Server._Funkystation.Powersuits;

public sealed class PowerSuitModuleSystem : EntitySystem
{
    [Dependency] private readonly UserInterfaceSystem _userInterfaceSystem = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly OpenableSystem _openable = default!;
    [Dependency] private readonly ItemSlotsSystem _itemSlotsSystem = default!;
    [Dependency] private readonly ArmorSystem _armorSystem = default!;
    [Dependency] private readonly IComponentFactory _compFactory = default!;
    [Dependency] private readonly ISerializationManager _serManager = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly INetManager _net = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<PowerSuitModuleComponent, MapInitEvent>(OnMapInit, after: new []{typeof(ItemSlotsSystem)});
        SubscribeLocalEvent<PowerSuitModuleComponent, ItemSlotEjectAttemptEvent>(OnItemEjected);
        SubscribeLocalEvent<PowerSuitModuleComponent, ItemSlotInsertAttemptEvent>(OnItemInserted);
    }

    private void OnMapInit(EntityUid uid, PowerSuitModuleComponent component, MapInitEvent args)
    {

        if (component.OnAdd == null)
        {
            return;
        }

        AddModuleComponents(uid, moduleComponent.OnAdd);
    }
}
