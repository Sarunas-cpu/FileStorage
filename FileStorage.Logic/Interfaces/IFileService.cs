using FileStorage.Database.Entities;
using FileStorage.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorage.Logic.Interfaces
{
    public interface IFileService
    {
        string CreateFileOrFolder([FromBody] SFileDto sfile);
        List<SFileRequest> FindByName(string name);
    }
}
