using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using BizTalkZombieManagement.Business.Configuration;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using BizTalkZombieManagement.Entities.CustomEnum;
using System.ServiceProcess;

namespace BizTalkZombieManagement.UI.Configuration.ViewModel
{
    public class ConfiguratorViewModel : BaseViewModel
    {
        #region Commandes
        public ICommand ClickBrowseFolder { get; set; }
        public ICommand saveConfigurationCommand { get; set; }
        public ICommand ManageServiceCommand { get; set; }
        #endregion

        #region Items Sources
        public ObservableCollection<String> WcfBindingType { get; private set; }
        #endregion

        #region private member
        WindowsServiceLogic _ServiceWindowsLogic = new WindowsServiceLogic();
        #endregion
        public ConfiguratorViewModel()
        {
            ClickBrowseFolder = new RelayCommand(param => BrowseFolder(), param => true);
            ManageServiceCommand = new RelayCommand(param => ManageService(), param => true);
            
            _ServiceWindowsLogic.OnStateChange += NewState;
            State = _ServiceWindowsLogic.state;
            WcfBindingType = new ObservableCollection<String>();
            this.initializeWcfBindingType();
        }

        private ServiceControllerStatus _State;
        /// <summary>
        /// Show the current service state
        /// </summary>
        public ServiceControllerStatus State
        {
            get { return _State; }
            set
            {
                if (_State != value)
                {
                    _State = value;

                    switch (_State)
                    {
                        case ServiceControllerStatus.Running:
                            ServiceAction = "Stop";
                            break;
                        case ServiceControllerStatus.Stopped:
                            ServiceAction = "Start";
                            break;
                        default:
                            break;
                    }

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
            get { return Enum.Equals(ServiceControllerStatus.Stopped, _State); }
        }

        private String _ServiceAction;
        /// <summary>
        /// display the button action start or stop service
        /// </summary>
        public String ServiceAction
        {
            get
            {
                return _ServiceAction;
            }
            set
            {
                _ServiceAction = String.Concat(value, " the service");
                OnPropertyChanged("ServiceAction");

            }
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

       


        private void initializeWcfBindingType()
        {
            WcfBindingType.Add("Select binding" );
            foreach (var EnumName in Enum.GetValues(typeof(WcfType)))
            {
                WcfBindingType.Add(EnumName.ToString());
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

        /// <summary>
        /// Open new dialog bo to select a folder
        /// </summary>
        private void BrowseFolder()
        {
            var dlg = new FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                FolderPath = dlg.SelectedPath;
            }
        }

        /// <summary>
        /// Start or stop the service windows
        /// </summary>
        private void ManageService()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            _ServiceWindowsLogic.StartOrStopService();
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

       
    }
}
