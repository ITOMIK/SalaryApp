
using SalaryApp.Models;
using System.Collections.Generic;
namespace SalaryApp
{


    public partial class MainPage : ContentPage
    {

        public List<Entry> EntryList { get; set; }

        public List<Employee> Employees { get; set; }

        public MainPage()
        {
            InitializeComponent();


            Employees = new List<Employee>();


            EntryList = new List<Entry>();

            BindingContext = this;

            AddEntry();
        }


        private void Entry_Unfocused(object sender, FocusEventArgs e)
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
            entry.Unfocused += Entry_Unfocused;
            var nameEntry = new Entry
            {
                Placeholder = "Имя",
                FontFamily = "Helvetica",
                FontSize = 22,
                MaxLength = 20,
                Keyboard = Keyboard.Default,
                WidthRequest = 200
            };
            nameEntry.Unfocused += Entry_Unfocused;
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
                employeesGrid.Children.Add(label);
            }

            double totalPercentage = Employees.Sum(emp => Convert.ToDouble(emp.Entry.Text));
            totalPercentageLabel.Text = $"Общая сумма процентов: {totalPercentage}%";
        }

        private void AddToList(object sender, EventArgs e)
        {
            AddEntry();
        }

    }

}
