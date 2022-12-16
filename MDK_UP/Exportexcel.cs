using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Markup;

namespace MDK_UP
{
    public class Exportexcel
    {
        public void CreateExcelfile(ObservableCollection<Staff> data)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage staff = new ExcelPackage();
            ExcelWorksheet sheet = staff.Workbook.Worksheets.Add("Лист");
            string[] headers = {  "Индитификатор сотрудника", "Фамилия", "Имя", "Отчество", "Дата рождения", "Номер телефона", "Отдел" };
            for (int i = 1; i <= headers.Length; i++)
            {
                sheet.Cells[1, i].Value = headers[i - 1];
            }
            int currString = 2;
            foreach (var person in data)
            {
                sheet.Cells[currString, 1].Value = person.Id_staff;
                sheet.Cells[currString, 2].Value = person.Surname;
                sheet.Cells[currString, 3].Value = person.Name;
                sheet.Cells[currString, 5].Value = person.Patronymic;
                sheet.Cells[currString, 6].Value = person.Data_birth;
                sheet.Cells[currString, 7].Value = person.Telephone_number;
                sheet.Cells[currString, 8].Value = person.Department;
                currString += 1;
            }
                for (int i = 1; i <= headers.Length; i++)
            {
                    sheet.Column(i).AutoFit();
            }
                string DRpath = "Reports"; 
                if (Directory.Exists(DRpath) == false)
            {
                    Directory.CreateDirectory(DRpath);
            }
                string exportfile = "Report.xlsx";
                DRpath = System.IO.Path.Combine(DRpath, exportfile);
                if (File.Exists(DRpath))
                    File.Delete(DRpath);
                    FileStream objFileStrm = File.Create(DRpath);
                    objFileStrm.Close();
                    File.WriteAllBytes(DRpath, staff.GetAsByteArray());
                    staff.Dispose();
        
    

        }
    }
}
     
           

