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

namespace User_Interface
{
    /// <summary>
    /// Interaction logic for AddMallMenu.xaml
    /// </summary>
    public partial class AddMallMenu : UserControl, IAppView
    {
        private MainWindow CurrentMainWindow;
        public AddMallMenu()
        {
            InitializeComponent();
        }

        public void SetMainWindow(MainWindow currentWindow)
        {
            CurrentMainWindow = currentWindow;
        }
    }
}