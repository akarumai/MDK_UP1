﻿using System;
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
using Newtonsoft.Json;
using System.IO;
using System.Collections;
using System.Data.Entity.Spatial;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;


namespace MDK_UP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationContext db = new ApplicationContext();
        private string fileName;
        private object dataGridView;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        // при загрузке окна
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            db.Database.EnsureCreated();

            db.Staff.Load();

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
        // удаление
        private void Delete_Click(object sender, RoutedEventArgs e)
        {

            Staff? Staff = usersList.SelectedItem as Staff;

            if (Staff is null) return;
            db.Staff.Remove(Staff);
            db.SaveChanges();
        }

        private void usersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void CreateExcelfile(object sender, RoutedEventArgs e)
        {
            Exportexcel ExcelExport = new Exportexcel();
            ExcelExport.CreateExcelfile(db.Staff.Local.ToObservableCollection());
        }
        private void CreateJsonfile(object sender, RoutedEventArgs e)
        {
            var Json = JsonConvert.SerializeObject(DataContext);
            string DRpath = "Reports";
            if (Directory.Exists(DRpath) == false)
            {
                Directory.CreateDirectory(DRpath);
            }
            string exportfile = "Report.json";
            DRpath = System.IO.Path.Combine(DRpath, exportfile);
            if (File.Exists(DRpath))
                File.Delete(DRpath);
            FileStream objFileStrm = File.Create(DRpath);
            objFileStrm.Close();
            File.WriteAllText(DRpath, Json.ToString());
        }

        private void info_click(object sender, RoutedEventArgs e)
        {
           
            Staff? user = usersList.SelectedItem as Staff;
           
            if (user is null) return;

            UserWindow UserWindow = new UserWindow(new Staff
            {
                Id_staff = user.Id_staff,
                Surname = user.Surname,
                Name = user.Name,
                Patronymic=user.Patronymic,
                Data_birth= user.Data_birth,
                Telephone_number=user.Telephone_number,
                Department = user.Department
            });

            if (UserWindow.ShowDialog() == true)
            {
                
                user = db.Staff.Find(UserWindow.Staff.Id);
                if (user != null)
                {
                    user.Id = UserWindow.Staff.Id;
                    user.Surname = UserWindow.Staff.Surname;
                    user.Name = UserWindow.Staff.Name;
                    user.Patronymic = UserWindow.Staff.Patronymic;
                    user.Data_birth = UserWindow.Staff.Data_birth;
                    user.Telephone_number = UserWindow.Staff.Telephone_number;
                    db.SaveChanges();
                    usersList.Items.Refresh();
                }
            }
        }
    }
}



