using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FileUploadAngular5WithAsp.NetCore.Controllers
{
	[Produces("application/json")]
	[Route("api/[controller]")]
	public class UploadController : Controller
	{
		private IHostingEnvironment _hostingEnvironment;

		public UploadController(IHostingEnvironment hostingEnvironment)
		{
			_hostingEnvironment = hostingEnvironment;
		}

		[HttpPost, DisableRequestSizeLimit]
		public ActionResult UploadFile()
		{
			try
			{
				var file = Request.Form.Files[0];
				string folderName = "Upload";
				string webRootPath = _hostingEnvironment.WebRootPath;
				string newPath = Path.Combine(webRootPath, folderName);
				if (!Directory.Exists(newPath))
				{
					Directory.CreateDirectory(newPath);
				}
				if (file.Length > 0)
				{
					string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
					string fullPath = Path.Combine(newPath, fileName);
					using (var stream = new FileStream(fullPath, FileMode.Create))
					{
						file.CopyTo(stream);
					}
				}
				return Json("Upload Successful.");
			}
			catch (System.Exception ex)
			{
				return Json("Upload Failed: " + ex.Message);
			}
		}

	}
}
