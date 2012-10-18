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
            WindowsServiceLogic logic = new WindowsServiceLogic();
            logic.OnStateChange += test;
            if (logic.ServiceFound)
            {
                label1.Content = logic.state;
            }
        }

        private void test (object o, ServiceWindowsEvent e)
        {
            Dispatcher.Invoke(
   System.Windows.Threading.DispatcherPriority.Normal,
   new Action(
     delegate()
     {
         label1.Content = e.NewStatus;
     }
 ));
        }
    }
}
