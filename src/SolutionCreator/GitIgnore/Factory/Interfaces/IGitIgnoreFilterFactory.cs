using SolutionCreator.Enums;

namespace SolutionCreator.GitIgnore.Factory.Interfaces
{
    public interface IGitIgnoreFilterFactory
    {
        IGitIgnoreFilter Get(SolutionType solutionType);
    }
}
