using ReservationApi.Model;

public class ReservationService
{
    private readonly DataContext _context;

    public ReservationService(DataContext context)
    {
        _context = context;
    }

    public List<Reservation> GetReservationByDate(DateTime date)
    {
        return _context.Reservations.Where(r => r.ReservationDate.Date == date.Date).ToList();
    }

    public string AddReservation(Reservation reservation)
    {
        try
        {
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
            return "OK";
        }
        catch (Exception ex)
        {
            return $"HATA AddReservation: {ex.Message}";
        }
    }

}
