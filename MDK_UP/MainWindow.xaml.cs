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
using Microsoft.EntityFrameworkCore;

namespace MDK_UP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationContext db = new ApplicationContext();
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        // при загрузке окна
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // гарантируем, что база данных создана
            db.Database.EnsureCreated();
            // загружаем данные из БД
            db.Staff.Load();
            // и устанавливаем данные в качестве контекста
            DataContext = db.Staff.Local.ToObservableCollection();
        }

        // добавление
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            UserWindow UserWindow = new UserWindow(new Staff());
            if (UserWindow.ShowDialog() == true)
            {
                Staff Staff = UserWindow.Staff;
                db.Staff.Add(Staff);
                db.SaveChanges();
            }
        }
        // редактирование
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            // получаем выделенный объект
            Staff? staff = usersList.SelectedItem as Staff;
            // если ни одного объекта не выделено, выходим
            if (staff is null) return;

            UserWindow UserWindow = new UserWindow(new Staff
            {
                Id = staff.Id,
                Surname = staff.Surname,
                Name = staff.Name,
                Patronymic = staff.Patronymic,
                Data_birth = staff.Data_birth,
                Telephone_number = staff.Telephone_number,
                Department= staff.Department
            });

            if (UserWindow.ShowDialog() == true)
            {
                // получаем измененный объект
                staff = db.Staff.Find(UserWindow.Staff.Id);
                if (staff != null)
                {
                    staff.Surname = UserWindow.Staff.Surname;
                    staff.Name = UserWindow.Staff.Name;
                    staff.Patronymic = UserWindow.Staff.Patronymic;
                    staff.Data_birth = UserWindow.Staff.Data_birth;
                    staff.Telephone_number = UserWindow.Staff.Telephone_number;
                    staff.Department = UserWindow.Staff.Department;
                    db.SaveChanges();
                    usersList.Items.Refresh();
                }
            }
        }
        // удаление
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // получаем выделенный объект
            Staff? Staff = usersList.SelectedItem as Staff;
            // если ни одного объекта не выделено, выходим
            if (Staff is null) return;
            db.Staff.Remove(Staff);
            db.SaveChanges();
        }
    }
}
