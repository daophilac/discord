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

namespace DictionaryManagement {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private Manager Manager { get; } = new Manager();
        public MainWindow() {
            InitializeComponent();
            string[] phrases = new string[2];
            phrases[0] = "dao phi lac";
            phrases[1] = "đào phi lạc";
            Manager.Load();
        }
    }
}
