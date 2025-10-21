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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Service _currenService = new Service();
        public AddEditPage(Service SelectedService)
        {
            InitializeComponent();
            if (SelectedService!=null)
                _currenService=SelectedService;
            DataContext = _currenService;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currenService.Title))
                errors.AppendLine("Укажение название услуги");
            if (_currenService.Cost == 0)
                errors.AppendLine("Укажите стоимость услуги");
            if (_currenService.DiscountIt < 0 || _currenService.DiscountIt > 100)
                errors.AppendLine("Укажите скидку");
            if (_currenService.DurationInSeconds <= 0)
                errors.AppendLine("Укажите длительность услуги");
            if (_currenService.DurationInSeconds > 240 || _currenService.DurationInSeconds < 0)
                errors.AppendLine("Длительность не может быть больше 240 минут или меньше 0");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            var allServices = БаязитовАвтосервисEntities.GetContext().Service.ToList();
            allServices=allServices.Where(p=>p.Title == _currenService.Title).ToList();

            if (allServices.Count == 0)
            {
                if (_currenService.ID == 0)
                    БаязитовАвтосервисEntities.GetContext().Service.Add(_currenService);
                try
                {
                    БаязитовАвтосервисEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("Уже существует такая услуга");
            }
        }
    }
}
