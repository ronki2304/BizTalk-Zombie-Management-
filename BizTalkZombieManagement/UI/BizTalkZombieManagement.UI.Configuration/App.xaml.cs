using System.Windows;

namespace BizTalkZombieManagement.UI.Configuration
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// permet de lancer la 1er fenêtre
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //Remplacer par votre fenêtre
            View.Configurator window = new View.Configurator();
            ViewModel.ConfiguratorViewModel  vm = new ViewModel.ConfiguratorViewModel();
            window.DataContext = vm;
            window.Show();
        }
    }
}
