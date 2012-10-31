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
using BizTalkZombieManagement.Business.Transport;

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
        ConfigurationFileEditor _editor;
        #endregion

        /// <summary>
        /// initialize component value
        /// </summary>
        public ConfiguratorViewModel()
        {
            //add delegate
            ClickBrowseFolder = new RelayCommand(param => BrowseFolder(), param => true);
            ManageServiceCommand = new RelayCommand(param => ManageService(), param => true);
            saveConfigurationCommand = new RelayCommand(param => saveConfiguration(), param => true);
            //use by the windows service logic timer
            _ServiceWindowsLogic.OnStateChange += NewState;
            State = _ServiceWindowsLogic.state;

            //initialize the combobox
            WcfBindingType = new ObservableCollection<String>();
            this.initializeWcfBindingType();
            SelectedBindingType = WcfBindingType[0];

            //initialize control content
            _editor = new ConfigurationFileEditor();
            FillControls();

        }


        private void FillControls()
        {
            DumpType dtype = _editor.GetTheCurrentDumpLayer();

            switch (dtype)
            {
                case DumpType.File:
                    FileSelected = true;
                    FolderPath = _editor.GetFolderPath();
                    break;
                case DumpType.Msmq:
                    MSMQSelected = true;
                    MsmqPath = _editor.GetMsmqPath();
                    break;
                case DumpType.Wcf:
                    SelectedBindingType = _editor.GetWcfType();
                    WcfSelected = true;
                    WcfUri = _editor.GetWcfUri((WcfType)Enum.Parse(typeof(WcfType),SelectedBindingType));
                    break;
                default:
                    break;
            }
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
                            ServiceAction = "Stop";
                            break;
                    }

                    OnPropertyChanged("State");
                    //refresh the active command
                    OnPropertyChanged("IsActiveCommand");
                    OnPropertyChanged("FileSelected");
                    OnPropertyChanged("MSMQSelected");
                    OnPropertyChanged("WcfSelected");
                    OnPropertyChanged("WcfAndBindingTypeSelect");
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

        private String _MsmqPath;
        public String MsmqPath
        {
            get
            {
                return _MsmqPath;
            }
            set
            {
                _MsmqPath = value;
                OnPropertyChanged("MsmqPath");
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

        private String _SelectedBindingType;
        public String SelectedBindingType
        {
            get { return _SelectedBindingType; }
            set
            {
                _SelectedBindingType = value;
                OnPropertyChanged("SelectedBindingType");

                WcfAndBindingTypeSelect = Enum.IsDefined(typeof(WcfType), value);
                
            }
        }

        private Boolean _WcfAndBindingTypeSelect;
        public Boolean WcfAndBindingTypeSelect
        {
            get { return _WcfAndBindingTypeSelect && WcfSelected && IsActiveCommand; }
            set
            {
                _WcfAndBindingTypeSelect = value;
                OnPropertyChanged("WcfAndBindingTypeSelect");
            }
        }
        private String _WcfUri;
        public String WcfUri
        {
            get { return _WcfUri; }
            set
            {
                _WcfUri = value;
                OnPropertyChanged("WcfUri");
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

        /// <summary>
        /// Saving the new configuration
        /// </summary>
        private void  saveConfiguration()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            DumpType dtype= DumpType.File ; //default value
            String svalue=String.Empty; //define the way where the xml will be dropped
            WcfType binding =  WcfType.NamedPipe; //default value

            if (!FileSelected && !MSMQSelected && !WcfSelected)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                MessageBox.Show("Please select a Type");
                return;
            }
            //retrieve the current dump layer
            if (FileSelected)
            {
                dtype= DumpType.File;
                svalue= FolderPath;
                //test if the folder exist
                if (!FileLogic.IsValidPathFolder(svalue))
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                    MessageBox.Show("Please specify a valid folder path");
                    return;
                }
                
            }

            if (MSMQSelected)
            {
                dtype= DumpType.Msmq;
                svalue=MsmqPath;

                if (!MsmqLayer.IsExist(svalue))
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                    MessageBox.Show("Please specify a valid Msmq path");
                    return;
                }
            }

            if (WcfSelected)
            {
                dtype = DumpType.Wcf;
                svalue= WcfUri;
                
                if (!Enum.TryParse<WcfType>(SelectedBindingType, out binding))
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                    MessageBox.Show("Please specify a valid Wcf Binding");
                    return;
                }

                if (String.IsNullOrEmpty(svalue))
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                    MessageBox.Show("Please specify a valid Wcf uri");
                    return;
                }
            }
           
            _editor.updateConfigurationFile(dtype, svalue, binding);
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;

            if (dtype== DumpType.Wcf)
            {
                MessageBox.Show(String.Format("Don't forget to create a BizTalk port with {0} binding and with no security \n if you want to change the security layer, please modify the service configueation file located in the installation path", binding.ToString()));
            }
            MessageBox.Show("Configuration Saved \n Now please restart the service");
        }
       
    }
}
