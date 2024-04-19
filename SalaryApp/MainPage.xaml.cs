namespace SalaryApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public List<string> Items { get; set; }

        public List<Entry> EntryList { get; set; }

        public MainPage()
        {
            InitializeComponent();

            // Пример заполнения коллекции элементов
            Items = new List<string> { "Item 1", "Item 2", "Item 3", "Item 4" };


            EntryList = new List<Entry>();
            // Установка контекста данных
            BindingContext = this;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }


        // Метод для добавления нового поля ввода
        private void AddEntry()
        {
            var entry = new Entry
            {
                Placeholder = "Введите проценты",
                FontFamily = "Helvetica",
                FontSize = 22,
                MaxLength = 20,
                Keyboard = Keyboard.Numeric
            };

            var button = new Button
            {
                Text = "-",
                BackgroundColor = new Color(0), // Устанавливаем красный цвет для кнопки "-"
                TextColor = new Color(1),
                WidthRequest = 40,
                HeightRequest = 40
            };

            // Обработчик нажатия кнопки "-"
            button.Clicked += (sender, e) =>
            {
                // Находим родительский StackLayout
                var parentLayout = (sender as Button).Parent as StackLayout;
                if (parentLayout != null)
                {
                    // Удаляем родительский StackLayout, который содержит как поле ввода, так и кнопку "-"
                    stackLayout.Children.Remove(parentLayout);
                    // Удаляем Entry из списка EntryList
                    var entryToRemove = EntryList.FirstOrDefault(ent => ent == entry);
                    if (entryToRemove != null)
                    {
                        EntryList.Remove(entryToRemove);
                    }
                }
            };

            var entryStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { entry, button }
            };

            EntryList.Add(entry);
            stackLayout.Children.Insert(stackLayout.Children.Count - 1, entryStackLayout);
        }

        // Обработчик нажатия на кнопку "+"
        private void AddToList(object sender, EventArgs e)
        {
            AddEntry();
        }

    }

}
