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
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model.SortOption;

namespace WorkingFilesList.ToolWindow.ViewModel
{
    public class DisplayOrderContainer : IDisplayOrderContainer
    {
        /// <summary>
        /// Order within this list determines the value of
        /// <see cref="ISortOption.DisplayIndex"/> of <see cref="ISortOption"/>
        /// instances
        /// </summary>
        public Type[] DisplayOrder { get; } =
        {
            typeof(DisableSorting),
            typeof(ChronologicalSort),
            typeof(AlphabeticalSort),
            typeof(ReverseAlphabeticalSort)
        };
    }
}
