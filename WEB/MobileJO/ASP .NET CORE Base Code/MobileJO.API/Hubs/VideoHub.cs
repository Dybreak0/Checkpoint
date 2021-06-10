using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using MobileJO.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MobileJO.API.Hubs
{
    public interface IVideoHub
    {
        /// <summary>
        ///    Sends the converted video data to client
        /// </summary>
        /// <param name="src"></param>
        /// <param name="answerID"></param>
        /// <returns></returns>
        Task ReceiveVideo(string src, int answerID);
    }

    public class VideoHub : Hub<IVideoHub>
    {
        private readonly IResponseService _responseService;

        public VideoHub(IResponseService responseService)
        {
            _responseService = responseService;
        }

        /// <summary>
        ///    Retrieves the video from the system and convert it
        ///    to base64 in chunks
        /// </summary>
        /// <param name="answerID"></param>
        /// <returns></returns>
        public async Task GetVideo(int answerID)
        {
            try
            {
                byte[] byteArr = _responseService.GetMedia(answerID);
                string src = "";

                if (byteArr != null)
                {
                    src = Convert.ToBase64String(byteArr, Base64FormattingOptions.None);
                }

                await Clients.Caller.ReceiveVideo(src, answerID);
                await Task.Delay(500);
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.StackTrace);
            }
        }
    }
}
