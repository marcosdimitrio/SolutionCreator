using SolutionCreator.Helpers;
using SolutionCreator.SolutionProcessor.Factory.Interfaces;
using SolutionCreator.SolutionProcessor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolutionCreator.SolutionProcessor.Factory
{
    public class SolutionProcessorFactory : ISolutionProcessorFactory
    {
        private readonly IList<ISolutionProcessor> SolutionProcessors;

        public SolutionProcessorFactory(IList<ISolutionProcessor> solutionProcessors)
        {
            SolutionProcessors = solutionProcessors;
        }

        public ISolutionProcessor Get(string sourceDir)
        {
            var processors = SolutionProcessors.Where(x => x.CanProcess(sourceDir));

            if (processors.Count() == 0) throw new InvalidOperationException("The provided folder doesn't have any known solution types.");

            if (processors.Count() != 1)
            {
                var typesDetected = string.Join(", ", processors.Select(x => x.SolutionProcessorType.Description()));

                throw new InvalidOperationException($"The provided folder has more than one known solution type, it must have only one. Types detected: {typesDetected}");
            }

            return processors.Single();
        }

    }
}
