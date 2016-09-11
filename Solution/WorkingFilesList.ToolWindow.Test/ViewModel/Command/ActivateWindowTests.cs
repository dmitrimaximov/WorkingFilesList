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
using EnvDTE80;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using WorkingFilesList.ToolWindow.Model;
using WorkingFilesList.ToolWindow.Test.TestingInfrastructure;
using WorkingFilesList.ToolWindow.ViewModel.Command;
using static WorkingFilesList.ToolWindow.Test.TestingInfrastructure.CommonMethods;

namespace WorkingFilesList.ToolWindow.Test.ViewModel.Command
{
    [TestFixture]
    public class ActivateWindowTests
    {
        [Test]
        public void CanExecuteReturnsTrue()
        {
            // Arrange

            var command = new ActivateWindow(Mock.Of<DTE2>());

            // Act

            var canExecute = command.CanExecute(null);

            // Assert

            Assert.IsTrue(canExecute);
        }

        [Test]
        public void ExecuteActivatesFirstDocumentWindow()
        {
            // Arrange

            var info = new DocumentMetadataInfo
            {
                FullName = "DocumentName"
            };

            var windowMock = new Mock<Window>();
            var windowList = new List<Window>
            {
                windowMock.Object
            };

            var windowsMock = CreateWindows(windowList);
            var documentMockList = new List<Document>
            {
                Mock.Of<Document>(d =>
                    d.FullName == info.FullName &&
                    d.Windows == windowsMock)
            };

            var documents = CreateDocuments(documentMockList);
            var dte2 = Mock.Of<DTE2>(d => d.Documents == documents);

            var builder = new DocumentMetadataFactoryBuilder();
            var factory = builder.CreateDocumentMetadataFactory(true);
            var metadata = factory.Create(info);

            var command = new ActivateWindow(dte2);

            // Act

            command.Execute(metadata);

            // Assert

            windowMock.Verify(w => w.Activate());
        }

        [Test]
        public void ExecuteDoesNotThrowExceptionWithNullParameter()
        {
            // Arrange

            var dteMock = new Mock<DTE2>();
            dteMock.Setup(d => d.Documents).Returns<Documents>(null);

            var command = new ActivateWindow(dteMock.Object);

            // Act

            Assert.DoesNotThrow(() => command.Execute(null));

            // Assert

            dteMock.Verify(d => d.Documents, Times.Never);
        }
    }
}
