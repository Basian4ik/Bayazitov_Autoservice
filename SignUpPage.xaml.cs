using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bayazitov_Autoservice
{
    /// <summary>
    /// Логика взаимодействия для SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        private Service _currentService = new Service();
        public SignUpPage(Service SelectedService)
        {
            InitializeComponent();
            if (SelectedService !=null)
            {
                this._currentService = SelectedService;
            }
            DataContext = _currentService;
            var _currentClient = БаязитовАвтосервисEntities.GetContext().Client.ToList();
            ComboClient.ItemsSource= _currentClient;
        }
        private ClientService _currentClientService = new ClientService();
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (ComboClient.SelectedItem == null)
                errors.AppendLine("Укажите ФИО клиента");

            if (StartDate.Text == "")
                errors.AppendLine("Укажите дату услуги");

            if (TBStart.Text == "")
            {
                errors.AppendLine("Укажите время начала услуги");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            _currentClientService.ClientID = ComboClient.SelectedIndex + 1;
            _currentClientService.ServiceID = _currentService.ID;
            _currentClientService.StartTime = Convert.ToDateTime(StartDate.Text + " " + TBStart.Text);

            if (_currentClientService.ID==0)
            {
                БаязитовАвтосервисEntities.GetContext().ClientService.Add(_currentClientService);
            }


            try
            {
                БаязитовАвтосервисEntities.GetContext().SaveChanges();
                MessageBox.Show("информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        

        private void TBStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = TBStart.Text;

            string[] start = s.Split(':');
            if (start.Length != 2)
            {
                TBEnd.Text = "Неверный формат времени";
                return;
            }

            if (!int.TryParse(start[0], out int startHour) || !int.TryParse(start[1], out int startMinute))
            {
                TBEnd.Text = "Неверный формать времени";
                return;
            }
            if (start[1].Length != 2)
            {
                TBEnd.Text = "Неверный формат времени";
                return;
            }

            if (startHour < 0 || startHour > 23 || startMinute < 0 || startMinute > 59)
            {
                TBEnd.Text = "Неверный формат времени";
                return;
            }
            int totalMinutes = startHour * 60 + startMinute + _currentService.DurationInSeconds;
            int EndHour = totalMinutes / 60;
            int EndMin = totalMinutes % 60;

            EndHour = EndHour % 24;
            s = EndHour.ToString("D2") + ":" + EndMin.ToString("D2");
            TBEnd.Text = s;
        }
    }
}
