using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using static EpoptesHelper.StaticData;

namespace EpoptesHelper;

public partial class InfoWindow : Window //���� �� ���������� �����������
{
    public InfoWindow()
    {
        InitializeComponent();
        image_back.Source = new Bitmap("assets/background.jpg");
    }

    private void InfoExit(object? sender, Avalonia.Interactivity.RoutedEventArgs e)//����� �� ���� �� ���������� �����������
    {
        _previousWindow.Show();
        this.Close();
    }
}