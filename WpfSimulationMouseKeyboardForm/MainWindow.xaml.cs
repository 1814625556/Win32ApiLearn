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

namespace WpfSimulationMouseKeyboardForm
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MessageShow(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Name:{((Button)sender).Name},Content:{((Button)sender).Content}","SiomonMessageBox");
        }

        private void ShowWinForm(object sender, RoutedEventArgs e)
        {
            new Window1().ShowDialog();
        }
    }
}
