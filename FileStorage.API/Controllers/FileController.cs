using FileStorage.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileStorage.Logic.Interfaces;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;
using System.IO.Compression;
using FileStorage.Database.Entities;
using FileStorage.Logic.Helpers;
using System.Text;
using FileStorage.Models;

namespace FileStorage.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        private readonly ILogger<FileController> _logger;

        public FileController(IFileService fileService, ILogger<FileController> logger)
        {
            _fileService = fileService;
        }

        [HttpGet("files/{searchString}")]
        public IActionResult Get(string searchString)
        {
            try
            {
                var filesFound = _fileService.FindByName(searchString);
                if (filesFound.Count == 0)
                    return Ok("File does not exist with given name");

                return Ok(filesFound);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("filesDownload/{searchString}")]
        public async Task<ActionResult> DownloadFile(string searchString)
        {
            var filesFound = _fileService.FindByName(searchString);
            if (filesFound.Count == 0)
                return Ok("File does not exist with given name");


            string filePath;
            byte[] fileBytes;
            string fileContentType = "application/zip";
            string tempDirectory = string.Empty;
            try
            {
                Utils.ZipNeededFiles(filesFound, out tempDirectory, out filePath);

                // read zip file if it was created else read file
                var provider = new FileExtensionContentTypeProvider();
                if (tempDirectory.Length == 0 && provider.TryGetContentType(filePath, out var contentType))
                {
                    fileContentType = contentType;
                    fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                }
                else
                {
                    fileBytes = await System.IO.File.ReadAllBytesAsync(tempDirectory);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (System.IO.File.Exists(tempDirectory))
                    System.IO.File.Delete(tempDirectory);
            }

            return File(fileBytes, fileContentType, Path.GetFileName(filePath));
        }

        [HttpPost("createFile")]
        public IActionResult CreateFileOrFolder([FromBody] SFileDto file)
        {
            try
            {
                string statusMsg = _fileService.CreateFileOrFolder(file);
                return Ok($"{statusMsg}: {file.Name}");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
