using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using EcoCentre.Models.Domain.Common.Commands;

namespace EcoCentre.Models.Domain.Giveaway
{
	public class SaveFileCommand
	{
		private readonly FileRepository _fileRepository;

		public SaveFileCommand(FileRepository fileRepository)
		{
			_fileRepository = fileRepository;
		}

		public SaveFileResult Execute(HttpPostedFileBase file)
		{
			var image = Image.FromStream(file.InputStream);
			var scaledStream = ToStream(ScaleImageDown(image, 480, 320), image.RawFormat);
			var id = _fileRepository.Save(scaledStream, file.FileName);
			return new SaveFileResult
			{
				Id = id.ToString(),
				Name = file.FileName,
				Size = file.ContentLength
			};
		}

		private Image ScaleImageDown(Image image, int maxWidth, int maxHeight)
		{
			var ratioX = (double)maxWidth / image.Width;
			var ratioY = (double)maxHeight / image.Height;
			var ratio = Math.Min(ratioX, ratioY);

			if (ratio > 1)
			{
				return image;
			}

			var newWidth = (int)(image.Width * ratio);
			var newHeight = (int)(image.Height * ratio);

			var newImage = new Bitmap(newWidth, newHeight);

			using (var graphics = Graphics.FromImage(newImage))
				graphics.DrawImage(image, 0, 0, newWidth, newHeight);

			return newImage;
		}

		private Stream ToStream(Image image, ImageFormat format)
		{
			var stream = new MemoryStream();
			image.Save(stream, format);
			stream.Position = 0;
			return stream;
		}
	}
}