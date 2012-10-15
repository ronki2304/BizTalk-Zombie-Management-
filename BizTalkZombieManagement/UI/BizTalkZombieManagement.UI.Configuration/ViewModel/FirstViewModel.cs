using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using BizTalkZombieManagement.Business;

namespace BizTalkZombieManagement.UI.Configuration.ViewModel
{
    public class FirstViewModel : BaseViewModel
    {
        #region Commandes
        public ICommand ClickCommand { get; set; }
        public ICommand ActiveIsEnable { get; set; }
        public ICommand DisActiveIsEnable { get; set; }
        #endregion

        /// <summary>
        /// constructeur
        /// </summary>
        public FirstViewModel()
        {
            ClickCommand = new RelayCommand(param => Click(), param => true);
            ActiveIsEnable = new RelayCommand(param => ActiveControl(), param => true);
            DisActiveIsEnable = new RelayCommand(param => DisactiveControls(), param => true);
        }

        /// <summary>
        /// réponse à la commande click
        /// </summary>
        private void Click()
        {
            LogHelper.WriteInfo(Comment);
            IsEnable = !_IsEnable;
        }
        
        private void ActiveControl()
        {
            IsEnable = true;
        }

        private void DisactiveControls()
        {
            IsEnable = false;
        }
        private String _Comment;
        public String Comment
        {
            get { return _Comment; }
            set
            {
                if (_Comment != value)
                {
                    _Comment = value;
                    OnPropertyChanged("Comment");
                }
            }
        }

        private Boolean _IsEnable;
        public Boolean IsEnable
        {
            get { return _IsEnable; }

            set
            {
                _IsEnable = value;
                OnPropertyChanged("IsEnable");
            }
        }

    }
}
