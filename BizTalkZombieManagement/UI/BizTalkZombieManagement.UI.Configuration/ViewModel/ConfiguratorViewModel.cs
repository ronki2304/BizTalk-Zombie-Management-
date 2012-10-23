using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using BizTalkZombieManagement.Business.Configuration;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using BizTalkZombieManagement.Entities.CustomEnum;

namespace BizTalkZombieManagement.UI.Configuration.ViewModel
{
    public class ConfiguratorViewModel : BaseViewModel
    {
        #region Commandes
        public ICommand ClickBrowseFolder { get; set; }
        public ICommand saveConfigurationCommand { get; set; }
        #endregion

        public ConfiguratorViewModel()
        {
            ClickBrowseFolder = new RelayCommand(param => BrowseFolder(), param => true);
            WindowsServiceLogic logic = new WindowsServiceLogic();
            logic.OnStateChange += NewState;
            State = logic.state;
            WcfBindingType = new ObservableCollection<string>();
            this.initializeWcfBindingType();
        }

        private String _State;
        /// <summary>
        /// Show the current service state
        /// </summary>
        public String State
        {
            get { return _State; }
            set
            {
                if (_State != value)
                {
                    _State = value;
                    OnPropertyChanged("State");
                    //refresh the active command
                    OnPropertyChanged("IsActiveCommand");
                    OnPropertyChanged("FileSelected");
                    OnPropertyChanged("MSMQSelected");
                }
            }
        }

        /// <summary>
        /// Active the control is the servcie state is Stopped
        /// </summary>
        public Boolean IsActiveCommand
        {
            get { return String.Equals("Stopped", _State); }
        }

        #region File case
        private Boolean _FileSelected;

        public Boolean FileSelected
        {
            get { return _FileSelected && IsActiveCommand; }
            set
            {
                _FileSelected = value;
                OnPropertyChanged("FileSelected");
            }
        }

        private String _FolderPath;

        public String FolderPath
        {
            get { return _FolderPath; }
            set
            {
                _FolderPath = value;
                OnPropertyChanged("FolderPath");
            }
        }
        #endregion


        #region MSMQ case
        private Boolean _MSMQSelected;
        public Boolean MSMQSelected
        {
            get { return _MSMQSelected && IsActiveCommand; }
            set
            {
                _MSMQSelected = value;
                OnPropertyChanged("MSMQSelected");
            }
        }
        #endregion

        #region WCF case

        private Boolean _WcfSelected;
        public Boolean WcfSelected
        {
            get { return _WcfSelected && IsActiveCommand; }
            set
            {
                _WcfSelected = value;
                OnPropertyChanged("WcfSelected");
            }


        }

        public ObservableCollection<String> WcfBindingType { get; private set; }


        private void initializeWcfBindingType()
        {

            ObservableCollection<String> list = new ObservableCollection<string>();
            list.Add("Select binding");
            foreach (var EnumName in Enum.GetValues(typeof(WcfType)))
            {
                list.Add(EnumName.ToString());
            }

        }
        #endregion
        /// <summary>
        /// Getting the new service state
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void NewState(object o, ServiceWindowsEvent e)
        {
            if (State != e.NewStatus)
            {
                State = e.NewStatus;
            }
        }

        private void BrowseFolder()
        {
            var dlg = new FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                FolderPath = dlg.SelectedPath;
            }
        }

    }
}
