﻿// WorkingFilesList
// Visual Studio extension tool window that shows a selectable list of files
// that are open in the editor
// Copyright(C) 2016 Anthony Fung

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program. If not, see<http://www.gnu.org/licenses/>.

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
