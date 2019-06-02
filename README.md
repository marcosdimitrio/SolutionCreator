# SolutionCreator

Create a new .NET solution based on an existing one.

You can download the <a href="https://github.com/marcosdimitrio/SolutionCreator/releases/latest">latest version</a>.

# How it works

It reads the source solution, and while it copies it's contents, it will:

* Replace the source solution name with the new one on all .sln, .csproj, .cs, .cshtml, .asax and .config files.
* Replace folder/file names that contain the source solution name with the new one.<br/>
For instance: `MyModelSolution.WebApi\MyModelSolution.WebApi.proj`<br/>
Becomes: `MyNewSolution.WebApi\MyNewSolution.WebApi.proj`.
* Generate new GUID for every project file (.csproj), and update related files accordingly.
* Copy packages and node_modules folders, if they exist.
* Use a gitignore format to skip many files that are not needed (e.g. .git, bin folders, .suo, and many others).

You can edit the 'solutionCreator.json' to set the default paths.

# About

Written by Marcos Dimitrio, using C# / WPF.
