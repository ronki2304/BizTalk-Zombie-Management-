using System;
using System.Collections.ObjectModel;
using System.ServiceProcess;
using System.Windows.Forms;
using System.Windows.Input;
using BizTalkZombieManagement.Business.Configuration;
using BizTalkZombieManagement.Business.Transport;
using BizTalkZombieManagement.Entities.CustomEnum;
using BizTalkZombieManagement.Business;
using BizTalkZombieManagement.Entities.ConstantName;

namespace BizTalkZombieManagement.UI.Configuration.ViewModel
{
    public class ConfiguratorViewModel : BaseViewModel
    {
        #region Commandes
        public ICommand ClickBrowseFolder { get; set; }
        public ICommand SaveConfigurationCommand { get; set; }
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
            SaveConfigurationCommand = new RelayCommand(param => saveConfiguration(), param => true);
            //use by the windows service logic timer
            _ServiceWindowsLogic.OnStateChange += NewState;
            State = _ServiceWindowsLogic.State;

            //initialize the combobox
            WcfBindingType = new ObservableCollection<String>();
            this.initializeWcfBindingType();
            SelectedBindingType = WcfBindingType[0];

            //initialize control content
            _editor = new ConfigurationFileEditor();
            FillControls();

            // for the moment config is only show
            ConfigChange = false;
        }


        private void FillControls()
        {
            DumpType dtype = _editor.CurrentDumpLayer;

            switch (dtype)
            {
                case DumpType.File:
                    FileSelected = true;
                    FolderPath = _editor.FolderPath;
                    MsmqPath = String.Empty;
                    SelectedBindingType = WcfBindingType[0];
                    WcfUri = String.Empty;
                    break;
                case DumpType.Msmq:
                    MsmqSelected = true;
                    MsmqPath = _editor.MsmqPath;
                    FolderPath = String.Empty;
                    SelectedBindingType = WcfBindingType[0];
                    WcfUri = String.Empty;
                    break;
                case DumpType.Wcf:
                    SelectedBindingType = _editor.WcfType;
                    WcfSelected = true;
                    WcfUri = _editor.GetWcfUri((WcfType)Enum.Parse(typeof(WcfType), SelectedBindingType)).ToString();
                    MsmqPath = String.Empty;
                    FolderPath = String.Empty;
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
                    OnPropertyChanged("MsmqSelected");
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
                if (_FileSelected != value)
                {
                    _FileSelected = value;
                    OnPropertyChanged("FileSelected");
                    ConfigChange = true;
                }
            }
        }

        private String _FolderPath;

        public String FolderPath
        {
            get { return _FolderPath; }
            set
            {
                if (_FolderPath != value)
                {
                    _FolderPath = value;
                    ConfigChange = true;
                    OnPropertyChanged("FolderPath");
                }
            }
        }
        #endregion


        #region MSMQ case
        private Boolean _MsmqSelected;
        public Boolean MsmqSelected
        {
            get { return _MsmqSelected && IsActiveCommand; }
            set
            {
                if (_MsmqSelected != value)
                {
                    _MsmqSelected = value;
                    OnPropertyChanged("MsmqSelected");
                    ConfigChange = true;
                }
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
                if (_MsmqPath != value)
                {
                    _MsmqPath = value;
                    OnPropertyChanged("MsmqPath");
                    ConfigChange = true;
                }
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
                if (_WcfSelected != value)
                {
                    _WcfSelected = value;
                    OnPropertyChanged("WcfSelected");
                    OnPropertyChanged("WcfAndBindingTypeSelect");
                    ConfigChange = true;
                }
            }


        }

        private String _SelectedBindingType;
        public String SelectedBindingType
        {
            get { return _SelectedBindingType; }
            set
            {


                if (_SelectedBindingType != value)
                {
                    _SelectedBindingType = value;
                    OnPropertyChanged("SelectedBindingType");
                    WcfAndBindingTypeSelect = Enum.IsDefined(typeof(WcfType), value);
                    ConfigChange = true;
                }


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
                if (_WcfUri != value)
                {
                    _WcfUri = value;
                    OnPropertyChanged("WcfUri");
                    ConfigChange = true;
                }
            }
        }
        private void initializeWcfBindingType()
        {
            WcfBindingType.Add("Select binding");
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
        private void NewState(object o, ServiceWindowsEventArgs e)
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
            try
            {
                System.Windows.Forms.DialogResult result = dlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    FolderPath = dlg.SelectedPath;
                }
            }
            catch (Exception e)
            {
                LogHelper.WriteError(e);
            }
            finally
            {
                dlg.Dispose();
            }
        }

        /// <summary>
        /// Start or stop the service windows
        /// </summary>
        private void ManageService()
        {
            //check if the config has changed without save
            if (ConfigChange)
            {
                DialogResult result = MessageBox.Show(ResourceLogic.GetString(ResourceKeyName.ConfigChange), ResourceLogic.GetString(ResourceKeyName.ConfigChangeTitle), MessageBoxButtons.YesNoCancel);

                switch (result)
                {
                    case DialogResult.Yes:
                        //let's save
                        saveConfiguration();
                        break;
                    case DialogResult.Cancel:
                        //stop service management
                        return;
                        break;
                    case DialogResult.No:
                        //reinit
                        FillControls();
                        break;
                    default:
                        //so continue with the old values
                        break;

                }

                ConfigChange = false;
            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            _ServiceWindowsLogic.StartOrStopService();
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        /// <summary>
        /// Saving the new configuration
        /// </summary>
        private void saveConfiguration()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            DumpType dtype = DumpType.File; //default value
            String svalue = String.Empty; //define the way where the xml will be dropped
            WcfType binding = WcfType.NamedPipe; //default value

            if (!FileSelected && !MsmqSelected && !WcfSelected)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                MessageBox.Show(ResourceLogic.GetString(ResourceKeyName.SelectType));
                return;
            }
            //retrieve the current dump layer
            if (FileSelected)
            {
                dtype = DumpType.File;
                svalue = FolderPath;
                //test if the folder exist
                if (!FileLogic.IsValidPathFolder(svalue))
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                    MessageBox.Show(ResourceLogic.GetString(ResourceKeyName.ValidFolderPath));
                    return;
                }

            }

            if (MsmqSelected)
            {
                dtype = DumpType.Msmq;
                svalue = MsmqPath;

                if (!MsmqLayer.IsExist(svalue))
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                    MessageBox.Show(ResourceLogic.GetString(ResourceKeyName.ValidMsmqPath));
                    return;
                }
            }

            if (WcfSelected)
            {
                dtype = DumpType.Wcf;
                svalue = WcfUri;

                if (!Enum.TryParse<WcfType>(SelectedBindingType, out binding))
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                    MessageBox.Show(ResourceLogic.GetString(ResourceKeyName.ValidWcfBinding));
                    return;
                }

                if (String.IsNullOrEmpty(svalue))
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                    MessageBox.Show(ResourceLogic.GetString(ResourceKeyName.ValidWcfUri));
                    return;
                }
            }

            _editor.updateConfigurationFile(dtype, svalue, binding);
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;

            if (dtype == DumpType.Wcf)
            {
                MessageBox.Show(String.Format(ResourceLogic.GetString(ResourceKeyName.WcfPortCreation), binding.ToString()));
            }

            //confguration is applied so just apply show mode 
            ConfigChange = false;

            MessageBox.Show(ResourceLogic.GetString(ResourceKeyName.ConfigurationSaved));

        }


        //check if the config change
        private Boolean _ConfigChange;

        public Boolean ConfigChange
        {
            get
            {
                return (_ConfigChange && IsActiveCommand);
            }

            set
            {
                _ConfigChange = value;
                OnPropertyChanged("ConfigChange");
            }
        }
    }
}
