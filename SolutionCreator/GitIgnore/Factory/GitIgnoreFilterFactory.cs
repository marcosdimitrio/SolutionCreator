using SolutionCreator.Enums;
using SolutionCreator.GitIgnore.Factory.Interfaces;
using SolutionCreator.GitIgnore.Filter;
using SolutionCreator.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolutionCreator.GitIgnore.Factory
{
    public class GitIgnoreFilterFactory : IGitIgnoreFilterFactory
    {
        private readonly IList<GitIgnoreFilterTemplate> GitIgnoreFilters;

        public GitIgnoreFilterFactory(IList<GitIgnoreFilterTemplate> gitIgnoreFilters)
        {
            GitIgnoreFilters = gitIgnoreFilters;
        }

        public IGitIgnoreFilter Get(SolutionType solutionType)
        {
            var gitIgnoreFilters = GitIgnoreFilters.Where(x => x.SolutionType == solutionType);

            if (gitIgnoreFilters.Count() == 0) throw new InvalidOperationException($"Couldn't find an IGitIgnoreFilter implementation to handle the solution type \"{solutionType.Description()}\".");

            if (gitIgnoreFilters.Count() != 1)
            {
                var typesDetected = string.Join(", ", gitIgnoreFilters.Select(x => x.SolutionType.Description()));

                throw new InvalidOperationException($"There are more than one IGitIgnoreFilter implementations registered. Types detected: {typesDetected}");
            }

            return gitIgnoreFilters.Single();
        }
    }
}
