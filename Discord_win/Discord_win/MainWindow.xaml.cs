﻿using System;
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

namespace Discord_win {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            Begin();
        }
        public void Begin() {
            Program.mainWindow = this;
            Program.Initialize();
            this.MainFrame.Navigate(Program.loginPage);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e) {
            Program.mainPage.ChangeSize();
        }
    }
}