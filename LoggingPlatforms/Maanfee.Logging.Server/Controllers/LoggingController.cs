using Maanfee.Dashboard.Resources;
using Maanfee.Logging.Domain;
using Maanfee.Logging.Domain.DAL;
using Maanfee.Web.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace Maanfee.Logging.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoggingController : Controller
    {
        public LoggingController(_BaseContext_SQLServer context, ILogger<LoggingController> logger)
        {
            db_SQLServer = context;
            Logger = logger;
        }

        protected readonly _BaseContext_SQLServer db_SQLServer;

        protected readonly ILogger<LoggingController> Logger;

        // Used : Logging->PaginationIndex
        [HttpPost("PaginationIndex")]
        // POST: api/Logging/PaginationIndex
        public async Task<CallbackResult<PaginatedListViewModel<LogInfo>>> PaginationIndex(LogTableStateViewModel<FilterLogViewModel> TableState)
        {
            try
            {
                PaginatedList<LogInfo> PaginatedList;

                IQueryable<LogInfo> Data = db_SQLServer.LogInfos.AsNoTracking()
                    .Include(x => x.LoggingPlatform)
                    .Include(x => x.LoggingLevel)
                    .OrderByDescending(x => x.LogDate);

                switch (TableState.state.SortLabel)
                {
                    case "Message":
                        Data = Data.OrderByDirection(TableState.state.SortDirection, o => o.Message);
                        break;
                }

                if (TableState.Filter != null)
                {
                    if (!string.IsNullOrEmpty(TableState.Filter.Message))
                    {
                        Data = Data.Where(p => p.Message.Contains(TableState.Filter.Message));
                    }
                    if (TableState.Filter.LogDateFrom.HasValue && TableState.Filter.LogDateTo.HasValue)
                    {
                        Data = Data.Where(p => p.LogDate.Date >= TableState.Filter.LogDateFrom.Value.Date
                         && p.LogDate.Date <= TableState.Filter.LogDateTo.Value.Date);
                    }

                    PaginatedList = await PaginatedList<LogInfo>.CreateAsync(Data, TableState.state.Page, TableState.state.PageSize);
                }
                else
                {
                    PaginatedList = await PaginatedList<LogInfo>.CreateAsync(Data, TableState.state.Page, TableState.state.PageSize);
                }

                return new CallbackResult<PaginatedListViewModel<LogInfo>>(new PaginatedListViewModel<LogInfo> { List = PaginatedList.List, TotalPages = PaginatedList.TotalPages, }, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<PaginatedListViewModel<LogInfo>>(null, new ExceptionError(ex.ToString()));
            }
        }

        // Used : Logging->Create
        [HttpPost("Create")]
        // POST: api/Logging/Create
        public async Task<CallbackResult<SubmitLogInfo>> Create(SubmitLogInfo Model)
        {
            try
            {
                var LogInfo = new LogInfo
                {
                    Id = Guid.NewGuid().ToString(),
                    IdLoggingLevel = Model.IdLoggingLevel,
                    IdLoggingPlatform = Model.IdLoggingPlatform,
                    LogDate = Model.LogDate,
                    Message = Model.Message,
                };
                db_SQLServer.LogInfos.Add(LogInfo);

                Logger.LogInformation($"{Model.Message} : {Model.LogDate.ToShortDateString()}");

                await db_SQLServer.SaveChangesAsync();

                return new CallbackResult<SubmitLogInfo>(Model, null);
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("Cannot insert duplicate key row in object"))
                {
                    return new CallbackResult<SubmitLogInfo>(null, new DuplicateError(DashboardResource.MessageCannotInsertDuplicate));
                }
                else
                {
                    return new CallbackResult<SubmitLogInfo>(null, new ExceptionError(ex.Message));
                }
            }
        }


    }
}
