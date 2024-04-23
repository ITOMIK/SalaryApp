
using Microsoft.Maui.Storage;
using SalaryApp.Models;
using System.Collections.Generic;
namespace SalaryApp
{


    public partial class MainPage : ContentPage
    {
        public string AttractorName = "Привлекатель";

        public string AttractorData = "0";

        public bool IsAttractorFirma = false;

        public string firmaData = "0";

        public string FilePath = "";

        public List<Entry> EntryList { get; set; }

        public List<Employee> Employees { get; set; }

        public MainPage()
        {
            InitializeComponent();

            attractorName.TextChanged += AttractorName_Changed;

            attractorData.TextChanged += AttractorData_Changed;

            FirmaCheckBox.CheckedChanged += IsAttractorFirma_Changed;

            FirmaData.TextChanged += FirmaData_Changed;

            Employees = new List<Employee>();

            EntryList = new List<Entry>();

            FilePath = "";

            BindingContext = this;

            if (FilePath.Length > 0) {
            ReadFromFile(FilePath);

            }
        }

        private void SaveData(object sender, EventArgs e)
        {
            if (FilePath.Length > 0)
            {
                WriteToFile(FilePath);
            }
            else
            {
                createNewFile();
            }
        }

        private void AttractorName_Changed(object sender, TextChangedEventArgs e)
        {
            AttractorName = e.NewTextValue;
            UpdateEmployeesList();
        }

        private void AttractorData_Changed(object sender, TextChangedEventArgs e)
        {
            AttractorData = e.NewTextValue;
            UpdateEmployeesList();
        }

        private void IsAttractorFirma_Changed(object sender, CheckedChangedEventArgs e)
        {
            attractorName.IsVisible = !e.Value;
            attractorData.IsVisible = !e.Value;
            IsAttractorFirma = e.Value;
            UpdateEmployeesList();
        }

        private void FirmaData_Changed(object sender, TextChangedEventArgs e)
        {
            firmaData = e.NewTextValue;
            UpdateEmployeesList();
        }

        private void Entry_Unfocused(object sender, TextChangedEventArgs e)
        {
            UpdateEmployeesList();
        }
        private void AddEntry(string name="", string data="")
        {
            var entry = new Entry
            {
                Placeholder = "%",
                Text = data,
                FontFamily = "Helvetica",
                FontSize = 22,
                MaxLength = 20,
                Keyboard = Keyboard.Numeric,
                WidthRequest = 40
            };
            entry.TextChanged += Entry_Unfocused;
            var nameEntry = new Entry
            {
                Placeholder = "Имя",
                FontFamily = "Helvetica",
                FontSize = 22,
                Text = name,
                MaxLength = 20,
                Keyboard = Keyboard.Default,
                WidthRequest = 200
            };
            nameEntry.TextChanged += Entry_Unfocused;
            var button = new Button
            {
                Text = "-",
                BackgroundColor = new Color(0),
                TextColor = new Color(1),
                WidthRequest = 40,
                
                HeightRequest = 40
            };

            button.Clicked += (sender, e) =>
            {
                var parentLayout = (sender as Button).Parent as StackLayout;
                if (parentLayout != null)
                {
                    EmploeesEntys.Children.Remove(parentLayout);

                    var entryToRemove = Employees.FirstOrDefault(emp => emp.Entry == entry);
                    if (entryToRemove != null)
                    {
                        Employees.Remove(entryToRemove);
                    }

                    UpdateEmployeesList();
                }
            };

            var entryStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { entry, nameEntry, button },
            };

            Employees.Add(new Employee(name: nameEntry, entry: entry));

            EmploeesEntys.Children.Insert(EmploeesEntys.Children.Count, entryStackLayout);

            UpdateEmployeesList();
        }

        private void UpdateEmployeesList()
        {
            employeesGrid.Children.Clear();

            foreach (var employee in Employees)
            {
                var label = new Label { Text = $"{employee.NameEntry.Text}: {employee.Entry.Text}%" };
                if (label.Text!=": %") { 
                employeesGrid.Children.Add(label);
                }
            }
            double totalPercentage = 0;
            try { 
             totalPercentage = Employees.Sum(emp => Convert.ToDouble(emp.Entry.Text));
            }
            catch {
                totalPercentage = 0;
            }
            totalPercentageLabel.Text = $"Общая сумма Работники: {totalPercentage}%";
            
            FirmaLabel.Text = $"Фирма процент: {firmaData}%";
            if (IsAttractorFirma)
            {
                AttractorLabel.IsVisible = false;
            }
            else {
                AttractorLabel.IsVisible = true;
                AttractorLabel.Text = $"{AttractorName} процент: {AttractorData}%";
            }
        }

        private void AddToList(object sender, EventArgs e)
        {
            AddEntry();
        }

    private void WriteToFile(string fileName)
        {
            try
            {
                // Путь к файлу в папке Documents на устройстве
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), fileName);

                // Создаем или открываем файл для записи
                using (StreamWriter writer = File.CreateText(filePath))
                {
                    // Записываем данные в файл
                    writer.WriteLine($"Attractor Name: {AttractorName}");
                    writer.WriteLine($"Attractor Data: {AttractorData}");
                    writer.WriteLine($"Is Attractor Firma: {IsAttractorFirma}");
                    writer.WriteLine($"Firma Data: {firmaData}");

                    writer.WriteLine("Employees:");
                    foreach (var employee in Employees)
                    {
                        writer.WriteLine($"{employee.NameEntry.Text}: {employee.Entry.Text}%");
                    }

                    // Закрываем файл
                    writer.Close();
                }

                // Показываем пользователю, что данные были успешно записаны
                DisplayAlert("Success", $"Data has been written to file successfully. {filePath}", "OK");
            }
            catch (Exception ex)
            {
                // Если возникла ошибка, сообщаем пользователю
                DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private void ReadFromFile(string fileName)
        {
            try
            {
                // Путь к файлу в папке Documents на устройстве
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), fileName);
                
                // Проверяем, существует ли файл
                if (File.Exists(filePath))
                {
                    // Открываем файл для чтения
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        // Читаем данные из файла
                        AttractorName = reader.ReadLine()?.Split(':').LastOrDefault()?.Trim();
                        AttractorData = reader.ReadLine()?.Split(':').LastOrDefault()?.Trim();
                        IsAttractorFirma = Convert.ToBoolean(reader.ReadLine()?.Split(':').LastOrDefault()?.Trim());
                        firmaData = reader.ReadLine()?.Split(':').LastOrDefault()?.Trim();
                        attractorName.Text = AttractorName;
                        attractorData.Text = AttractorData;
                        FirmaCheckBox.IsChecked = IsAttractorFirma;
                        FirmaData.Text = firmaData;

                        // Пропускаем строку с заголовком "Employees:"
                        reader.ReadLine();

                        // Читаем сотрудников из файла
                        Employees.Clear();
                        EmploeesEntys.Children.Clear();
                        string line;

                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] parts = line.Split(':');
                            string name = parts[0].Trim();
                            string entryValue = parts[1].Trim().TrimEnd('%');

                            Entry a = new Entry { Text = name };
                            Entry b = new Entry { Text = entryValue };
                            AddEntry(a.Text, b.Text);
                            // Добавляем сотрудника на основе данных из файла
                        }

                        // Закрываем файл
                        reader.Close();
                    }

                    // Показываем пользователю, что данные были успешно считаны
                    DisplayAlert("Success", $"Data has been read from file successfully. {filePath}", "OK");
                }
                else
                {
                    DisplayAlert("File Not Found", $"The file '{fileName}' does not exist.", "OK");
                }
            }
            catch (Exception ex)
            {
                // Если возникла ошибка, сообщаем пользователю
                DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async void OnButtonClick(object sender, EventArgs e)
        {
            // Вызываем метод для выбора файла и получаем его путь
            string filePath = await PickFile();
            
            if (!string.IsNullOrEmpty(filePath))
            {
                FilePath = filePath;
                _fileName.Text = filePath;
                ReadFromFile(filePath);
            }
        }

        private async Task<string> PickFile()
        {
            try
            {
                // Выбираем файл
                var result = await FilePicker.PickAsync();
                if (result != null)
                {
                    // Возвращаем путь выбранного файла
                    return result.FullPath;
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }

            // Если файл не выбран или произошла ошибка, возвращаем пустую строку
            return string.Empty;
        }


        private async void createNewFile()
        {
            string name = await DisplayPromptAsync("Название документа", "Введите название документа");
            try
            {
                // Путь к файлу в папке Documents на устройстве
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), name);

                // Создаем или открываем файл для записи
                using (StreamWriter writer = File.CreateText(filePath))
                {
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
            ReadFromFile(name);
            FilePath = name;
            _fileName.Text = FilePath;
            AddEntry();
            UpdateEmployeesList();
        }
        private async void CreateNewFile(object sender, EventArgs e)
        {
            createNewFile();
        }


    }

}