// Apache License 2.0
//
// Copyright 2017 Marcos Dimitrio
//
// Licensed under the Apache License, Version 2.0 the "License";
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using SolutionCreator.GitIgnore;
using Xunit;

namespace SolutionCreator.Tests
{

    public class GitIgnoreFilterTest
    {

        public class AcceptsMethod
        {

            [Theory]
            [InlineData(@"d:\solution\abc.sln")]                        // *.sln
            [InlineData(@"d:\solution\someProject\abc.cs")]             // *.cs
            [InlineData(@"d:\solution\someProject\abc.csproj")]         // *.csproj
            [InlineData(@"d:\solution\someProject\abc.txt")]            // *.txt
            [InlineData(@"d:\solution\abc.suos")]                       // *.suo
            [InlineData(@"d:\solution\abc.sln.docstate")]               // *.sln.docstates
            [InlineData(@"d:\solution\.vs")]                            // .vs/
            [InlineData(@"d:\solution\BuildLog")]                       // [Bb]uild[Ll]og.*
            [InlineData(@"d:\solution\TestResult.txt")]                 // TestResult.xml
            [InlineData(@"d:\solution\project.lock.json.file")]         // project.lock.json
            [InlineData(@"d:\solution\_Chutzpa")]                       // _Chutzpah*
            [InlineData(@"d:\solution\Chutzpah")]                       // _Chutzpah*
            [InlineData(@"d:\solution\mm.abc")]                         // *.mm.*
            [InlineData(@"d:\solution\abc.mm")]                         // *.mm.*
            [InlineData(@"d:\solution\.crunc.local.xml")]               // .*crunch*.local.xml
            [InlineData(@"d:\solution\DocProject\Help\HxT")]            // DocProject/Help/*.HxT
            [InlineData(@"d:\solution\Publish.xml")]                    // *.[Pp]ublish.xml
            [InlineData(@"d:\solution\~a$")]                            // ~$*
            [InlineData(@"d:\solution\a~a")]                            // *~
            [InlineData(@"d:\solution\.gitignore")]                     // .git
            public void MustAcceptTheseFiles(string file)
            {

                // Arrange
                var gitIgnoreFilter = new GitIgnoreFilter();

                // Act
                var test = gitIgnoreFilter.Accepts(file, false);

                // Assert
                Assert.True(test);
            }

            [Theory]
            [InlineData(@"d:\solution\Debugs\")]                        // [Dd]ebug/
            [InlineData(@"d:\solution\builds\s")]                       // build/
            [InlineData(@"d:\solution\ab.vs\")]                         // .vs/
            [InlineData(@"d:\solution\TestResul\")]                     // [Tt]est[Rr]esult*/
            public void MustAcceptTheseFolders(string file)
            {

                // Arrange
                var gitIgnoreFilter = new GitIgnoreFilter();

                // Act
                var test = gitIgnoreFilter.Accepts(file, true);

                // Assert
                Assert.True(test);
            }

            [Theory]
            [InlineData(@"d:\solution\abc.suo")]                        // *.suo
            [InlineData(@"d:\solution\abc.sln.docstates")]              // *.sln.docstates
            [InlineData(@"d:\solution\build\abc")]                      // build/
            [InlineData(@"d:\solution\build\abc.txt")]                  // build/
            [InlineData(@"d:\solution\BuildLog.txt")]                   // [Bb]uild[Ll]og.*
            [InlineData(@"d:\solution\TestResult.xml")]                 // TestResult.xml
            [InlineData(@"d:\solution\project.lock.json")]              // project.lock.json
            [InlineData(@"d:\solution\_i.c")]                           // *_i.c
            [InlineData(@"d:\solution\abc_i.c")]                        // *_i.c
            [InlineData(@"d:\solution\_Chutzpah")]                      // _Chutzpah*
            [InlineData(@"d:\solution\_ChutzpahAbc")]                   // _Chutzpah*
            [InlineData(@"d:\solution\abc.mm.abc")]                     // *.mm.*
            [InlineData(@"d:\solution\.crunch.local.xml")]              // .*crunch*.local.xml
            [InlineData(@"d:\solution\.crunchAbc.local.xml")]           // .*crunch*.local.xml
            [InlineData(@"d:\solution\.Abccrunch.local.xml")]           // .*crunch*.local.xml
            [InlineData(@"d:\solution\.AbccrunchAbc.local.xml")]        // .*crunch*.local.xml
            [InlineData(@"d:\solution\DocProject\buildhelp\a")]         // DocProject/buildhelp/
            [InlineData(@"d:\solution\DocProject\Help\Abc.HxT")]        // DocProject/Help/*.HxT
            [InlineData(@"d:\solution\Abc.Publish.xml")]                // *.[Pp]ublish.xml
            [InlineData(@"d:\solution\~$")]                             // ~$*
            [InlineData(@"d:\solution\~$Abc")]                          // ~$*
            [InlineData(@"d:\solution\~")]                              // *~
            [InlineData(@"d:\solution\Abc~")]                           // *~
            [InlineData(@"d:\solution\packages\a")]                     // **/packages/*
            [InlineData(@"d:\solution\.git")]                           // .git
            [InlineData(@"d:\solution\.git\abc")]                       // .git
            public void MustRejectTheseFiles(string file)
            {

                // Arrange
                var gitIgnoreFilter = new GitIgnoreFilter();

                // Act
                var test = gitIgnoreFilter.Accepts(file, false);

                // Assert
                Assert.False(test);
            }

            [Theory]
            [InlineData(@"d:\solution\Debug\")]                         // [Dd]ebug/
            [InlineData(@"d:\solution\build\")]                         // build/
            [InlineData(@"d:\solution\.vs\")]                           // .vs/
            [InlineData(@"d:\solution\testresult\")]                    // [Tt]est[Rr]esult*/
            [InlineData(@"d:\solution\TestResult\")]                    // [Tt]est[Rr]esult*/
            [InlineData(@"d:\solution\TestResults\")]                   // [Tt]est[Rr]esult*/
            [InlineData(@"d:\solution\DocProject\buildhelp\")]          // DocProject/buildhelp/
            [InlineData(@"d:\solution\packages\")]                      // **/packages/*
            [InlineData(@"d:\solution\.git")]                           // .git
            [InlineData(@"d:\solution\.git\")]                          // .git
            [InlineData(@"d:\solution\.git\abc")]                       // .git
            [InlineData(@"d:\solution\.git\abc\")]                      // .git
            public void MustRejectTheseFolders(string file)
            {

                // Arrange
                var gitIgnoreFilter = new GitIgnoreFilter();

                // Act
                var test = gitIgnoreFilter.Accepts(file, true);

                // Assert
                Assert.False(test);
            }

            [Theory]
            [InlineData(@"d:\solution\packages\build\")]                // !**/packages/build/
            [InlineData(@"d:\solution\packages\build\abc")]             // !**/packages/build/
            [InlineData(@"d:\solution\packages\build\abc.*")]           // !**/packages/build/
            [InlineData(@"d:\solution\abc.cache")]                      // !*.[Cc]ache/
            public void MustNotRejectTheseFolders(string file)
            {

                // Arrange
                var gitIgnoreFilter = new GitIgnoreFilter();

                // Act
                var test = gitIgnoreFilter.Accepts(file, true);

                // Assert
                Assert.True(test);
            }

        }

    }
}
