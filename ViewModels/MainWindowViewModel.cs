using ReactiveUI;

namespace SuperBasketBall.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public AdministratorViewModel AdministratorViewModel { get; set; } = new AdministratorViewModel();
    public ManagerViewModel ManagerViewModel { get; set; } = new ManagerViewModel();

    public void AdministratorView()
    {
        AdministratorViewModel.AdministratorViewVisible = true;
        RoleStackPanelIsVisible = false;
    }

    public void ManagerView()
    {
        ManagerViewModel.ManagerViewVisible = true;
        RoleStackPanelIsVisible = false;
    }

    private bool _roleStackPanelIsVisible = true;

    public bool RoleStackPanelIsVisible
    {
        get => _roleStackPanelIsVisible;
        set => this.RaiseAndSetIfChanged(ref _roleStackPanelIsVisible, value);
    }
}