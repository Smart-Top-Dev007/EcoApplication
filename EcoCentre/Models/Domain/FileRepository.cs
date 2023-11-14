using System.IO;
using EcoCentre.Models.Infrastructure;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace EcoCentre.Models.Domain
{
    public class FileRepository
    {
        private readonly UnitOfWork _unitOfWork;

        public FileRepository(UnitOfWork unitOfWork)
        {
	        _unitOfWork = unitOfWork;
        }

        public ObjectId Save(Stream file, string name)
        {
			var bucket = new GridFSBucket(_unitOfWork.Database);
	        return bucket.UploadFromStream(name, file);
        }

        public GridFSFileInfo Find(string id)
        {
	        var bucket = new GridFSBucket(_unitOfWork.Database);
	        var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", new ObjectId(id));
			return bucket.Find(filter).FirstOrDefault();
        }
		public byte[] GetBytes(string id)
        {
	        var bucket = new GridFSBucket(_unitOfWork.Database);
	        return bucket.DownloadAsBytes(new ObjectId(id));
        }
    }
}