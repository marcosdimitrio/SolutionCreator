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

using System.Windows;
using System;

namespace SolutionCreator.Wpf.Services.MessageExchange
{
    public class MessagingService : IMessagingService
    {

        public bool Show(string text, string title)
        {
            return Show(text, title, MessagingImage.None);
        }

        public bool Show(string text, string title, MessagingImage image)
        {
            var button = MessageBoxButton.OK;

            MessageBox.Show(Application.Current.MainWindow, text, title, button, Map(image));

            return true;
        }

        public MessagingResult Ask(string text, string title, MessagingButton messagingButton)
        {
            var result = MessageBox.Show(Application.Current.MainWindow, text, title, Map(messagingButton));

            return Mapear(result);
        }

        private MessageBoxImage Map(MessagingImage image)
        {
            switch (image)
            {
                case MessagingImage.None:
                    return MessageBoxImage.None;
                case MessagingImage.Hand:
                    return MessageBoxImage.Hand;
                case MessagingImage.Question:
                    return MessageBoxImage.Question;
                case MessagingImage.Warning:
                    return MessageBoxImage.Warning;
                case MessagingImage.Information:
                    return MessageBoxImage.Information;
                default:
                    throw new ArgumentNullException(nameof(image));
            }
        }

        private MessageBoxButton Map(MessagingButton messagingButton)
        {
            switch (messagingButton)
            {
                case MessagingButton.OK:
                    return MessageBoxButton.OK;
                case MessagingButton.OKCancel:
                    return MessageBoxButton.OKCancel;
                case MessagingButton.YesNoCancel:
                    return MessageBoxButton.YesNoCancel;
                case MessagingButton.YesNo:
                    return MessageBoxButton.YesNo;
                default:
                    throw new ArgumentNullException(nameof(messagingButton));
            }
        }

        private MessagingResult Mapear(MessageBoxResult result)
        {
            switch (result)
            {
                case MessageBoxResult.None:
                    return MessagingResult.None;
                case MessageBoxResult.OK:
                    return MessagingResult.OK;
                case MessageBoxResult.Cancel:
                    return MessagingResult.Cancel;
                case MessageBoxResult.Yes:
                    return MessagingResult.Yes;
                case MessageBoxResult.No:
                    return MessagingResult.No;
                default:
                    throw new ArgumentNullException(nameof(result));
            }
        }

    }
}
