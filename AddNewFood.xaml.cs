using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Windows.Media.Media3D;
using System.Text.RegularExpressions;


namespace Kaloricka_kalkulacka
{
    /// <summary>
    /// Interakční logika pro AddNewFood.xaml
    /// </summary>
    public partial class AddNewFood : Window
    {
        Food food;
        public AddNewFood()
        {
            InitializeComponent();
            food = new Food();
            DataContext = food;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string path = "Data/food.txt";
            if (lbError.Visibility == Visibility.Visible)
            {
                MessageBox.Show("Jsou požadovaná povinná data!");
            }
            else
            {
                string line = food.food + " " + food.proteinnw + " " + food.carbohydratesnw + " " + food.fatnw + " " + food.sugarnw;
                if (File.Exists(path))
                {
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(line);
                        sw.Close();
                    }                   
                }
                else
                {
                    Directory.CreateDirectory("Data");
                    using (StreamWriter sw = new StreamWriter(path))
                    {
                        sw.WriteLine(line);
                        sw.Close();
                    }
                }              
                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();
            }
        }

        private void tbNewFood_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((food.food == null)||(food.food == ""))
            {
                lbError.Visibility = Visibility.Visible;
            }
            else
            {
                lbError.Visibility = Visibility.Hidden;
            }
        }

        private void tbNewProtein_LostFocus(object sender, RoutedEventArgs e)
        {
            if (food.proteinnw <= 0)
            {
                lbError.Visibility = Visibility.Visible;
            }
            else
            {
                lbError.Visibility = Visibility.Hidden;
            }
        }

        private void tbNewCarbohydrates_LostFocus(object sender, RoutedEventArgs e)
        {
            if (food.carbohydratesnw <= 0)
            {
                lbError.Visibility = Visibility.Visible;
            }
            else
            {
                lbError.Visibility = Visibility.Hidden;
            }
        }

        private void tbNewSugar_LostFocus(object sender, RoutedEventArgs e)
        {
            if (food.sugarnw <= 0)
            {
                lbError.Visibility = Visibility.Visible;
            }
            else
            {
                lbError.Visibility = Visibility.Hidden;
            }
        }

        private void tbNewFat_LostFocus(object sender, RoutedEventArgs e)
        {
            if (food.fatnw <= 0)
            {
                lbError.Visibility = Visibility.Visible;
            }
            else
            {
                lbError.Visibility = Visibility.Hidden;
            }
        }

        private void tbNewProtein_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbNewCarbohydrates_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbNewSugar_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbNewFat_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
    class Food : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _food;
        private double _proteinnw;
        private double _carbohydratesnw;
        private double _sugarnw;
        private double _fatnw;
        public string food
        {
            get => _food;
            set
            {
                _food = value;
                OnPropertyChanged("food");
                OnPropertyChanged("Status");
            }
        }
        public double proteinnw
        {
            get => _proteinnw;
            set
            {
                _proteinnw = value;
                OnPropertyChanged("proteinnw");
                OnPropertyChanged("Status");
            }
        }
        public double carbohydratesnw
        {
            get => _carbohydratesnw;
            set
            {
                _carbohydratesnw = value;
                OnPropertyChanged("carbohydratesnw");
                OnPropertyChanged("Status");
            }
        }
        public double sugarnw
        {
            get => _sugarnw;
            set
            {
                _sugarnw = value;
                OnPropertyChanged("sugarnw");
                OnPropertyChanged("Status");
            }
        }
        public double fatnw
        {
            get => _fatnw;
            set
            {
                _fatnw = value;
                OnPropertyChanged("fatnw");
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
            get => $"{food} {proteinnw} {carbohydratesnw} {sugarnw} {fatnw}";
        }
        public override string ToString()
        {
            return $"{food} {proteinnw} {carbohydratesnw} {sugarnw} {fatnw}";
        }
    }
}
