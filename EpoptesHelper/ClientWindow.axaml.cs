using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Runtime;
using static EpoptesHelper.StaticData;
using System;
using System.IO;
using System.Diagnostics;
using Avalonia.Media.Imaging;

namespace EpoptesHelper;

public partial class ClientWindow : Window //Окно установки клиентской версии
{
    private string _hostIp = "";//Поле для IP хоста
    public ClientWindow()
    {
        InitializeComponent();
        image_back.Source = new Bitmap("assets/background.jpg");
    }

    private void CommandExeqution(string command) //Метод для выполнения команд в терминале
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

    private void OpenInfo(object? sender, Avalonia.Interactivity.RoutedEventArgs e) //Открытие окна со справочной информацией
    {
        InfoWindow infoWindow = new InfoWindow();
        infoWindow.Show();
        _previousWindow = this;
        this.Hide();
    }

    private bool IpCheck() //Проверка корректности введенного IP
    {
        string ip = mtbox_userIp.Text;
        string[] ipParts = ip.Split('.');
        for (int i = 0; i < ipParts.Length; i++)
        {
            int underlinesCount = 0;
            int temp;
            foreach (char c in ipParts[i])
            {
                underlinesCount = c == '_' ? underlinesCount + 1 : underlinesCount;
            }

            if (underlinesCount == 0)
            {
                temp = Convert.ToInt32(ipParts[i]);
                ipParts[i] = Convert.ToString(temp);
            }
            else if (underlinesCount == 1)
            {
                if (ipParts[i].IndexOf('_') == 1)
                {
                    return false;
                }
                else
                {
                    temp = Convert.ToInt32(ipParts[i].Replace("_", string.Empty));
                    ipParts[i] = Convert.ToString(temp);
                }
            }
            else if (underlinesCount == 2)
            {
                ipParts[i] = ipParts[i].Replace("_", string.Empty);
            }
            else if (underlinesCount == 3)
            {
                return false;
            }
        }

        foreach (string ipPart in ipParts)
        {
            _hostIp += ipPart + '.';
        }
        _hostIp = _hostIp.Remove(_hostIp.Length - 1);

        string pattern = @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";
        return Regex.IsMatch(_hostIp, pattern);
    }

    private void AddHost(object? sender, Avalonia.Interactivity.RoutedEventArgs e) //Добавление IP  файл "etc/hosts" и выпуск OpenSSL сертификата
    {
        CommandExeqution($"echo \"{_hostIp} server\" >> /etc/hosts");
        CommandExeqution("epoptes-client -c");
        mtbox_userIp.Clear();
        _hostIp = "";
    }

    private void IpInput(object? sender, Avalonia.Input.KeyEventArgs e) //Ввод IP
    {
        tblock_insertedIp.IsVisible = false;
        tblock_incorrectIp.IsVisible = false;
        _hostIp = "";
        if (IpCheck() == true)
        {
            tblock_insertedIp.Text = $"Введенный IP:\n{_hostIp}";
            tblock_insertedIp.IsVisible = true;
        }
        else
        {
            tblock_incorrectIp.IsVisible = true;
        }
    }

    private void BackToMain(object? sender, Avalonia.Interactivity.RoutedEventArgs e)//Переход к гланому окну
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }

    private void InstallClient(object? sender, Avalonia.Interactivity.RoutedEventArgs e) //Установка клиентской версии 
    {
        CommandExeqution("apt install -y epoptes-client");
    }
    private void ClientDelete(object? sender, Avalonia.Interactivity.RoutedEventArgs e) //Удаление клиентской версии 
    {
        CommandExeqution("apt remove -y epoptes-client");
        CommandExeqution("apt autoremove -y");
    }

    private void Reboot(object? sender, Avalonia.Interactivity.RoutedEventArgs e) //Перезагрузка
    {
        CommandExeqution("reboot");
    }
}