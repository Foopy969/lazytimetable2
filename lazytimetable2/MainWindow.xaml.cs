using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace lazytimetable2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string[] WEEK = { "", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
        public static List<string> TIME = new List<string> { "", "08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30", "15:00", "15:30", "16:00", "16:30", "17:00", "17:30", "18:00", "18:30", "19:00", "19:30", "20:00", "20:30", "21:00", "21:30" , "22:00" };
        public static Dictionary<string, string[]> data;
        TimeTableManager manager;

        public MainWindow()
        {
            InitializeComponent();

            for (int i = 0; i < 7; i++)
            {
                var label = new Label() { Content = WEEK[i], Style = TimeTable.FindResource("Week") as Style };
                Grid.SetColumn(label, i);
                TimeTable.Children.Add(label);
            }

            for (int i = 1; i < 30; i++)
            {
                var label = new Label() { Content = TIME[i], Style = TimeTable.FindResource("Time") as Style };
                Grid.SetRow(label, i);
                TimeTable.Children.Add(label);
            }

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    var border = new Border() 
                    { 
                        BorderThickness = new Thickness(0, 0, 1, 1), 
                        BorderBrush = new SolidColorBrush() 
                        {
                            Color = Color.FromRgb(128, 128, 128) 
                        } 
                    };
                    Grid.SetColumn(border, i);
                    Grid.SetRow(border, j);
                    TimeTable.Children.Add(border);
                }
            }

            using (StreamReader r = new StreamReader("data.json"))
            {
                string json = r.ReadToEnd();
                data = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(json);
            }

            manager = new TimeTableManager();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            int row = Grid.GetRow(sender as Button);

            Grid.SetRow(sender as Button, row + 2);

            var course = new ComboBox();
            var classno = new ComboBox() { IsEnabled = false };

            course.SelectionChanged += Course_SelectionChanged;
            classno.SelectionChanged += Class_SelectionChanged;

            data.Keys.ToList().ForEach(x => course.Items.Add(x));

            Grid.SetRow(course, row);
            Grid.SetRow(classno, row + 1);

            manager.Add(course, classno);

            ClassSelector.Children.Add(course);
            ClassSelector.Children.Add(classno);
        }

        private void Course_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = manager[sender as ComboBox].ClassSelector;
            combo.Items.Clear();
            foreach (string cl in data[e.AddedItems[0] as string].Where((x, i) => i % 4 == 1).Distinct())
            {
                combo.Items.Add(cl);
            }
            combo.IsEnabled = true;
            manager.Update(TimeTable);
        }

        private void Class_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            manager.Update(TimeTable);
        }
    }
}
