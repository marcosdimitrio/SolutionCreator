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

using System;
using System.Collections.Generic;

namespace SolutionCreator.Helpers
{
    public static class ExceptionExtensions
    {

        /// <summary>
        /// Retrieves all messages from the current exception to the last inner exception.
        /// </summary>
        public static IList<string> GetAllMessages(this Exception ex)
        {
            var mensagens = new List<string>();

            while (ex != null)
            {
                mensagens.Add(ex.Message);
                ex = ex.InnerException;
            }

            return mensagens;
        }

    }
}
