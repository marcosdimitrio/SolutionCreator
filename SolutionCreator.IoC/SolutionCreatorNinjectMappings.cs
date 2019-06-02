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

using Ninject;
using Ninject.Activation;
using Ninject.Modules;
using SolutionCreator.GitIgnore;
using SolutionCreator.GuidReplaceService;
using SolutionCreator.Infra.Settings;
using SolutionCreator.Infra.Settings.Interfaces;
using SolutionCreator.SolutionProcessor;
using SolutionCreator.SolutionProcessor.Factory;
using SolutionCreator.SolutionProcessor.Factory.Interfaces;
using SolutionCreator.SolutionProcessor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SolutionCreator.IoC
{
    public class SolutionCreatorNinjectMappings : NinjectModule
    {

        public override void Load()
        {
            Bind<IFileCopy>().To<FileCopy>();
            Bind<IGuidReplace>().To<GuidReplace>();
            Bind<IGitIgnoreFilter>().To<GitIgnoreFilter>();
            Bind<ICreator>().To<Creator>();
            Bind<ISettingsReader>().To<SettingsReader>();
            Bind<ISolutionProcessorFactory>().To<SolutionProcessorFactory>();
            Bind<ISolutionProcessor>().To<SolutionProcessorMvc>();

            BindSettings();
        }

        private IList<ISolutionProcessor> bindSolutionProcessors(IContext arg)
        {
            var processors = new List<ISolutionProcessor>();

            return processors;
        }

        private void BindSettings()
        {
            var settings = Assembly.GetAssembly(typeof(SettingsReader))
                                .GetTypes()
                                .Where(t => t.Name.EndsWith("Settings", StringComparison.InvariantCulture))
                                .ToList();

            var settingsReader = Kernel.Get<ISettingsReader>();

            settings.ForEach(type =>
            {
                Bind(type).ToMethod(x => settingsReader.LoadSection(type));
            });
        }

    }
}
