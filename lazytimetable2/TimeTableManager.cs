using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static lazytimetable2.MainWindow;

namespace lazytimetable2
{
    public class TimeTableManager
    {
        List<TimeTableElement> timeTableElements;
        List<TextBlock> storage;

        public TimeTableElement this[ComboBox i]
        {
            get => timeTableElements.First(x => x.Includes(i));
        }

        public TimeTableManager()
        {
            timeTableElements = new List<TimeTableElement>();
            storage = new List<TextBlock>();
        }

        public void Add(ComboBox courseSelector, ComboBox classSelector)
        {
            timeTableElements.Add(new TimeTableElement(courseSelector, classSelector));
        }

        public void Update(Grid grid)
        {
            foreach (var s in storage)
            {
                grid.Children.Remove(s);
            }

            storage = new List<TextBlock>();

            foreach (var e in timeTableElements)
            {
                if (e.CourseSelector.SelectedValue is null) continue;

                var field = data[e.CourseSelector.SelectedValue as string];

                if (e.ClassSelector.SelectedValue is null) continue;

                foreach (int i in field.Select((x, i) => new { x, i }).Where(x => x.x == e.ClassSelector.SelectedValue as string).Select(x => x.i))
                {
                    var textBlock = new TextBlock() { Text = e.CourseSelector.SelectedValue + "\n" + field[0] + "\n" + field[i + 3], Style = grid.FindResource("TimeTableElement") as Style };

                    int row = TIME.IndexOf(field[i + 2]), column = Convert.ToInt32(field[i + 1]);

                    //look for collision
                    bool collided = false;
                    foreach (var s in storage)
                    {
                        if (Grid.GetColumn(s) == column && Grid.GetRow(s) == row)
                        {
                            s.Style = grid.FindResource("CollidedElement") as Style;
                            s.Text = s.Text[..8] + "&\n" + e.CourseSelector.SelectedValue;
                            collided = true;
                        }
                    }
                    if (!collided)
                    {
                        Grid.SetColumn(textBlock, column);
                        Grid.SetRow(textBlock, row);

                        storage.Add(textBlock);
                        grid.Children.Add(textBlock);
                    }
                }
            }
        }
    }

    public class TimeTableElement
    {
        ComboBox courseSelector;
        ComboBox classSelector;

        public ComboBox CourseSelector => courseSelector;
        public ComboBox ClassSelector => classSelector;
        public bool Includes(ComboBox x) => x == courseSelector || x == classSelector;

        public TimeTableElement(ComboBox _courseSelector, ComboBox _classSelector)
        {
            courseSelector = _courseSelector;
            classSelector = _classSelector;
        }
    }
}
