using System.IO;
using System.Web;

namespace EcoCentre.Models.Domain.Common.Commands
{
	public class SaveTmpFileCommand
	{
		private readonly FileRepository _fileRepository;

		public SaveTmpFileCommand(FileRepository fileRepository)
		{
			_fileRepository = fileRepository;
		}

		public SaveFileResult Execute(HttpPostedFileBase file)
		{			
			var id = _fileRepository.Save(file.InputStream, file.FileName);
			return new SaveFileResult
			{
				Id = id.ToString(),
				Name = file.FileName,
				Size = file.ContentLength
			};
		}
	}
}