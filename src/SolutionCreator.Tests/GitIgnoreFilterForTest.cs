using SolutionCreator.Enums;
using SolutionCreator.GitIgnore.Filter;

namespace SolutionCreator.Tests
{
    public class GitIgnoreFilterForTest : GitIgnoreFilterTemplate
    {
        public override SolutionType SolutionType => SolutionType.AspNetMvc;

        protected override string GetIgnoreFile()
        {
            return Properties.Resources.IgnoreFileTests;
        }
    }
}
