﻿// Apache License 2.0
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

using SolutionCreator.Wpf.ViewModel;
using Ninject;
using Ninject.Modules;
using System.Collections.Generic;
using SolutionCreator.IoC;
using Ninject.Planning.Bindings.Resolvers;

namespace SolutionCreator.Wpf.DependencyInjection
{
    public class NinjectServiceLocator
    {
        private readonly IKernel Kernel;

        public NinjectServiceLocator()
        {
            Kernel = new StandardKernel();

            Kernel.Components.Remove<IMissingBindingResolver, SelfBindingResolver>();

            var modules = new List<INinjectModule>
            {
                new WpfNinjectMappings(),
                new SolutionCreatorNinjectMappings(),
            };

            Kernel.Load(modules);
        }

        public MainWindowViewModel MainWindowViewModel
        {
            get { return Kernel.Get<MainWindowViewModel>(); }
        }

    }
}