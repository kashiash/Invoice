using Common.Module.Utils;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Invoice.Blazor.Server.Dtos;
using Invoice.Module.BusinessObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoice.Blazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> UploadFile(IFormFile myFile, [FromQuery] UploadFileDto uploadFileDto)
        {
            XpoTypesInfoHelper.GetXpoTypeInfoSource();
            XafTypesInfo.Instance.RegisterEntity(typeof(AssignedFileData));
            XafTypesInfo.Instance.RegisterEntity(typeof(ProjectTask));
            XafTypesInfo.Instance.RegisterEntity(typeof(FileData));
            XafTypesInfo.Instance.RegisterEntity(typeof(Project));
            XafTypesInfo.Instance.RegisterEntity(typeof(Customer));

            try
            {
                using var directProvider = new XPObjectSpaceProvider(AppSettings.ConnectionString, null, true);
                {
                    using var objectSpace = directProvider.CreateObjectSpace();
                    var task = await objectSpace.GetObjectsQuery<ProjectTask>().Where(x => x.Oid == uploadFileDto.TaskId).FirstOrDefaultAsync();
                    var project = await objectSpace.GetObjectsQuery<Project>().Where(x => x.Oid == uploadFileDto.ProjectId).FirstOrDefaultAsync();
                    var customer = await objectSpace.GetObjectsQuery<Customer>().Where(x => x.Oid == uploadFileDto.CustomerId).FirstOrDefaultAsync();

                    var assignedFile = objectSpace.CreateObject<AssignedFileData>();

                    assignedFile.Task = task;
                    assignedFile.Project = project;
                    assignedFile.Customer = customer;
                    assignedFile.File = objectSpace.CreateObject<FileData>();
                    assignedFile.File.LoadFromStream(myFile.FileName, myFile.OpenReadStream());
                    objectSpace.CommitChanges();
                }
            }
            catch
            {
                return BadRequest();
            }
            return new EmptyResult();
        }
    }
}
