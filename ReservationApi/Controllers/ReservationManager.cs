using Microsoft.AspNetCore.Mvc;
using ReservationApi.Model;
using RezervationSystem.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace RezervationSystem.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationService _reservationService;
        private readonly TableService _tableService;
        private readonly EMailSenderService _mailSenderService;

        public ReservationController(ReservationService reservationService, TableService tableService, EMailSenderService mailSenderService)
        {
            _reservationService = reservationService;
            _tableService = tableService;
            _mailSenderService = mailSenderService;
        }

        [HttpPost("MakeReservation")]
        [SwaggerOperation(Summary = "MakeReservation")]
        public async Task<IActionResult> MakeReservation(string name, DateTime date, int guests, string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
                {
                    return StatusCode(500,"Geçersiz isim veya e-posta.");
                }
                if (guests <= 0)
                {
                    return StatusCode(500, "Girilen misafir sayısı 0 dan büyük olmalıdır.");
                }
                List<Tables> tables = _tableService.GetAllTables();
                if (tables == null || tables.Count == 0)
                {
                    return StatusCode(500,"Rezervasyon işlemi yapabilmeni için öncelikle sisteme adminler tarafından masa ve toplam katılımcı sayını belirtmesi gereklidir.");
                }

                List<Reservation> reservations = _reservationService.GetReservationByDate(date);
                if (reservations == null)
                {
                    return StatusCode(500, "Rezervasyonlar alınamadı.");
                }

                List<int> reservedTables = reservations.Select(r => r.TableNumber).ToList();
                if (tables.Count(t => !reservedTables.Contains(t.Number) && t.Capacity >= guests) == 0)
                {
                    return StatusCode(500, "Seçili tarihlerde müsaitlik bulunmamaktadır.");
                }
                int rezerveEdilecekMasa = tables.Where(t => !reservedTables.Contains(t.Number) && t.Capacity >= guests).FirstOrDefault()?.Number ?? 0;
                if (rezerveEdilecekMasa == 0)
                {
                    return StatusCode(500, "Masa rezervasyonu işlemi sırasında hata oluştu.");
                }
                var reservation = new Reservation
                {
                    CustomerName = name,
                    ReservationDate = date,
                    NumberOfGuests = guests,
                    TableNumber = rezerveEdilecekMasa
                };
                string insertResult = _reservationService.AddReservation(reservation);
                if (insertResult.Contains("HATA"))
                {
                    return StatusCode(500, "Rezervasyon kayıt işlemi sırasında hata oluştu.");
                }
                string mailBody = $"Sayın {name}, rezervasyonunuz başarıyla alındı. Masa No: {rezerveEdilecekMasa}, Tarih: {date}, Kişi Sayısı: {guests}";
                string fromName = _mailSenderService.fromName;
                string fromMail = _mailSenderService.fromMail;
                string fromPass = _mailSenderService.fromPass;
                if (String.IsNullOrEmpty(fromName) || String.IsNullOrEmpty(fromMail) || String.IsNullOrEmpty(fromPass))
                {
                    return Ok("Sistemde tanımlı mail adresi olmadığı için bilgilendirme maili gönderilmeden rezervasyon işleminiz yapılmıştır.");
                }
                string mailReult = await _mailSenderService.SendEmailAsync(email, "Rezervasyon Onayı", mailBody, name, fromMail, fromPass, fromName);
                return Ok("Rezervasyon işleminiz yapılmıştır.");
            }
            catch (Exception ex)
            {
                return StatusCode(404, $"E-posta gönderimi başarısız: {ex.Message}");
            }
        }


    }
}
