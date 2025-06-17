using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context;

        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;

            if (minDate.HasValue)
            {
                var minDateUtc = DateTime.SpecifyKind(minDate.Value, DateTimeKind.Utc);
                result = result.Where(x => x.Date >= minDateUtc);
            }

            if (maxDate.HasValue)
            {
                var maxDateUtc = DateTime.SpecifyKind(maxDate.Value, DateTimeKind.Utc);
                result = result.Where(x => x.Date <= maxDateUtc);
            }

            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;

            if (minDate.HasValue)
            {
                var minDateUtc = DateTime.SpecifyKind(minDate.Value, DateTimeKind.Utc);
                result = result.Where(x => x.Date >= minDateUtc);
            }

            if (maxDate.HasValue)
            {
                var maxDateUtc = DateTime.SpecifyKind(maxDate.Value, DateTimeKind.Utc);
                result = result.Where(x => x.Date <= maxDateUtc);
            }

            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .GroupBy(x => x.Seller.Department)
                .ToListAsync();
        }
    }
}
