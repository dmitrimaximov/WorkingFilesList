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

using System;
using NUnit.Framework;
using WorkingFilesList.ToolWindow.Model.SortOption;

namespace WorkingFilesList.ToolWindow.Test.Model.SortOption
{
    [TestFixture]
    public class DisableSortingTests
    {
        [Test]
        public void GetSortDescriptionThrowsNotSupportedException()
        {
            // Arrange

            var sortOption = new DisableSorting();

            // Assert

            Assert.Throws<NotSupportedException>(() => sortOption.GetSortDescription());
        }

        [Test]
        public void HasSortDescriptionIsFalse()
        {
            // Arrange

            var sortOption = new DisableSorting();

            // Assert

            Assert.IsFalse(sortOption.HasSortDescription);
        }

        [Test]
        public void ApplicableTypeIsCorrect()
        {
            // Arrange

            var sortOption = new DisableSorting();

            // Act

            var isProjectType =
                sortOption.ApplicableType == ProjectItemType.Project;

            // Assert

            Assert.IsTrue(isProjectType);
        }
    }
}
