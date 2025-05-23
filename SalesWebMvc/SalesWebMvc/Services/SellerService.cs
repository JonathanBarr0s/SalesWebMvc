using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;

namespace SalesWebMvc.Services
{
	public class SellerService
	{
		private readonly SalesWebMvcContext _context;

		public SellerService(SalesWebMvcContext context)
		{
			_context = context;
		}

		public List<Seller> FindAll()
		{
			return _context.Seller.ToList();
		}

		public void Insert(Seller obj)
		{
			if (obj.BirthDate.Kind == DateTimeKind.Unspecified)
			{
				obj.BirthDate = DateTime.SpecifyKind(obj.BirthDate, DateTimeKind.Utc);
			}

			var maxId = _context.Seller.Max(s => s.Id);
			_context.Database.ExecuteSqlRaw($"SELECT setval('\"Seller_Id_seq\"', {maxId})");

			_context.Add(obj);
			_context.SaveChanges();
		}

		public Seller FindById(int id)
		{
			return _context.Seller.FirstOrDefault(obj => obj.Id == id);
		}

		public void Remove(int id)
		{
			var obj = _context.Seller.Find(id);
			_context.Seller.Remove(obj);
			_context.SaveChanges();
		}
	}
}
