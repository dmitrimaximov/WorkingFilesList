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

using System;
using WorkingFilesList.Interface;
using WorkingFilesList.Model;

namespace WorkingFilesList.Factory
{
    /// <summary>
    /// Contains methods to create new instances of <see cref="DocumentMetadata"/>
    /// passing both the full name provided, and an automatically evaluated version
    /// matching the casing as written on the file system as constructor parameters
    /// </summary>
    public class DocumentMetadataFactory : IDocumentMetadataFactory
    {
        private readonly IPathCasingRestorer _pathCasingRestorer;
        private readonly ITimeProvider _timeProvider;

        public DocumentMetadataFactory(
            IPathCasingRestorer pathCasingRestorer,
            ITimeProvider timeProvider)
        {
            _pathCasingRestorer = pathCasingRestorer;
            _timeProvider = timeProvider;
        }

        /// <summary>
        /// Creates a new <see cref="DocumentMetadata"/>, setting
        /// <see cref="DocumentMetadata.ActivatedAt"/> at the current time in UTC
        /// </summary>
        /// <param name="fullName">Full path and name of document file</param>
        /// <returns>A new <see cref="DocumentMetadata"/> instance</returns>
        public DocumentMetadata Create(string fullName)
        {
            var utcNow = _timeProvider.UtcNow;
            var metadata = Create(fullName, utcNow);
            return metadata;
        }

        /// <summary>
        /// Creates a new <see cref="DocumentMetadata"/>, setting
        /// <see cref="DocumentMetadata.ActivatedAt"/> at the specified time
        /// </summary>
        /// <param name="fullName">Full path and name of document file</param>
        /// <param name="activatedAt">
        /// Value to set <see cref="DocumentMetadata.ActivatedAt"/> as
        /// </param>
        /// <returns>A new <see cref="DocumentMetadata"/> instance</returns>
        public DocumentMetadata Create(string fullName, DateTime activatedAt)
        {
            var correctedCasing = _pathCasingRestorer.RestoreCasing(fullName);
            var metadata = new DocumentMetadata(correctedCasing, fullName)
            {
                ActivatedAt = activatedAt
            };

            return metadata;
        }
    }
}
