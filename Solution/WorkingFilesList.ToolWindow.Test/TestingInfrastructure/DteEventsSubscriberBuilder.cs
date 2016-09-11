﻿// Working Files List
// Visual Studio extension tool window that shows a selectable list of files
// that are open in the editor
// Copyright © 2016 Anthony Fung

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.

using EnvDTE;
using Moq;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Service.EventRelay;

namespace WorkingFilesList.ToolWindow.Test.TestingInfrastructure
{
    internal class DteEventsSubscriberBuilder
    {
        public Mock<IProjectItemsEventsService> ProjectItemsEventsServiceMock { get; }
            = new Mock<IProjectItemsEventsService>();

        public Mock<ISolutionEventsService> SolutionEventsServiceMock { get; }
            = new Mock<ISolutionEventsService>();

        public Mock<IWindowEventsService> WindowEventsServiceMock { get; }
            = new Mock<IWindowEventsService>();

        /// <summary>
        /// Create and return a new <see cref="DteEventsSubscriber"/>, configured
        /// with the properties available in this builder instance
        /// </summary>
        /// <returns>
        /// A new <see cref="DteEventsSubscriber"/> for use in unit tests
        /// </returns>
        public DteEventsSubscriber CreateDteEventsSubscriber()
        {
            WindowEventsServiceMock.Setup(w => w
                .WindowActivated(
                    It.IsAny<Window>(),
                    It.IsAny<Window>()));

            WindowEventsServiceMock.Setup(w => w
                .WindowClosing(It.IsAny<Window>()));

            WindowEventsServiceMock.Setup(w => w
                .WindowCreated(It.IsAny<Window>()));

            var subscriber = new DteEventsSubscriber(
                ProjectItemsEventsServiceMock.Object,
                SolutionEventsServiceMock.Object,
                WindowEventsServiceMock.Object);

            return subscriber;
        }
    }
}
