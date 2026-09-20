using Corp.Core.Libraries.Interface;
using System.ServiceModel;

namespace Corp.Core.Libraries.Proxy
{
    public class FileTransferServiceClient : ClientBase<IFileTransferService>, IFileTransferService 
    {
        public RemoteFileInfo DownloadFile(DownloadRequest request)
        {
            return base.Channel.DownloadFile(request);
        }
        public string GetFileContents(string fileName)
        {
            return base.Channel.GetFileContents(fileName);
        }
    }
}
