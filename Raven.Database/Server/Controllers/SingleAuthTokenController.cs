﻿using System.Net;
using System.Net.Http;
using System.Web.Http;
using Raven.Database.Extensions;
using Raven.Database.Server.Security;
using Raven.Database.Server.WebApi.Attributes;

namespace Raven.Database.Server.Controllers
{
    public class DbSingleAuthTokenController : BaseDatabaseApiController
    {
        [HttpGet]
        [RavenRoute("singleAuthToken")]
        [RavenRoute("databases/{databaseName}/singleAuthToken")]
        public HttpResponseMessage SingleAuthGet()
        {
            var authorizer = (MixedModeRequestAuthorizer) ControllerContext.Configuration.Properties[typeof (MixedModeRequestAuthorizer)];
            bool shouldCheckIfMachineAdmin = false;


            if ((ResourceName == "<system>" || string.IsNullOrEmpty(ResourceName)) && bool.TryParse(GetQueryStringValue("CheckIfMachineAdmin"), out shouldCheckIfMachineAdmin) && shouldCheckIfMachineAdmin)
            {

                if (!User.IsAdministrator(AnonymousUserAccessMode.None) &&
                    DatabasesLandlord.SystemConfiguration.AnonymousUserAccessMode != AnonymousUserAccessMode.Admin)
                {
                    return GetMessageWithObject(new
                    {
                        Reason = "User is null or not authenticated"
                    }, HttpStatusCode.Unauthorized);
                }

            }

            var token = authorizer.GenerateSingleUseAuthToken(ResourceName, User);

            return GetMessageWithObject(new
            {
                Token = token
            });
        }
    }
}

namespace Raven.Database.FileSystem.Controllers
{

    public class FsSingleAuthTokenController : BaseFileSystemApiController
    {
        [HttpGet]
        [RavenRoute("fs/{fileSystemName}/singleAuthToken")]
        public HttpResponseMessage SingleAuthGet()
        {
            var authorizer = (MixedModeRequestAuthorizer) ControllerContext.Configuration.Properties[typeof (MixedModeRequestAuthorizer)];

            var token = authorizer.GenerateSingleUseAuthToken(TenantNamePrefix + ResourceName, User);

            return GetMessageWithObject(new
            {
                Token = token
            });
        }
    }
}

namespace Raven.Database.Counters.Controllers
{
    public class CounterSingleAuthTokenController : BaseCountersApiController
    {
        [HttpGet]
        [RavenRoute("cs/{counterStorageName}/singleAuthToken")]
        public HttpResponseMessage SingleAuthGet()
        {
            var authorizer = (MixedModeRequestAuthorizer) ControllerContext.Configuration.Properties[typeof (MixedModeRequestAuthorizer)];

            var token = authorizer.GenerateSingleUseAuthToken(TenantNamePrefix + ResourceName, User);

            return GetMessageWithObject(new
            {
                Token = token
            });
        }
    }
}

namespace Raven.Database.TimeSeries.Controllers
{
	public class TimeSeriesSingleAuthTokenController : BaseTimeSeriesApiController
    {
        [HttpGet]
        [RavenRoute("ts/{timeSeriesName}/singleAuthToken")]
        public HttpResponseMessage SingleAuthGet()
        {
            var authorizer = (MixedModeRequestAuthorizer) ControllerContext.Configuration.Properties[typeof (MixedModeRequestAuthorizer)];

            var token = authorizer.GenerateSingleUseAuthToken(TenantNamePrefix + ResourceName, User);

            return GetMessageWithObject(new
            {
                Token = token
            });
        }
    }
}
