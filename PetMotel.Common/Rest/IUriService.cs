using System;
using PetMotel.Common.Rest.Model;

namespace PetMotel.Common.Rest
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}