using apbd_project.Data;
using Microsoft.EntityFrameworkCore;

namespace apbd_project.Service.Impl;

public class SoftwareProductService : ISoftwareProductService
{
    private readonly ApplicationContext _context;

    public SoftwareProductService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<bool> DoesSoftwareProductExist(int id)
    {
        return await _context.SoftwareProducts.AnyAsync(c => c.Id == id);
    }
}