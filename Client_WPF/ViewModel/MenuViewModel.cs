﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DataModel;
using Client_WPF.View;
using System.Windows.Input;
using Client_WPF.Utils;
using System.Windows;
using System.ComponentModel;

namespace Client_WPF.ViewModel
{
    class MenuViewModel : BindableObject
    {
        #region Properties
        private string search;
        public string Search
        {
            get { return search; }
            set
            {
                search = value;
                RaisePropertyChange("Search");

            }
        }

        public ICommand CloseCommand { get; private set; }
        public ICommand ShowConnectionDialog { get; private set; }

        private UserDataModel UserData { get;set; }
        public LoginModal ConnectionModal { get; private set; }

        private PropertyChangedEventHandler PropertyChangedHandler { get; set; }
        #endregion

        

        #region CTor
        public MenuViewModel()
        {
            UserData = UserDataModel.Instance;
            CloseCommand = new RelayCommand((param) => Application.Current.Shutdown());
            ShowConnectionDialog = new RelayCommand((param) => ShowConnectionModel_IFN());
            PropertyChangedHandler = new System.ComponentModel.PropertyChangedEventHandler(UserData_PropertyChanged);
            UserData.PropertyChanged += PropertyChangedHandler;
            
            search = "Search Feeds ...";
        }

        ~MenuViewModel()
        {
            UserData.PropertyChanged -= PropertyChangedHandler;
        }

        void UserData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsConnected")
            {
                if (!(sender as UserDataModel).IsConnected)
                {
                    if (ConnectionModal == null)
                    {
                        ConnectionModal = new LoginModal();
                        ConnectionModal.ShowDialog();
                    }
                }
                else
                {
                    if (ConnectionModal != null)
                        ConnectionModal.Close();
                    ConnectionModal = null;
                }
            }
        }
        #endregion

        #region private methods
        void ShowConnectionModel_IFN()
        {
            if (!UserData.IsConnected && ConnectionModal == null)
            {
                ConnectionModal = new LoginModal();
                ConnectionModal.ShowDialog();
            }
        }
        #endregion

    }
}
