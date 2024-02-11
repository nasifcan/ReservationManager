using ReservationApi.Model;

public class TableService
{
    private readonly DataContext _context;

    public TableService(DataContext context)
    {
        _context = context;
    }

    public List<Tables> GetAllTables()
    {
        return _context.Tables.ToList();
    }

    public string AddTable(Tables table)
    {
        try
        {
            _context.Tables.Add(table);
            _context.SaveChanges();
            return "OK";
        }
        catch (Exception ex)
        {
            return $"HATA AddTable: {ex.Message}";
        }
    }

    public string UpdateTable(Tables table)
    {
        try
        {
            _context.Tables.Update(table);
            _context.SaveChanges();
            return "OK";
        }
        catch (Exception ex)
        {
            return $"HATA AddTable: {ex.Message}";
        }
    }

    public string DeleteTable(Tables table)
    {
        try
        {
            _context.Tables.Remove(table);
            _context.SaveChanges();
            return "OK";
        }
        catch (Exception ex)
        {
            return $"HATA AddTable: {ex.Message}";
        }
    }
}
