using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Kaloricka_kalkulacka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Calculating calculating;
        public MainWindow()
        {
            InitializeComponent();
            if (File.Exists($"Data/limit.txt"))
            {
                string[] limit = File.ReadAllLines($"Data/limit.txt");
                lbProteinLim.Content = limit[0];
                lbCarbohydratesLim.Content = limit[1];
                lbFatLim.Content = limit[2];
                lbSugarLim.Content = limit[3];                  
            }
            if (File.Exists($"Data/dailyamount.txt"))
            {
                DateTime dt = DateTime.Now;
                string[] data = File.ReadAllLines($"Data/dailyamount.txt");
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[0] == dt.ToString("MM.dd.yyyy"))
                    {
                        lbProtein.Content = data[1];
                        lbCarbohydrates.Content = data[2];
                        lbFat.Content = data[3];
                        lbSugar.Content = data[4];
                    }
                    else
                    {
                        File.Delete($"Data/dailyamount.txt");
                        File.Delete($"Data/dailyfood.txt");
                    }
                }
            }
            if (File.Exists($"Data/dailyfood.txt"))
            {
                string[] data = File.ReadAllLines($"Data/dailyfood.txt");
                for (int i = 0; i < data.Length; i++)
                {
                    string[] line = data[i].Split(' ');
                    Calculating e = new Calculating();
                    e.foodname = line[0];
                    e.amount = Convert.ToDouble(line[1]);
                    e._ID = Guid.Parse(line[2]);
                    Calculating.AllFood.Add(e);
                }
            }
            DateTime datet = DateTime.Now;
            if (File.Exists($"Data/time.txt"))
            {
                string[] time = File.ReadAllLines($"Data/time.txt");
                lbRow.Content = time[1];
            }
        
            calculating = new Calculating();
            calculating._ID = Guid.NewGuid();
            lv.ItemsSource = Calculating.AllFood;
            DataContext = calculating;
        }

        private void btNewLimit_Click(object sender, RoutedEventArgs e)
        {
            ChangeLimit chl = new ChangeLimit();
            chl.Show();
            this.Close();
        }

        private void btNewFood_Click(object sender, RoutedEventArgs e)
        {
            AddNewFood nf = new AddNewFood();
            nf.Show();
            this.Close();
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            string[] lines = File.ReadAllLines($"Data/food.txt");
            string[] food_name = new string[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                string[] spread = lines[i].Split(' ');
                food_name[i] = spread[0];
            }
            cb.Items.Clear();
            for (int i = 0; i < food_name.Length; i++)
            {
                cb.Items.Add(food_name[i]);
            }
        }
        public double protein = 0;
        public double carbohydrates = 0;
        public double fat = 0;
        public double sugar = 0;
        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            if (lbError.Visibility == Visibility.Visible)
            {
                MessageBox.Show("Jsou požadovaná povinná data!");
            }
            else
            {
                Calculating q = Calculating.AllFood.Find(t => t._ID == calculating._ID);
                int qIndex = Calculating.AllFood.IndexOf(q);
                if (q != null)
                {
                    Calculating.AllFood[qIndex] = Calculating.Copy(calculating);
                }
                else
                {
                    Calculating.AllFood.Add(Calculating.Copy(calculating));
                    Calculating.Clear(calculating);
                }
                lv.ItemsSource = null;
                lv.ItemsSource = Calculating.AllFood;
            }
            string[] array = new string[Calculating.AllFood.Count];
            Directory.CreateDirectory("Data");
            for (int i = 0; i < Calculating.AllFood.Count; i++)
            {
                array[i] = Calculating.AllFood[i].ToString();
            }
            File.WriteAllLines($"Data/dailyfood.txt", array);
            if (File.Exists($"Data/dailyfood.txt"))
            {
                string foodname;
                double amount;
                string[] data = File.ReadAllLines($"Data/dailyfood.txt");
                protein = carbohydrates = fat = sugar = 0;
                for (int i = 0; i < data.Length; i++) //projede to tolik krát kolik jídla za dnešek snědl
                {
                    string[] line = data[i].Split(' ');
                    foodname = line[0];
                    amount = Convert.ToDouble(line[1]);
                    double xprotein, xcarbohydrates, xsugar, xfat;
                    string xfoodname;
                    string[] foodinfo = File.ReadAllLines($"Data/food.txt");
                    for (int d = 0; d < foodinfo.Length; d++) // projíždí a kontroluje o jaké jídlo se jedná               
                    {
                        string[] lineinfo = foodinfo[d].Split(' ');
                        xfoodname = lineinfo[0];
                        xprotein = Convert.ToDouble(lineinfo[1]);
                        xcarbohydrates = Convert.ToDouble(lineinfo[2]);
                        xfat = Convert.ToDouble(lineinfo[3]);
                        xsugar = Convert.ToDouble(lineinfo[4]);
                        if (xfoodname == foodname)
                        {
                            protein += xprotein / 100 * amount;
                            carbohydrates += xcarbohydrates / 100 * amount;
                            fat += xfat / 100 * amount;
                            sugar += xsugar / 100 * amount;
                        }
                    }
                }
                lbProtein.Content = protein;
                lbCarbohydrates.Content = carbohydrates;
                lbFat.Content = fat;
                lbSugar.Content = sugar;
                DateTime dt = new DateTime();
                dt = DateTime.Now;
                string[] dailyinfo = new string[] {dt.ToString("MM.dd.yyyy"), protein.ToString(), carbohydrates.ToString(), sugar.ToString(), fat.ToString() };
                File.WriteAllLines($"Data/dailyamount.txt", dailyinfo);
                if (File.Exists($"Data/limit.txt"))
                {
                    string[] limit = File.ReadAllLines($"Data/limit.txt");
                    if ((protein >= Convert.ToDouble(limit[0]))&&(carbohydrates >= Convert.ToDouble(limit[1]))&&(fat >= Convert.ToDouble(limit[2]))&&(sugar >= Convert.ToDouble(limit[3])))
                    {
                        if (File.Exists($"Data/time.txt"))
                        {
                            string day;
                            string[] time = File.ReadAllLines($"Data/time.txt");
                            if (time[0] == Convert.ToString(dt.AddDays(-1)))
                            {
                                day = time[1];
                                int row = Convert.ToInt32(day);
                                row++;
                                day = Convert.ToString(row);
                                string[] xtime = new string[] { dt.ToString("MM.dd.yyyy"), day };
                                File.WriteAllLines($"Data/time.txt", xtime);
                                lbRow.Content = day;
                            }
                        }
                        else
                        {
                            string day = "1";
                            string[] time = new string[] { dt.ToString("MM.dd.yyyy"), day };
                            File.WriteAllLines($"Data/time.txt",time);
                            lbRow.Content = day;
                        }
                    }
                }               
            }
        }

        private void cb_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((calculating.foodname==null)||(cb.Text == "")||(calculating.foodname ==""))
            {
                lbError.Visibility = Visibility.Visible;
            }
            else
            {
                lbError.Visibility = Visibility.Hidden;
            }
        }
        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            Calculating toDelete = ((Button)sender).DataContext as Calculating;
            Calculating.AllFood.Remove(toDelete);
            lv.ItemsSource = null;
            lv.ItemsSource = Calculating.AllFood;
            string[] array = new string[Calculating.AllFood.Count];
            for (int i = 0; i < Calculating.AllFood.Count; i++)
            {
                array[i] = Calculating.AllFood[i].ToString();
            }
            File.WriteAllLines($"Data/dailyfood.txt", array);
            string foodname;
            double amount;
            string[] data = File.ReadAllLines($"Data/dailyfood.txt");
            protein = carbohydrates = fat = sugar = 0;
            for (int i = 0; i < data.Length; i++) //projede to tolik krát kolik jídla za dnešek snědl
            {
                string[] line = data[i].Split(' ');
                foodname = line[0];
                amount = Convert.ToDouble(line[1]);
                double xprotein, xcarbohydrates, xsugar, xfat;
                string xfoodname;
                string[] foodinfo = File.ReadAllLines($"Data/food.txt");
                for (int d = 0; d < foodinfo.Length; d++) // projíždí a kontroluje o jaké jídlo se jedná               
                {
                    string[] lineinfo = foodinfo[d].Split(' ');
                    xfoodname = lineinfo[0];
                    xprotein = Convert.ToDouble(lineinfo[1]);
                    xcarbohydrates = Convert.ToDouble(lineinfo[2]);
                    xfat = Convert.ToDouble(lineinfo[3]);
                    xsugar = Convert.ToDouble(lineinfo[4]);
                    if (xfoodname == foodname)
                    {
                        protein += xprotein / 100 * amount;
                        carbohydrates += xcarbohydrates / 100 * amount;
                        fat += xfat / 100 * amount;
                        sugar += xsugar / 100 * amount;
                    }
                }
            }
            lbProtein.Content = protein;
            lbCarbohydrates.Content = carbohydrates;
            lbFat.Content = fat;
            lbSugar.Content = sugar;
            DateTime dt = new DateTime();
            dt = DateTime.Now;
            string[] dailyinfo = new string[] { dt.ToString("MM.dd.yyyy"), protein.ToString(), carbohydrates.ToString(), sugar.ToString(), fat.ToString() };
            File.WriteAllLines($"Data/dailyamount.txt", dailyinfo);
        }
        private void btChange_Click(object sender, RoutedEventArgs e)
        {
            calculating.foodname = (((Button)sender).DataContext as Calculating).foodname;
            calculating.amount = (((Button)sender).DataContext as Calculating).amount;
            calculating._ID = (((Button)sender).DataContext as Calculating)._ID;
        }

        private void tbAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (calculating.amount >0)
            {                
                lbError.Visibility = Visibility.Hidden;
            }
            else
            {
                lbError.Visibility = Visibility.Visible;
            }
        }
    }
    class Calculating : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public static List<Calculating> AllFood { get; set; } = new List<Calculating>();
        private string _foodname;
        private double _amount;
        public Guid _ID { get; set; }
        public static Calculating Copy(Calculating p)
        {
            Calculating q = new Calculating()
            {
                amount = p.amount,
                foodname = p.foodname,
                _ID = Guid.NewGuid(),
            };
            return q;
        }
        public static void Clear(Calculating p)
        {
            p.foodname = string.Empty;
            p.amount = 0;
        }
        public string foodname
        {
            get => _foodname;
            set
            {
                _foodname = value;
                OnPropertyChanged("foodname");
                OnPropertyChanged("Status");
            }
        }
        public double amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged("amount");
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
            get => $"{foodname} {amount}";
        }
        public override string ToString()
        {
            return $"{foodname} {amount} {_ID}";
        }
    }
}
