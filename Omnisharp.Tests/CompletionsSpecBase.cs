using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ICSharpCode.NRefactory.Completion;
using OmniSharp;

namespace Omnisharp.Tests
{
    public class CompletionsSpecBase
    {
        readonly FakeSolution _solution;

        public CompletionsSpecBase()
        {
            _solution = new FakeSolution();
        }

        public IEnumerable<ICompletionData> GetCompletions(string editorText)
        {
            var cursorPosition = editorText.IndexOf("$", StringComparison.Ordinal);
            var partialWord = GetPartialWord(editorText);
            editorText = editorText.Replace("$", "" );

            var project = new FakeProject();
            project.AddFile(editorText);
            _solution.Projects.Add("dummyproject", project);
            var provider = new CompletionProvider(_solution, new Logger());
            
            // vim removes the word to complete.... so we do here also
            string parsedText = editorText.Remove(cursorPosition - partialWord.Length, partialWord.Length);
            Console.WriteLine("Cursor = " + cursorPosition);
            Console.WriteLine("--------editor--------");
            Console.WriteLine("|" + editorText + "|");
            Console.WriteLine("--------parsed--------");
            Console.WriteLine("|" + parsedText + "|");
            return provider.CreateProvider("myfile", partialWord, parsedText, editorText, cursorPosition, true);
        }

        private static string GetPartialWord(string editorText)
        {
            var matches = Regex.Matches(editorText, @"([a-zA-Z_]*)\$");
            return matches[0].Groups[1].ToString();
        }
    }
}