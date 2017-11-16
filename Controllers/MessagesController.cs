using System.Threading.Tasks;
using System.Web.Http;
using System;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System.Net.Http;
using System.Web.Http.Description;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Text;

namespace Microsoft.Bot.Sample.FormBot
{
    
    
    
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private static readonly HttpClient client = new HttpClient();
        
        internal static IDialog<ConferenceEntry> MakeRootDialog()
        {
            return Chain.From(() => FormDialog.FromForm(ConferenceEntry.BuildForm)).Do(async (context,formResult)=>{
                var completed = await formResult;
                var jsoncontent = JsonConvert.SerializeObject(completed);
                Console.Write(jsoncontent);
                var content = new StringContent(jsoncontent, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://prod-02.northeurope.logic.azure.com:443/workflows/ebb5d46a8ea048ad92bb93910c124b10/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=Rg_B-x0EsW0xxt1t_CPS6pJg3MOFQLbezLEFKxcGxJw", content);
            });
        }

        /// <summary>
        /// POST: api/Messages
        /// receive a message from a user and send replies
        /// </summary>
        /// <param name="activity"></param>
        [ResponseType(typeof(void))]
        public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            if (activity != null)
            {
                // one of these will have an interface and process it
                switch (activity.GetActivityType())
                {
                    case ActivityTypes.Message:
                        await Conversation.SendAsync(activity, MakeRootDialog);
                        break;

                    case ActivityTypes.ConversationUpdate:
                    case ActivityTypes.ContactRelationUpdate:
                    case ActivityTypes.Typing:
                    case ActivityTypes.DeleteUserData:
                    default:
                        Trace.TraceError($"Unknown activity type ignored: {activity.GetActivityType()}");
                        break;
                }
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }
    }
}