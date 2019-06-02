using SolutionCreator.Enums;

namespace SolutionCreator.GitIgnore.Filter.Mvc
{
    public class GitIgnoreFilterMvc : GitIgnoreFilterTemplate
    {
        public override SolutionType SolutionType => SolutionType.AspNetMvc;

        protected override string GetIgnoreFile()
        {
            return Properties.Resources.IgnoreFileMvc;
        }
    }
}
