﻿using Microsoft.AspNetCore.Mvc;
using SampleWebApiAspNetCore.Models;

namespace SampleWebApiAspNetCore.Services
{
    public interface ILinkService<T>
    {
        object ExpandSingleCSGOItem(object resource, int identifier, ApiVersion version);

        List<LinkDto> CreateLinksForCollection(QueryParameters queryParameters, int totalCount, ApiVersion version);
    }
}
