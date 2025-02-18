using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace JoygameInventory.Business.Services
{
    public class AssigmentService : IAssigmentService
    {
        private readonly InventoryContext _context;

        public AssigmentService(InventoryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InventoryAssigment>> GetUserAssignmentsAsync(int userId)
        {
            var inventoryAssignments = await _context.InventoryAssigments
                .Include(ia => ia.Product)
                .Include(ia => ia.User)
                .Where(ia => ia.UserId == userId)
                .ToListAsync();

            return inventoryAssignments;
        }

        // Ürüne ait envanter atamaları
        public async Task<IEnumerable<InventoryAssigment>> GetProductAssignmentsAsync(int productId)
        {
            var inventoryAssignments = await _context.InventoryAssigments
                .Include(ia => ia.Product)
                .Include(ia => ia.User)
                .Where(ia => ia.ProductId == productId)
                .ToListAsync();

            return inventoryAssignments;
        }

        public async Task<List<Category>> GetAllStaffsAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<InventoryAssigment> GetAssignmentByIdAsync(int inventoryAssigmentId)
        {
            var inventoryAssignments = await _context.InventoryAssigments
                .Where(a => a.Id == inventoryAssigmentId)
                .FirstOrDefaultAsync();
            return inventoryAssignments;
        }

        public async Task<List<InventoryAssigment>> GetPreviousAssignmentsAsync(int productId)
        {
            var previousAssignments = await _context.InventoryAssigments
                .Where(pa => pa.ProductId == productId && pa.PreviousAssigmenId != 0)
                .OrderByDescending(pa => pa.AssignmentDate)
                .ToListAsync();

            return previousAssignments;
        }

        public async Task<JoyStaff> GetPreviousUserByIdAsync(int previousUserId)
        {
            var previousUser = await _context.JoyStaffs
                .Where(user => user.Id == previousUserId)
                .FirstOrDefaultAsync();

            return previousUser;
        }

        public async Task UpdateAssigmentAsync(InventoryAssigment assignment)
        {
            if (assignment == null)
            {
                throw new ArgumentNullException(nameof(assignment), "Atama verisi boş olamaz.");
            }

            var existingAssignment = await _context.InventoryAssigments
                .FirstOrDefaultAsync(a => a.Id == assignment.Id);

            existingAssignment.UserId = assignment.UserId;
            existingAssignment.AssignmentDate = assignment.AssignmentDate;

            _context.InventoryAssigments.Update(existingAssignment);
            await _context.SaveChangesAsync();
        }

        public async Task AddAssignmentAsync(InventoryAssigment newAssignment)
        {
            _context.InventoryAssigments.Add(newAssignment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAssignmentAsync(int id)
        {
            var deleteAssigment = await _context.InventoryAssigments.FindAsync(id);
            if (deleteAssigment != null)
            {
                _context.InventoryAssigments.Remove(deleteAssigment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddAssignmentHistoryAsync(AssigmentHistory assignmentHistory)
        {
            var assignmentHistories = await _context.AssigmentHistorys
                                                      .Where(a => a.ProductId == assignmentHistory.ProductId)
                                                      .OrderByDescending(a => a.AssignmentDate) // Tarihe göre sıralama
                                                      .ToListAsync();

            if (assignmentHistories.Count >= 5)
            {
                var oldestHistory = assignmentHistories.Last();
                _context.AssigmentHistorys.Remove(oldestHistory);
            }

            _context.AssigmentHistorys.Add(assignmentHistory);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AssigmentHistory>> GetAssignmentHistoryAsync(int productId)
        {
            return await _context.AssigmentHistorys
                                 .Where(a => a.ProductId == productId)
                                 .OrderByDescending(a => a.AssignmentDate)  // Son yapılan atamayı en önce göstermek için
                                 .ToListAsync();
        }
    }
}
