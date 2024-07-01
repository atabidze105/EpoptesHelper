using Avalonia.Controls;
using Avalonia.Media.Imaging;
using static EpoptesHelper.StaticData;

namespace EpoptesHelper
{
    public partial class MainWindow : Window //������� ���� ���������
    {
        public MainWindow()
        {
            InitializeComponent();
            image_back.Source = new Bitmap("assets/background.jpg");
        }

        private void OpenInfo(object? sender, Avalonia.Interactivity.RoutedEventArgs e) //�������� ���� �� ���������� �����������
        {
            InfoWindow infoWindow = new InfoWindow();
            infoWindow.Show();
            _previousWindow = this;
            this.Hide();
        }

        private void GetToInstall(object? sender, Avalonia.Interactivity.RoutedEventArgs e) //����������� � ���� � ����������
        {
            Button button = sender as Button;
            switch (button.Name)
            {
                case "btn_client": //� ���� � ���������� ���������� �������
                    {
                        ClientWindow clientWindow = new ClientWindow();
                        clientWindow.Show();
                        this.Close();
                    }
                    break;
                case "btn_server": //� ���� � ��������� ��������� �������
                    {
                        ServerWindow serverWindow = new ServerWindow();
                        serverWindow.Show();
                        this.Close();
                    }
                    break;
            }
        }
    }
}