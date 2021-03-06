﻿using CaseExtensions;
using SolutionCreator.Dto;
using SolutionCreator.SolutionNameReplacerService.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SolutionCreator.SolutionNameReplacerService
{
    public class SolutionNameReplacer : ISolutionNameReplacer
    {
        public void Replace(string newSolutionDir, SolutionName solutionName)
        {
            var allFiles = new List<string>();

            allFiles.AddRange(Directory.GetFiles(newSolutionDir, "*.*", SearchOption.AllDirectories));

            foreach (var file in allFiles)
            {
                string text;
                Encoding encoding;

                using (var reader = new StreamReader(file))
                {
                    text = reader.ReadToEnd();
                    encoding = reader.CurrentEncoding;
                }

                var newText = text;

                newText = newText.Replace(solutionName.OldName.ToPascalCase(), solutionName.NewName.ToPascalCase());
                newText = newText.Replace(solutionName.OldName.ToCamelCase(), solutionName.NewName.ToCamelCase());
                newText = newText.Replace(solutionName.OldName.ToKebabCase(), solutionName.NewName.ToKebabCase());
                newText = newText.Replace(solutionName.OldName.ToKebabCase().ToUpperInvariant(), solutionName.NewName.ToKebabCase().ToUpperInvariant());
                newText = newText.Replace(solutionName.OldName.ToSnakeCase(), solutionName.NewName.ToSnakeCase());

                if (newText != text)
                {
                    File.WriteAllBytes(file, encoding.GetBytes(newText));
                }
            }

        }

    }
}
