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

using SolutionCreator.Infra.Settings.Settings;
using SolutionCreator.Wpf.ViewModel;

namespace SolutionCreator.Wpf.Services.Defaults
{
    public class DefaultsService : IDefaultsService
    {

        private readonly DefaultSettings DefaultSettings;

        public DefaultsService(DefaultSettings defaultSettings)
        {
            DefaultSettings = defaultSettings;
        }

        public void SetDefaults(MainWindowViewModel mainWindowViewModel)
        {
            SetInvariableDefaults(mainWindowViewModel);

            SetUserDefineableDefaults(mainWindowViewModel);
        }

        private static void SetInvariableDefaults(MainWindowViewModel mainWindowViewModel)
        {
            mainWindowViewModel.Title = "Solution Creator";
            mainWindowViewModel.NewSolutionName = "NewSolution";
        }

        private void SetUserDefineableDefaults(MainWindowViewModel mainWindowViewModel)
        {
            mainWindowViewModel.SourceDir = DefaultSettings.SourceDir;
            mainWindowViewModel.DestinationDir = DefaultSettings.DestinationDir;
        }

    }
}
