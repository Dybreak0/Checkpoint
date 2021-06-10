using Microsoft.AspNetCore.SignalR;
using MobileJO.Domain.Contracts;
using System;
using System.Threading.Tasks;

namespace MobileJO.API.Hubs
{
    public interface IImageHub
    {
        /// <summary>
        ///    Sends the converted image data to client
        /// </summary>
        /// <param name="src"></param>
        /// <param name="answerID"></param>
        /// <returns></returns>
        Task ReceiveImage(string src, int answerID);
    }

    public class ImageHub : Hub<IImageHub>
    {
        private readonly IResponseService _responseService;

        public ImageHub(IResponseService responseService)
        {
            _responseService = responseService;
        }

        /// <summary>
        ///    Retrieves the video from the system and convert it
        ///    to base64
        /// </summary>
        /// <param name="answerID"></param>
        /// <returns></returns>
        public async Task GetImage(int answerID)
        {
            try
            {
                byte[] byteArr = _responseService.GetMedia(answerID);
                string src = "";

                if (byteArr != null)
                {
                    src = Convert.ToBase64String(byteArr, Base64FormattingOptions.None);
                }

                await Clients.Caller.ReceiveImage(src, answerID);
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.StackTrace);
            }
        }
    }
}
