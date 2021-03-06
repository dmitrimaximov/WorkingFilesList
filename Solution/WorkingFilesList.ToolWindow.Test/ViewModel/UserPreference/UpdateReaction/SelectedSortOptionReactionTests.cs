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

using Moq;
using NUnit.Framework;
using System.ComponentModel;
using System.Windows.Data;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model;
using WorkingFilesList.ToolWindow.ViewModel.UserPreference.UpdateReaction;

namespace WorkingFilesList.ToolWindow.Test.ViewModel.UserPreference.UpdateReaction
{
    [TestFixture]
    public class SelectedSortOptionReactionTests
    {
        [Test]
        public void UpdateCollectionReplacesSortDescriptionsWithEvaluatedDescriptions()
        {
            // Arrange

            var existing = new SortDescription
            {
                PropertyName = "Existing"
            };

            var evaluated = new SortDescription
            {
                PropertyName = "Evaluated"
            };

            var service = Mock.Of<ISortOptionsService>(s =>
                s.EvaluateAppliedSortDescriptions(It.IsAny<IUserPreferences>()) == new[]
                {
                    evaluated
                });

            var collection = new DocumentMetadata[0];

            var view = new ListCollectionView(collection);
            view.SortDescriptions.Add(existing);

            var updateReaction = new SelectedSortOptionReaction(service);

            // Act

            updateReaction.UpdateCollection(
                view,
                Mock.Of<IUserPreferences>());

            // Assert

            Assert.That(view.SortDescriptions.Count, Is.EqualTo(1));

            Assert.That(
                view.SortDescriptions[0].PropertyName,
                Is.EqualTo(evaluated.PropertyName));
        }
    }
}
