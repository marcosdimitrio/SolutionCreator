using SolutionCreator.Enums;

namespace SolutionCreator.GitIgnore.Filter.Angular
{
    public class GitIgnoreFilterAngular : GitIgnoreFilterTemplate
    {
        public override SolutionType SolutionType => SolutionType.Angular;

        protected override string GetIgnoreFile()
        {
            return Properties.Resources.IgnoreFileAngular;
        }
    }
}
