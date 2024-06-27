namespace apbd_project.Service;

public interface ISoftwareProductService
{
    Task<bool> DoesSoftwareProductExist(int id);
}