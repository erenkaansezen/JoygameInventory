//using JoygameInventory.Data.Context;
//using JoygameInventory.Data.Entities;
//using Microsoft.EntityFrameworkCore;

//namespace JoygameInventory.Business.Services
//{
//    public class DocumentService
//    {
//        private readonly InventoryContext _context;

//        public DocumentService(InventoryContext context)
//        {
//            _context = context;
//        }

//        public async Task<IEnumerable<ZimmetDocument>> GetZimmetDocumentsAsync(string userId)
//        {
//            var zimmetDocuments = await _context.ZimmetDocuments
//                .Where(zd => zd.UserId == userId) // Kullanıcıya ait zimmet dosyalarını al
//                .OrderByDescending(zd => zd.DateUploaded) // En son yüklenen dosyalar üstte olsun
//                .ToListAsync();

//            return zimmetDocuments;
//        }

//        public async Task<ZimmetDocument> GetZimmetDocumentByIdAsync(int documentId)
//        {
//            return await _context.ZimmetDocuments
//                .FirstOrDefaultAsync(d => d.Id == documentId);
//        }
//    }
//}
