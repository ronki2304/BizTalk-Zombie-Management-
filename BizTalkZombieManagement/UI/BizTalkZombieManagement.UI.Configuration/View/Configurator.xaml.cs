using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ServiceProcess;
using BizTalkZombieManagement.Entities.ConstantName;
using BizTalkZombieManagement.Business.Configuration;
using System.Windows.Forms;

namespace BizTalkZombieManagement.UI.Configuration.View
{
    /// <summary>
    /// Interaction logic for Configurator.xaml
    /// </summary>
    public partial class Configurator : Window
    {
        public Configurator()
        {
            InitializeComponent();
           
        }

        private void BrowseFolder(object sender, RoutedEventArgs e)
        {
            var dlg = new FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dlg.ShowDialog();
            txtFolder.Text = dlg.SelectedPath;
        }

   
    }
}
