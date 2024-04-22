
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

            BindingContext = this;

            AddEntry();
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
        private void AddEntry()
        {
            var entry = new Entry
            {
                Placeholder = "%",
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
                    StackFirst.Children.Remove(parentLayout);

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

            StackFirst.Children.Insert(StackFirst.Children.Count, entryStackLayout);

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
            double totalPercentage = Employees.Sum(emp => Convert.ToDouble(emp.Entry.Text));
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

    }

}
