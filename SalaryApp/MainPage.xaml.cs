
using Microsoft.Maui.Storage;
using SalaryApp.Models;
using System.Collections.Generic;
using System.Linq;
namespace SalaryApp
{


    public partial class MainPage : ContentPage
    {
        public string AttractorName = "Привлекатель";

        public string AttractorData = "0";

        public bool IsAttractorFirma = false;

        public string firmaData = "0";

        public string FilePath = "";

        public int number = 0;

        public string FirstChildData = "0";

        public bool isChecking = false;

        public static RedDiamondDrawable RedDiamond = new RedDiamondDrawable();


        public List<Entry> EntryList { get; set; }

        public List<Employee> Employees { get; set; }

        public MainPage()
        {
            InitializeComponent();

            attractorName.TextChanged += AttractorName_Changed;

            attractorData.TextChanged += AttractorData_Changed;

            FirmaCheckBox.CheckedChanged += IsAttractorFirma_Changed;

            FirmaData.TextChanged += FirmaData_Changed;
            suma.TextChanged += number_Changed;

            Employees = new List<Employee>();

            EntryList = new List<Entry>();

            FilePath = "";

            BindingContext = this;

            View.IsVisible = FilePath.Length > 0;
            SaveButton.IsVisible = View.IsVisible;

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

        private void number_Changed(object sender, TextChangedEventArgs e)
        {
            try { 
            number = Convert.ToInt32(e.NewTextValue);
            }
            catch {
                number = 0;
            }
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
                    UpdateEmployeeEntys();
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
            UpdateEmployeeEntys();
        }


        private void UpdateEmployeesList()
        {
            try
            {
                if (FilePath != null)
                {
                    View.IsVisible = FilePath.Length > 0;
                    SaveButton.IsVisible = View.IsVisible;
                }
            }
            catch
            {
                View.IsVisible = false;
            }

            double totalPercentage = 0;
            double sam = 0;

            try
            {
                sam = Convert.ToDouble(firmaData);
                if (!IsAttractorFirma)
                {
                    sam += Convert.ToDouble(AttractorData);
                }
            }
            catch
            {
                sam = 0;
            }

            employeesGrid.Children.Clear();

            bool validInput = true;
            foreach (var employee in Employees)
            {
                double employeePercentage = 0;
                if (!double.TryParse(employee.Entry.Text, out employeePercentage))
                {
                    validInput = false;
                    break;
                }

                totalPercentage += employeePercentage;
            }

            if (!validInput || totalPercentage != 100 && isChecking)
            {
                // Создаем метку с сообщением об ошибке
                var errorLabel = new Label
                {
                    Text = "Сумма процентов должна быть 100 процентов",
                    TextColor = Colors.Red,
                    FontSize = 18,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };

                // Создаем GraphicsView для рисования ромба
                var diamondView = new GraphicsView
                {
                    Drawable = RedDiamond,
                    WidthRequest = 100,
                    HeightRequest = 100,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };

                // Добавляем GraphicsView и метку в grid
                employeesGrid.Children.Add(diamondView);
                employeesGrid.Children.Add(errorLabel);
            }
            else
            {
                foreach (var employee in Employees)
                {
                    double num = 0;
                    try
                    {
                        num = Convert.ToDouble(employee.Entry.Text) * number * (100 - sam) / 10000;
                    }
                    catch
                    {
                        num = 0;
                    }

                    var label = new Label { Text = $"{employee.NameEntry.Text}: {num}" };

                    if (label.Text != ": %")
                    {
                        employeesGrid.Children.Add(label);
                    }
                }
            }

            double totalSum = 0;
            try
            {
                totalSum = Employees.Sum(emp => Convert.ToDouble(emp.Entry.Text) * number * (100 - sam) / 10000);
            }
            catch
            {
                totalSum = 0;
            }
            totalPercentageLabel.Text = $"Общая сумма Работники: {totalSum}";

            sum__.Text = "Сумма: " + number.ToString();
            double firmaNum = 0;
            try
            {
                firmaNum = Convert.ToDouble(firmaData) * number / 100;
            }
            catch
            {
                firmaNum = 0;
            }
            FirmaLabel.Text = $"Фирма: {firmaNum}";

            if (IsAttractorFirma)
            {
                AttractorLabel.IsVisible = false;
            }
            else
            {
                AttractorLabel.IsVisible = true;
                double attractorNum = 0;
                try
                {
                    attractorNum = Convert.ToDouble(AttractorData) * number / 100;
                }
                catch
                {
                    attractorNum = 0;
                }
                AttractorLabel.Text = $"{AttractorName}: {attractorNum}";
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
                    writer.WriteLine($"Number: {number}");

                    writer.WriteLine("Employees:");
                    foreach (var employee in Employees)
                    {
                        writer.WriteLine($"{employee.NameEntry.Text}: {employee.Entry.Text}%");
                    }

                    // Закрываем файл
                    writer.Close();
                }

                // Показываем пользователю, что данные были успешно записаны
                DisplayAlert("Успех!", $"Данные были успешно сохраненны по пути: {filePath}", "OK");
            }
            catch (Exception ex)
            {
                // Если возникла ошибка, сообщаем пользователю
                DisplayAlert("Ошибка", $"Подробнее: {ex.Message}", "OK");
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
                        number = Convert.ToInt32(reader.ReadLine()?.Split(':').LastOrDefault()?.Trim());
                        attractorName.Text = AttractorName;
                        attractorData.Text = AttractorData;
                        FirmaCheckBox.IsChecked = IsAttractorFirma;
                        FirmaData.Text = firmaData;
                        suma.Text = number.ToString() ;

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
                        UpdateEmployeeEntys();
                        // Закрываем файл
                        reader.Close();
                    }
                }
                else
                {
                    DisplayAlert("Ошибка", $"Файл '{fileName}' не получилось открыть", "OK");
                }
            }
            catch (Exception ex)
            {
                // Если возникла ошибка, сообщаем пользователю
                DisplayAlert("Ошибка", $"Подробности: {ex.Message}", "OK");
            }
        }

        private void UpdateEmployeeEntys()
        {
            if (Employees.Count == 1)
            {
                FirstChildData = Employees[0].Entry.Text;
                Employees[0].Entry.Text = "100";
                Employees[0].Entry.IsVisible = false;
                isChecking = false;
            }
            if (Employees.Count > 1)
            {
                Employees[0].Entry.Text = FirstChildData;
                Employees[0].Entry.IsVisible = true;
                isChecking = true;
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
                await DisplayAlert("Ошибка", $"Подробности: {ex.Message}", "OK");
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
                DisplayAlert("Ошибка", $"Подробности: {ex.Message}", "OK");
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