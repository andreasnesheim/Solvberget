﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Implementation
{
    public class LibraryListXmlRepository : IListRepository
    {
        private const string StdFolderPath = @"App_Data\librarylists\";
        private readonly string _folderPath;
        private readonly IRepository _repository;
        private readonly IImageRepository _imageRepository;

        public LibraryListXmlRepository(IRepository repository, IImageRepository imageRepository, string folderPath = null)
        {
            _repository = repository;
            _imageRepository = imageRepository;
            _folderPath = string.IsNullOrEmpty(folderPath) ? StdFolderPath : folderPath;
        }

        public List<LibraryList> GetLists(int? limit = null)
        {
            var lists = new ConcurrentBag<LibraryList>();

            Directory.EnumerateFiles(_folderPath, "*.xml").AsParallel().ToList().ForEach(file => lists.Add(LibraryList.GetLibraryListFromXml(file)));

            lists.ToList().ForEach(liblist => { if (liblist != null) AddContentToList(liblist); });

            return limit != null 
                ? lists.OrderBy(list => list.Priority).Take((int)limit).ToList() 
                : lists.OrderBy(list => list.Priority).ToList();
        }

        private void AddContentToList(LibraryList libraryList)
        {
            foreach(var docnr in libraryList.DocumentNumbers)
            {
                Document document = (_repository.GetDocument(docnr, true));
                //We want to add the thumbnail url to the document in this case
                document.ThumbnailUrl = _imageRepository.GetDocumentThumbnailImage(docnr, "60");
                libraryList.Documents.Add(document);
            }
        }

    }
}
