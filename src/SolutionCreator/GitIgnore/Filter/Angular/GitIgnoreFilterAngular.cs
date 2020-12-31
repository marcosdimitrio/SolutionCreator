using SolutionCreator.Enums;
using System.IO;

namespace SolutionCreator.GitIgnore.Filter.Angular
{
    public class GitIgnoreFilterAngular : GitIgnoreFilterTemplate
    {
        public override SolutionType SolutionType => SolutionType.Angular;

        protected override string GetIgnoreFile()
        {
            var fileName = "IgnoreFileAngular.txt";

            if (File.Exists(fileName))
            {
                return File.ReadAllText(fileName);
            }

            return Properties.Resources.IgnoreFileAngular;
        }
    }
}
