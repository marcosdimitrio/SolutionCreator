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

using SolutionCreator.Wpf.Services.MessageExchange;
using Ninject.Modules;
using SolutionCreator.Wpf.Services.Defaults;
using SolutionCreator.Wpf.Services.IO;
using SolutionCreator.Wpf.ViewModel;

namespace SolutionCreator.Wpf.DependencyInjection
{
    public class WpfNinjectMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<IMessagingService>().To<MessagingService>();
            Bind<IDefaultsService>().To<DefaultsService>();
            Bind<IDialogServices>().To<DialogServices>();

            // ViewModels
            Bind<MainWindowViewModel>().ToSelf();
        }
    }
}