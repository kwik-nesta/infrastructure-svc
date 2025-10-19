using CSharpTypes.Extensions.Date;
using CSharpTypes.Extensions.Enumeration;
using KwikNesta.Contracts.Enums;
using KwikNesta.Infrastruture.Svc.Application.Models;
using KwikNesta.Infrastruture.Svc.Application.Queries.AuditTrails;
using KwikNesta.Infrastruture.Svc.Application.Queries.AuditTrails.Dtos;
using KwikNesta.Infrastruture.Svc.Domain.Entities;

namespace KwikNesta.Infrastruture.Svc.Application.Common.Extensions
{
    public static class SearchExtensions
    {
        public static IQueryable<Country> Search(this IQueryable<Country> query, string? search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return query;
            }

            return query.Where(s => s.Name.ToLower().Contains(search.ToLower()) || 
                                    s.Nationality.ToLower().Contains(search.ToLower()) || 
                                    s.Region.ToLower().Contains(search.ToLower()) || 
                                    s.SubRegion.ToLower().Contains(search.ToLower()));
        }

        public static IQueryable<AuditTrail> Filter(this IQueryable<AuditTrail> query, GetAuditTrailsQuery trailsQuery)
        {
            if (trailsQuery.Domain.HasValue)
            {
                query = query.Where(au => au.Domain.Equals(trailsQuery.Domain.Value));
            }
            if (trailsQuery.Action.HasValue)
            {
                query = query.Where(au => au.Action.Equals(trailsQuery.Action.Value));
            }
            if(trailsQuery.StartDate.HasValue && trailsQuery.EndDate.HasValue)
            {
                query = query.Where(au => au.Timestamp >= trailsQuery.StartDate.Value && au.Timestamp <= trailsQuery.EndDate.Value);
            }

            return query;
        }

        public static IEnumerable<AuditTrailDto> QueryData(this IEnumerable<AuditTrail> audits, List<UserDto> users)
        {
            var query = from audit in audits
                        join performer in users on audit.PerformedBy equals performer.Id into performerGroup
                        from performer in performerGroup.DefaultIfEmpty()
                        join profile in users on audit.TargetId equals profile.Id into profileGroup
                        from profile in profileGroup.DefaultIfEmpty()
                        select new AuditTrailDto
                        {
                            PerformedBy = performer != null ? string.Format("{0} {1}", performer.FirstName, performer.LastName) : "Unknown",
                            Target = profile != null && (int)audit.Domain == (int)AuditDomain.Identity && performer.Id == profile.Id ? "Self" :
                                profile != null ? string.Format("{0} {1}", profile.FirstName, profile.LastName) : "Others",
                            Timestamp = audit.Timestamp,
                            Action = audit.Action.GetDescription(),
                            Domain = audit.Domain.GetDescription()
                        };

            return query;
        }
    }
}