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

using SimpleInjector;
using SolutionCreator.GitIgnore;
using SolutionCreator.GuidReplaceService;
using SolutionCreator.Infra.Settings;
using SolutionCreator.Infra.Settings.Interfaces;
using SolutionCreator.SolutionProcessor;
using SolutionCreator.SolutionProcessor.Factory;
using SolutionCreator.SolutionProcessor.Factory.Interfaces;
using SolutionCreator.SolutionProcessor.Interfaces;
using System;
using System.Linq;
using System.Reflection;

namespace SolutionCreator.IoC
{
    public static class SolutionCreatorMappings
    {

        public static void RegisterServices(Container container, Lifestyle lifestyle)
        {
            container.Register<IFileCopy, FileCopy>(lifestyle);
            container.Register<IGuidReplace, GuidReplace>(lifestyle);
            container.Register<IGitIgnoreFilter, GitIgnoreFilter>(lifestyle);
            container.Register<ICreator, Creator>(lifestyle);
            container.Register<ISettingsReader, SettingsReader>(lifestyle);
            container.Register<ISolutionProcessorFactory, SolutionProcessorFactory>(lifestyle);
            container.Collection.Register<ISolutionProcessor>(typeof(SolutionProcessorMvc));

            BindSettingClasses(container, lifestyle);
        }

        private static void BindSettingClasses(Container container, Lifestyle lifestyle)
        {
            var settingsClasses = Assembly.GetAssembly(typeof(SettingsReader))
                                .GetTypes()
                                .Where(t => t.Name.EndsWith("Settings", StringComparison.InvariantCulture))
                                .ToList();

            settingsClasses.ForEach(type =>
            {
                container.Register(type, () => container.GetInstance<ISettingsReader>().LoadSection(type), lifestyle);
            });
        }

    }
}
