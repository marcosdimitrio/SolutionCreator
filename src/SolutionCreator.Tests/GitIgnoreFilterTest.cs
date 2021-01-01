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

using Xunit;

namespace SolutionCreator.Tests
{

    public class GitIgnoreFilterTest
    {

        public class AcceptsMethod
        {

            [Theory]
            [InlineData(@"abc.sln")]                        // *.sln
            [InlineData(@"someProject\abc.cs")]             // *.cs
            [InlineData(@"someProject\abc.csproj")]         // *.csproj
            [InlineData(@"someProject\abc.txt")]            // *.txt
            [InlineData(@"abc.suos")]                       // *.suo
            [InlineData(@"abc.sln.docstate")]               // *.sln.docstates
            [InlineData(@".vs")]                            // .vs/
            [InlineData(@"BuildLog")]                       // [Bb]uild[Ll]og.*
            [InlineData(@"TestResult.txt")]                 // TestResult.xml
            [InlineData(@"project.lock.json.file")]         // project.lock.json
            [InlineData(@"_Chutzpa")]                       // _Chutzpah*
            [InlineData(@"Chutzpah")]                       // _Chutzpah*
            [InlineData(@"mm.abc")]                         // *.mm.*
            [InlineData(@"abc.mm")]                         // *.mm.*
            [InlineData(@".crunc.local.xml")]               // .*crunch*.local.xml
            [InlineData(@"DocProject\Help\HxT")]            // DocProject/Help/*.HxT
            [InlineData(@"Publish.xml")]                    // *.[Pp]ublish.xml
            [InlineData(@"~a$")]                            // ~$*
            [InlineData(@"a~a")]                            // *~
            [InlineData(@".gitignore")]                     // .git
            [InlineData(@"someFolder\dist")]                // /dist
            public void MustAcceptTheseFiles(string file)
            {
                // Arrange
                var gitIgnoreFilter = new GitIgnoreFilterForTest();

                // Act
                var test = gitIgnoreFilter.AcceptsFile(file);

                // Assert
                Assert.True(test);
            }

            [Theory]
            [InlineData(@"Debugs\")]                        // [Dd]ebug/
            [InlineData(@"builds\s")]                       // build/
            [InlineData(@"ab.vs\")]                         // .vs/
            [InlineData(@"TestResul\")]                     // [Tt]est[Rr]esult*/
            [InlineData(@"someFolder\dist\")]               // /dist
            public void MustAcceptTheseFolders(string file)
            {
                // Arrange
                var gitIgnoreFilter = new GitIgnoreFilterForTest();

                // Act
                var test = gitIgnoreFilter.AcceptsFolder(file);

                // Assert
                Assert.True(test);
            }

            [Theory]
            [InlineData(@"abc.suo")]                        // *.suo
            [InlineData(@"abc.sln.docstates")]              // *.sln.docstates
            [InlineData(@"build\abc")]                      // build/
            [InlineData(@"build\abc.txt")]                  // build/
            [InlineData(@"BuildLog.txt")]                   // [Bb]uild[Ll]og.*
            [InlineData(@"TestResult.xml")]                 // TestResult.xml
            [InlineData(@"project.lock.json")]              // project.lock.json
            [InlineData(@"_i.c")]                           // *_i.c
            [InlineData(@"abc_i.c")]                        // *_i.c
            [InlineData(@"_Chutzpah")]                      // _Chutzpah*
            [InlineData(@"_ChutzpahAbc")]                   // _Chutzpah*
            [InlineData(@"abc.mm.abc")]                     // *.mm.*
            [InlineData(@".crunch.local.xml")]              // .*crunch*.local.xml
            [InlineData(@".crunchAbc.local.xml")]           // .*crunch*.local.xml
            [InlineData(@".Abccrunch.local.xml")]           // .*crunch*.local.xml
            [InlineData(@".AbccrunchAbc.local.xml")]        // .*crunch*.local.xml
            [InlineData(@"DocProject\buildhelp\a")]         // DocProject/buildhelp/
            [InlineData(@"DocProject\Help\Abc.HxT")]        // DocProject/Help/*.HxT
            [InlineData(@"Abc.Publish.xml")]                // *.[Pp]ublish.xml
            [InlineData(@"~$")]                             // ~$*
            [InlineData(@"~$Abc")]                          // ~$*
            [InlineData(@"~")]                              // *~
            [InlineData(@"Abc~")]                           // *~
            [InlineData(@"packages\a")]                     // **/packages/*
            [InlineData(@".git")]                           // .git
            [InlineData(@".git\abc")]                       // .git
            [InlineData(@"dist")]                           // /dist
            public void MustRejectTheseFiles(string file)
            {
                // Arrange
                var gitIgnoreFilter = new GitIgnoreFilterForTest();

                // Act
                var test = gitIgnoreFilter.AcceptsFile(file);

                // Assert
                Assert.False(test);
            }

            [Theory]
            [InlineData(@"Debug\")]                         // [Dd]ebug/
            [InlineData(@"build\")]                         // build/
            [InlineData(@".vs\")]                           // .vs/
            [InlineData(@"testresult\")]                    // [Tt]est[Rr]esult*/
            [InlineData(@"TestResult\")]                    // [Tt]est[Rr]esult*/
            [InlineData(@"TestResults\")]                   // [Tt]est[Rr]esult*/
            [InlineData(@"DocProject\buildhelp\")]          // DocProject/buildhelp/
            [InlineData(@"packages\")]                      // **/packages/*
            [InlineData(@".git")]                           // .git
            [InlineData(@".git\")]                          // .git
            [InlineData(@".git\abc")]                       // .git
            [InlineData(@".git\abc\")]                      // .git
            [InlineData(@"dist\")]                          // /dist
            public void MustRejectTheseFolders(string file)
            {
                // Arrange
                var gitIgnoreFilter = new GitIgnoreFilterForTest();

                // Act
                var test = gitIgnoreFilter.AcceptsFolder(file);

                // Assert
                Assert.False(test);
            }

            [Theory]
            [InlineData(@"packages\build\")]                // !**/packages/build/
            [InlineData(@"packages\build\abc")]             // !**/packages/build/
            [InlineData(@"packages\build\abc.*")]           // !**/packages/build/
            [InlineData(@"abc.cache")]                      // !*.[Cc]ache/
            public void MustNotRejectTheseFolders(string file)
            {
                // Arrange
                var gitIgnoreFilter = new GitIgnoreFilterForTest();

                // Act
                var test = gitIgnoreFilter.AcceptsFolder(file);

                // Assert
                Assert.True(test);
            }

        }

    }
}
