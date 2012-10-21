using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using BizTalkZombieManagement.Business.Configuration;

namespace BizTalkZombieManagement.UI.Configuration.ViewModel
{
    public class ConfiguratorViewModel : BaseViewModel
    {
        #region Commandes
        public ICommand ClickCommand { get; set; }
        #endregion

        public ConfiguratorViewModel()
        {
            WindowsServiceLogic logic = new WindowsServiceLogic();
            logic.OnStateChange += NewState;
            State = logic.state;
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
                }
            }
        }

        /// <summary>
        /// Active the control is the servcie state is Stopped
        /// </summary>
        public Boolean IsActiveCommand
        {
            get { return String.Equals("Stopped",_State); }
        }

       /// <summary>
       /// Getting the new service state
       /// </summary>
       /// <param name="o"></param>
       /// <param name="e"></param>
        private void NewState(object o, ServiceWindowsEvent e)
        {
            if (State != e.NewStatus)
            {
                State = String.Concat("Currently the service is ", e.NewStatus);
            }
        }

    }
}
