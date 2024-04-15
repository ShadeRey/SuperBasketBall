using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using DynamicData;
using MySqlConnector;
using ReactiveUI;
using SukiUI.Controls;
using SuperBasketBall.DataBase;
using SuperBasketBall.Models;

namespace SuperBasketBall.ViewModels;

public class ManagerViewModel: ViewModelBase
{
    public ManagerViewModel()
    {
        Player = GetPlayersFromDb();
    }
    
    private bool _managerViewVisible = false;
    
    public bool ManagerViewVisible
    {
        get => _managerViewVisible;
        set => this.RaiseAndSetIfChanged(ref _managerViewVisible, value);
    }
    
    private AvaloniaList<Player> GetPlayersFromDb()
    {
        AvaloniaList<Player> players = new AvaloniaList<Player>();

        using (MySqlConnection connection = new MySqlConnection(DataBaseConnectionString.ConnectionString))
        {
            try
            {
                connection.Open();
                string selectAllPlayers = """
                                         SELECT player.Id, PlayerSurname, Position, Weight, Height, BirthDate, StartGameDate, Team, PositionName, TeamName From Player
                                         join position on player.Position = position.Id
                                         join team on player.Employee = team.Id
                                         """;
                MySqlCommand cmd = new MySqlCommand(selectAllPlayers, connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Player playersItem = new Player();
                    if (!reader.IsDBNull(reader.GetOrdinal("Id")))
                    {
                        playersItem.Id = reader.GetInt32("Id");
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("PlayerSurname")))
                    {
                        playersItem.PlayerSurname = reader.GetString("PlayerSurname");
                    }
                    
                    if (!reader.IsDBNull(reader.GetOrdinal("Position")))
                    {
                        playersItem.Position = reader.GetInt32("Position");
                    }
                    
                    if (!reader.IsDBNull(reader.GetOrdinal("Weight")))
                    {
                        playersItem.Weight = reader.GetDouble("Weight");
                    }
                    
                    if (!reader.IsDBNull(reader.GetOrdinal("Height")))
                    {
                        playersItem.Height = reader.GetDouble("Height");
                    }
                    
                    if (!reader.IsDBNull(reader.GetOrdinal("BirthDate")))
                    {
                        playersItem.BirthDate = reader.GetDateTimeOffset("BirthDate");
                    }
                    
                    if (!reader.IsDBNull(reader.GetOrdinal("StartGameDate")))
                    {
                        playersItem.StartGameDate = reader.GetDateTimeOffset("StartGameDate");
                    }
                    
                    if (!reader.IsDBNull(reader.GetOrdinal("Team")))
                    {
                        playersItem.Team = reader.GetInt32("Team");
                    }
                    
                    if (!reader.IsDBNull(reader.GetOrdinal("PositionName")))
                    {
                        playersItem.PositionName = reader.GetString("PositionName");
                    }
                    
                    if (!reader.IsDBNull(reader.GetOrdinal("TeamName")))
                    {
                        playersItem.TeamName = reader.GetString("TeamName");
                    }

                    players.Add(playersItem);
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Ошибка подключения к БД: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        return players;
    }
    
    private AvaloniaList<Player> _player;

    public AvaloniaList<Player> Player
    {
        get => _player;
        set => this.RaiseAndSetIfChanged(ref _player, value);
    }
    
    private Player _playerSelectedItem;

    public Player PlayerSelectedItem {
        get => _playerSelectedItem;
        set => this.RaiseAndSetIfChanged(ref _playerSelectedItem, value);
    }
    
    public void OnDelete(Player player) {
        Player.Remove(player);
    }
    
    public void OnEdit(Player player) {
        Player.Replace(PlayerSelectedItem, player);
    }
    
    private AvaloniaList<Player> _playersPreSearch;

    public AvaloniaList<Player> PlayersPreSearch
    {
        get => _playersPreSearch;
        set => this.RaiseAndSetIfChanged(ref _playersPreSearch, value);
    }
    
    public void DeletePlayerFromDB()
    {
        if (PlayerSelectedItem is null)
        {
            return;
        }
        var db = new DataBaseDelete();
        int playerId = PlayerSelectedItem.Id;
        var delete = ReactiveCommand.Create<Player>((i) =>
        {
            db.DeleteData(
                "Player",
                playerId
            );
            OnDelete(i);
            SukiHost.CloseDialog();
        });

        SukiHost.ShowDialog(new StackPanel()
        {
            DataContext = PlayerSelectedItem,
            Children =
            {
                new TextBlock()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Classes = { "h2" },
                    Text = "Удалить?"
                },
                new Button()
                {
                    Content = "Да",
                    Classes = { "Primary" },
                    Command = delete,
                    Foreground = Brushes.White,
                    [!Button.CommandParameterProperty] = new Binding(".")
                },
                new Button()
                {
                    Content = "Закрыть",
                    Command = ReactiveCommand.Create(SukiHost.CloseDialog)
                }
            }
        }, allowBackgroundClose: true);
    }
}