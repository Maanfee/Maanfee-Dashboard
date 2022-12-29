using Maanfee.Dashboard.Domain.DAL;
using Maanfee.Dashboard.Domain.Entities.Communications;
using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Resources;
using Maanfee.Web.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Services.Controllers.Communications
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ChatsController : _BaseController
    {
        public ChatsController(_BaseContext_SQLServer context, CommonService CommonService, HttpClient http) : base(context, CommonService, http)
        {
        }

        // Used : Chats->GetUserChats
        [HttpGet("GetUserChats")]
        // GET: api/Chats/GetUserChats?IdUser=
        public async Task<CallbackResult<IEnumerable<ChatMessage>>> GetUserChats(string IdUser)
        {
            try
            {
                if (string.IsNullOrEmpty(IdUser))
                {
                    return new CallbackResult<IEnumerable<ChatMessage>>(null, new Error(ErrorCode.ChangeIsNotPossible, DashboardResource.MessageChangeIsNotPossible));
                }

                var Chats = await db_SQLServer.ChatMessages
                    .Include(x => x.FromUser)
                    .Where(x => x.IdFromUser == IdUser || x.IdToUser == IdUser)
                    .OrderBy(x => x.SendDate)
                    .ToListAsync();

                return new CallbackResult<IEnumerable<ChatMessage>>(Chats, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<IEnumerable<ChatMessage>>(null, new ExceptionError(ex.Message));
            }
        }

        // Used : Chats->Create
        [HttpPost("Create")]
        // POST: api/Chats/Create
        public async Task<CallbackResult<ChatMessage>> Create(ChatMessage Model)
        {
            try
            {
                var ChatMessage = new ChatMessage
                {
                    Id = Guid.NewGuid().ToString(),
					SendDate = Model.SendDate,
                    IdFromUser = Model.IdFromUser,
                    IdToUser = Model.IdToUser,
                    Message = Model.Message,
                };
                db_SQLServer.ChatMessages.Add(ChatMessage);

                await db_SQLServer.SaveChangesAsync();

                return new CallbackResult<ChatMessage>(Model, null);
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("Cannot insert duplicate key row in object"))
                {
                    return new CallbackResult<ChatMessage>(null, new DuplicateError(DashboardResource.MessageCannotInsertDuplicate));
                }
                else
                {
                    return new CallbackResult<ChatMessage>(null, new ExceptionError(ex.Message));
                }
            }
        }

		// Used : Chats->Edit
		[HttpPut("Edit")]
		// Usage :   var Investment = await Http.PutAsJsonAsync("api/Chats/Edit", item.TrimString());
		//[HttpPut("{id}")]
		// PUT: api/Chats/Edit/Model
		public async Task<CallbackResult<ChatMessage>> Edit(ChatMessage Model)
		{
			try
			{
				var Details = await db_SQLServer.ChatMessages.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Model.Id);
				if (Details == null)
				{
					return new CallbackResult<ChatMessage>(null, new ExceptionError(DashboardResource.MessageChangeIsNotPossible));
				}

				Details.ReadDate = DateTime.Now;

				//db_SQLServer.Entry(Details).State = EntityState.Modified;
				db_SQLServer.ChatMessages.Update(Details);
				await db_SQLServer.SaveChangesAsync();

				return new CallbackResult<ChatMessage>(Details, null, DashboardResource.MessageSavedSuccessfully);
			}
			catch (DbUpdateException ex)
			{
				if (ex.ToString().Contains("Cannot insert duplicate key row in object"))
				{
					return new CallbackResult<ChatMessage>(null, new DuplicateError(DashboardResource.MessageCannotInsertDuplicate));
				}
				else
				{
					return new CallbackResult<ChatMessage>(null, new ExceptionError(ex.Message));
				}
			}
		}


	}
}
