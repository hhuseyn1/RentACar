using Admin.Models;
using ArrayToPdf;
using ClosedXML.Excel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Wpf.Ui.Common.Interfaces;

namespace Admin.ViewModels
{
    public partial class SettingsViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _appVersion = String.Empty;

        [ObservableProperty]
        private Wpf.Ui.Appearance.ThemeType _currentTheme = Wpf.Ui.Appearance.ThemeType.Unknown;

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom()
        {
        }

        private void InitializeViewModel()
        {
            CurrentTheme = Wpf.Ui.Appearance.Theme.GetAppTheme();
            AppVersion = $"Admin - {GetAssemblyVersion()}";

            _isInitialized = true;
        }

        private string GetAssemblyVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? String.Empty;
        }

        [RelayCommand]
        private void ExportAllData()
        {
            PdfFile<Car> receiverPdfCar = new(new List<Car>(Car.AllCars));
            PdfFile<User> receiverPdfUser = new(new List<User>(User.AllUsers));
            ExcelFile<Car> receiverExcelCar = new(new List<Car>(Car.AllCars));
            FileCreateInvoker invoker = new();
            invoker.SetCommand(new CreatePdfTableActionCommand<Car>(receiverPdfCar));
            invoker.CreateFile();
            invoker.SetCommand(new CreatePdfTableActionCommand<User>(receiverPdfUser));
            invoker.CreateFile();
            //invoker.SetCommand(new CreateExcelTableActionCommand<Car>(receiverExcel));
            //invoker.CreateFile();
        }

        public class ExcelFile<T>
        {
            private readonly List<T> _list;
            public string FileName => $"{typeof(T).Name}.xlsx";


            public ExcelFile(List<T> list)
            {
                _list = list;
            }

            public MemoryStream Create()
            {
                var wb = new XLWorkbook();
                var ds = new DataSet();

                ds.Tables.Add(GetTable());
                wb.Worksheets.Add(ds);

                var excelMemory = new MemoryStream();
                wb.SaveAs(excelMemory);

                return excelMemory;
            }


            private DataTable GetTable()
            {
                var table = new DataTable();

                var type = typeof(T);

                type.GetProperties()
                    .ToList()
                    .ForEach(x => table.Columns.Add(x.Name, x.PropertyType));


                _list.ForEach(x =>
                {
                    var values = type.GetProperties()
                                        .Select(properyInfo => properyInfo
                                        .GetValue(x, null))
                                        .ToArray();

                    table.Rows.Add(values);
                });

                return table;
            }
        }

        public class PdfFile<T>
        {
            private readonly List<T> _list;
            public string FileName => $"{typeof(T).Name}.pdf";


            public PdfFile(List<T> list)
            {
                _list = list;
            }

            public MemoryStream Create()
            {
                return GetTable().ToPdfStream();
            }


            private DataTable GetTable()
            {
                var table = new DataTable();

                var type = typeof(T);

                //type.GetProperties().ToList().ForEach(x => table.Columns.Add(x.Name, x.PropertyType));
                foreach (var item in type.GetProperties().ToList())
                {
                    if (!item.Name.StartsWith("All"))
                        table.Columns.Add(item.Name, item.PropertyType);
                }


                _list.ForEach(x =>
                {
                    var values = type.GetProperties().Select(properyInfo => properyInfo
                                        .GetValue(x, null)).Skip(1)
                                        .ToArray();

                    table.Rows.Add(values);
                });

                return table;
            }


        }

        public interface ITableActionCommand
        {
            void Execute();
        }


        public class CreateExcelTableActionCommand<T> : ITableActionCommand
        {
            private readonly ExcelFile<T> _excelFile;

            public CreateExcelTableActionCommand(ExcelFile<T> excelFile)
                => _excelFile = excelFile;

            public void Execute()
            {
                MemoryStream excelMemoryStream = _excelFile.Create();
                File.WriteAllBytes(_excelFile.FileName, excelMemoryStream.ToArray());
            }
        }

        public class CreatePdfTableActionCommand<T> : ITableActionCommand
        {
            private readonly PdfFile<T> _pdfFile;

            public CreatePdfTableActionCommand(PdfFile<T> pdfFile)
                => _pdfFile = pdfFile;

            public void Execute()
            {
                MemoryStream excelMemoryStream = _pdfFile.Create();
                File.WriteAllBytes(_pdfFile.FileName, excelMemoryStream.ToArray());
            }
        }

        // Invoker
        class FileCreateInvoker
        {
            private ITableActionCommand _tableActionCommand;
            private List<ITableActionCommand> tableActionCommands = new List<ITableActionCommand>();

            public void SetCommand(ITableActionCommand tableActionCommand)
            {
                _tableActionCommand = tableActionCommand;
            }

            public void AddCommand(ITableActionCommand tableActionCommand)
            {
                tableActionCommands.Add(tableActionCommand);
            }

            public void CreateFile()
            {
                _tableActionCommand.Execute();
            }

            public void CreateFiles()
            {
                throw new NotImplementedException();
                // ZipArchive
            }
        }


        [RelayCommand]
        private void OnChangeTheme(string parameter)
        {
            switch (parameter)
            {
                case "theme_light":
                    if (CurrentTheme == Wpf.Ui.Appearance.ThemeType.Light)
                        break;

                    Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light);
                    CurrentTheme = Wpf.Ui.Appearance.ThemeType.Light;

                    break;

                default:
                    if (CurrentTheme == Wpf.Ui.Appearance.ThemeType.Dark)
                        break;

                    Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark);
                    CurrentTheme = Wpf.Ui.Appearance.ThemeType.Dark;

                    break;
            }
        }
    }
}
