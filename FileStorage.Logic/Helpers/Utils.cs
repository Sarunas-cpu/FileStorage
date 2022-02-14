﻿using FileStorage.Database.Entities;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace FileStorage.Logic.Helpers
{
    public static class Utils
    {
        public static string ZipNeededFiles(List<SFile> filesFound, out string tempDirectory, out string filePath)
        {
            tempDirectory = string.Empty;
            if (filesFound.Count > 1)
                tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            var provider = new FileExtensionContentTypeProvider();
            filePath = string.Empty;
            foreach (SFile file in filesFound)
            {
                filePath = file.Path;

                // check if its file or a folder, folders needs to be zipped. If more than one file is returned it is zipped
                if (provider.TryGetContentType(filePath, out var contentType))
                {
                    System.IO.File.Copy(filePath, tempDirectory);
                }
                else
                {
                    if (tempDirectory.Equals(string.Empty))
                        tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

                    ZipFile.CreateFromDirectory(filePath, tempDirectory);
                }
            }

            return tempDirectory;
        }
    }
}
