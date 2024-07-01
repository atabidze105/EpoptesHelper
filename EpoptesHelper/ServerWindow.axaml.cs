using Avalonia;
using System.IO;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using System.Collections.Generic;
using System.Text;
using static EpoptesHelper.StaticData;

namespace EpoptesHelper;

public partial class ServerWindow : Window //Окно для установки серверной версии
{
    private string _ipAddress = ""; //поле для IP

    public ServerWindow()
    {
        InitializeComponent();
        image_back.Source = new Bitmap("assets/background.jpg");
        ListBoxInit();
    }
    
    private void ListBoxInit() //Передача данных листбоксу
    {
        lbox_users.ItemsSource = _Users.Select(x => new
        {
            x.Id,
            x.Name
        });
    }

    private void CommandExeqution(string command) //Метод для исполнения команд в терминале
    {
        Process process = new Process();
        process.StartInfo.FileName = "/bin/bash";
        process.StartInfo.Arguments = $"-c \"{command}\"";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.Start();
        process.WaitForExit();
        process.Close();
    }
    private string getIp() //Метод для получения IP
    {
        string output = string.Empty;
        try
        {
            Process process = new Process();
            process.StartInfo.FileName = "bash";
            process.StartInfo.Arguments = "-c \"hostname -I\""; 
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
            output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
        }
        catch (Exception ex)
        {
            output = $"Error: {ex.Message}";
        }
        return output.Trim();
    }


    private void OpenInfo(object? sender, Avalonia.Interactivity.RoutedEventArgs e) //Открытие окна со справочной информацией
    {
        InfoWindow infoWindow = new InfoWindow();
        infoWindow.Show();
        _previousWindow = this;
        this.Hide();
    }

    private void GetHostIp(object? sender, Avalonia.Interactivity.RoutedEventArgs e) //Получение IP с отображением его в окне 
    {
        _ipAddress = getIp();
        tblock_hostIp.Text = _ipAddress;
    }

    private void BackToMain(object? sender, Avalonia.Interactivity.RoutedEventArgs e) //Возвращение к гланому окну
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }

    private void ServerInstall(object? sender, Avalonia.Interactivity.RoutedEventArgs e) //Установка серверной версии
    {
        CommandExeqution("apt install -y epoptes");
    }
    private void DeleteServer(object? sender, Avalonia.Interactivity.RoutedEventArgs e) //Удаление серверной версии
    {
        CommandExeqution("apt remove -y epoptes");
        CommandExeqution("apt autoremove -y");
    }

    private void AddUser(object? sender, Avalonia.Interactivity.RoutedEventArgs e) //Добавление пользователя в группу
    {
        if (tbox_username.Text != "")
        {
            using (var users = File.AppendText("assets/Users.txt"))
            {
                users.WriteLine($"{tbox_username.Text}");
            }
            UsersInit();
            ListBoxInit();
            CommandExeqution($"gpasswd -a {tbox_username.Text} epoptes");
            tbox_username.Text = "";
        }
    }

    private void DeleteUser(object? sender, RoutedEventArgs e) //Удаление пользователя из группы
    {
        Button button = sender as Button;
        _Users.Clear();
        string[] UsersList = File.ReadAllLines("assets/Users.txt", Encoding.Default);
        List<string> redNewUsers = [];
        int j = 0;
        for (int i = 0; i < UsersList.Length; i++)
        {
            if (i != (int)button!.Tag!)
            {
                _Users.Add(new User(j, UsersList[i]));
                redNewUsers.Add(UsersList[i]);
                j++;
            }
            else
            {
                CommandExeqution($"gpasswd --delete {UsersList[i]} epoptes");
            }
        }
        File.WriteAllLines("assets/Users.txt", redNewUsers, Encoding.Default);
        ListBoxInit();
    }
}