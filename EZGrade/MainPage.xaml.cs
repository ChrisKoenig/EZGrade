using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace EZGrade
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void ShowDataButton_Click(object sender, RoutedEventArgs e)
        {
            Calculate();
        }

        private void Calculate()
        {
            int problems;
            if (!Int32.TryParse(NumberOfQuestionsTextbox.Text, out problems))
            {
                ShowErrorMessage("Please enter only numbers");
                NumberOfQuestionsTextbox.Focus();
                return;
            }

            var grades = new List<Grade>();
            for (double i = 1; i <= problems; i++)
            {
                double result = (problems - i) / problems;
                double grade = Math.Round(result * 100, 0);
                grades.Add(new Grade((int)i, (int)grade));
            }
            ScoresListBox.ItemsSource = grades;
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK);
        }
    }
}