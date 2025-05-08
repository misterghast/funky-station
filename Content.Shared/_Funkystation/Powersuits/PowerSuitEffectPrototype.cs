using Robust.Shared.Prototypes;

namespace Content.Server._Funkystation.Powersuits;

[Prototype("powerSuitEffect")]
public sealed partial class PowerSuitEffectPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;


}
