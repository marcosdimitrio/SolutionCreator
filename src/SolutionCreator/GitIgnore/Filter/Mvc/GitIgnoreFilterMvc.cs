using SolutionCreator.Enums;
using System.IO;

namespace SolutionCreator.GitIgnore.Filter.Mvc
{
    public class GitIgnoreFilterMvc : GitIgnoreFilterTemplate
    {
        public override SolutionType SolutionType => SolutionType.AspNetMvc;

        protected override string GetIgnoreFile()
        {
            var fileName = "IgnoreFileMvc.txt";

            if (File.Exists(fileName))
            {
                return File.ReadAllText(fileName);
            }

            return Properties.Resources.IgnoreFileMvc;
        }
    }
}
