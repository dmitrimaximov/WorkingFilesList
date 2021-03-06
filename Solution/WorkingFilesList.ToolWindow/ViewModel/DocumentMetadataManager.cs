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

using EnvDTE;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model;

namespace WorkingFilesList.ToolWindow.ViewModel
{
    public class DocumentMetadataManager : IDocumentMetadataManager
    {
        private readonly IDocumentMetadataEqualityService _documentMetadataEqualityService;
        private readonly IDocumentMetadataFactory _documentMetadataFactory;
        private readonly INormalizedUsageOrderService _normalizedUsageOrderService;
        private readonly ITimeProvider _timeProvider;
        private readonly IUserPreferences _userPreferences;
        private readonly ObservableCollection<DocumentMetadata> _activeDocumentMetadata;

        /// <summary>
        /// Full name of the document file last activated, i.e. the single
        /// <see cref="DocumentMetadata"/> in <see cref="_activeDocumentMetadata"/>
        /// for which <see cref="DocumentMetadata.IsActive"/> is true
        /// </summary>
        private string _activatedDocument;

        public ICollectionView ActiveDocumentMetadata { get; }

        public DocumentMetadataManager(
            ICollectionViewGenerator collectionViewGenerator,
            IDocumentMetadataEqualityService documentMetadataEqualityService,
            IDocumentMetadataFactory documentMetadataFactory,
            INormalizedUsageOrderService normalizedUsageOrderService,
            ITimeProvider timeProvider,
            IUpdateReactionManager updateReactionManager,
            IUserPreferences userPreferences)
        {
            _activeDocumentMetadata = new ObservableCollection<DocumentMetadata>();

            ActiveDocumentMetadata = collectionViewGenerator.CreateView(
                _activeDocumentMetadata);

            _documentMetadataEqualityService = documentMetadataEqualityService;
            _documentMetadataFactory = documentMetadataFactory;
            _normalizedUsageOrderService = normalizedUsageOrderService;
            _timeProvider = timeProvider;
            _userPreferences = userPreferences;

            updateReactionManager.Initialize(ActiveDocumentMetadata);
        }

        /// <summary>
        /// Adds the provided name to <see cref="ActiveDocumentMetadata"/> if
        /// not already present. Does nothing otherwise.
        /// </summary>
        /// <param name="info">
        /// Information about the document's full name and containing project
        /// </param>
        public void Add(DocumentMetadataInfo info)
        {
            var metadataExists = _activeDocumentMetadata.Any(m =>
                _documentMetadataEqualityService.Compare(info, m));

            if (!metadataExists)
            {
                var metadata = _documentMetadataFactory.Create(info);
                _activeDocumentMetadata.Add(metadata);
            }
        }

        /// <summary>
        /// Sets <see cref="DocumentMetadata.IsActive"/> to true and
        /// <see cref="DocumentMetadata.ActivatedAt"/> to the current time in UTC
        /// for the specified file. Sets <see cref="DocumentMetadata.IsActive"/>
        /// to false for all other files
        /// </summary>
        /// <param name="fullName">Full path and name of document file</param>
        public void Activate(string fullName)
        {
            if (fullName == _activatedDocument)
            {
                return;
            }

            var activated = false;

            foreach (var metadata in _activeDocumentMetadata)
            {
                metadata.IsActive = string.CompareOrdinal(metadata.FullName, fullName) == 0;

                if (metadata.IsActive)
                {
                    _activatedDocument = metadata.FullName;
                    var utcNow = _timeProvider.UtcNow;
                    metadata.ActivatedAt = utcNow;
                    activated = true;
                }
            }

            if (activated)
            {
                _normalizedUsageOrderService.SetUsageOrder(
                    _activeDocumentMetadata,
                    _userPreferences);

                ActiveDocumentMetadata.Refresh();
            }
        }

        /// <summary>
        /// Replaces the specified document metadata item with another that
        /// reflects the updated full name. Does not alter the value of
        /// <see cref="DocumentMetadata.ActivatedAt"/>
        /// </summary>
        /// <param name="newName">
        /// The new full path and name of the document file
        /// </param>
        /// <param name="oldName">
        /// Full path and name of the document file that was renamed
        /// </param>
        public void UpdateFullName(string newName, string oldName)
        {
            for (int i = 0; i < _activeDocumentMetadata.Count; i++)
            {
                var existingMetadata = _activeDocumentMetadata[i];

                var match = string.CompareOrdinal(
                    existingMetadata.CorrectedFullName,
                    oldName) == 0;

                if (match)
                {
                    var info = new DocumentMetadataInfo
                    {
                        FullName = newName,

                        ProjectDisplayName = _activeDocumentMetadata[i]
                            .ProjectNames.DisplayName,

                        ProjectFullName = _activeDocumentMetadata[i]
                            .ProjectNames.FullName
                    };

                    var newMetadata = _documentMetadataFactory.Create(
                        info,
                        _activeDocumentMetadata[i].ActivatedAt);

                    _activeDocumentMetadata[i] = newMetadata;

                    _normalizedUsageOrderService.SetUsageOrder(
                        _activeDocumentMetadata,
                        _userPreferences);

                    break;
                }
            }
        }

        /// <summary>
        /// Updates <see cref="ActiveDocumentMetadata"/> to reflect the documents
        /// in the method argument. Does not update the value of
        /// <see cref="DocumentMetadata.ActivatedAt"/> for existing metadata; sets
        /// as the current time in UTC for new metadata.
        /// </summary>
        /// <param name="documents">
        /// <see cref="Documents"/> that <see cref="ActiveDocumentMetadata"/>
        /// should reflect
        /// </param>
        /// <param name="setUsageOrder">
        /// true to update <see cref="DocumentMetadata.UsageOrder"/> for every
        /// <see cref="DocumentMetadata"/> in <see cref="ActiveDocumentMetadata"/>
        /// after Synchronization, false otherwise
        /// </param>
        public void Synchronize(Documents documents, bool setUsageOrder)
        {
            // DocumentMetadataInfo for each Document in 'documents'
            var documentsInfoSet = new HashSet<DocumentMetadataInfo>();

            // Add documents unique to method parameter collection

            try
            {
                foreach (var obj in documents)
                {
                    var document = (Document)obj;

                    if (document.ActiveWindow == null)
                    {
                        continue;
                    }

                    var info = new DocumentMetadataInfo
                    {
                        FullName = document.FullName,
                        ProjectDisplayName = document.ProjectItem.ContainingProject.Name,
                        ProjectFullName = document.ProjectItem.ContainingProject.FullName
                    };

                    documentsInfoSet.Add(info);
                    Add(info);
                }
            }
            catch (COMException)
            {
                // COMException is thrown during enumeration of 'documents'
                // when a project is closed in Visual Studio. Do nothing: this
                // will result in the active documents metadata collection
                // being emptied, which is appropriate
            }

            // Remove documents not in method parameter collection

            for (int i = 0; i < _activeDocumentMetadata.Count; i++)
            {
                var removeMetadata = documentsInfoSet.All(info =>
                    !_documentMetadataEqualityService.Compare(
                        info, _activeDocumentMetadata[i]));

                if (removeMetadata)
                {
                    _activeDocumentMetadata.RemoveAt(i);
                    i--;
                }
            }

            if (setUsageOrder)
            {
                _normalizedUsageOrderService.SetUsageOrder(
                    _activeDocumentMetadata,
                    _userPreferences);
            }
        }
    }
}