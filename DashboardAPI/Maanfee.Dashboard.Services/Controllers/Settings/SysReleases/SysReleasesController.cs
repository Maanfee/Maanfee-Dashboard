using Maanfee.Dashboard.Domain.DAL;
using Maanfee.Dashboard.Domain.Entities;
using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Resources;
using Maanfee.Web.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace Maanfee.Dashboard.Services.Controllers.Settings.SysReleases
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SysReleasesController : ControllerBase
    {
        public SysReleasesController(_BaseContext_SQLite context, CommonService commonService)
        {
            db_SQLite = context;
            CommonService = commonService;
        }

        protected readonly _BaseContext_SQLite db_SQLite;
        protected readonly CommonService CommonService;

        // Used : 
        [HttpPost("PaginationIndex")]
        // GET: api/SysReleases/PaginationIndex?pageNumber=1&pageSize=10
        public async Task<CallbackResult<PaginatedList<SysRelease>>> PaginationIndex(TableStateViewModel<FilterReleaseViewModel> TableState)
        {
            try
            {
                PaginatedList<SysRelease> PaginatedList;

                IQueryable<SysRelease> Data = db_SQLite.SysReleases
                    .OrderByDescending(x => x.ReleaseDate).ThenBy(x => x.Version);

                switch (TableState.state.SortLabel)
                {
                    case "Version":
                        Data = Data.OrderByDirection(TableState.state.SortDirection, o => o.Version);
                        break;
                    case "Date":
                        Data = Data.OrderByDirection(TableState.state.SortDirection, o => o.ReleaseDate);
                        break;
                    case "IsActive":
                        Data = Data.OrderByDirection(TableState.state.SortDirection, o => o.IsActive);
                        break;
                }

                if (TableState.Filter != null)
                {
                    if (!string.IsNullOrEmpty(TableState.Filter.Version))
                    {
                        Data = Data.Where(p => p.Version == TableState.Filter.Version);
                    }
                    //if (TableState.Filter.BazdashtDateFrom.HasValue && TableState.Filter.BazdashtDateTo.HasValue)
                    //{
                    //    Data = Data.Where(p => p.BazdashtDate.Value.Date >= TableState.Filter.BazdashtDateFrom.Value.Date
                    //     && p.BazdashtDate.Value.Date <= TableState.Filter.BazdashtDateTo.Value.Date);
                    //}

                    PaginatedList = await PaginatedList<SysRelease>.CreateAsync(Data, TableState.state.Page, TableState.state.PageSize);
                }
                else
                {
                    PaginatedList = await PaginatedList<SysRelease>.CreateAsync(Data, TableState.state.Page, TableState.state.PageSize);
                }

                return new CallbackResult<PaginatedList<SysRelease>>(PaginatedList, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<PaginatedList<SysRelease>>(null, new ExceptionError(ex.ToString()));
            }
        }

        // Used : SysReleases->Edit  (GetModel)
        // Used : SysReleases->Details 
        [HttpGet("Details/{Id}")]
        // GET: api/SysReleases/Details/5
        public async Task<CallbackResult<SysRelease>> Details(string Id)
        {
            try
            {
                if (string.IsNullOrEmpty(Id))
                {
                    return new CallbackResult<SysRelease>(null, new Error(ErrorCode.ChangeIsNotPossible, DashboardResource.MessageChangeIsNotPossible));
                }

                var Model = await db_SQLite.SysReleases.AsNoTracking()
                    .Include(x => x.SysReleaseFeatures).ThenInclude(x => x.SysReleaseType)
                    .FirstOrDefaultAsync(x => x.Id == Id);

                return new CallbackResult<SysRelease>(Model, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<SysRelease>(null, new ExceptionError(ex.ToString()));
            }
        }

        [HttpPost("Create")]
        // POST: api/SysReleases/Create
        public async Task<CallbackResult<SubmitReleaseViewModel>> Create(SubmitReleaseViewModel Model)
        {
            var SQLTransaction = db_SQLite.Database.BeginTransaction();

            try
            {
                var SysRelease = new SysRelease()
                {
                    Id = Guid.NewGuid().ToString(),
                    IsActive = Model.IsActive,
                    ReleaseDate = Model.ReleaseDate,
                    Version = Model.Version,
                };
                db_SQLite.SysReleases.Add(SysRelease);

                #region - Sys Release Features -

                if (Model != null && Model.SysReleaseFeatureViewModels.Any())
                {
                    foreach (var item in Model.SysReleaseFeatureViewModels)
                    {
                        db_SQLite.SysReleaseFeatures.Add(
                            new SysReleaseFeature
                            {
                                Id = item.Id,
                                IdSysRelease = SysRelease.Id,
                                IdSysReleaseType = item.IdSysReleaseType,
                                Comment = item.Comment,
                                FeatureDate = item.FeatureDate,
                            });
                    }
                }

                #endregion

                await db_SQLite.SaveChangesAsync();
                SQLTransaction.Commit();
                return new CallbackResult<SubmitReleaseViewModel>(Model, null);
            }
            catch (Exception ex)
            {
                await SQLTransaction.RollbackAsync();

                if (ex.ToString().Contains("Cannot insert duplicate key row in object") || ex.ToString().Contains("UNIQUE constraint failed"))
                {
                    return new CallbackResult<SubmitReleaseViewModel>(null, new DuplicateError(DashboardResource.MessageCannotInsertDuplicate));
                }
                else
                {
                    return new CallbackResult<SubmitReleaseViewModel>(null, new ExceptionError(ex.InnerException.Message));
                }
            }
        }

        // Used : 
        [HttpPut("Edit")]
        // Put: api/Profiles/Edit
        public async Task<CallbackResult<SubmitReleaseViewModel>> Edit(SubmitReleaseViewModel Model)
        {
            var SQLTransaction = db_SQLite.Database.BeginTransaction();

            try
            {
                var SysRelease = await db_SQLite.SysReleases.FirstOrDefaultAsync(x => x.Id == Model.Id);
                SysRelease.Version = Model.Version;
                SysRelease.IsActive = Model.IsActive;
                SysRelease.ReleaseDate = Model.ReleaseDate;

                db_SQLite.SysReleases.Update(SysRelease);

                #region - Sys Release Features -

                if (Model != null && Model.SysReleaseFeatureViewModels.Any())
                {
                    var StoredSysReleaseFeatures = await db_SQLite.SysReleaseFeatures.Where(x => x.IdSysRelease == SysRelease.Id).ToListAsync();
                    var AvailableSysReleaseFeatures = Model.SysReleaseFeatureViewModels.Select(Sys => new SysReleaseFeature
                    {
                        Id = Guid.NewGuid().ToString(),
                        IdSysRelease = SysRelease.Id,
                        Comment = Sys.Comment,
                        IdSysReleaseType = Sys.IdSysReleaseType,
                        FeatureDate = Sys.FeatureDate,
                    });

                    db_SQLite.RemoveRange(StoredSysReleaseFeatures.Except(AvailableSysReleaseFeatures));
                    db_SQLite.AddRange(AvailableSysReleaseFeatures.Except(StoredSysReleaseFeatures));
                }

                #endregion

                await db_SQLite.SaveChangesAsync();
                SQLTransaction.Commit();
                return new CallbackResult<SubmitReleaseViewModel>(Model, null);
            }
            catch (Exception ex)
            {
                await SQLTransaction.RollbackAsync();

                if (ex.ToString().Contains("Cannot insert duplicate key row in object") || ex.ToString().Contains("UNIQUE constraint failed"))
                {
                    return new CallbackResult<SubmitReleaseViewModel>(null, new DuplicateError(DashboardResource.MessageCannotInsertDuplicate));
                }
                else
                {
                    return new CallbackResult<SubmitReleaseViewModel>(null, new ExceptionError(ex.InnerException.Message));
                }
            }
        }

        [HttpGet("GetSysReleaseTypes")]
        // GET: api/SysReleases/GetSysReleaseTypes
        public async Task<CallbackResult<IEnumerable<SysReleaseType>>> GetSysReleaseTypes()
        {
            try
            {
                var list = await db_SQLite.SysReleaseTypes.ToListAsync();

                return new CallbackResult<IEnumerable<SysReleaseType>>(list, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<IEnumerable<SysReleaseType>>(null, new ExceptionError(ex.Message));
            }
        }

        // Used : Roles->Delete        
        [HttpDelete("Delete/{Id}")]
        // GET: api/SysReleases/Delete/1
        public async Task<CallbackResult<SysRelease>> Delete(string Id)
        {
            try
            {
                if (string.IsNullOrEmpty(Id))
                {
                    return new CallbackResult<SysRelease>(null, new Error(ErrorCode.ChangeIsNotPossible, DashboardResource.MessageChangeIsNotPossible));
                }

                var Model = await db_SQLite.SysReleases.FirstOrDefaultAsync(x => x.Id == Id);
                if (Model == null)
                {
                    return new CallbackResult<SysRelease>(null, new Error(ErrorCode.ChangeIsNotPossible, DashboardResource.MessageChangeIsNotPossible));
                }

                db_SQLite.Remove(Model);
                await db_SQLite.SaveChangesAsync();

                return new CallbackResult<SysRelease>(Model, null);
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("The DELETE statement conflicted with the "))
                {
                    return new CallbackResult<SysRelease>(null, new DeleteError(DashboardResource.MessageDeleteConstraint));
                }
                else
                {
                    return new CallbackResult<SysRelease>(null, new ExceptionError(ex.InnerException.Message));
                }
            }
        }

        // *******************************************

        // Used : 
        [HttpGet("GetAllReleases")]
        public async Task<CallbackResult<List<SysRelease>>> GetAllReleases()
        {
            try
            {
                var Data = await db_SQLite.SysReleases.AsNoTracking().AsSplitQuery()
                    .Include(x => x.SysReleaseFeatures).ThenInclude(x => x.SysReleaseType)
                    .Where(x => x.IsActive)
                    .OrderByDescending(x => x.ReleaseDate)
                    .ToListAsync();

                return new CallbackResult<List<SysRelease>>(Data, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<List<SysRelease>>(null, new ExceptionError(ex.ToString()));
            }
        }

        [HttpGet("GetLatestRelease")]
        // GET: api/SysReleases/GetLatestRelease
        public async Task<CallbackResult<SysRelease>> GetLatestRelease()
        {
            try
            {
                var Model = await db_SQLite.SysReleases.AsNoTracking()
                    .OrderByDescending(x => x.ReleaseDate)
                    .FirstOrDefaultAsync(x => x.IsActive);

                return new CallbackResult<SysRelease>(Model, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<SysRelease>(null, new ExceptionError(ex.ToString()));
            }
        }

    }
}
