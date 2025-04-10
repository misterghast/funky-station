using Content.Shared.Paper;
using Robust.Shared.GameStates;

namespace Content.Shared._Funkystation.Paper;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed class PaperworkPacketComponent : Component
{
    [DataField, AutoNetworkedField]
    public List<PaperComponent> Papers = new();

    [DataField]
    public int CurrentlySelected;

    [DataField, AutoNetworkedField]
    public PaperComponent? SelectedPaper;
}
