﻿// Working Files List
// Visual Studio extension tool window that shows a selectable list of files
// that are open in the editor
// Copyright © 2016 Anthony Fung

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//     http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using NUnit.Framework;
using System.Collections.Generic;
using System.Windows.Data;
using WorkingFilesList.ToolWindow.Model;
using WorkingFilesList.ToolWindow.Model.SortOption;

namespace WorkingFilesList.ToolWindow.Test.Model.SortOption
{
    [TestFixture]
    public class AlphabeticalSortTests
    {
        private static DocumentMetadata CreateDocumentMetadata(string displayName)
        {
            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null)
            {
                DisplayName = displayName
            };

            return metadata;
        }

        [Test]
        public void BeginningOfAlphabetAppearsAtStartOfSortedList()
        {
            // Arrange

            const string displayNameA = "A";

            var metadataList = new List<DocumentMetadata>
            {
                CreateDocumentMetadata("B"),
                CreateDocumentMetadata("C"),
                CreateDocumentMetadata(displayNameA)
            };

            var sortOption = new AlphabeticalSort();
            var sortDescription = sortOption.GetSortDescription();
            var view = new ListCollectionView(metadataList);

            // Act

            view.SortDescriptions.Add(sortDescription);

            // Assert

            view.MoveCurrentToFirst();
            var firstItem = (DocumentMetadata) view.CurrentItem;

            Assert.That(firstItem.DisplayName, Is.EqualTo(displayNameA));
        }

        [Test]
        public void HasSortDescriptionIsTrue()
        {
            // Arrange

            var sortOption = new AlphabeticalSort();

            // Assert

            Assert.IsTrue(sortOption.HasSortDescription);
        }

        [Test]
        public void ApplicableTypeIsCorrect()
        {
            // Arrange

            var sortOption = new AlphabeticalSort();

            // Act

            var isDocumentType =
                sortOption.ApplicableType == ProjectItemType.Document;

            // Assert

            Assert.IsTrue(isDocumentType);
        }
    }
}
