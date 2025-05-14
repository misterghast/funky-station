using Content.Server.Armor;
using Content.Server.Chemistry.Components;
using Content.Shared.Chemistry;
using Content.Shared.Chemistry.Dispenser;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Nutrition.EntitySystems;
using Robust.Server.GameObjects;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Timing;

namespace Content.Server._Funkystation.Powersuits;

public sealed class PowerSuitSystem : EntitySystem
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
        SubscribeLocalEvent<PowerSuitComponent, MapInitEvent>(OnMapInit, before: new []{typeof(ItemSlotsSystem)});
    }

    /// <summary>
    /// Automatically generate storage slots for all NumSlots, and fill them with their initial modules.
    /// The actual spawning of entities happens in ItemSlotsSystem's MapInit.
    /// </summary>
    private void OnMapInit(EntityUid uid, PowerSuitComponent component, MapInitEvent args)
    {
        // Get list of pre-loaded containers
        List<string> preLoad = new List<string>();
        if (component.PackPrototypeId is not null
            && _prototypeManager.TryIndex(component.PackPrototypeId, out ReagentDispenserInventoryPrototype? packPrototype))
        {
            preLoad.AddRange(packPrototype.Inventory);
        }

        // Populate storage slots with base storage slot whitelist
        for (var i = 0; i < component.NumSlots; i++)
        {
            var storageSlotId = PowerSuitComponent.BaseStorageSlotId + i;
            ItemSlot storageComponent = new();
            storageComponent.Whitelist = component.StorageWhitelist;
            storageComponent.Swap = false;
            storageComponent.EjectOnBreak = true;

            // Check corresponding index in pre-loaded container (if exists) and set starting item
            if (i < preLoad.Count)
                storageComponent.StartingItem = preLoad[i];

            component.ModuleSlotIds.Add(storageSlotId);
            component.ModuleSlots.Add(storageComponent);
            component.ModuleSlots[i].Name = "Module Slot " + (i+1);
            _itemSlotsSystem.AddItemSlot(uid, component.ModuleSlotIds[i], component.ModuleSlots[i]);
        }
    }

    private void OnItemEjected(EntityUid uid, PowerSuitComponent comp, ItemSlotEjectAttemptEvent args)
    {
        if (args.Cancelled || !TryComp<PowerSuitModuleComponent>(args.Item, out var moduleComponent)
            || moduleComponent.OnAdd == null)
        {
            return;
        }

        RemoveComponents(uid, moduleComponent.OnAdd);
    }

    private void OnItemInserted(EntityUid uid, PowerSuitComponent comp, ItemSlotInsertAttemptEvent args)
    {
        if (args.Cancelled || !TryComp<PowerSuitModuleComponent>(args.Item, out var moduleComponent)
                           || moduleComponent.OnAdd == null)
        {
            return;
        }

        AddModuleComponents(uid, moduleComponent.OnAdd);
    }

    public void AddModuleComponents(EntityUid suit, ComponentRegistry reg)
    {
        foreach (var (key, comp) in reg)
        {
            var compType = comp.Component.GetType();
            if (HasComp(suit, compType))
                continue;

            var newComp = (Component) _serManager.CreateCopy(comp.Component, notNullableOverride: true);
            EntityManager.AddComponent(suit, newComp, true);
            if (newComp.NetSyncEnabled)
            {
                Dirty(suit, newComp);
            }
        }
    }

    private void RemoveComponents(EntityUid suit,
        ComponentRegistry reg)
    {
        foreach (var (key, comp) in reg)
        {
            RemComp(suit, comp.Component.GetType());
        }
    }
}
