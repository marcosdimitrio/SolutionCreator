// Apache License 2.0
//
// Copyright 2017 Marcos Dimitrio
//
// Licensed under the Apache License, Version 2.0 the "License";
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using SolutionCreator.Helpers;
using SolutionCreator.Wpf.Commands;
using SolutionCreator.Wpf.Services.Defaults;
using SolutionCreator.Wpf.Services.IO;
using SolutionCreator.Wpf.Services.MessageExchange;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SolutionCreator.Wpf.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {

        private readonly IMessagingService MessagingService;
        private readonly ICreator Creator;
        private readonly IDialogServices DialogServices;

        private bool _isRunning = false;
        public bool IsRunning { get { return _isRunning; } set { _isRunning = value; RaisePropertyChanged(); } }

        private string _title;
        public string Title { get { return _title; } set { _title = value; RaisePropertyChanged(); } }

        private string _sourceDir;
        public string SourceDir { get { return _sourceDir; } set { _sourceDir = value; RaisePropertyChanged(); } }

        private string _destinationDir;
        public string DestinationDir { get { return _destinationDir; } set { _destinationDir = value; RaisePropertyChanged(); } }

        private string _newSolutionName;
        public string NewSolutionName { get { return _newSolutionName; } set { _newSolutionName = value; RaisePropertyChanged(); } }

        private string _statusMessage;
        public string StatusMessage { get { return _statusMessage; } set { _statusMessage = value; RaisePropertyChanged(); } }

        public ICommand GenerateCommand
        {
            get
            {
                return new RelayCommand((x) => Generate(), (x) => !IsRunning);
            }
        }

        public ICommand SourceDialogCommand
        {
            get
            {
                return new RelayCommand((x) => ShowSourceDialog(), (x) => !IsRunning);
            }
        }

        public ICommand DestinationDialogCommand
        {
            get
            {
                return new RelayCommand((x) => ShowDestinationDialog(), (x) => !IsRunning);
            }
        }

        private void ShowSourceDialog()
        {
            var folderName = DialogServices.OpenFolderDialog(SourceDir);

            if (folderName != "")
            {
                SourceDir = folderName;
            }
        }

        private void ShowDestinationDialog()
        {
            var folderName = DialogServices.OpenFolderDialog(DestinationDir);

            if (folderName != "")
            {
                DestinationDir = folderName;
            }
        }

        public MainWindowViewModel(ICreator creator, IMessagingService messagingService, IDefaultsService defaultsService, IDialogServices dialogServices)
        {
            MessagingService = messagingService;
            Creator = creator;
            DialogServices = dialogServices;

            defaultsService.SetDefaults(this);
        }

        private async void Generate()
        {
            IsRunning = true;
            StatusMessage = "Running...";
            Exception thrownException = null;

            try
            {
                await Task.Run(
                    () =>
                    {
                        try
                        {
                            Creator.Create(SourceDir, DestinationDir, NewSolutionName);
                        }
                        catch (Exception ex)
                        {
                            thrownException = ex;
                        }
                    }
                );
            }
            finally
            {
                IsRunning = false;
            }

            if (thrownException != null)
            {
                MessagingService.Show(string.Join("\r\n", thrownException.GetAllMessages()), "Error", MessagingImage.Error);
            }
            else
            {
                MessagingService.Show("Done!", "Generate", MessagingImage.Information);
                StatusMessage = "Done.";
            }

        }

    }
}
