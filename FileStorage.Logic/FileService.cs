using FileStorage.Database;
using FileStorage.Database.Entities;
using FileStorage.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using FileStorage.Models;
using System.IO;

namespace FileStorage.Logic
{
    public class FileService : IFileService
    {
        private readonly DatabaseContext _context;

        private readonly int _maxDiresctoriesDepth = 6;
        private readonly int _rootFolderId = 1;
        public FileService(DatabaseContext context)
        {
            _context = context;
        }

        string IFileService.CreateFileOrFolder([FromBody] SFileDto sFileDto)
        {
            var parentFile = _context.SFiles.Where(file => file.Id == sFileDto.ParentId).First();

            if (Path.HasExtension(parentFile.Path))
                return $"Parent file with Id {parentFile.Id} is a file";

            string destFilePath = Path.Combine(parentFile.Path, sFileDto.Name);
            if (sFileDto.FileContents.Equals(string.Empty))
            {
                try
                {
                    // parent file path is never empty
                    var diresctoriesDepth = parentFile.Path.Split("\\").Length;
                    if (diresctoriesDepth >= _maxDiresctoriesDepth)
                        return $"Maximum depth of the file structure is {_maxDiresctoriesDepth}";

                    if (!Directory.Exists(destFilePath))
                        Directory.CreateDirectory(destFilePath);
                    else
                        return "Directory already exists";
                }
                catch (Exception ex)
                {
                    Directory.Delete(destFilePath);
                    throw ex;
                }
            }
            else
            {
                if (!System.IO.File.Exists(destFilePath))
                {
                    byte[] bytes = Convert.FromBase64String(sFileDto.FileContents);
                    System.IO.File.WriteAllBytes(destFilePath, bytes);
                }
                else
                    return "File already exists";
            }

            var sFile = new SFile()
            {
                ParentId = sFileDto.ParentId,
                RootId = sFileDto.RootId,
                Name = sFileDto.Name,
                Path = destFilePath,
                CreatedOn = DateTime.Now,
                CreatedBy = sFileDto.CreatedBy,
                IsDeleted = sFileDto.IsDeleted
            };

            _context.SFiles.Add(sFile);
            _context.SaveChanges();

            return sFileDto.FileContents.Equals(string.Empty) ? "Directory have been created" : "File have been created";
        }

        List<SFileRequest> IFileService.FindByName(string name)
        {
            var Sfiles = _context.SFiles.Where(file => file.Name.Contains(name) && file.RootId == _rootFolderId)
                                        .Select(file => new SFileRequest()
                                                                            {
                                                                                ParentId = file.ParentId,
                                                                                RootId = file.RootId,
                                                                                Name = file.Name,
                                                                                Path = file.Path,
                                                                                CreatedOn = file.CreatedOn,
                                                                                CreatedBy = file.CreatedBy,
                                                                                IsDeleted = file.IsDeleted
                                                                            }
                                        ).ToList();

            foreach(var sfile in Sfiles)
            {
                if (Path.HasExtension(sfile.Path))
                {
                    byte[] bytes = File.ReadAllBytes(sfile.Path);
                    sfile.FileContents = Convert.ToBase64String(bytes);
                }
            }

            return Sfiles;
        }
    }
}
