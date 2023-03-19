using System;
using System.ComponentModel;
using System.Windows;
using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Kaloricka_kalkulacka
{
    /// <summary>
    /// Interakční logika pro ChangeLimit.xaml
    /// </summary>
    public partial class ChangeLimit : Window
    {
        Limits limit;
        public ChangeLimit()
        {
            InitializeComponent();
            limit = new Limits();
            DataContext = limit;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (lbError.Visibility == Visibility.Visible)
            {
                MessageBox.Show("Jsou požadovaná povinná data!");
            }
            else
            {
                string[] array = new string[4] {limit.protein.ToString(), limit.carbohydrates.ToString() , limit.sugar.ToString() , limit.fat.ToString()} ;
                Directory.CreateDirectory("Data");
                File.WriteAllLines($"Data/limit.txt", array);
                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();              
            }
        }

        private void tbChangeCarbohydrates_LostFocus(object sender, RoutedEventArgs e)
        {
            if (limit.carbohydrates == 0)
            {
                lbError.Visibility = Visibility.Visible;
            }
            else
            {
                lbError.Visibility = Visibility.Hidden;
            }
        }

        private void tbChangeProtein_LostFocus(object sender, RoutedEventArgs e)
        {
            if (limit.protein == 0)
            {
                lbError.Visibility = Visibility.Visible;
            }
            else
            {
                lbError.Visibility = Visibility.Hidden;
            }
        }

        private void tbChangeSugar_LostFocus(object sender, RoutedEventArgs e)
        {
            if (limit.sugar == 0)
            {
                lbError.Visibility = Visibility.Visible;
            }
            else
            {
                lbError.Visibility = Visibility.Hidden;
            }
        }

        private void tbChangeFat_LostFocus(object sender, RoutedEventArgs e)
        {
            if (limit.fat == 0)
            {
                lbError.Visibility = Visibility.Visible;
            }
            else
            {
                lbError.Visibility = Visibility.Hidden;
            }
        }

        private void tbChangeProtein_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbChangeCarbohydrates_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbChangeSugar_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbChangeFat_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
    class Limits : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private double _protein;
        private double _carbohydrates;
        private double _sugar;
        private double _fat;
        public double protein
        {
            get => _protein;
            set
            {
                _protein = value;
                OnPropertyChanged("protein");
                OnPropertyChanged("Status");
            }
        }
        public double carbohydrates
        {
            get => _carbohydrates;
            set
            {
                _carbohydrates = value;
                OnPropertyChanged("protein");
                OnPropertyChanged("Status");
            }
        }
        public double sugar
        {
            get => _sugar;
            set
            {
                _sugar = value;
                OnPropertyChanged("protein");
                OnPropertyChanged("Status");
            }
        }
        public double fat
        {
            get => _fat;
            set
            {
                _fat = value;
                OnPropertyChanged("protein");
                OnPropertyChanged("Status");
            }
        }
        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        public string Status
        {
            get => $"{protein} {carbohydrates} {sugar} {fat}";
        }
        public override string ToString()
        {
            return $"{protein} {carbohydrates} {sugar} {fat}";
        }
    }
}
