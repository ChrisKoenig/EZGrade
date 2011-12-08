using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Marketplace;

namespace EZGrade
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Loaded += (sender, e) =>
            {
                var li = new LicenseInformation();
                if (!li.IsTrial())
                {
                    MyAdControl.Visibility = Visibility.Collapsed;
                }
                var ints = new List<string>();
                for (int i = 1; i < 150; i++)
                {
                    ints.Add(i.ToString());
                }
                var ds = new LoopingSelectorDataSource<string>(ints);
                NumberOfQuestions.DataSource = ds;
                var initialValue = RetrieveInitialValue();
                ds.SelectedItem = initialValue.ToString();
                Calculate(initialValue);
            };
        }

        private int RetrieveInitialValue()
        {
            var settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Keys.Count > 0)
            {
                return Int32.Parse(settings["initial_value"].ToString());
            }
            else
            {
                settings["initial_value"] = 25;
                settings.Save();
                return 25;
            }
        }

        private void SetInitialValue(int value)
        {
            var settings = IsolatedStorageSettings.ApplicationSettings;
            settings["initial_value"] = value;
            settings.Save();
        }

        private void Calculate(int problems)
        {
            SetInitialValue(problems);
            NumberOfQuestionsTextBox.Text = problems.ToString();
            var grades = new List<Grade>();
            for (double i = 1; i <= problems; i++)
            {
                double result = (problems - i) / problems;
                double grade = Math.Round(result * 100, 0);
                grades.Add(new Grade(-(int)i, (int)grade));
            }
            ScoresListBox.ItemsSource = grades;
        }

        private void SetQuestionsButton_Click(object sender, EventArgs e)
        {
            SelectNumberOfQuestionsPopup.IsOpen = true;
        }

        private void SelectNumberOfQuestionsPopup_Closed(object sender, EventArgs e)
        {
            var item = NumberOfQuestions.DataSource.SelectedItem.ToString();
            int problems = Int32.Parse(item);
            Calculate(problems);
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (SelectNumberOfQuestionsPopup.IsOpen)
            {
                SelectNumberOfQuestionsPopup.IsOpen = false;
                e.Cancel = true;
            }
            else
            {
                base.OnBackKeyPress(e);
            }
        }
    }
}