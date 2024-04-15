using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SuperBasketBall.ViewModels;

namespace SuperBasketBall.Views;

public partial class ManagerView : UserControl
{
    public ManagerView()
    {
        InitializeComponent();
    }
    
    public ManagerViewModel ViewModel => (DataContext as ManagerViewModel)!;
    
    private void PlayerSearch_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (ViewModel.PlayersPreSearch is null)
        {
            ViewModel.PlayersPreSearch = ViewModel.Player;
        }

        if (PlayerSearch.Text is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(PlayerSearch.Text))
        {
            PlayerGrid.ItemsSource = ViewModel.PlayersPreSearch;
            return;
        }

        Filter();
    }

    private void Filter()
    {
        if (PlayerSearch.Text is null)
        {
            return;
        }
        else
        {
            if (PlayerFilter.SelectedIndex == 0)
            {
                var filtered = ViewModel.PlayersPreSearch.Where(
                    it => it.PlayerSurname.Contains(PlayerSearch.Text)
                ).ToList();
                filtered = filtered.OrderBy(id => id.Id).ToList();
                PlayerGrid.ItemsSource = filtered;
            }
            else if (PlayerFilter.SelectedIndex == 1)
            {
                var filtered = ViewModel.PlayersPreSearch
                    .Where(it => it.PositionName == PlayerFilter.SelectedItem).ToList();
                filtered = filtered.OrderBy(position => position.PositionName).ToList();
                PlayerGrid.ItemsSource = filtered;
            }
            else if (PlayerFilter.SelectedIndex == 2)
            {
                var filtered = ViewModel.PlayersPreSearch
                    .Where(it => it.PositionName == PlayerFilter.SelectedItem).ToList();
                filtered = filtered.OrderBy(position => position.PositionName).ToList();
                PlayerGrid.ItemsSource = filtered;
            }
        }
    }

    private void PlayerFilter_OnSelectionChanged(object? sender, SelectionChangedEventArgs e) => Filter();
}